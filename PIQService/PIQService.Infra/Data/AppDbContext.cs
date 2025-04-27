using Microsoft.EntityFrameworkCore;
using PIQService.Models.Dbo;

namespace PIQService.Infra.Data;

public class AppDbContext : DbContext
{
    public DbSet<EventDbo> Events { get; set; }

    public DbSet<DirectionDbo> Directions { get; set; }

    public DbSet<ProjectDbo> Projects { get; set; }

    public DbSet<TeamDbo> Teams { get; set; }

    public DbSet<UserDbo> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserDbo>()
            .HasOne(u => u.Team)
            .WithMany(t => t.Members)
            .HasForeignKey(u => u.TeamId);
    }
}