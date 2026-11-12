namespace PapayAgenda.Application.DTOs
{
    public class CategoriaServicoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int EstabelecimentoId { get; set; }
    }
}