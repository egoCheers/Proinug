using System;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Proinug.WebUI.Components;

namespace Proinug.Tests;

public class RedirectToLoginPageTest
{
    [Fact]
    public void Redirect_NavReturnLoginRelativeUri()
    {
        using var ctx = new TestContext();
        var nav = ctx.Services.GetRequiredService<NavigationManager>();

        var component = ctx.RenderComponent<RedirectToLoginPage>();
        Assert.Equal("login", nav.ToBaseRelativePath(nav.Uri));
    }
}