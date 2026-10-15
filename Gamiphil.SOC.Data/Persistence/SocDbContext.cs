
using Gamiphil.SOC.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Gamiphil.SOC.Data.Persistence;

public sealed class SocDbContext(DbContextOptions<SocDbContext> options)
    : DbContext(options)
{
    public DbSet<Endpoint> Endpoints => Set<Endpoint>();

    public DbSet<SecurityEvent> SecurityEvents => Set<SecurityEvent>();

    public DbSet<SecurityIncident> SecurityIncidents =>
        Set<SecurityIncident>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Endpoint>()
            .HasKey(endpoint => endpoint.Id);

        modelBuilder.Entity<SecurityEvent>()
            .HasKey(securityEvent => securityEvent.Id);

        modelBuilder.Entity<SecurityEvent>()
            .HasIndex(securityEvent => securityEvent.EndpointId);

        modelBuilder.Entity<SecurityIncident>()
            .HasKey(incident => incident.Id);

        modelBuilder.Entity<Endpoint>()
            .Property(endpoint => endpoint.Hostname)
            .HasMaxLength(255);

        modelBuilder.Entity<SecurityEvent>()
            .Property(securityEvent => securityEvent.Description)
            .HasMaxLength(4000);

        modelBuilder.Entity<SecurityIncident>()
            .Property(incident => incident.Title)
            .HasMaxLength(255);
    }
}
