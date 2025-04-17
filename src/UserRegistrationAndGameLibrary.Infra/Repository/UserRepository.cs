using Microsoft.EntityFrameworkCore;
using UserRegistrationAndGameLibrary.Domain.Entities;
using UserRegistrationAndGameLibrary.Domain.Interfaces;
using UserRegistrationAndGameLibrary.Infra;

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
            .FirstOrDefaultAsync(gl => gl.Email == email);
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
}