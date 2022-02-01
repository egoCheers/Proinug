using Bunit;
using Proinug.WebUI.Shared;
using Xunit;

namespace Proinug.Tests;

public class MainLayoutTest
{
    [Fact]
    public void MainLayout_HaveNavbarPresent()
    {
        using var ctx = new TestContext();
        
        var component = ctx.RenderComponent<MainLayout>();
        var navbar = component.Find("#Navbar");
        
        Assert.Equal("Navbar", navbar.Id);
    }
}