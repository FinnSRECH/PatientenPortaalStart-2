using Hospital.PatientPortal.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
	// Standaard moet iedere pagina in het patiëntenportaal
	// alleen toegankelijk zijn voor ingelogde gebruikers.
	options.Conventions.AuthorizeFolder("/");

	// Alleen accountpagina's zoals inloggen en registreren
	// mogen zonder ingelogde gebruiker geopend worden.
	options.Conventions.AllowAnonymousToFolder("/Account");

	// De foutpagina moet ook beschikbaar blijven
	// wanneer iemand niet ingelogd is.
	options.Conventions.AllowAnonymousToPage("/Error");
});

builder.Services
	.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/Account/Inloggen";
		options.AccessDeniedPath = "/Account/Inloggen";

		options.Cookie.Name = "Hospital.PatientPortal.Auth";

		// JavaScript mag de authenticatiecookie niet uitlezen.
		options.Cookie.HttpOnly = true;

		// Cookie alleen via HTTPS versturen.
		options.Cookie.SecurePolicy =
			CookieSecurePolicy.Always;

		// Bescherming tegen het meesturen van cookies
		// bij verzoeken vanaf andere websites.
		options.Cookie.SameSite =
			SameSiteMode.Strict;

		// De gebruiker wordt na 30 minuten inactiviteit
		// opnieuw gevraagd om in te loggen.
		options.ExpireTimeSpan =
			TimeSpan.FromMinutes(30);

		options.SlidingExpiration = true;
	});

builder.Services.AddAuthorization();

builder.Services.AddSingleton<PasswordService>();
builder.Services.AddSingleton<HospitalDataService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();