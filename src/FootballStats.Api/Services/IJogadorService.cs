using FootballStats.Api.DTOs;
using FootballStats.Api.Models;

namespace FootballStats.Api.Services;

public interface IJogadorService
{
    Task<Jogador> CriarAsync(CriarJogadorRequest request);
    Task<List<Jogador>> ListarTodosAsync();
    Task ExcluirAsync(Guid id);
}
