using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Proinug.WebUI.Extensions;
using Proinug.WebUI.Interfaces;
using Proinug.WebUI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAuthService(builder.Configuration)
    .AddScoped<ProtectedLocalStorage>()
    .AddScoped<ICwAuthenticationStateProvider, CwAuthenticationStateProvider>()
    .AddScoped<AuthenticationStateProvider>(c => 
        (AuthenticationStateProvider?) c.GetService<ICwAuthenticationStateProvider>()!)
    .AddSingleton<ISystemClock, RealSystemClock>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();