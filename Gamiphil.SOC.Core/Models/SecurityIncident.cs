
namespace Gamiphil.SOC.Core.Models;

public sealed class SecurityIncident
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public SeverityLevel Severity { get; set; } = SeverityLevel.Medium;

    public IncidentStatus Status { get; set; } = IncidentStatus.New;

    public List<Guid> RelatedEventIds { get; set; } = [];

    public DateTimeOffset CreatedAt { get; init; } =
        DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? ResolvedAt { get; set; }

    public string AssignedTo { get; set; } = string.Empty;
}

public enum IncidentStatus
{
    New,
    Investigating,
    Contained,
    Resolved,
    Closed
}
