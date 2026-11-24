using Microsoft.AspNetCore.Mvc;
using PapayAgenda.Application.DTOs;
using PapayAgenda.Application.Interfaces;

namespace PapayAgenda.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BloqueiosAgendaController : ControllerBase
    {
        private readonly IBloqueioAgendaService _bloqueioService;

        public BloqueiosAgendaController(IBloqueioAgendaService bloqueioService)
        {
            _bloqueioService = bloqueioService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarBloqueioAgendaDto dto)
        {
            try
            {
                var bloqueio = await _bloqueioService.CriarAsync(dto);
                return Ok(bloqueio);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("estabelecimento/{estabelecimentoId}")]
        public async Task<IActionResult> ObterPorPeriodo(
            int estabelecimentoId,
            [FromQuery] DateTime inicio,
            [FromQuery] DateTime fim)
        {
            try
            {
                var bloqueios = await _bloqueioService.ObterPorPeriodoAsync(estabelecimentoId, inicio, fim);
                return Ok(bloqueios);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            var removido = await _bloqueioService.RemoverAsync(id);
            if (!removido)
            {
                return NotFound(new { mensagem = "Bloqueio não encontrado." });
            }

            return NoContent();
        }
    }
}