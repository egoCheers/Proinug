namespace Proinug.WebUI.Models;

/// <summary>
/// User roles
/// </summary>
public enum Roles
{
    /// <summary>
    /// Not authorized
    /// </summary>
    NoAuthorized,

    /// <summary>
    /// Base role
    /// </summary>
    Base = 100,

    /// <summary>
    /// Base application role
    /// </summary>
    Application = 110,

    /// <summary>
    /// Support
    /// </summary>
    Support = 200,

    /// <summary>
    /// Administrator
    /// </summary>
    Administrator = 300,

    /// <summary>
    /// Super administrator
    /// </summary>
    SuperAdministrator = 400
}