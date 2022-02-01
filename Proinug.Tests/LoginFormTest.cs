using System.Linq;
using Bunit;
using Bunit.TestDoubles;
using Proinug.WebUI.Components;
using Xunit;

namespace Proinug.Tests;

public class LoginFormTest
{
    [Fact]
    public void LoginForm_ContainsUsernamePasswordSubmitInputs()
    {
        using var ctx = new TestContext();
        
        var component = ctx.RenderComponent<LoginForm>();
        var inputUsername = component.Find("#input-login");
        var inputPassword = component.Find("#input-password");
        var buttonSubmit = component.Find("#button-submit");
        
        Assert.Equal("input-login", inputUsername.Id);
        Assert.Equal("input-password", inputPassword.Id);
        Assert.Equal("button-submit", buttonSubmit.Id);
    }

    [Fact]
    public void LoginForm_UsernameAndPasswordFieldsAreRequired()
    {
        using var ctx = new TestContext();
        
        var component = ctx.RenderComponent<LoginForm>();
        var buttonSubmit = component.Find("#button-submit");
        buttonSubmit.Click();
        var invalidMessages = component.FindAll(".validation-message");
        var inputUsername = component.Find("#input-login");
        var inputPassword = component.Find("#input-password");
        
        Assert.NotNull(invalidMessages.FirstOrDefault(x => x.TextContent == "Username is required"));
        Assert.NotNull(invalidMessages.FirstOrDefault(x => x.TextContent == "Password is required"));
        Assert.NotNull(inputUsername.ClassList.FirstOrDefault(x => x == "invalid"));
        Assert.NotNull(inputPassword.ClassList.FirstOrDefault(x => x == "invalid"));
    }
    
    [Fact]
    public void LoginForm_SubmitButtonHaveSpinnerSpanAfterClicking()
    {
        using var ctx = new TestContext();
        ctx.AddTestAuthorization();
        
        var component = ctx.RenderComponent<LoginForm>();
        var buttonSubmit = component.Find("#button-submit");
        var inputUsername = component.Find("#input-login");
        var inputPassword = component.Find("#input-password");
        inputUsername.Change("test");
        inputPassword.Change("test");
        buttonSubmit.Click();
        var spinner = component.Find(".spinner-border");
        
        Assert.Equal("SPAN", spinner.TagName);
    }
}