﻿using Dapper;
using Doco.Server.Core;
using Doco.Server.Gateway.Dal.Exceptions.Users;
using Doco.Server.Gateway.Dal.Models.Users;
using Doco.Server.Gateway.Dal.Services.DatabaseAccess;
using UD = Doco.Server.Gateway.Dal.Descriptions.Users.UserDbDescription;

namespace Doco.Server.Gateway.Dal.Repositories.Impl;

internal sealed class UserRepository : IUserRepository
{
    private readonly IDbConnectionProvider _connectionProvider;

    public UserRepository(IDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<UserToAuth?> GetAuthUserAsync(
        string email,
        CancellationToken cancellationToken)
    {
        const string sql =
            $"""
              SELECT {UD.Id} as Id, 
              {UD.HashedPassword} as HashedPassword, 
              {UD.HashPasswordSalt} as HashPasswordSalt, 
              {UD.DeletedAt} as DeletedAt 
              FROM {UD.Table} WHERE {UD.Email} = @email
             """;

        await using var connection = _connectionProvider.GetConnection();
        var cmd = new CommandDefinition(
            sql,
            parameters: new { email },
            cancellationToken: cancellationToken);

        await connection.OpenAsync(cancellationToken);
        var result = await connection.QueryFirstOrDefaultAsync<UserToAuth>(cmd);
        await connection.CloseAsync();

        return result;
    }

    public async Task CreateUserAsync(
        UserToCreate user,
        CancellationToken cancellationToken)
    {
        try
        {
            const string sql =
                $"""
                 INSERT INTO {UD.Table} 
                     ({UD.Id}, {UD.Name}, {UD.Email}, {UD.HashedPassword}, {UD.HashPasswordSalt}, {UD.IsAdmin})
                 VALUES 
                     (@id, @name, @email, @hashedPassword, @hashPasswordSalt, @isAdmin);
                 """;

            await using var connection = _connectionProvider.GetConnection();

            await connection.OpenAsync(cancellationToken);

            var cmd = new CommandDefinition(
                sql,
                parameters: new
                {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.HashedPassword,
                    user.HashPasswordSalt,
                    user.IsAdmin
                },
                cancellationToken: cancellationToken);

            await connection.ExecuteAsync(cmd);

            await connection.CloseAsync();
        }
        catch (Npgsql.PostgresException e)
        {
            if (e.SqlState is not PgSqlExceptionStates.NotUnique)
                throw;

            switch (e.ConstraintName)
            {
                case UD.IdxEmail:
                    throw new DbUserEmailNotUniqueException(
                        "user email is not unique");

                case UD.ConstrName:
                    throw new DbUserNameNotUniqueException(
                        "user name is not unique");
            }
        }
    }

    public async Task<IEnumerable<UserDto>> GetUsersAsync(CancellationToken cancellationToken)
    {
        const string sql =
            $"""
              SELECT {UD.Id} as Id, 
              {UD.Name} as Name, 
              {UD.Email} as Email, 
              {UD.IsAdmin} as IsAdmin , 
              {UD.CreatedAt} as CreatedAt , 
              {UD.DeletedAt} as DeletedAt 
              FROM {UD.Table}
              ORDER BY CreatedAt DESC
             """;

        await using var connection = _connectionProvider.GetConnection();
        var cmd = new CommandDefinition(sql, cancellationToken: cancellationToken);

        await connection.OpenAsync(cancellationToken);
        var result = await connection.QueryAsync<UserDto>(cmd);
        await connection.CloseAsync();

        return result;
    }
}