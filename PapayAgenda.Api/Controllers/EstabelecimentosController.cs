using Microsoft.AspNetCore.Mvc;
using PapayAgenda.Application.DTOs;
using PapayAgenda.Application.Interfaces;

namespace PapayAgenda.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstabelecimentosController : ControllerBase
    {
        private readonly IEstabelecimentoService _estabelecimentoService;

        public EstabelecimentosController(IEstabelecimentoService estabelecimentoService)
        {
            _estabelecimentoService = estabelecimentoService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarEstabelecimentoDto dto)
        {
            try
            {
                var resultado = await _estabelecimentoService.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorSlug), new { slug = resultado.Slug }, resultado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var estabelecimentos = await _estabelecimentoService.ObterTodosAsync();
            return Ok(estabelecimentos);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> ObterPorSlug(string slug)
        {
            var estabelecimento = await _estabelecimentoService.ObterPorSlugAsync(slug);
            if (estabelecimento == null)
            {
                return NotFound(new { mensagem = "Estabelecimento não encontrado." });
            }

            return Ok(estabelecimento);
        }
    }
}