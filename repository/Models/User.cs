using System.ComponentModel.DataAnnotations;

namespace Repository.Models;

public class User : BaseEntity
{
    [Required]
    
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [StringLength(100)]
    public string DisplayName { get; set; }

    public string RefreshToken { get; set; }

    public RoleCode? RoleCode { get; set; }
}

