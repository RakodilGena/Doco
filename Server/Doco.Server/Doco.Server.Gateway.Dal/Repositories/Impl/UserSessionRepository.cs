using System.Diagnostics;
using Dapper;
using Doco.Server.Gateway.Dal.Models.Sessions;
using Doco.Server.Gateway.Dal.Services.DatabaseAccess;
using USD = Doco.Server.Gateway.Dal.Descriptions.Users.UserSessionDbDescription;
using UD = Doco.Server.Gateway.Dal.Descriptions.Users.UserDbDescription;

namespace Doco.Server.Gateway.Dal.Repositories.Impl;

internal sealed class UserSessionRepository : IUserSessionRepository
{
    private readonly IDbConnectionProvider _connectionProvider;

    public UserSessionRepository(IDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task AddUserSessionAsync(
        UserSessionToCreate session,
        CancellationToken ct)
    {
        const string sql =
            $"""
             INSERT INTO {USD.Table}
                ({USD.UserId}, {USD.JwtToken}, {USD.RefreshToken}, {USD.RefreshTokenExpiresAt})
             VALUES
                (@userId, @jwtToken, @refreshToken, @refreshTokenExpiresAt)
             """;

        await using var connection = _connectionProvider.GetConnection();

        await connection.OpenAsync(ct);

        var cmd = new CommandDefinition(
            sql,
            parameters: new
            {
                session.UserId,
                session.JwtToken,
                session.RefreshToken,
                session.RefreshTokenExpiresAt
            },
            cancellationToken: ct);

        await connection.ExecuteAsync(cmd);

        await connection.CloseAsync();
    }

    public async Task<bool> UserSessionIsValidAsync(
        Guid userId,
        string jwtToken)
    {
        const string sql =
            $"""
             SELECT EXISTS(
                SELECT 1 FROM 
                {UD.Table} as u JOIN {USD.Table} as us ON u.{UD.Id} = us.{USD.UserId}
                WHERE u.{UD.Id} = @userId 
                  AND u.{UD.DeletedAt} IS NULL
                  AND us.{USD.JwtToken} = @jwtToken)
             """;

        await using var connection = _connectionProvider.GetConnection();
        var cmd = new CommandDefinition(sql,
            parameters: new
            {
                userId,
                jwtToken
            });

        await connection.OpenAsync();
        var result = await connection.ExecuteScalarAsync<bool>(cmd);
        await connection.CloseAsync();

        return result;
    }

    public async Task<bool> UserCanRefreshSessionAsync(
        Guid userId, 
        string refreshToken,
        CancellationToken ct)
    {
        const string sql =
            $"""
             SELECT EXISTS(
                SELECT 1 FROM 
                {UD.Table} as u JOIN {USD.Table} as us ON u.{UD.Id} = us.{USD.UserId}
                WHERE us.{USD.UserId} = @userId 
                  AND u.{UD.DeletedAt} IS NULL
                  AND us.{USD.RefreshToken} = @refreshToken
                  AND us.{USD.RefreshTokenExpiresAt} > @utcNow)
             """;

        await using var connection = _connectionProvider.GetConnection();

        var utcNow = DateTime.UtcNow;
        var cmd = new CommandDefinition(sql,
            parameters: new
            {
                userId,
                refreshToken,
                utcNow
            },
            cancellationToken: ct);

        await connection.OpenAsync(ct);
        var result = await connection.ExecuteScalarAsync<bool>(cmd);
        await connection.CloseAsync();

        return result;
    }

    public async Task RefreshUserSessionAsync(
        UserSessionToRefresh session, 
        CancellationToken ct)
    {
        const string sql = 
            $"""
             UPDATE {USD.Table}
             SET {USD.JwtToken} = @newJwtToken,
                 {USD.RefreshToken} = @newRefreshToken,
                 {USD.RefreshTokenExpiresAt} = @newRefreshTokenExpiresAt
             
             WHERE {USD.UserId} = @userId AND {USD.RefreshToken} = @oldRefreshToken
             """;
        
        await using var connection = _connectionProvider.GetConnection();

        await connection.OpenAsync(ct);

        var cmd = new CommandDefinition(
            sql,
            parameters: new
            {
                session.NewJwtToken,
                session.NewRefreshToken,
                session.NewRefreshTokenExpiresAt,
                
                session.UserId,
                session.OldRefreshToken
            },
            cancellationToken: ct);

        var rows = await connection.ExecuteAsync(cmd);
        Debug.Assert(rows is 1);

        await connection.CloseAsync();
    }

    public async Task RemoveSessionAsync(Guid userId, string jwtToken)
    {
        //user is authenticated, session is active
        const string sql = 
            $"""
             DELETE FROM {USD.Table}
             WHERE {USD.UserId} = @userId AND {USD.JwtToken} = @jwtToken
             """;
        
        await using var connection = _connectionProvider.GetConnection();

        await connection.OpenAsync();

        var cmd = new CommandDefinition(
            sql,
            parameters: new
            {
                userId,
                jwtToken
            });

        var rows = await connection.ExecuteAsync(cmd);
        Debug.Assert(rows is 1);

        await connection.CloseAsync();
    }
}