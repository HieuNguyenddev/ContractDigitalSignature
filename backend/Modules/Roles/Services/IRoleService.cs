using ContractDigitalSignature.Modules.Roles.Common.DTOs;
using ContractDigitalSignature.Modules.Roles.DTOs;

namespace ContractDigitalSignature.Modules.Roles.Services;

public interface IRoleService
{
    Task<PagedResult<RoleResponse>> SearchAsync(
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task<RoleResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<RoleResponse> CreateAsync(RoleRequest request, CancellationToken cancellationToken);

    Task<RoleResponse> UpdateAsync(Guid id, RoleRequest request, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
