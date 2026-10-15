
namespace Gamiphil.SOC.Core.Models;

public sealed class SecurityEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid EndpointId { get; set; }

    public string EventType { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public SeverityLevel Severity { get; set; } = SeverityLevel.Informational;

    public string Source { get; set; } = string.Empty;

    public DateTimeOffset OccurredAt { get; set; } =
        DateTimeOffset.UtcNow;

    public bool IsAcknowledged { get; set; }
}

public enum SeverityLevel
{
    Informational,
    Low,
    Medium,
    High,
    Critical
}
