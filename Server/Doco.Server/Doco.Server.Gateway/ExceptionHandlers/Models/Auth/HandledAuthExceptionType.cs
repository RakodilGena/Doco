namespace Doco.Server.Gateway.ExceptionHandlers.Models.Auth;

/// <summary>
/// 
/// </summary>
public enum HandledAuthExceptionType
{
    /// <summary>
    /// 
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// 
    /// </summary>
    AccessRestricted = 1,
    
    /// <summary>
    /// 
    /// </summary>
    InvalidCredentials = 2,
    
    /// <summary>
    /// 
    /// </summary>
    UnableToRefreshSession = 3
}