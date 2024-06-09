using System.ComponentModel.DataAnnotations;

namespace Repository.ViewModels.Requestes;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}

