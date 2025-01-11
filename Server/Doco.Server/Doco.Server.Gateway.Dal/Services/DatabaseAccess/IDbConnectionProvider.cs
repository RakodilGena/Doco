using Npgsql;

namespace Doco.Server.Gateway.Dal.Services.DatabaseAccess;

internal interface IDbConnectionProvider
{
    NpgsqlConnection GetConnection();
}