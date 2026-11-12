using PapayAgenda.Domain.Enums;

namespace PapayAgenda.Application.DTOs
{
    public class AgendamentoDto
    {
        public int Id { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public string ClienteTelefone { get; set; } = string.Empty;
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorSinal { get; set; }
        public StatusAgendamento Status { get; set; }
        public bool Lembrete24hEnviado { get; set; }
        public int EstabelecimentoId { get; set; }
        public List<ServicoDto> Servicos { get; set; } = new List<ServicoDto>();
    }
}