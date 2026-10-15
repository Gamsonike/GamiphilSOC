
using Gamiphil.SOC.Core.Models;

namespace Gamiphil.SOC.Core.Interfaces;

public interface ISecurityIncidentRepository
{
    Task<SecurityIncident?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SecurityIncident>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<SecurityIncident> AddAsync(
        SecurityIncident incident,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        SecurityIncident incident,
        CancellationToken cancellationToken = default);
}
