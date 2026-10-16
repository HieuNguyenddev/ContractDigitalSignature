using System.ComponentModel.DataAnnotations;
using ContractDigitalSignature.Modules.Roles.Entities;

namespace ContractDigitalSignature.Modules.Roles.DTOs;

public class RoleRequest
{
    [Required(ErrorMessage = "Tên vai trò là bắt buộc.")]
    [StringLength(Role.NameMaxLength, MinimumLength = Role.NameMinLength, ErrorMessage = "Tên vai trò phải từ {2} đến {1} ký tự.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(Role.DescriptionMaxLength, ErrorMessage = "Mô tả không được dài quá {1} ký tự.")]
    public string? Description { get; set; }
}
