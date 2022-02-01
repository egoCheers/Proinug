using Bunit;
using Proinug.WebUI.Components;
using Proinug.WebUI.Shared;
using Xunit;

namespace Proinug.Tests;

public class NavBarTest
{
    [Fact]
    public void Navbar_HaveTopRowClass()
    {
        using var ctx = new TestContext();
        
        var component = ctx.RenderComponent<Navbar>();
        var navbar = component.Find("#Navbar");
        
        if (navbar.ClassName != null) Assert.Contains("top-row", navbar.ClassName);
    }
}