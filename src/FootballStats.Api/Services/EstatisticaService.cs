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
            .Select(j => new EstatisticaJogador(
                j.Nome,
                j.Registros.Sum(r => r.Gols),
                j.Registros.Sum(r => r.Assistencias)))
            .OrderByDescending(e => e.TotalGols)
            .ThenByDescending(e => e.TotalAssistencias)
            .ToListAsync();

        return placar;
    }
}
