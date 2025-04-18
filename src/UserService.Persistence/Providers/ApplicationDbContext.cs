using Microsoft.EntityFrameworkCore;
using UserService.Domain.Users;

namespace UserService.Persistence.Providers;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FullName).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(100);
            entity.Property(x => x.CreatedAt).IsRequired();
            entity.Property(x => x.CreatedBy).IsRequired();
            entity.Property(x => x.RowVersion).IsRowVersion();
        });
    }
}