using Microsoft.AspNetCore.Mvc;
using PapayAgenda.Application.DTOs;
using PapayAgenda.Application.Interfaces;

namespace PapayAgenda.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentosController : ControllerBase
    {
        private readonly IAgendamentoService _agendamentoService;

        public AgendamentosController(IAgendamentoService agendamentoService)
        {
            _agendamentoService = agendamentoService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarAgendamentoDto dto)
        {
            try
            {
                var resultado = await _agendamentoService.CriarAsync(dto);
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
            var agendamentos = await _agendamentoService.ObterPorEstabelecimentoAsync(estabelecimentoId);
            return Ok(agendamentos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var agendamento = await _agendamentoService.ObterPorIdAsync(id);
            if (agendamento == null)
            {
                return NotFound(new { mensagem = "Agendamento não encontrado." });
            }
            return Ok(agendamento);
        }
    }
}