using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Configuration
// --------------------
builder.Configuration.AddJsonFile("appsettings.json", optional: false);

// --------------------
// Dependency Injection
// --------------------

// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();

    var options = new ConfigurationOptions
    {
        AbortOnConnectFail = false,
        ConnectRetry = 5,
        ConnectTimeout = 5000
    };

    options.EndPoints.Add($"{config["Redis:Host"]}:{config["Redis:Port"]}");
    return ConnectionMultiplexer.Connect(options);
});

// SQL Server
builder.Services.AddTransient<SqlConnection>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new SqlConnection(config.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();
    
// --------------------
// Endpoints
// --------------------

app.MapGet("/api/health", () =>
{
    return Results.Ok(new
    {
        status = "Healthy",
        time = DateTime.UtcNow
    });
});

// 🔹 USERS
app.MapGet("/api/users", async (SqlConnection conn) =>
{
    await conn.OpenAsync();

    var cmd = new SqlCommand(
        "SELECT Id, Name, Email FROM appdb.dbo.Users",
        conn
    );

    var reader = await cmd.ExecuteReaderAsync();
    var users = new List<object>();

    while (await reader.ReadAsync())
    {
        users.Add(new
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Email = reader.GetString(2)
        });
    }

    return Results.Ok(users);
});

// 🔹 DATA (Redis + SQL)
app.MapGet("/api/data", async (
    IConnectionMultiplexer redis,
    SqlConnection conn) =>
{
    var cacheKey = "sample:data";
    var db = redis.GetDatabase();

    // 1️⃣ Try Redis
    var cached = await db.StringGetAsync(cacheKey);
    if (cached.HasValue)
    {
        return Results.Ok(new
        {
            source = "redis",
            value = cached.ToString()
        });
    }

    // 2️⃣ SQL fallback
    await conn.OpenAsync();

    var cmd = new SqlCommand(
        "SELECT TOP 1 Name FROM appdb.dbo.Users",
        conn
    );

    var value = (string?)await cmd.ExecuteScalarAsync() ?? "No Data";

    // 3️⃣ Cache result
    await db.StringSetAsync(cacheKey, value, TimeSpan.FromSeconds(30));

    return Results.Ok(new
    {
        source = "database",
        value
    });
});

app.Run();
