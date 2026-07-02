namespace FootballStats.Api.Models;

public class RegistroPartida
{
    public Guid Id { get; set; }
    public Guid JogadorId { get; set; }
    public DateOnly Data { get; set; }
    public int Gols { get; set; }
    public int Assistencias { get; set; }
    public Jogador Jogador { get; set; } = null!;
}
