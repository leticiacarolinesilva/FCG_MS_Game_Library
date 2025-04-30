using Microsoft.EntityFrameworkCore;

using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Interfaces;

namespace UserRegistrationAndGameLibrary.Infra.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserRegistrationDbContext _context;

    public UserRepository(UserRegistrationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .Include(gl => gl.GameLibrary)
            .FirstOrDefaultAsync(gl => gl.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(gl => gl.GameLibrary)
            .FirstOrDefaultAsync(gl => gl.Email.Value == email);
    }

    public async  Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        
    }

    public async Task<List<User>> SearchUsersAsync(string? email, string? name)
    {
        var query = _context.Users
            .Include(u => u.GameLibrary)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(u => u.Name.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            query = query.Where(u => u.Email.Value == email);
        }

        return await query.ToListAsync();
    }
}
