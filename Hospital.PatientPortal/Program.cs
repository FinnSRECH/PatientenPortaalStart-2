using Microsoft.AspNetCore.Authentication.Cookies;
using Hospital.PatientPortal.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Klachten");
    options.Conventions.AuthorizeFolder("/Planning");
    options.Conventions.AuthorizeFolder("/Profiel");
    options.Conventions.AuthorizePage("/Index");
    options.Conventions.AllowAnonymousToFolder("/Account");
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Inloggen";
        options.AccessDeniedPath = "/Account/Inloggen";
        options.Cookie.Name = "Hospital.PatientPortal.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
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
