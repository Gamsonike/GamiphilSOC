
using Gamiphil.SOC.Core.Models;

namespace Gamiphil.SOC.Core.Interfaces;

public interface IEndpointRepository
{
    Task<Endpoint?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Endpoint>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Endpoint> AddAsync(
        Endpoint endpoint,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Endpoint endpoint,
        CancellationToken cancellationToken = default);
}
