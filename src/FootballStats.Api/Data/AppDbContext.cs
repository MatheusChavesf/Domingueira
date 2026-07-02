using Microsoft.EntityFrameworkCore;
using FootballStats.Api.Models;

namespace FootballStats.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Jogador> Jogadores => Set<Jogador>();
    public DbSet<RegistroPartida> RegistrosPartida => Set<RegistroPartida>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Jogador>(entity =>
        {
            entity.HasKey(j => j.Id);
            entity.Property(j => j.Nome).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<RegistroPartida>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Data)
                  .HasColumnType("date")
                  .IsRequired();
            entity.Property(r => r.Gols).IsRequired();
            entity.Property(r => r.Assistencias).IsRequired();
            entity.HasOne(r => r.Jogador)
                  .WithMany(j => j.Registros)
                  .HasForeignKey(r => r.JogadorId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.ToTable(t =>
            {
                t.HasCheckConstraint("CK_RegistrosPartida_Gols", "\"Gols\" >= 0 AND \"Gols\" <= 99");
                t.HasCheckConstraint("CK_RegistrosPartida_Assistencias", "\"Assistencias\" >= 0 AND \"Assistencias\" <= 99");
            });
        });
    }
}
