using UserService.Persistence.Providers;
using UserService.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Users;


namespace UserService.Persistence.Unit.Tests.Respositories;

public class UserRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly UserRepository _repository;
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public UserRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(_options);
        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task AddAsync_Should_Add_User_To_Database()
    {
        var user = new User(Guid.NewGuid(), "John Doe", "john@example.com", "test");

        await _repository.AddAsync(user);

        var savedUser = await _context.Users.FindAsync(user.Id);
        savedUser.Should().NotBeNull();
        savedUser!.Email.Should().Be("john@example.com");
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_User_When_Exists()
    {
        var user = new User(Guid.NewGuid(), "jane Doe", "jane@example.com", "test");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(user.Id);
        result.Should().NotBeNull();
        result!.Email.Should().Be("jane@example.com");
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Not_Exists()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Existing_User()
    {
        var user = new User(Guid.NewGuid(), "Initial Name", "initial@example.com", "test");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        user.Update("Updated Name", "updated@example.com", "test", user.RowVersion); 

        await _repository.UpdateAsync(user);

        var updated = await _context.Users.FindAsync(user.Id);
        updated!.FullName.Should().Be("Updated Name");
        updated.Email.Should().Be("updated@example.com");
    }

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_User_Is_Detached()
    {
        var user = new User(Guid.NewGuid(), "Detached User", "detached@example.com", "test");

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        _context.Entry(user).State = EntityState.Detached;

        var newContext = new ApplicationDbContext(_options);
        var repo = new UserRepository(newContext);

        Func<Task> act = async () => await repo.UpdateAsync(user);
        await act.Should().NotThrowAsync(); // EF Core will attach and track the update
    }

}

