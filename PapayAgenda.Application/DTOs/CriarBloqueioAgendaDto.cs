namespace PapayAgenda.Application.DTOs
{
    public class CriarBloqueioAgendaDto
    {
        public int EstabelecimentoId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public bool DiaInteiro { get; set; }
        public string? Motivo { get; set; }
    }
}