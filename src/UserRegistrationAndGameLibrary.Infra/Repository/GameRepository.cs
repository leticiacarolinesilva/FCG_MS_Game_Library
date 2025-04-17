using Microsoft.EntityFrameworkCore;
using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Enums;
using UserRegistrationAndGameLibrary.Domain.Interfaces;
using UserRegistrationAndGameLibrary.Infra;

namespace UserRegistrationAndGameLibrary.Infra.Repository;

public class GameRepository : IGameRepository
{
    private readonly UserRegistrationDbContext _context;

    public GameRepository(UserRegistrationDbContext context)
    {
        _context = context;
    }

    public async Task<Game?> GetByIdAsync(Guid id)
    {
        return await _context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<Game>> GetAllAsync()
    {
        return await _context.Games
            .AsNoTracking()
            .OrderBy(g => g.Title)
            .ToListAsync();
    }

    public async Task<IEnumerable<Game>> GetByGenreAsync(GameGenre genre)
    {
        return await _context.Games
            .AsNoTracking()
            .Where(g => g.Genre == genre)
            .OrderBy(g => g.Title)
            .ToListAsync();
    }

    public async Task<IEnumerable<Game>> SearchAsync(string searchTerm)
    {
        return await _context.Games
            .AsNoTracking()
            .Where(g => g.Title.Contains(searchTerm) || 
                        g.Description.Contains(searchTerm))
            .OrderBy(g => g.Title)
            .ToListAsync();
    }

    public async Task<Game> AddAsync(Game game)
    {
        await _context.Games.AddAsync(game);
        await _context.SaveChangesAsync();
        return game;
    }

    public async Task UpdateAsync(Game game)
    {
        _context.Games.Update(game);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Game game)
    {
        _context.Games.Remove(game);
        await _context.SaveChangesAsync();
    }
}