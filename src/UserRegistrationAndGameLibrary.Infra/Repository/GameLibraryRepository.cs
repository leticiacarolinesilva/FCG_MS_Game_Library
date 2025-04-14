using Microsoft.EntityFrameworkCore;
using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Interfaces;
using UserRegistrationAndGameLibrary.Infra.Data;

namespace UserRegistrationAndGameLibrary.Infra.Repository;

public class GameLibraryRepository : IGameLibraryRepository
{
    private readonly UserRegistrationDbContext _context;

    public GameLibraryRepository(UserRegistrationDbContext context)
    {
        _context = context;
    }

    public async Task<GameLibrary?> GetByIdAsync(Guid id)
    {
        return await _context.GameLibraries
            .Include(gl => gl.Game)
            .Include(gl => gl.User)
            .FirstOrDefaultAsync(gl => gl.Id == id);
    }

    public async Task<IEnumerable<GameLibrary>> GetByUserIdAsync(Guid userId)
    {
        return await _context.GameLibraries
            .Where(gl => gl.UserId == userId)
            .Include(gl => gl.Game)
            .OrderByDescending(gl => gl.PurchaseDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<GameLibrary>> GetInstalledGamesAsync(Guid userId)
    {
        return await _context.GameLibraries
            .Where(gl => gl.UserId == userId && gl.IsInstalled)
            .Include(gl => gl.Game)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> UserOwnsGameAsync(Guid userId, Guid gameId)
    {
        return await _context.GameLibraries
            .AnyAsync(gl => gl.UserId == userId && gl.GameId == gameId);
    }

    public async Task<GameLibrary> AddAsync(GameLibrary gameLibrary)
    {
        await _context.GameLibraries.AddAsync(gameLibrary);
        await _context.SaveChangesAsync();
        return gameLibrary;
    }

    public async Task UpdateAsync(GameLibrary gameLibrary)
    {
        _context.GameLibraries.Update(gameLibrary);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateInstallationStatusAsync(Guid gameLibraryId, bool isInstalled)
    {
        var gameLibrary = await _context.GameLibraries.FindAsync(gameLibraryId);
        if (gameLibrary != null)
        {
            gameLibrary.SetInstalledStatus(isInstalled);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveFromLibraryAsync(Guid gameLibraryId)
    {
        var gameLibrary = await _context.GameLibraries.FindAsync(gameLibraryId);
        if (gameLibrary != null)
        {
            _context.GameLibraries.Remove(gameLibrary);
            await _context.SaveChangesAsync();
        }
    }
}