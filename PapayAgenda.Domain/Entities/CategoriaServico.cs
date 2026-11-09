namespace PapaiAgenda.Domain.Entities
{
    public class CategoriaServico
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty; 
        public string? Descricao { get; set; }
        public bool Ativa { get; set; } = true;
        
        public int EstabelecimentoId { get; set; }
        public Estabelecimento? Estabelecimento { get; set; }
        
        public ICollection<Servico> Servicos { get; set; } = new List<Servico>();
    }
}