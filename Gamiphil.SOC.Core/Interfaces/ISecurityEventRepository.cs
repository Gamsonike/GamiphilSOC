
using Gamiphil.SOC.Core.Models;

namespace Gamiphil.SOC.Core.Interfaces;

public interface ISecurityEventRepository
{
    Task<SecurityEvent?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SecurityEvent>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SecurityEvent>> GetByEndpointIdAsync(
        Guid endpointId,
        CancellationToken cancellationToken = default);

    Task<SecurityEvent> AddAsync(
        SecurityEvent securityEvent,
        CancellationToken cancellationToken = default);

    Task<bool> AcknowledgeAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
