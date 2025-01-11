namespace Doco.Server.Gateway.Dal.Descriptions.Users;

public static class UserSessionDbDescription
{
    public const string Table = "user_sessions";
    
    public const string UserId = "user_id";
    
    public const string RefreshToken = "refresh_token";
    
    public const string JwtToken = "jwt_token";
}