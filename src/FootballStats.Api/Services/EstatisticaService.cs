using Microsoft.EntityFrameworkCore;
using FootballStats.Api.Data;
using FootballStats.Api.DTOs;

namespace FootballStats.Api.Services;

public class EstatisticaService : IEstatisticaService
{
    private readonly AppDbContext _context;

    public EstatisticaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<EstatisticaJogador>> ObterPlacarGeralAsync()
    {
        var placar = await _context.Jogadores
            .AsNoTracking()
            .GroupJoin(
                _context.RegistrosPartida,
                jogador => jogador.Id,
                registro => registro.JogadorId,
                (jogador, registros) => new { jogador.Nome, Registros = registros })
            .SelectMany(
                x => x.Registros.DefaultIfEmpty(),
                (x, registro) => new { x.Nome, Gols = registro != null ? registro.Gols : 0, Assistencias = registro != null ? registro.Assistencias : 0 })
            .GroupBy(x => x.Nome)
            .Select(g => new EstatisticaJogador(
                g.Key,
                g.Sum(x => x.Gols),
                g.Sum(x => x.Assistencias)))
            .OrderByDescending(e => e.TotalGols)
            .ThenByDescending(e => e.TotalAssistencias)
            .ToListAsync();

        return placar;
    }
}
