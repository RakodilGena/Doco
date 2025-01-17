﻿using Doco.Server.Gateway.Exceptions.Auth;
using Doco.Server.Gateway.Models.Dtos.Auth;
using Doco.Server.Gateway.Models.Responses.Auth;

namespace Doco.Server.Gateway.Services.Auth;

internal interface ILoginUserService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="InvalidLoginCredentialsException"></exception>
    /// <exception cref="AccountAccessRestrictedException"></exception>
    public Task<LoginUserResult> LoginUserAsync(
        LoginUserRequestDto request, 
        CancellationToken ct);
}