
using Gamiphil.SOC.Core.Interfaces;
using Gamiphil.SOC.Core.Models;
using Gamiphil.SOC.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gamiphil.SOC.Data.Repositories;

public sealed class EndpointRepository(SocDbContext dbContext)
    : IEndpointRepository
{
    public async Task<Endpoint?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Endpoints
            .AsNoTracking()
            .SingleOrDefaultAsync(
                endpoint => endpoint.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Endpoint>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Endpoints
            .AsNoTracking()
            .OrderBy(endpoint => endpoint.Hostname)
            .ToListAsync(cancellationToken);
    }

    public async Task<Endpoint> AddAsync(
        Endpoint endpoint,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(endpoint);

        dbContext.Endpoints.Add(endpoint);

        await dbContext.SaveChangesAsync(cancellationToken);

        return endpoint;
    }

    public async Task UpdateAsync(
        Endpoint endpoint,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(endpoint);

        var existing = await dbContext.Endpoints
            .SingleOrDefaultAsync(
                item => item.Id == endpoint.Id,
                cancellationToken);

        if (existing is null)
        {
            throw new KeyNotFoundException(
                $"Endpoint '{endpoint.Id}' was not found.");
        }

        existing.Hostname = endpoint.Hostname;
        existing.OperatingSystem = endpoint.OperatingSystem;
        existing.IpAddress = endpoint.IpAddress;
        existing.AgentVersion = endpoint.AgentVersion;
        existing.Status = endpoint.Status;
        existing.LastSeenAt = endpoint.LastSeenAt;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
