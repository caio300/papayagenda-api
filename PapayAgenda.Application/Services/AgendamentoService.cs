using Microsoft.EntityFrameworkCore;
using PapayAgenda.Application.DTOs;
using PapayAgenda.Application.Interfaces;
using PapayAgenda.Domain.Entities;
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

            return new AgendamentoDto
            {
                Id = agendamento.Id,
                ClienteNome = agendamento.ClienteNome,
                ClienteTelefone = agendamento.ClienteTelefone,
                DataHoraInicio = agendamento.DataHoraInicio,
                DataHoraFim = agendamento.DataHoraFim,
                ValorTotal = agendamento.ValorTotal,
                ValorSinal = agendamento.ValorSinal,
                Status = agendamento.Status,
                Lembrete24hEnviado = agendamento.Lembrete24hEnviado,
                EstabelecimentoId = agendamento.EstabelecimentoId,
                Servicos = servicos.Select(s => new ServicoDto
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

        public async Task<IEnumerable<AgendamentoDto>> ObterPorEstabelecimentoAsync(int estabelecimentoId)
        {
            var agendamentos = await _context.Agendamentos
                .Include(a => a.Servicos)
                .Where(a => a.EstabelecimentoId == estabelecimentoId)
                .ToListAsync();

            return agendamentos.Select(a => new AgendamentoDto
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
            });
        }

        public async Task<AgendamentoDto?> ObterPorIdAsync(int id)
        {
            var a = await _context.Agendamentos
                .Include(a => a.Servicos)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (a == null) return null;

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
    }
}