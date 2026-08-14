using System.ComponentModel.DataAnnotations;

namespace Hospital.Domain.Models;

public class Patient
{
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Required, StringLength(120)]
    public string Address { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
}
