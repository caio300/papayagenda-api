using PapayAgenda.Application.DTOs;

namespace PapayAgenda.Application.Interfaces
{
    public interface IServicoService
    {
        Task<ServicoDto> CriarAsync(CriarServicoDto dto);
        Task<IEnumerable<ServicoDto>> ObterPorEstabelecimentoAsync(int estabelecimentoId);
        Task<ServicoDto?> ObterPorIdAsync(int id);
    }
}