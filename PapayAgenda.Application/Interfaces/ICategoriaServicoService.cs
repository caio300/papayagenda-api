using PapayAgenda.Application.DTOs;

namespace PapayAgenda.Application.Interfaces
{
    public interface ICategoriaServicoService
    {
        Task<CategoriaServicoDto> CriarAsync(CriarCategoriaServicoDto dto);
        Task<IEnumerable<CategoriaServicoDto>> ObterPorEstabelecimentoAsync(int estabelecimentoId);
        Task<CategoriaServicoDto?> ObterPorIdAsync(int id);
    }
}