using AccountService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Data;

public class AccountDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
    {
    }
}