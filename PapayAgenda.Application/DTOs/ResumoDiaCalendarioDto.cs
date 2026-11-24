namespace PapayAgenda.Application.DTOs
{
    public class ResumoDiaCalendarioDto
    {
        public DateOnly Data { get; set; }
        public int QuantidadeTotal { get; set; }
        public int QuantidadeAtivos { get; set; }
        public int Pendentes { get; set; }
        public int Confirmados { get; set; }
        public int Concluidos { get; set; }
        public int Cancelados { get; set; }
    }
}