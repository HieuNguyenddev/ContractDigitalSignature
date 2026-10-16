using ContractDigitalSignature.Modules.Roles.Data;
using ContractDigitalSignature.Modules.Roles.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContractDigitalSignature.Modules.Roles.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _db;

    public RoleRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _db.Roles.FirstOrDefaultAsync(role => role.Id == id, cancellationToken);
    }

    public Task<bool> NameExistsAsync(string normalizedName, Guid? exceptId, CancellationToken cancellationToken)
    {
        return _db.Roles.AnyAsync(
            role => role.NormalizedName == normalizedName && (exceptId == null || role.Id != exceptId),
            cancellationToken);
    }

    public async Task<(List<Role> Items, int TotalCount)> SearchAsync(
        List<string> searchWords,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = _db.Roles.AsQueryable();
        foreach (var word in searchWords)
        {
            var current = word;
            query = query.Where(role => role.NormalizedSearch.Contains(current));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(role => role.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task AddAsync(Role role, CancellationToken cancellationToken)
    {
        _db.Roles.Add(role);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public Task SaveAsync(CancellationToken cancellationToken)
    {
        return _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Role role, CancellationToken cancellationToken)
    {
        _db.Roles.Remove(role);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
