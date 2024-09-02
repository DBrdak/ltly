using Amazon.Lambda.Core;

namespace Ltly.Lambda;

public sealed class LoggingService
{
    private ILambdaLogger? _logger;

    public void InitializeLogger(ILambdaLogger logger)
    {
        _logger = logger;
    }

    public void Log(string message) => _logger?.Log(message);
}