using Microsoft.EntityFrameworkCore;
using PapayAgenda.Application.DTOs;
using PapayAgenda.Application.Interfaces;
using PapayAgenda.Domain.Entities;
using PapayAgenda.Domain.Enums;
using PapayAgenda.Infrastructure.Data;

namespace PapayAgenda.Application.Services
{
    public class AgendamentoService : IAgendamentoService
    {
        private readonly AppDbContext _context;

        public AgendamentoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AgendamentoDto> CriarAsync(CriarAgendamentoDto dto)
        {
            var estabelecimentoExiste = await _context.Estabelecimentos
                .AnyAsync(e => e.Id == dto.EstabelecimentoId);

            if (!estabelecimentoExiste)
            {
                throw new InvalidOperationException($"O estabelecimento com ID {dto.EstabelecimentoId} não foi encontrado.");
            }

            var servicos = await _context.Servicos
                .Where(s => dto.ServicoIds.Contains(s.Id) && s.EstabelecimentoId == dto.EstabelecimentoId)
                .ToListAsync();

            if (servicos.Count != dto.ServicoIds.Count)
            {
                throw new InvalidOperationException("Um ou mais serviços informados não existem ou não pertencem a este estabelecimento.");
            }

            int duracaoTotalMinutos = servicos.Sum(s => s.DuracaoMinutos);
            decimal valorTotal = servicos.Where(s => !s.EBrindeOuIncluso).Sum(s => s.Preco);
            var dataHoraFim = dto.DataHoraInicio.AddMinutes(duracaoTotalMinutos);

            var existeConflito = await ExisteConflitoHorarioAsync(
                dto.EstabelecimentoId,
                dto.DataHoraInicio,
                dataHoraFim
                );

            if (existeConflito)
            {
                throw new InvalidOperationException("Já existe um agendamento ativo nesse intervalo de horário.");
            }

            var existeBloqueio = await ExisteBloqueioHorarioAsync(
                dto.EstabelecimentoId,
                dto.DataHoraInicio,
                dataHoraFim);

            if (existeBloqueio)
            {
                throw new InvalidOperationException("O período selecionado está bloqueado na agenda do estabelecimento.");
            }

            var agendamento = new Agendamento
            {
                ClienteNome = dto.ClienteNome,
                ClienteTelefone = dto.ClienteTelefone,
                DataHoraInicio = dto.DataHoraInicio,
                DataHoraFim = dataHoraFim,
                ValorTotal = valorTotal,
                ValorSinal = 0,
                EstabelecimentoId = dto.EstabelecimentoId,
                Servicos = servicos
            };

            _context.Agendamentos.Add(agendamento);
            await _context.SaveChangesAsync();

            return MapearParaDto(agendamento);
        }

        public async Task<IEnumerable<AgendamentoDto>> ObterPorEstabelecimentoAsync(int estabelecimentoId)
        {
            var agendamentos = await _context.Agendamentos
                .AsNoTracking()
                .Include(a => a.Servicos)
                .Where(a => a.EstabelecimentoId == estabelecimentoId)
                .OrderBy(a => a.DataHoraInicio)
                .ToListAsync();

            return agendamentos.Select(MapearParaDto);
        }

        public async Task<AgendamentoDto?> ObterPorIdAsync(int id)
        {
            var agendamento = await _context.Agendamentos
                .AsNoTracking()
                .Include(a => a.Servicos)
                .FirstOrDefaultAsync(x => x.Id == id);

            return agendamento == null ? null : MapearParaDto(agendamento);
        }

        public async Task<IEnumerable<AgendamentoDto>> ObterPorDiaAsync(
            int estabelecimentoId,
            DateOnly data,
            StatusAgendamento? status = null)
        {
            var inicioDia = data.ToDateTime(TimeOnly.MinValue);
            var fimDia = inicioDia.AddDays(1);

            var query = _context.Agendamentos
                .AsNoTracking()
                .Include(a => a.Servicos)
                .Where(a => a.EstabelecimentoId == estabelecimentoId
                         && a.DataHoraInicio >= inicioDia
                         && a.DataHoraInicio < fimDia);

            if (status.HasValue)
            {
                query = query.Where(a => a.Status == status.Value);
            }

            var agendamentos = await query
                .OrderBy(a => a.DataHoraInicio)
                .ToListAsync();

            return agendamentos.Select(MapearParaDto);
        }

