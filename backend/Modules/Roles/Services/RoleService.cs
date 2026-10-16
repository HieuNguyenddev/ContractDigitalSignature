using System.Globalization;
using System.Text;
using ContractDigitalSignature.Modules.Roles.Common;
using ContractDigitalSignature.Modules.Roles.Common.DTOs;
using ContractDigitalSignature.Modules.Roles.Entities;
using ContractDigitalSignature.Modules.Roles.DTOs;
using ContractDigitalSignature.Modules.Roles.Repositories;

namespace ContractDigitalSignature.Modules.Roles.Services;

public class RoleService : IRoleService
{
    public const int DefaultPageSize = 10;
    public const int MaxPageSize = 100;

    private readonly IRoleRepository _roles;

    public RoleService(IRoleRepository roles)
    {
        _roles = roles;
    }

    public async Task<PagedResult<RoleResponse>> SearchAsync(
        string? search,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        if (page < 1)
        {
            throw new ValidationException("Số trang phải từ 1 trở lên.");
        }

        if (pageSize < 1 || pageSize > MaxPageSize)
        {
            throw new ValidationException($"Kích thước trang phải từ 1 đến {MaxPageSize}.");
        }

        var words = RemoveAccent(search).Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        var (items, totalCount) = await _roles.SearchAsync(words, page, pageSize, cancellationToken);

        return new PagedResult<RoleResponse>
        {
            Items = items.Select(ToResponse).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<RoleResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var role = await _roles.GetByIdAsync(id, cancellationToken);
        if (role == null)
        {
            throw new NotFoundException("Không tìm thấy vai trò.");
        }

        return ToResponse(role);
    }

    public async Task<RoleResponse> CreateAsync(RoleRequest request, CancellationToken cancellationToken)
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow
        };
        Apply(role, request);

        if (await _roles.NameExistsAsync(role.NormalizedName, null, cancellationToken))
        {
            throw new ConflictException("Tên vai trò đã tồn tại.");
        }

        await _roles.AddAsync(role, cancellationToken);
        return ToResponse(role);
    }

    public async Task<RoleResponse> UpdateAsync(Guid id, RoleRequest request, CancellationToken cancellationToken)
    {
        var role = await _roles.GetByIdAsync(id, cancellationToken);
        if (role == null)
        {
            throw new NotFoundException("Không tìm thấy vai trò.");
        }

        Apply(role, request);

        if (await _roles.NameExistsAsync(role.NormalizedName, role.Id, cancellationToken))
        {
            throw new ConflictException("Tên vai trò đã tồn tại.");
        }

        await _roles.SaveAsync(cancellationToken);
        return ToResponse(role);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var role = await _roles.GetByIdAsync(id, cancellationToken);
        if (role == null)
        {
            throw new NotFoundException("Không tìm thấy vai trò.");
        }

        await _roles.DeleteAsync(role, cancellationToken);
    }

    private static void Apply(Role role, RoleRequest request)
    {
        var name = Clean(request.Name);
        var description = Clean(request.Description);

        if (name.Length < Role.NameMinLength || name.Length > Role.NameMaxLength)
        {
            throw new ValidationException(
                $"Tên vai trò phải từ {Role.NameMinLength} đến {Role.NameMaxLength} ký tự.");
        }

        if (description.Length > Role.DescriptionMaxLength)
        {
            throw new ValidationException(
                $"Mô tả không được dài quá {Role.DescriptionMaxLength} ký tự.");
        }

        role.Name = name;
        role.Description = description.Length == 0 ? null : description;
        role.NormalizedName = name.ToLowerInvariant();
        role.NormalizedSearch = RemoveAccent(description.Length == 0 ? name : name + " " + description);
        role.UpdatedAt = DateTimeOffset.UtcNow;
    }

    private static RoleResponse ToResponse(Role role)
    {
        return new RoleResponse
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            CreatedAt = role.CreatedAt,
            UpdatedAt = role.UpdatedAt
        };
    }

    private static string Clean(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return string.Join(' ', value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
    }

    private static string RemoveAccent(string? value)
    {
        var text = Clean(value).ToLowerInvariant();
        if (text.Length == 0)
        {
            return string.Empty;
        }

        var builder = new StringBuilder();
        foreach (var character in text.Normalize(NormalizationForm.FormD))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            builder.Append(character == 'đ' ? 'd' : character);
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }
}
