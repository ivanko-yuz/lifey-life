using System.Security.Claims;
using LifeyLife.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LifeyLife.Tests;

/// <summary>Shared helpers for controller unit tests.</summary>
internal static class ControllerHelper
{
    /// <summary>Controller context with an authenticated user claim set to <paramref name="userId"/>.</summary>
    internal static ControllerContext AuthContext(Guid userId)
    {
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);
        return new ControllerContext { HttpContext = new DefaultHttpContext { User = principal } };
    }

    /// <summary>Controller context with no user (anonymous / unauthenticated).</summary>
    internal static ControllerContext AnonContext() =>
        new() { HttpContext = new DefaultHttpContext() };

    /// <summary>
    /// Creates a real <see cref="JwtHandler"/> backed by an in-memory configuration
    /// with dummy-but-valid JWT settings (32-char key, 60-minute expiry).
    /// </summary>
    internal static JwtHandler CreateJwtHandler()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JwtSettings:securityKey"]     = "super-secret-test-key-min-32-chars",
                ["JwtSettings:validIssuer"]     = "TestIssuer",
                ["JwtSettings:validAudience"]   = "TestAudience",
                ["JwtSettings:expiryInMinutes"] = "60"
            })
            .Build();

        return new JwtHandler(config);
    }
}