        public async Task<IEnumerable<ResumoDiaCalendarioDto>> ObterResumoMensalAsync(
            int estabelecimentoId,
            int ano,
            int mes)
        {
            if (mes < 1 || mes > 12)
            {
                throw new InvalidOperationException("O mês informado deve estar entre 1 e 12.");
            }

            var inicioMes = new DateTime(ano, mes, 1, 0, 0, 0);
            var fimMes = inicioMes.AddMonths(1);

            var registrosMes = await _context.Agendamentos
                .AsNoTracking()
                .Where(a => a.EstabelecimentoId == estabelecimentoId
                         && a.DataHoraInicio >= inicioMes
                         && a.DataHoraInicio < fimMes)
                .Select(a => new { a.DataHoraInicio, a.Status })
                .ToListAsync();

            return registrosMes
                .GroupBy(a => DateOnly.FromDateTime(a.DataHoraInicio))
                .OrderBy(g => g.Key)
                .Select(g => new ResumoDiaCalendarioDto
                {
                    Data = g.Key,
                    QuantidadeTotal = g.Count(),
                    QuantidadeAtivos = g.Count(x => x.Status != StatusAgendamento.Cancelado),
                    Pendentes = g.Count(x => x.Status == StatusAgendamento.Pendente),
                    Confirmados = g.Count(x => x.Status == StatusAgendamento.Confirmado),
                    Concluidos = g.Count(x => x.Status == StatusAgendamento.Concluido),
                    Cancelados = g.Count(x => x.Status == StatusAgendamento.Cancelado)
                });
        }

        private static AgendamentoDto MapearParaDto(Agendamento a)
        {
            return new AgendamentoDto
            {
                Id = a.Id,
                ClienteNome = a.ClienteNome,
                ClienteTelefone = a.ClienteTelefone,
                DataHoraInicio = a.DataHoraInicio,
                DataHoraFim = a.DataHoraFim,
                ValorTotal = a.ValorTotal,
                ValorSinal = a.ValorSinal,
                Status = a.Status,
                Lembrete24hEnviado = a.Lembrete24hEnviado,
                EstabelecimentoId = a.EstabelecimentoId,
                Servicos = a.Servicos.Select(s => new ServicoDto
                {
                    Id = s.Id,
                    Nome = s.Nome,
                    Preco = s.Preco,
                    DuracaoMinutos = s.DuracaoMinutos,
                    Ativo = s.Ativo,
                    OrdemExibicao = s.OrdemExibicao,
                    EstabelecimentoId = s.EstabelecimentoId
                }).ToList()
            };
        }

        public async Task<AgendamentoDto?> AtualizarStatusAsync(int id, StatusAgendamento novoStatus)
        {
            var agendamento = await _context.Agendamentos
                .Include(a => a.Servicos)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agendamento == null)
            {
                return null;
            }

            if (agendamento.Status == novoStatus)
            {
                return MapearParaDto(agendamento);
            }

            ValidarTransicaoStatus(agendamento.Status, novoStatus);

            agendamento.Status = novoStatus;
            await _context.SaveChangesAsync();

            return MapearParaDto(agendamento);
        }

        private static void ValidarTransicaoStatus(StatusAgendamento statusAtual, StatusAgendamento novoStatus)
        {
            if (statusAtual == StatusAgendamento.Cancelado)
            {
                throw new InvalidOperationException("Não é possível alterar o status de um agendamento cancelado.");
            }

            if (statusAtual == StatusAgendamento.Concluido)
            {
                throw new InvalidOperationException("Não é possível alterar o status de um agendamento já concluído.");
            }

            if (statusAtual == StatusAgendamento.Confirmado && novoStatus == StatusAgendamento.Pendente)
            {
                throw new InvalidOperationException("Não é possível retornar um agendamento confirmado para pendente.");
            }
        }

        public async Task<AgendamentoDto?> RemarcarAsync(int id, DateTime novaDataHoraInicio)
        {
            var agendamento = await _context.Agendamentos
                .Include(a => a.Servicos)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agendamento == null)
            {
                return null;
            }

            if (agendamento.Status == StatusAgendamento.Cancelado || agendamento.Status == StatusAgendamento.Concluido)
            {
                throw new InvalidOperationException("Não é possível remarcar um agendamento cancelado ou já concluído.");
            }

            int duracaoTotalMinutos = agendamento.Servicos.Sum(s => s.DuracaoMinutos);
            var novaDataHoraFim = novaDataHoraInicio.AddMinutes(duracaoTotalMinutos);

            var existeConflito = await ExisteConflitoHorarioAsync(
                agendamento.EstabelecimentoId,
                novaDataHoraInicio,
                novaDataHoraFim,
                agendamento.Id
                );

            if (existeConflito)
            {
                throw new InvalidOperationException("Já existe um agendamento ativo nesse intervalo de horário.");
            }

