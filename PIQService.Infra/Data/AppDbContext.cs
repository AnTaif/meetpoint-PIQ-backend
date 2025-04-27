using Microsoft.EntityFrameworkCore;
using PIQService.Models.Dbo;

namespace PIQService.Infra.Data;

public class AppDbContext : DbContext
{
    public DbSet<TeamDbo> Teams { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}