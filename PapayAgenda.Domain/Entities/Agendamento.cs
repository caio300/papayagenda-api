using PapayAgenda.Domain.Enums;

namespace PapayAgenda.Domain.Entities
{
    public class Agendamento
    {
        public int Id { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public string ClienteTelefone { get; set; } = string.Empty;
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorSinal { get; set; }
        public StatusAgendamento Status { get; set; } = StatusAgendamento.PendentePagamento;

        // Controle de Notificação e Robô de 24h
        public bool Lembrete24hEnviado { get; set; } = false;
        public string? GoogleEventId { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

        // Chave Estrangeira do Multi-tenant
        public int EstabelecimentoId { get; set; }
        public Estabelecimento? Estabelecimento { get; set; }

        // Relacionamento com os serviços escolhidos
        public ICollection<Servico> Servicos { get; set; } = new List<Servico>();
    }
}