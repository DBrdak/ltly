using System.Diagnostics.CodeAnalysis;
using Amazon.Lambda.Core;

namespace Ltly.Lambda;

public sealed class LoggingService
{
    public ILambdaLogger? Logger { get; private set; }

    private LoggingService()
    {
        Logger = null;
    }

    public void InitializeLogger(ILambdaLogger logger)
    {
        Logger = logger;
    }

    public void Log(string message) => Logger?.Log(message);
}