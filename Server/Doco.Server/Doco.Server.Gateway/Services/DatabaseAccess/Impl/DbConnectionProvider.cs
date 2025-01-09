using Doco.Server.Gateway.Dal.Config;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Doco.Server.Gateway.Services.DatabaseAccess.Impl;

internal sealed class DbConnectionProvider : IDbConnectionProvider
{
    private readonly string _connectionString;

    public DbConnectionProvider(IOptions<PostgreSqlConnectionConfig> configOptions)
    {
        var config = configOptions.Value;

        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = config.Host,
            Port = config.Port,
            Database = config.Database,
            Username = config.Username,
            Password = config.Password,
            CommandTimeout = config.CommandTimeout
        };

        _connectionString = connectionStringBuilder.ToString();
    }

    public NpgsqlConnection GetConnection() => new(_connectionString);
}