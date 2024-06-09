using System.ComponentModel.DataAnnotations;

namespace Repository.ViewModels.Requestes;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string DisplayName { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }
}

