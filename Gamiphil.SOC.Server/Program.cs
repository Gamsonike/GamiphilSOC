
using Gamiphil.SOC.Core.Interfaces;
using Gamiphil.SOC.Data.Persistence;
using Gamiphil.SOC.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Resolve the existing database from the repository root.
// In development, ContentRootPath is the server project directory.
var databasePath = Path.GetFullPath(
    Path.Combine(
        builder.Environment.ContentRootPath,
        "..",
        "Gamiphil.SOC.Data",
        "gamiphil-soc.db"));

if (!File.Exists(databasePath))
{
    throw new FileNotFoundException(
        $"The existing SOC database was not found: {databasePath}");
}

var connectionString = builder.Configuration
    .GetConnectionString("SocDatabase");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string 'SocDatabase' is missing.");
}

// Use the confirmed database path rather than a working-directory-dependent path.
var sqliteConnectionString =
    new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder(connectionString)
    {
        DataSource = databasePath
    }.ToString();

builder.Services.AddDbContext<SocDbContext>(options =>
    options.UseSqlite(sqliteConnectionString));

builder.Services.AddScoped<IEndpointRepository, EndpointRepository>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/endpoints", async (
    IEndpointRepository repository,
    CancellationToken cancellationToken) =>
{
    var endpoints = await repository.GetAllAsync(cancellationToken);
    return Results.Ok(endpoints);
})
.WithName("GetEndpoints");

app.MapGet("/api/endpoints/{id:guid}", async (
    Guid id,
    IEndpointRepository repository,
    CancellationToken cancellationToken) =>
{
    var endpoint = await repository.GetByIdAsync(id, cancellationToken);

    return endpoint is null
        ? Results.NotFound()
        : Results.Ok(endpoint);
})
.WithName("GetEndpointById");

app.Run();
