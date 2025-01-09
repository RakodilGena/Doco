using Npgsql;

namespace Doco.Server.Gateway.Services.DatabaseAccess;

internal interface IDbConnectionProvider
{
    NpgsqlConnection GetConnection();
}