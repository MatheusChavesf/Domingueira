using FootballStats.Api.DTOs;
using FootballStats.Api.Exceptions;
using FootballStats.Api.Services;

namespace FootballStats.Api.Endpoints;

public static class RegistrosEndpoints
{
    public static WebApplication MapRegistrosEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/registros");

        group.MapPost("", async (CriarRegistroRequest request, IRegistroPartidaService service) =>
        {
            try
            {
                var registro = await service.CriarAsync(request);
                return Results.Created($"/api/registros/{registro.Id}", registro);
            }
            catch (ValidationException ex)
            {
                return Results.BadRequest(new ErroResponse(ex.Message, ex.Erros));
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(new ErroResponse(ex.Message));
            }
        });

        group.MapPut("/{id}", async (string id, AtualizarRegistroRequest request, IRegistroPartidaService service) =>
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                return Results.BadRequest(new ErroResponse("Formato de identificador inválido"));
            }

            try
            {
                var registro = await service.AtualizarAsync(guidId, request);
                return Results.Ok(registro);
            }
            catch (ValidationException ex)
            {
                return Results.BadRequest(new ErroResponse(ex.Message, ex.Erros));
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(new ErroResponse(ex.Message));
            }
        });

        group.MapDelete("/{id}", async (string id, IRegistroPartidaService service) =>
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
