using TradeBotPro.App;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TradeBotPro.App.Services.Interfaces;
using TradeBotPro.App.Services;
using TradeBotPro.App.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

// Add Services
builder.Services.AddRazorPages();
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySQL(configuration.GetConnectionString("TradeBotProDatabase")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
    });

builder.Services.AddControllersWithViews();

// Add Scoped Services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapDefaultControllerRoute();

// Seed Data
using (var serviceScope = app.Services.CreateScope())
{
    DataSeeder.Seed(serviceScope.ServiceProvider);
}

app.Run();