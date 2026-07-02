namespace FootballStats.Api.DTOs;

public record CriarJogadorRequest(string Nome);
public record CriarRegistroRequest(Guid JogadorId, DateOnly Data, int Gols, int Assistencias);
public record AtualizarRegistroRequest(int Gols, int Assistencias);
