using Microsoft.EntityFrameworkCore;
using PapayAgenda.Domain.Entities;

namespace PapayAgenda.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Estabelecimento> Estabelecimentos { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<BloqueioAgenda> BloqueiosAgenda { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Garante que o Slug do Estabelecimento seja único na base (ex: /inconstante)
            modelBuilder.Entity<Estabelecimento>()
                .HasIndex(e => e.Slug)
                .IsUnique();

            // Relacionamento N para N entre Agendamento e Servico (Um agendamento pode ter múltiplos serviços)
            modelBuilder.Entity<Agendamento>()
                .HasMany(a => a.Servicos)
                .WithMany();

            modelBuilder.Entity<BloqueioAgenda>(entity =>
            {
                entity.Property(b => b.Motivo).HasMaxLength(200);
                entity.HasOne(b => b.Estabelecimento)
                    .WithMany()
                    .HasForeignKey(b => b.EstabelecimentoId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}