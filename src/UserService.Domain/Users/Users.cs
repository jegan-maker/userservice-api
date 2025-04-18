using System.ComponentModel.DataAnnotations;
using System.Data;

namespace UserService.Domain.Users;

public class User
{
    public Guid Id { get; init; }
    public string FullName { get; set; }
    public string Email { get; set; }

    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; } = null!;

    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public User(Guid id, string fullName, string email, string createdBy)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;
    }

    public void Update(string fullName, string email, string updatedBy, byte[] rowVersion)
    {
        FullName = fullName;
        UpdatedBy = updatedBy;
        Email = email;
        UpdatedAt = DateTime.UtcNow;
        RowVersion = rowVersion;
    }
}