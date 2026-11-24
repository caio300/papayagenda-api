using PapayAgenda.Domain.Enums;

namespace PapayAgenda.Application.DTOs
{
    public class CriarAgendamentoManualDto
    {
        public int EstabelecimentoId { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public string ClienteTelefone { get; set; } = string.Empty;
        public DateTime DataHoraInicio { get; set; }
        public List<int> ServicoIds { get; set; } = new List<int>();
        public StatusAgendamento StatusInicial { get; set; } = StatusAgendamento.Confirmado;
        public decimal ValorSinal { get; set; } = 0;
    }
}