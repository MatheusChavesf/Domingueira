using FootballStats.Api.DTOs;
using FootballStats.Api.Exceptions;
using FootballStats.Api.Services;

namespace FootballStats.Api.Endpoints;

public static class JogadoresEndpoints
{
    public static WebApplication MapJogadoresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/jogadores");

        group.MapPost("", async (CriarJogadorRequest request, IJogadorService service) =>
        {
            try
            {
                var jogador = await service.CriarAsync(request);
                return Results.Created($"/api/jogadores/{jogador.Id}", jogador);
            }
            catch (ValidationException ex)
            {
                return Results.BadRequest(new ErroResponse(ex.Message, ex.Erros));
            }
        });

        group.MapGet("", async (IJogadorService service) =>
        {
            var jogadores = await service.ListarTodosAsync();
            return Results.Ok(jogadores);
        });

        group.MapDelete("/{id}", async (string id, IJogadorService service) =>
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                return Results.BadRequest(new ErroResponse("Formato de identificador inválido"));
            }

            try
            {
                await service.ExcluirAsync(guidId);
                return Results.NoContent();
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(new ErroResponse(ex.Message));
            }
        });

        return app;
    }
}
