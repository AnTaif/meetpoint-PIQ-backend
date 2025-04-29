using AccountService.Models;
using Core.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Data;

public class AccountDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var roles = new[] { RolesConstants.Admin, RolesConstants.Member, RolesConstants.Tutor };
        var identityRoles = roles.Select(role =>
            new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = role,
                NormalizedName = role.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            });

        modelBuilder.Entity<IdentityRole<Guid>>().HasData(identityRoles);

        base.OnModelCreating(modelBuilder);
    }
}