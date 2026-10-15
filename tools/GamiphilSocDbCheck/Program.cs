
using Gamiphil.SOC.Core.Models;
using Gamiphil.SOC.Data.Persistence;
using Gamiphil.SOC.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var databasePath = Path.GetFullPath(
    Path.Combine(
        Directory.GetCurrentDirectory(),
        "Gamiphil.SOC.Data",
        "gamiphil-soc.db"));

if (!File.Exists(databasePath))
{
    Console.Error.WriteLine($"FAIL: Database not found: {databasePath}");
    Environment.ExitCode = 1;
    return;
}

var options = new DbContextOptionsBuilder<SocDbContext>()
    .UseSqlite($"Data Source={databasePath}")
    .Options;

var testEndpointId = Guid.Parse(
    "11111111-2222-3333-4444-555555555555");

var missingEndpointId = Guid.Parse(
    "99999999-8888-7777-6666-555555555555");

try
{
    await using var db = new SocDbContext(options);
    var repository = new EndpointRepository(db);

    // Test 1: Insert the endpoint once, then verify persistence
    // on subsequent process runs before changing anything.
    var endpoint = await repository.GetByIdAsync(testEndpointId);

    if (endpoint is null)
    {
        endpoint = new Endpoint
        {
            Id = testEndpointId,
            Hostname = "GAMIPHIL-TEST-ENDPOINT",
            OperatingSystem = "Windows Test",
            IpAddress = "192.0.2.10",
            AgentVersion = "test-only",
            Status = EndpointStatus.Offline
        };

        await repository.AddAsync(endpoint);
        Console.WriteLine("PASS: Endpoint inserted.");
    }
    else
    {
        if (endpoint.Status != EndpointStatus.Online ||
            endpoint.LastSeenAt is null)
        {
            throw new InvalidOperationException(
                "Previously updated status did not survive the new process run.");
        }

        Console.WriteLine(
            "PASS: Updated status and LastSeenAt survived a new process run.");
        Console.WriteLine(
            "PASS: Existing endpoint retrieved; no duplicate inserted.");
    }

    // Test 2: Verify stored fields.
    var retrieved = await repository.GetByIdAsync(testEndpointId);

    if (retrieved is null ||
        retrieved.Hostname != "GAMIPHIL-TEST-ENDPOINT" ||
        retrieved.IpAddress != "192.0.2.10")
    {
        throw new InvalidOperationException(
            "Retrieved endpoint fields do not match expected values.");
    }

    Console.WriteLine("PASS: Retrieved endpoint fields match.");

    // Test 3: Update status and verify the database read-back.
    retrieved.Status = EndpointStatus.Online;
    retrieved.LastSeenAt = DateTimeOffset.UtcNow;

    await repository.UpdateAsync(retrieved);

    var updated = await repository.GetByIdAsync(testEndpointId);

    if (updated is null ||
        updated.Status != EndpointStatus.Online ||
        updated.LastSeenAt is null)
    {
        throw new InvalidOperationException(
            "Endpoint status or LastSeenAt update was not persisted.");
    }

    Console.WriteLine("PASS: Endpoint update persisted.");

    // Test 4: Verify the endpoint appears in the full list.
    var allEndpoints = await repository.GetAllAsync();

    if (!allEndpoints.Any(item => item.Id == testEndpointId))
    {
        throw new InvalidOperationException(
            "Test endpoint was not found in GetAllAsync.");
    }

    Console.WriteLine("PASS: Endpoint appears in GetAllAsync.");

    // Test 5: Reject updates for nonexistent endpoints.
    var missingEndpoint = new Endpoint
    {
        Id = missingEndpointId,
        Hostname = "NONEXISTENT-TEST-ENDPOINT",
        OperatingSystem = "Test",
        IpAddress = "192.0.2.11",
        AgentVersion = "test-only",
        Status = EndpointStatus.Offline
    };

    var rejectedMissingEndpoint = false;

    try
    {
        await repository.UpdateAsync(missingEndpoint);
    }
    catch (KeyNotFoundException)
    {
        rejectedMissingEndpoint = true;
    }

    if (!rejectedMissingEndpoint)
    {
        throw new InvalidOperationException(
            "Updating a nonexistent endpoint was not rejected.");
    }

    Console.WriteLine(
        "PASS: Updating a nonexistent endpoint is rejected.");

    Console.WriteLine("\nPASS: All endpoint repository checks completed.");
}
catch (Exception ex)
{
    Console.Error.WriteLine(
        $"FAIL: {ex.GetType().Name}: {ex.Message}");

    Environment.ExitCode = 1;
}
