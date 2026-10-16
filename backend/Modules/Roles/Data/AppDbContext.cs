using ContractDigitalSignature.Modules.Roles.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContractDigitalSignature.Modules.Roles.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Role> Roles => Set<Role>();
}
