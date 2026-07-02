namespace FootballStats.Api.DTOs;

public record EstatisticaJogador(string Nome, int TotalGols, int TotalAssistencias);
public record ErroResponse(string Mensagem, Dictionary<string, string[]>? Erros = null);
