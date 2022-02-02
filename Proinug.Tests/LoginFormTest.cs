using System;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Proinug.WebUI.Components;
using Proinug.WebUI.Interfaces;
using Proinug.WebUI.Models;
using Xunit;

namespace Proinug.Tests;

public class LoginFormTest : IDisposable
{
    private readonly TestContext _ctx;
    private readonly Mock<ICwAuthenticationStateProvider> _moqAuth;

    public LoginFormTest()
    {
        _ctx = new TestContext();
        _moqAuth = new Mock<ICwAuthenticationStateProvider>();
        _ctx.Services.AddSingleton<ICwAuthenticationStateProvider>(_moqAuth.Object);
        _ctx.Services.AddSingleton<AuthenticationStateProvider>(c =>
            (AuthenticationStateProvider) c.GetService<ICwAuthenticationStateProvider>()!);
    }

    public void Dispose()
    {
        _ctx.Dispose();
    }
    
    [Fact]
    public void LoginForm_ContainsUsernamePasswordSubmitInputs()
    {
        var component = _ctx.RenderComponent<LoginForm>();
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
        var component = _ctx.RenderComponent<LoginForm>();
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
        _moqAuth.Setup(s => s.LoginAsync(It.IsAny<Credentials>()))
            .Returns(
                async () =>
                {
                    await Task.Delay(10);
                    return (0, null);
                });

        var component = _ctx.RenderComponent<LoginForm>();
        
        var buttonSubmit = component.Find("#button-submit");
        var inputUsername = component.Find("#input-login");
        var inputPassword = component.Find("#input-password");
        inputUsername.Change("test");
        inputPassword.Change("test");
        buttonSubmit.Click();
        var spinner = component.Find(".spinner-border");
        
        Assert.Equal("SPAN", spinner.TagName);
    }

    [Fact]
    public void LoginForm_ShowAlertWrongUsernameOrPasswordIfServerReturn401()
    {
        _moqAuth.Setup(s => s.LoginAsync(It.IsAny<Credentials>()))
            .Returns(Task.FromResult<(int, AuthenticationState?)>((401, null)));

        var component = _ctx.RenderComponent<LoginForm>();
        
        var buttonSubmit = component.Find("#button-submit");
        var inputUsername = component.Find("#input-login");
        var inputPassword = component.Find("#input-password");
        inputUsername.Change("test");
        inputPassword.Change("test");
        buttonSubmit.Click();
        var errorMessageDiv = component.Find("#alert-error-message");
        var errorCodeDiv = component.Find("#alert-error-code");
        
        Assert.Contains("Wrong username or password", errorMessageDiv.TextContent);
        Assert.Contains("401", errorCodeDiv.TextContent);
    }
    
    [Fact]
    public void LoginForm_ShowAlertSomethingWentWrongIfServerError()
    {
        _moqAuth.Setup(s => s.LoginAsync(It.IsAny<Credentials>()))
            .Returns(Task.FromResult<(int, AuthenticationState?)>((1000, null)));

        var component = _ctx.RenderComponent<LoginForm>();
        
        var buttonSubmit = component.Find("#button-submit");
        var inputUsername = component.Find("#input-login");
        var inputPassword = component.Find("#input-password");
        inputUsername.Change("test");
        inputPassword.Change("test");
        buttonSubmit.Click();
        var errorMessageDiv = component.Find("#alert-error-message");
        var errorCodeDiv = component.Find("#alert-error-code");
        
        Assert.Contains("Something went wrong while login.", errorMessageDiv.TextContent);
        Assert.Contains("1000", errorCodeDiv.TextContent);
    }
}