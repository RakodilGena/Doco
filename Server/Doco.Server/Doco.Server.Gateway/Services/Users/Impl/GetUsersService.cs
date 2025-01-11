using Doco.Server.Gateway.Dal.Repositories;
using Doco.Server.Gateway.Models.Responses.Users;

namespace Doco.Server.Gateway.Services.Users.Impl;

internal sealed class GetUsersService : IGetUsersService
{
    private readonly IUserRepository _userRepository;

    public GetUsersService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetUsersResultDto> GetUsersAsync(CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetUsersAsync(cancellationToken);

        return new GetUsersResultDto(users.ToArray());
    }
}