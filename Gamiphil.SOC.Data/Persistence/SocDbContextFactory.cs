
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Gamiphil.SOC.Data.Persistence;

public sealed class SocDbContextFactory
    : IDesignTimeDbContextFactory<SocDbContext>
{
    public SocDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SocDbContext>();

        optionsBuilder.UseSqlite(
            "Data Source=gamiphil-soc.db");

        return new SocDbContext(optionsBuilder.Options);
    }
}
