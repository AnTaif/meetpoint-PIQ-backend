using AccountService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AccountService.Data;

public class AccountDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
}