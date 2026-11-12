namespace PapayAgenda.Application.DTOs
{
    public class CriarServicoDto
    {
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int DuracaoMinutos { get; set; }
        public int CategoriaServicoId { get; set; }
        public int EstabelecimentoId { get; set; }
    }
}