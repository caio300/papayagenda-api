using Microsoft.AspNetCore.Mvc;
using PapayAgenda.Application.DTOs;
using PapayAgenda.Application.Interfaces;
using PapayAgenda.Domain.Enums;

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

        [HttpGet("estabelecimento/{estabelecimentoId}/dia")]
        public async Task<IActionResult> ObterPorDia(
            int estabelecimentoId,
            [FromQuery] DateOnly data,
            [FromQuery] StatusAgendamento? status = null)
        {
            var agendamentos = await _agendamentoService.ObterPorDiaAsync(estabelecimentoId, data, status);
            return Ok(agendamentos);
        }

        [HttpGet("estabelecimento/{estabelecimentoId}/resumo-mensal")]
        public async Task<IActionResult> ObterResumoMensal(
            int estabelecimentoId,
            [FromQuery] int ano,
            [FromQuery] int mes)
        {
            try
            {
                var resumo = await _agendamentoService.ObterResumoMensalAsync(estabelecimentoId, ano, mes);
                return Ok(resumo);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
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

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] AtualizarStatusAgendamentoDto dto)
        {
            try
            {
                var agendamento = await _agendamentoService.AtualizarStatusAsync(id, dto.NovoStatus);
                if (agendamento == null)
                {
                    return NotFound(new { mensagem = "Agendamento não encontrado." });
                }

                return Ok(agendamento);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPatch("{id}/remarcar")]
        public async Task<IActionResult> Remarcar(int id, [FromBody] RemarcarAgendamentoDto dto)
        {
            try
            {
                var agendamento = await _agendamentoService.RemarcarAsync(id, dto.NovaDataHoraInicio);
                if (agendamento == null)
                {
                    return NotFound(new { mensagem = "Agendamento não encontrado." });
                }

                return Ok(agendamento);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPost("manual")]
        public async Task<IActionResult> CriarManual([FromBody] CriarAgendamentoManualDto dto)
        {
            try
            {
                var resultado = await _agendamentoService.CriarManualAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}