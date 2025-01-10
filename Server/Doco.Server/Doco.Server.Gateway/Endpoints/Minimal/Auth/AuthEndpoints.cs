using Doco.Server.Gateway.Authentication.Options;

namespace Doco.Server.Gateway.Endpoints.Minimal.Auth;

internal static partial class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder app)
    {
        var group = app
            .MapGroup("auth")
            .RequireAuthorization(JwtAuthConfig.PolicyName);

        group
            .MapLogin();

        return app;
    }
}