using PapayAgenda.Application.DTOs;
using PapayAgenda.Domain.Enums;

namespace PapayAgenda.Application.Interfaces
{
    public interface IAgendamentoService
    {
        Task<AgendamentoDto> CriarAsync(CriarAgendamentoDto dto);
        Task<IEnumerable<AgendamentoDto>> ObterPorEstabelecimentoAsync(int estabelecimentoId);
        Task<AgendamentoDto?> ObterPorIdAsync(int id);
        Task<IEnumerable<AgendamentoDto>> ObterPorDiaAsync(int estabelecimentoId, DateOnly data, StatusAgendamento? status = null);
        Task<IEnumerable<ResumoDiaCalendarioDto>> ObterResumoMensalAsync(int estabelecimentoId, int ano, int mes);
        Task<AgendamentoDto?> AtualizarStatusAsync(int id, StatusAgendamento novoStatus);
        Task<AgendamentoDto?> RemarcarAsync(int id, DateTime novaDataHoraInicio);
        Task<AgendamentoDto> CriarManualAsync(CriarAgendamentoManualDto dto);
    }
}