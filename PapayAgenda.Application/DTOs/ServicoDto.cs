namespace PapayAgenda.Application.DTOs
{
    public class ServicoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int DuracaoMinutos { get; set; }
        public bool Ativo { get; set; }
        public int CategoriaServicoId { get; set; }
        public int EstabelecimentoId { get; set; }
    }
}