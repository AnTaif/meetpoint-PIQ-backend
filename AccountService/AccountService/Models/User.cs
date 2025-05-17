using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Models;

public sealed class User : IdentityUser<Guid>
{
    [MaxLength(255)]
    public string FirstName { get; private set; } = null!;

    [MaxLength(255)]
    public string LastName { get; private set; } = null!;

    [MaxLength(255)]
    public string? MiddleName { get; private set; }

    [MaxLength(255)]
    public string? AvatarUrl { get; private set; }

    public User()
    {
        Id = Guid.NewGuid();
    }

    public User(Guid id, string email, string firstName, string lastName, string? middleName)
    {
        Id = id;
        Email = email;
        UserName = email;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }

    public void SetAvatar(string? url)
    {
        AvatarUrl = url;
    }
}