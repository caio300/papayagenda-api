namespace PapayAgenda.Application.DTOs
{
    public class CriarEstabelecimentoDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string TelefoneWhatsApp { get; set; } = string.Empty;
        public string EmailGestor { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
    }
}