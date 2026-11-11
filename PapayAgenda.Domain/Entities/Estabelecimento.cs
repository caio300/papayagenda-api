namespace PapayAgenda.Domain.Entities
{
    public class Estabelecimento
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string TelefoneWhatsApp { get; set; } = string.Empty;
        public string GoogleCalendarId { get; set; } = string.Empty;
        public string EmailGestor { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

        // Relacionamentos
        public ICollection<CategoriaServico> Categorias { get; set; } = new List<CategoriaServico>();
        public ICollection<Servico> Servicos { get; set; } = new List<Servico>();
        public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
    }
}