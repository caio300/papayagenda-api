namespace PapayAgenda.Domain.Entities
{
    public class CategoriaServico
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty; 
        public string? Descricao { get; set; }
        public bool Ativa { get; set; } = true;
        public bool PermiteApenasUmPorAgendamento { get; set; } = false;
        public int OrdemExibicao { get; set; } = 1;

        public int? CategoriaPaiId { get; set; }
        public CategoriaServico? CategoriaPai { get; set; }
        public ICollection<CategoriaServico> CategoriasFilhas { get; set; } = new List<CategoriaServico>();

        public int EstabelecimentoId { get; set; }
        public Estabelecimento? Estabelecimento { get; set; }
        public ICollection<Servico> Servicos { get; set; } = new List<Servico>();
    }
}