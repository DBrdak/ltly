using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Ltly.Lambda;

public sealed class RequestContextAccessor
{
    public APIGatewayHttpApiV2ProxyRequest? RequestContext { get; private set; }
    public ILambdaContext? LambdaContext { get; private set; }

    public void InitializeAccessor(
        APIGatewayHttpApiV2ProxyRequest requestContext,
        ILambdaContext lambdaContext)
    {
        RequestContext = requestContext;
        LambdaContext = lambdaContext;
    }
}