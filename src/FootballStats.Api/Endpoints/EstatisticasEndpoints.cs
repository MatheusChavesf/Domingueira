using FootballStats.Api.Services;

namespace FootballStats.Api.Endpoints;

public static class EstatisticasEndpoints
{
    public static WebApplication MapEstatisticasEndpoints(this WebApplication app)
    {
        app.MapGet("/api/estatisticas", async (IEstatisticaService service) =>
        {
            var result = await service.ObterPlacarGeralAsync();
            return Results.Ok(result);
        });

        return app;
    }
}
