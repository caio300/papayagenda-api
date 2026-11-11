using PapayAgenda.Application.DTOs;

namespace PapayAgenda.Application.Interfaces
{
    public interface IEstabelecimentoService
    {
        Task<EstabelecimentoDto> CriarAsync(CriarEstabelecimentoDto dto);
        Task<IEnumerable<EstabelecimentoDto>> ObterTodosAsync();
        Task<EstabelecimentoDto?> ObterPorSlugAsync(string slug);
    }
}