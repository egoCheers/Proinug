using Microsoft.AspNetCore.Authentication;

namespace Proinug.WebUI.Services;

public class RealSystemClock: ISystemClock
{
    public DateTimeOffset UtcNow => DateTime.UtcNow;
}