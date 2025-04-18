using Microsoft.EntityFrameworkCore;
using UserService.Domain.Users;
using UserService.Persistence.Providers;

namespace UserService.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _context.Users.FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving the user by ID.", ex);
        }
    }

    public async Task AddAsync(User user)
    {
        try
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("An error occurred while adding the user to the database.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An unexpected error occurred while adding the user.", ex);
        }
    }

    public async Task UpdateAsync(User user)
    {
        try
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InvalidOperationException("Concurrency conflict occurred while updating the user.", ex);
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("An error occurred while updating the user in the database.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An unexpected error occurred while updating the user.", ex);
        }
    }
}
