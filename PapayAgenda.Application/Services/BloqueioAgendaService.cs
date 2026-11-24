using Microsoft.EntityFrameworkCore;
using PapayAgenda.Application.DTOs;
using PapayAgenda.Application.Interfaces;
using PapayAgenda.Domain.Entities;
using PapayAgenda.Domain.Enums;
using PapayAgenda.Infrastructure.Data;

namespace PapayAgenda.Application.Services
{
    public class BloqueioAgendaService : IBloqueioAgendaService
    {
        private readonly AppDbContext _context;

        public BloqueioAgendaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BloqueioAgendaDto> CriarAsync(CriarBloqueioAgendaDto dto)
        {
            if (dto.DataHoraFim <= dto.DataHoraInicio)
            {
                throw new InvalidOperationException("A data/hora de término do bloqueio deve ser maior que a de início.");
            }

            var estabelecimentoExiste = await _context.Estabelecimentos
                .AnyAsync(e => e.Id == dto.EstabelecimentoId);

            if (!estabelecimentoExiste)
            {
                throw new InvalidOperationException($"O estabelecimento com ID {dto.EstabelecimentoId} não foi encontrado.");
            }

            // Opção A: impede criar bloqueio se já houver agendamento ativo no intervalo
            var existeAgendamentoAtivo = await _context.Agendamentos
                .AsNoTracking()
                .AnyAsync(a => a.EstabelecimentoId == dto.EstabelecimentoId
                            && a.Status != StatusAgendamento.Cancelado
                            && a.DataHoraInicio < dto.DataHoraFim
                            && a.DataHoraFim > dto.DataHoraInicio);

            if (existeAgendamentoAtivo)
            {
                throw new InvalidOperationException(
                    "Já existem agendamentos ativos nesse intervalo. Cancele ou remarque-os antes de criar o bloqueio.");
            }

            var bloqueio = new BloqueioAgenda
            {
                EstabelecimentoId = dto.EstabelecimentoId,
                DataHoraInicio = dto.DataHoraInicio,
                DataHoraFim = dto.DataHoraFim,
                DiaInteiro = dto.DiaInteiro,
                Motivo = string.IsNullOrWhiteSpace(dto.Motivo) ? null : dto.Motivo.Trim()
            };

            _context.BloqueiosAgenda.Add(bloqueio);
            await _context.SaveChangesAsync();

            return MapearParaDto(bloqueio);
        }

        public async Task<IEnumerable<BloqueioAgendaDto>> ObterPorPeriodoAsync(
            int estabelecimentoId,
            DateTime inicio,
            DateTime fim)
        {
            if (fim <= inicio)
            {
                throw new InvalidOperationException("A data/hora final do período deve ser maior que a inicial.");
            }

            var bloqueios = await _context.BloqueiosAgenda
                .AsNoTracking()
                .Where(b => b.EstabelecimentoId == estabelecimentoId
                         && b.DataHoraInicio < fim
                         && b.DataHoraFim > inicio)
                .OrderBy(b => b.DataHoraInicio)
                .ToListAsync();

            return bloqueios.Select(MapearParaDto);
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var bloqueio = await _context.BloqueiosAgenda.FirstOrDefaultAsync(b => b.Id == id);
            if (bloqueio == null)
            {
                return false;
            }

            _context.BloqueiosAgenda.Remove(bloqueio);
            await _context.SaveChangesAsync();
            return true;
        }

        private static BloqueioAgendaDto MapearParaDto(BloqueioAgenda b)
        {
            return new BloqueioAgendaDto
            {
                Id = b.Id,
                EstabelecimentoId = b.EstabelecimentoId,
                DataHoraInicio = b.DataHoraInicio,
                DataHoraFim = b.DataHoraFim,
                DiaInteiro = b.DiaInteiro,
                Motivo = b.Motivo
            };
        }
    }
}