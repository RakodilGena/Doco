namespace Doco.Server.Gateway.Endpoints.Minimal.Users;

internal static partial class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("users");

        group
            .MapGetUsers()
            .MapCreateUser();

        return app;
    }
}