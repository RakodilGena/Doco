using Dapper;
using Doco.Server.Gateway.Dal.Models.Sessions;
using Doco.Server.Gateway.Dal.Services.DatabaseAccess;
using USD = Doco.Server.Gateway.Dal.Descriptions.Users.UserSessionDbDescription;

namespace Doco.Server.Gateway.Dal.Repositories.Impl;

internal sealed class UserSessionRepository : IUserSessionRepository
{
    private readonly IDbConnectionProvider _connectionProvider;

    public UserSessionRepository(IDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task AddUserSessionAsync(
        UserSessionToCreate userSession,
        CancellationToken ct)
    {
        const string sql =
            $"""
             INSERT INTO {USD.Table}
                ({USD.UserId}, {USD.JwtToken}, {USD.RefreshToken})
             VALUES
                (@userId, @jwtToken, @refreshToken)
             """;

        await using var connection = _connectionProvider.GetConnection();

        await connection.OpenAsync(ct);

        var cmd = new CommandDefinition(
            sql,
            parameters: new
            {
                userSession.UserId,
                userSession.JwtToken,
                userSession.RefreshToken
            },
            cancellationToken: ct);

        await connection.ExecuteAsync(cmd);

        await connection.CloseAsync();
    }
}