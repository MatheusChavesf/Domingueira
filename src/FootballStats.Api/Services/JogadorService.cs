using Microsoft.EntityFrameworkCore;
using FootballStats.Api.Data;
using FootballStats.Api.DTOs;
using FootballStats.Api.Exceptions;
using FootballStats.Api.Models;

namespace FootballStats.Api.Services;

public class JogadorService : IJogadorService
{
    private readonly AppDbContext _context;

    public JogadorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Jogador> CriarAsync(CriarJogadorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
        {
            throw new ValidationException("Nome", "O nome do jogador é obrigatório");
        }

        var nomeTrimmed = request.Nome.Trim();

        if (nomeTrimmed.Length > 100)
        {
            throw new ValidationException("Nome", "O nome do jogador excede o comprimento máximo de 100 caracteres");
        }

        var jogador = new Jogador
        {
            Id = Guid.NewGuid(),
            Nome = nomeTrimmed
        };

        _context.Jogadores.Add(jogador);
        await _context.SaveChangesAsync();

        return jogador;
    }

    public async Task<List<Jogador>> ListarTodosAsync()
    {
        return await _context.Jogadores
            .OrderBy(j => j.Nome)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task ExcluirAsync(Guid id)
    {
        var jogador = await _context.Jogadores.FindAsync(id);

        if (jogador is null)
        {
            throw new NotFoundException("Jogador não encontrado");
        }

        _context.Jogadores.Remove(jogador);
        await _context.SaveChangesAsync();
    }
}
