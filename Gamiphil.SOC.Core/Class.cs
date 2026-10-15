
namespace Gamiphil.SOC.Core.Models;

public sealed class Endpoint
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Hostname { get; set; } = string.Empty;

    public string OperatingSystem { get; set; } = string.Empty;

    public string IpAddress { get; set; } = string.Empty;

    public string AgentVersion { get; set; } = string.Empty;

    public EndpointStatus Status { get; set; } = EndpointStatus.Unknown;

    public DateTimeOffset RegisteredAt { get; init; } =
        DateTimeOffset.UtcNow;

    public DateTimeOffset? LastSeenAt { get; set; }
}

public enum EndpointStatus
{
    Unknown,
    Online,
    Offline,
    Isolated
}
