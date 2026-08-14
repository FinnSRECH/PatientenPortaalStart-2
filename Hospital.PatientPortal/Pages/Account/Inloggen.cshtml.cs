using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hospital.PatientPortal.Pages.Account;

public class InloggenModel(HospitalDataService data, PasswordService passwords) : PageModel
{
    [BindProperty] public LoginInput Input { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public class LoginInput
    {
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required, DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var patient = data.FindPatientByEmail(Input.Email);
        if (patient is null || !passwords.Verify(Input.Password, patient.PasswordHash, patient.PasswordSalt))
        {
            ErrorMessage = "E-mailadres of wachtwoord is onjuist.";
            return Page();
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, patient.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{patient.FirstName} {patient.LastName}"),
            new Claim(ClaimTypes.Email, patient.Email),
            new Claim(ClaimTypes.Role, "Patient")
        };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));
        return RedirectToPage("/Index");
    }
}
