namespace PapayAgenda.Application.DTOs
{
    public class EstabelecimentoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string TelefoneWhatsApp { get; set; } = string.Empty;
        public string EmailGestor { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}