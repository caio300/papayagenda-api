namespace PapaiAgenda.Domain.Entities
{
    public class Servico
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int DuracaoMinutos { get; set; }
        public bool Ativo { get; set; } = true;

        public int CategoriaServicoId { get; set; }
        public CategoriaServico? CategoriaServico { get; set; }

        public int EstabelecimentoId { get; set; }
        public Estabelecimento? Estabelecimento { get; set; }
    }
}