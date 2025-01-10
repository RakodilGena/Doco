using Doco.Server.Gateway.Authentication.Services;
using Doco.Server.Gateway.Models.Domain.Auth;
using Doco.Server.Gateway.Models.Responses.Auth;
using Doco.Server.Gateway.Services.Repositories;
using Doco.Server.PasswordEncryption;

namespace Doco.Server.Gateway.Services.Auth.Impl;

internal sealed class LoginUserService : ILoginUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJWTokenCreator _tokenCreator;

    public LoginUserService(
        IUserRepository userRepository, 
        IJWTokenCreator tokenCreator)
    {
        _userRepository = userRepository;
        _tokenCreator = tokenCreator;
    }

    public async Task<LoginUserResult> LoginUserAsync(
        LoginUserRequestDto request, 
        CancellationToken ct)
    {
        var user = await _userRepository.GetAuthUserAsync(request.Email, ct);
        
        //todo: throw proper exceptions methodwise
        
        if (user == null)
            throw new Exception("Invalid username or password");
        
        var passwordValid = PasswordEncryptor.ComparePasswords(
            request.Password,
            user.HashedPassword,
            user.HashPasswordSalt);
        
        if (passwordValid is false)
            throw new Exception("Invalid username or password");
        
        if (user.DeletedAt.HasValue)
            throw new Exception("Your account has been deleted");

        var token = _tokenCreator.CreateToken(user.Id);
        return new LoginUserResult(token);
    }
}