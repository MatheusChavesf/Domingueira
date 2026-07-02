namespace FootballStats.Api.Models;

public class Jogador
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public List<RegistroPartida> Registros { get; set; } = new();
}
