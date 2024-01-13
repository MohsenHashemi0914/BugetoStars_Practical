using System.ComponentModel.DataAnnotations;

namespace WebSite.EndPoint.Models.ViewModels.Account;

public sealed record LoginViewModel
{
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; init; }

    [Required]
    [MaxLength(20)]
    [DataType(DataType.Password)]
    public string Password { get; init; }

    [Display(Name =("RememberMe"))]
    public bool IsPersistent { get; init; }

    public string? ReturnUrl { get; init; }
}