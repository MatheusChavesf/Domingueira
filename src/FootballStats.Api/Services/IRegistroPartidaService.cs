using FootballStats.Api.DTOs;
using FootballStats.Api.Models;

namespace FootballStats.Api.Services;

public interface IRegistroPartidaService
{
    Task<RegistroPartida> CriarAsync(CriarRegistroRequest request);
    Task<RegistroPartida> AtualizarAsync(Guid id, AtualizarRegistroRequest request);
    Task ExcluirAsync(Guid id);
}
