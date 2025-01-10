using Doco.Server.Gateway.Authentication.Options;

namespace Doco.Server.Gateway.Endpoints.Minimal.Users;

internal static partial class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this RouteGroupBuilder app)
    {
        var group = app
            .MapGroup("users")
            .RequireAuthorization(JwtAuthConfig.PolicyName);

        group
            .MapGetUsers()
            .MapCreateUser();

        return app;
    }
}