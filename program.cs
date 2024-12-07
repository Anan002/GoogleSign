using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["GoogleAuth:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["GoogleAuth:ClientSecret"];
    googleOptions.CallbackPath = builder.Configuration["GoogleAuth:CallbackPath"]; // This reads the CallbackPath from appsettings.json
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ensure authentication is used before authorization
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Values}/{action=Login}");

// Explicitly map the sign-in and callback routes for Google
app.MapControllerRoute(
    name: "google-signin",
    pattern: "signin-google",
    defaults: new { controller = "Signin", action = "GoogleSignIn" });

app.MapControllerRoute(
    name: "google-callback",
    pattern: "google-callback",
    defaults: new { controller = "Signin", action = "GoogleCallback" });

app.Run();




