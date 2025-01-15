using Npgsql;

namespace Doco.Server.FileService.Dal.Services.DatabaseAccess;

internal interface IDbConnectionProvider
{
    NpgsqlConnection GetConnection();
}