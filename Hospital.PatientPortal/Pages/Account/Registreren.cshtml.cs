using System.ComponentModel.DataAnnotations;
using Hospital.Domain.Models;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hospital.PatientPortal.Pages.Account;

public class RegistrerenModel(HospitalDataService data, PasswordService passwords) : PageModel
{
    [BindProperty] public RegisterInput Input { get; set; } = new();

    public class RegisterInput
    {
        [Required, StringLength(50)] public string FirstName { get; set; } = string.Empty;
        [Required, StringLength(80)] public string LastName { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public DateOnly DateOfBirth { get; set; }
        [Required, StringLength(120)] public string Address { get; set; } = string.Empty;
        [Phone] public string PhoneNumber { get; set; } = string.Empty;
        [Required, MinLength(8), DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();
        if (data.FindPatientByEmail(Input.Email) is not null)
        {
            ModelState.AddModelError("Input.Email", "Er bestaat al een account met dit e-mailadres.");
            return Page();
        }
        var password = passwords.HashPassword(Input.Password);
        data.AddPatient(new Patient
        {
            FirstName = Input.FirstName, LastName = Input.LastName, Email = Input.Email,
            DateOfBirth = Input.DateOfBirth, Address = Input.Address, PhoneNumber = Input.PhoneNumber,
            PasswordHash = password.Hash, PasswordSalt = password.Salt
        });
        return RedirectToPage("/Account/Inloggen");
    }
}
