using Microsoft.EntityFrameworkCore;

namespace ContractDigitalSignature.Modules.Roles.Data;

public static class DbInitializer
{
    public static void Apply(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }
}
