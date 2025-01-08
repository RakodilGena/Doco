using Doco.Server.Gateway.Dal.Config;
using Npgsql;

namespace Doco.Server.Gateway.Dal.Extensions;

public static class GatewayDbCreator
{
    public static void EnsureDatabaseCreated(
        PostgreSqlConnectionConfig config)
    {
        var dbName = config.Database;
        var connectionString = BuildMasterConnectionString(config);

        using NpgsqlConnection conn = new NpgsqlConnection(connectionString);

        string sql = $"SELECT DATNAME FROM pg_catalog.pg_database WHERE DATNAME = '{dbName}'";
        using var command = new NpgsqlCommand(sql, conn);

        conn.Open();
        var result = command.ExecuteScalar();
        if (result is not null)
        {
            //always 'true' (if it exists) or 'null' (if it doesn't)
            var exDbname = result.ToString();
            if (exDbname is not null && exDbname.Equals(dbName))
            {
                Console.WriteLine($"database \"{dbName}\" successfully located.");
                return;
            }
        }

        Console.WriteLine("No database found, creating...");
        CreateDatabase(
            conn,
            dbName,
            userName: config.Username);

        conn.Close();
    }

    private static string BuildMasterConnectionString(PostgreSqlConnectionConfig config)
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = config.Host,
            Port = config.Port,
            Username = config.Username,
            Password = config.Password
        };

        return connectionStringBuilder.ToString();
    }

    private static void CreateDatabase(
        NpgsqlConnection conn,
        string dbName,
        string userName)
    {
        var sql = $"CREATE DATABASE {dbName} WITH OWNER = {userName} ENCODING = 'UTF8' CONNECTION LIMIT = -1";
        using var cmd = new NpgsqlCommand(sql, conn);

        cmd.ExecuteNonQuery();
        Console.WriteLine($"database \"{dbName}\" with owner \"{userName}\" created.");
    }
}