using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ContractDigitalSignature.Modules.Roles.Entities;

[Table("Roles")]
[Index(nameof(NormalizedName), IsUnique = true)]
public class Role
{
    public const int NameMinLength = 2;
    public const int NameMaxLength = 100;
    public const int DescriptionMaxLength = 500;
    public const int SearchMaxLength = NameMaxLength + DescriptionMaxLength + 1;

    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(DescriptionMaxLength)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string NormalizedName { get; set; } = string.Empty;

    [Required]
    [MaxLength(SearchMaxLength)]
    public string NormalizedSearch { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}
