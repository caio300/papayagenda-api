namespace PapayAgenda.Application.DTOs
{
    public class BloqueioAgendaDto
    {
        public int Id { get; set; }
        public int EstabelecimentoId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public bool DiaInteiro { get; set; }
        public string? Motivo { get; set; }
    }
}