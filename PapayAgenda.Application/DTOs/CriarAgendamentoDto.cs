namespace PapayAgenda.Application.DTOs
{
    public class CriarAgendamentoDto
    {
        public string ClienteNome { get; set; } = string.Empty;
        public string ClienteTelefone { get; set; } = string.Empty;
        public DateTime DataHoraInicio { get; set; }
        public int EstabelecimentoId { get; set; }
        public List<int> ServicoIds { get; set; } = new List<int>();
    }
}