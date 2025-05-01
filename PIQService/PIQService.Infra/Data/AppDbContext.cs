using Microsoft.EntityFrameworkCore;
using PIQService.Models.Dbo;
using PIQService.Models.Dbo.Assessments;

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

        modelBuilder.Entity<AssessmentDbo>()
            .HasMany(f => f.Teams)
            .WithMany()
            .UsingEntity("assessments_teams");

        modelBuilder.Entity<FormDbo>()
            .HasMany(f => f.CriteriaList)
            .WithMany()
            .UsingEntity("forms_criteria");

        modelBuilder.Entity<AssessmentMarkDbo>()
            .HasMany(f => f.Choices)
            .WithMany()
            .UsingEntity("marks_choices");
    }
}