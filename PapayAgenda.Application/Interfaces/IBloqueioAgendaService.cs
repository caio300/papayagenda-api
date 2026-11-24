using PapayAgenda.Application.DTOs;

namespace PapayAgenda.Application.Interfaces
{
    public interface IBloqueioAgendaService
    {
        Task<BloqueioAgendaDto> CriarAsync(CriarBloqueioAgendaDto dto);
        Task<IEnumerable<BloqueioAgendaDto>> ObterPorPeriodoAsync(int estabelecimentoId, DateTime inicio, DateTime fim);
        Task<bool> RemoverAsync(int id);
    }
}