using FootballStats.Api.Data;
using FootballStats.Api.DTOs;
using FootballStats.Api.Exceptions;
using FootballStats.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballStats.Api.Services;

public class RegistroPartidaService : IRegistroPartidaService
{
    private readonly AppDbContext _context;

    public RegistroPartidaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RegistroPartida> CriarAsync(CriarRegistroRequest request)
    {
        // Validate player exists
        var jogadorExiste = await _context.Jogadores.AnyAsync(j => j.Id == request.JogadorId);
        if (!jogadorExiste)
        {
            throw new NotFoundException("Jogador não encontrado");
        }

        // Validate gols range [0, 99]
        if (request.Gols < 0 || request.Gols > 99)
        {
            throw new ValidationException("Gols", "Gols deve ser um valor entre 0 e 99");
        }

        // Validate assistencias range [0, 99]
        if (request.Assistencias < 0 || request.Assistencias > 99)
        {
            throw new ValidationException("Assistencias", "Assistências deve ser um valor entre 0 e 99");
        }

        var registro = new RegistroPartida
        {
            Id = Guid.NewGuid(),
            JogadorId = request.JogadorId,
            Data = request.Data,
            Gols = request.Gols,
            Assistencias = request.Assistencias
        };

        _context.RegistrosPartida.Add(registro);
        await _context.SaveChangesAsync();

        return registro;
    }

    public async Task<RegistroPartida> AtualizarAsync(Guid id, AtualizarRegistroRequest request)
    {
        // Validate record exists
        var registro = await _context.RegistrosPartida.FindAsync(id);
        if (registro is null)
        {
            throw new NotFoundException("Registro de partida não encontrado");
        }

        // Validate gols range [0, 99]
        if (request.Gols < 0 || request.Gols > 99)
        {
            throw new ValidationException("Gols", "Gols deve ser um valor entre 0 e 99");
        }

        // Validate assistencias range [0, 99]
        if (request.Assistencias < 0 || request.Assistencias > 99)
        {
            throw new ValidationException("Assistencias", "Assistências deve ser um valor entre 0 e 99");
        }

        registro.Gols = request.Gols;
        registro.Assistencias = request.Assistencias;

        await _context.SaveChangesAsync();

        return registro;
    }

    public async Task ExcluirAsync(Guid id)
    {
        // Validate record exists
        var registro = await _context.RegistrosPartida.FindAsync(id);
        if (registro is null)
        {
            throw new NotFoundException("Registro de partida não encontrado");
        }

        _context.RegistrosPartida.Remove(registro);
        await _context.SaveChangesAsync();
    }
}
