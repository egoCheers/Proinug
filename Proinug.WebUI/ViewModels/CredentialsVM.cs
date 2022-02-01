using System.ComponentModel.DataAnnotations;

namespace Proinug.WebUI.ViewModels;

/// <summary>
/// Credentials view model (for login form)
/// </summary>
public class CredentialsVM
{
    [Required]
    public string Username { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}