using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Ltly.Lambda.Domain.Primitives;
using Ltly.Lambda.Domain.Urls;
using Ltly.Lambda.Functions.Shared;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Ltly.Lambda.Functions;

public sealed class UrlShortenerFunctions
{
    private readonly IUrlRepository _urlRepository;
    private readonly LoggingService _loggingService;
    private readonly RequestContextAccessor _requestContextAccessor;

    public UrlShortenerFunctions(
        IUrlRepository urlRepository,
        LoggingService loggingService,
        RequestContextAccessor requestContextAccessor)
    {
        _urlRepository = urlRepository;
        _loggingService = loggingService;
        _requestContextAccessor = requestContextAccessor;
    }

    [LambdaFunction(ResourceName = nameof(ShortenUrl))]
    [HttpApi(LambdaHttpMethod.Get, "api/v1/shorten")]
    public async Task<APIGatewayHttpApiV2ProxyResponse> ShortenUrl(
        [FromQuery]string url,
        APIGatewayHttpApiV2ProxyRequest requestContext,
        ILambdaContext lambdaContext)
    {
        _loggingService.InitializeLogger(lambdaContext.Logger);
        _requestContextAccessor.InitializeAccessor(requestContext, lambdaContext);

        _loggingService.Log($"Initialized URL shortening of: {url}");

        var urlGenerateResult = Url.Generate(url);

        if (urlGenerateResult.IsFailure)
        {
            return urlGenerateResult.ReturnAPIResponse();
        }

        var generatedUrl = urlGenerateResult.Value;

        generatedUrl = await _urlRepository.GetOrAddAsync(generatedUrl);

        _loggingService.Log(
            $"Successfully shortened the URL from: {generatedUrl.OriginalValue} to: {generatedUrl.ShortenedValue}");

        return Result.Success(generatedUrl).ReturnAPIResponse();
    }
}