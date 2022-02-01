using System.ComponentModel.DataAnnotations;

namespace Proinug.WebUI.ViewModels;

/// <summary>
/// Credentials view model (for login form)
/// </summary>
public class CredentialsVm
{
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = "";
}