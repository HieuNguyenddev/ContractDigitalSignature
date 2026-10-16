using ContractDigitalSignature.Modules.Roles.Entities;

namespace ContractDigitalSignature.Modules.Roles.Repositories;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> NameExistsAsync(string normalizedName, Guid? exceptId, CancellationToken cancellationToken);

    Task<(List<Role> Items, int TotalCount)> SearchAsync(
        List<string> searchWords,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task AddAsync(Role role, CancellationToken cancellationToken);

    Task SaveAsync(CancellationToken cancellationToken);

    Task DeleteAsync(Role role, CancellationToken cancellationToken);
}
