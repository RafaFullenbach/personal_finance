using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using personal_finance.Domain.Entities;
using personal_finance.Domain.Enums;
using personal_finance.Infrastructure.Persistence;

namespace personal_finance.Tests.Infrastructure.TestFactory;

public sealed class ApiFactory : WebApplicationFactory<Program>
{
    private string? _dbPath;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Remove o AppDbContext registrado originalmente
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            _dbPath = Path.Combine(Path.GetTempPath(), $"pf_test_{Guid.NewGuid():N}.db");
            var conn = $"Data Source={_dbPath}";

            services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(conn));

            // Aplica migrations automaticamente
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (_dbPath is not null && File.Exists(_dbPath))
        {
            try { File.Delete(_dbPath); } catch { }
        }
    }

    public async Task<Guid> SeedAccountAsync()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var account = new Account("Conta Teste", AccountType.Bank);

        db.Accounts.Add(account);
        await db.SaveChangesAsync();

        return account.Id;
    }
}