            var existeBloqueio = await ExisteBloqueioHorarioAsync(
                agendamento.EstabelecimentoId,
                novaDataHoraInicio,
                novaDataHoraFim);

            if (existeBloqueio)
            {
                throw new InvalidOperationException("O período selecionado está bloqueado na agenda do estabelecimento.");
            }

            agendamento.DataHoraInicio = novaDataHoraInicio;
            agendamento.DataHoraFim = novaDataHoraFim;

            await _context.SaveChangesAsync();

            return MapearParaDto(agendamento);
        }

        public async Task<AgendamentoDto> CriarManualAsync(CriarAgendamentoManualDto dto)
        {
            if (dto.ServicoIds == null || dto.ServicoIds.Count == 0)
            {
                throw new InvalidOperationException("Selecione ao menos um serviço para criar o agendamento.");
            }

            if (dto.StatusInicial == StatusAgendamento.Cancelado || dto.StatusInicial == StatusAgendamento.Concluido)
            {
                throw new InvalidOperationException("O agendamento manual deve iniciar como Pendente ou Confirmado.");
            }

            if (dto.ValorSinal < 0)
            {
                throw new InvalidOperationException("O valor do sinal não pode ser negativo.");
            }

            var estabelecimentoExiste = await _context.Estabelecimentos
                .AnyAsync(e => e.Id == dto.EstabelecimentoId);

            if (!estabelecimentoExiste)
            {
                throw new InvalidOperationException($"O estabelecimento com ID {dto.EstabelecimentoId} não foi encontrado.");
            }

            var servicos = await _context.Servicos
                .Where(s => dto.ServicoIds.Contains(s.Id) && s.EstabelecimentoId == dto.EstabelecimentoId)
                .ToListAsync();

            if (servicos.Count != dto.ServicoIds.Distinct().Count())
            {
                throw new InvalidOperationException("Um ou mais serviços informados não existem ou não pertencem a este estabelecimento.");
            }

            int duracaoTotalMinutos = servicos.Sum(s => s.DuracaoMinutos);
            decimal valorTotal = servicos.Where(s => !s.EBrindeOuIncluso).Sum(s => s.Preco);

            if (dto.ValorSinal > valorTotal)
            {
                throw new InvalidOperationException("O valor do sinal não pode ser maior que o valor total do agendamento.");
            }

            var dataHoraFim = dto.DataHoraInicio.AddMinutes(duracaoTotalMinutos);

            var existeConflito = await ExisteConflitoHorarioAsync(
                dto.EstabelecimentoId,
                dto.DataHoraInicio,
                dataHoraFim);

            if (existeConflito)
            {
                throw new InvalidOperationException("Já existe um agendamento ativo nesse intervalo de horário.");
            }

            var existeBloqueio = await ExisteBloqueioHorarioAsync(
                dto.EstabelecimentoId,
                dto.DataHoraInicio,
                dataHoraFim);

            if (existeBloqueio)
            {
                throw new InvalidOperationException("O período selecionado está bloqueado na agenda do estabelecimento.");
            }

            var agendamento = new Agendamento
            {
                ClienteNome = dto.ClienteNome,
                ClienteTelefone = dto.ClienteTelefone,
                DataHoraInicio = dto.DataHoraInicio,
                DataHoraFim = dataHoraFim,
                ValorTotal = valorTotal,
                ValorSinal = dto.ValorSinal,
                Status = dto.StatusInicial,
                EstabelecimentoId = dto.EstabelecimentoId,
                Servicos = servicos
            };

            _context.Agendamentos.Add(agendamento);
            await _context.SaveChangesAsync();

            return MapearParaDto(agendamento);
        }

        private Task<bool> ExisteConflitoHorarioAsync(
            int estabelecimentoId,
            DateTime inicio,
            DateTime fim,
            int? ignorarAgendamentoId = null)

            
        {
            return _context.Agendamentos
                .AsNoTracking()
                .AnyAsync(a => a.EstabelecimentoId == estabelecimentoId
                            && (!ignorarAgendamentoId.HasValue || a.Id != ignorarAgendamentoId.Value)
                            && a.Status != StatusAgendamento.Cancelado
                            && a.DataHoraInicio < fim
                            && a.DataHoraFim > inicio);
        }

        private Task<bool> ExisteBloqueioHorarioAsync(int estabelecimentoId, DateTime inicio, DateTime fim)
        {
            return _context.BloqueiosAgenda
                .AsNoTracking()
                .AnyAsync(b => b.EstabelecimentoId == estabelecimentoId
                            && b.DataHoraInicio < fim
                            && b.DataHoraFim > inicio);
        }
    }
}