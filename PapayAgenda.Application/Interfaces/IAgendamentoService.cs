using PapayAgenda.Application.DTOs;

namespace PapayAgenda.Application.Interfaces
{
    public interface IAgendamentoService
    {
        Task<AgendamentoDto> CriarAsync(CriarAgendamentoDto dto);
        Task<IEnumerable<AgendamentoDto>> ObterPorEstabelecimentoAsync(int estabelecimentoId);
        Task<AgendamentoDto?> ObterPorIdAsync(int id);
    }
}