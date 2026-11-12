using Microsoft.AspNetCore.Mvc;
using PapayAgenda.Application.DTOs;
using PapayAgenda.Application.Interfaces;

namespace PapayAgenda.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasServicoController : ControllerBase
    {
        private readonly ICategoriaServicoService _categoriaService;

        public CategoriasServicoController(ICategoriaServicoService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarCategoriaServicoDto dto)
        {
            try
            {
                var resultado = await _categoriaService.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("estabelecimento/{estabelecimentoId}")]
        public async Task<IActionResult> ObterPorEstabelecimento(int estabelecimentoId)
        {
            var categorias = await _categoriaService.ObterPorEstabelecimentoAsync(estabelecimentoId);
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var categoria = await _categoriaService.ObterPorIdAsync(id);
            if (categoria == null)
            {
                return NotFound(new { mensagem = "Categoria não encontrada." });
            }
            return Ok(categoria);
        }
    }
}