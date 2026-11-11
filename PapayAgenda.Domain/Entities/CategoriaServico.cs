namespace PapayAgenda.Domain.Entities
{
    public class CategoriaServico
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty; 
        public string? Descricao { get; set; }
        public bool Ativa { get; set; } = true;

        // Pertence a um estabelecimento específico
        public int EstabelecimentoId { get; set; }
        public Estabelecimento? Estabelecimento { get; set; }
        
        // Uma categoria possui vários serviços
        public ICollection<Servico> Servicos { get; set; } = new List<Servico>();
    }
}