using Proinug.WebUI.Models;

namespace Proinug.WebUI.Dto;

/// <summary>
/// Account DTO
/// </summary>
public class AccountDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User name
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// User role
    /// </summary>
    public Roles Role { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; set; } = "";
}