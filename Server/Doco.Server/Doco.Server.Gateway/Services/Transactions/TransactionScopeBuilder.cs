using System.Transactions;

namespace Doco.Server.Gateway.Services.Transactions;

internal static class TransactionScopeBuilder
{
    public static TransactionScope Build(
        IsolationLevel isolationLevel,
        TimeSpan timeout)
    {
        var options = new TransactionOptions
        {
            IsolationLevel = isolationLevel,
            Timeout = timeout
        };

        return new TransactionScope(
            TransactionScopeOption.Required,
            options,
            TransactionScopeAsyncFlowOption.Enabled);
    }
}