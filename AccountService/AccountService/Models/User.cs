using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength

namespace AccountService.Models;

public sealed class User : IdentityUser<Guid>
{
    [MaxLength(255)] 
    public string FirstName { get; private set; } = null!;

    [MaxLength(255)] 
    public string LastName { get; private set; } = null!;
    
    [MaxLength(255)]
    public string? MiddleName { get; private set; }
    
    public User() { Id = Guid.NewGuid(); }
    
    public User(string email, string firstName, string lastName, string? middleName)
    {
        Email = email;
        UserName = email;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }
    
}