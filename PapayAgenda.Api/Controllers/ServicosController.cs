using Microsoft.AspNetCore.Mvc;
using PapayAgenda.Application.DTOs;
using PapayAgenda.Application.Interfaces;

namespace PapayAgenda.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicosController : ControllerBase
    {
        private readonly IServicoService _servicoService;

        public ServicosController(IServicoService servicoService)
        {
            _servicoService = servicoService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarServicoDto dto)
        {
            try
            {
                var resultado = await _servicoService.CriarAsync(dto);
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
            var servicos = await _servicoService.ObterPorEstabelecimentoAsync(estabelecimentoId);
            return Ok(servicos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var servico = await _servicoService.ObterPorIdAsync(id);
            if (servico == null)
            {
                return NotFound(new { mensagem = "Serviço não encontrado." });
            }

            return Ok(servico);
        }
    }
}