using System.ComponentModel.DataAnnotations;

namespace WebSite.EndPoints.Models.ViewModels.Account;

public sealed record RegisterViewModel
{
    [Required]
    [MaxLength(100)]
    public string FullName { get; init; }

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; init; }

    [Required]
    [MaxLength(20)]
    [DataType(DataType.Password)]
    public string Password { get; init; }

    [Required]
    [MaxLength(20)]
    [Compare(nameof(Password))]
    [DataType(DataType.Password)]
    public string Re_Password { get; init; }

    public string? PhoneNumber { get; init; }
}