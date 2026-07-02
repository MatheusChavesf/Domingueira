using FootballStats.Api.DTOs;

namespace FootballStats.Api.Services;

public interface IEstatisticaService
{
    Task<List<EstatisticaJogador>> ObterPlacarGeralAsync();
}
