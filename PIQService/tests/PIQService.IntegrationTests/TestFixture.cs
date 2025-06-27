using Microsoft.EntityFrameworkCore;
using PIQService.Infra.Data;

namespace PIQService.IntegrationTests;

[TestFixture]
public class TestFixture
{
    private AppDbContext DbContext { get; set; }

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;
        
        DbContext = new AppDbContext(options);
        DbContext.Database.EnsureCreated();
    }

    [TearDown]
    public void TearDown()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}