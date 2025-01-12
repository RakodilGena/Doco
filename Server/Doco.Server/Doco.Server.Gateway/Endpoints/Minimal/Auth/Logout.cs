using Doco.Server.Gateway.Services.Auth;

namespace Doco.Server.Gateway.Endpoints.Minimal.Auth;

internal static partial class AuthEndpoints
{
    private static RouteGroupBuilder MapLogout(this RouteGroupBuilder group)
    {
        group.MapPut("/logout", Logout);
        return group;
    }

    /// <summary>
    /// Allows user to logout and remove current session.
    /// </summary>
    /// <param name="logoutUserService"></param>
    private static Task Logout(
        ILogoutUserService logoutUserService)
        => logoutUserService.LogoutAsync();
}