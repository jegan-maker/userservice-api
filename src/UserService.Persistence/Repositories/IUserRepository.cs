using UserService.Domain.Users;

namespace UserService.Persistence.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}
