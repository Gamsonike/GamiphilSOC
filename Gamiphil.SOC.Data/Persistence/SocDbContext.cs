
using System.Text.Json;
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

        modelBuilder.Entity<Endpoint>(entity =>
        {
            entity.HasKey(endpoint => endpoint.Id);

            entity.Property(endpoint => endpoint.Hostname)
                .HasMaxLength(255);

            entity.Property(endpoint => endpoint.OperatingSystem)
                .HasMaxLength(100);

            entity.Property(endpoint => endpoint.IpAddress)
                .HasMaxLength(45);

            entity.Property(endpoint => endpoint.AgentVersion)
                .HasMaxLength(50);
        });

        modelBuilder.Entity<SecurityEvent>(entity =>
        {
            entity.HasKey(securityEvent => securityEvent.Id);

            entity.HasIndex(securityEvent => securityEvent.EndpointId);

            entity.Property(securityEvent => securityEvent.EventType)
                .HasMaxLength(100);

            entity.Property(securityEvent => securityEvent.Description)
                .HasMaxLength(4000);

            entity.Property(securityEvent => securityEvent.Source)
                .HasMaxLength(100);
        });

        modelBuilder.Entity<SecurityIncident>(entity =>
        {
            entity.HasKey(incident => incident.Id);

            entity.Property(incident => incident.Title)
                .HasMaxLength(255);

            entity.Property(incident => incident.AssignedTo)
                .HasMaxLength(255);

            entity.Property(incident => incident.RelatedEventIds)
                .HasConversion(
                    ids => JsonSerializer.Serialize(ids, (JsonSerializerOptions?)null),
                    json => JsonSerializer.Deserialize<List<Guid>>(
                        json, (JsonSerializerOptions?)null) ?? new List<Guid>());
        });
    }
}
