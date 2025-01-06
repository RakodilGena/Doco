namespace Doco.Server.Gateway.Options;

internal sealed class PostgreSqlConnectionConfig
{
    public const string SectionName = "PostgreSqlConnection";

    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string Database { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int CommandTimeout { get; set; }
}