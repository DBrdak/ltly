using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Runtime.Internal;
using Ltly.Lambda.Domain.Urls;
using Ltly.Lambda.Functions.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Kernel.Primitives;

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
    [HttpApi(LambdaHttpMethod.Post, "/s")]
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

    [LambdaFunction(ResourceName = nameof(Redirect))]
    [HttpApi(LambdaHttpMethod.Get, "/s/{token}")]
    public async Task<APIGatewayHttpApiV2ProxyResponse> Redirect(
        string token,
        APIGatewayHttpApiV2ProxyRequest requestContext,
        ILambdaContext lambdaContext)
    {
        _loggingService.InitializeLogger(lambdaContext.Logger);
        _requestContextAccessor.InitializeAccessor(requestContext, lambdaContext);

        var shortenedUrlGetResult = Url.GetShortenedUrlFromToken(token);

        if (shortenedUrlGetResult.IsFailure)
        {
            _loggingService.Log($"Client provided invalid token: {token}");
            return shortenedUrlGetResult.ReturnAPIRedirectResponse();
        }

        var shoretenedUrl = shortenedUrlGetResult.Value;

        _loggingService.Log($"Client is trying to access {shoretenedUrl}");

        var originalUrlGetResult = await _urlRepository.GetOriginalUrlAsync(shoretenedUrl);

        switch (originalUrlGetResult)
        {
            case { IsFailure: true }:
                _loggingService.Log($"Shortened URL: {shoretenedUrl} not found");
                break;
            case { IsSuccess: true }:
                _loggingService.Log($"Redirecting from {shoretenedUrl} to {originalUrlGetResult.Value}");
                break;
        }

        return originalUrlGetResult.ReturnAPIRedirectResponse();
    }

    [LambdaFunction(ResourceName = nameof(RemoveOldUrls))]
    public async Task RemoveOldUrls(ILambdaContext lambdaContext)
    {
        _loggingService.InitializeLogger(lambdaContext.Logger);

        _loggingService.Log("Starting old URLs removal");

        try
        {
            var result = await _urlRepository.RemoveOldUrls();

            switch (result)
            {
                case { IsFailure: true }:
                    _loggingService.Log("Failed to remove old URLs");
                    return;
                case { IsSuccess: true }:
                    _loggingService.Log($"Successfully removed {result.Value} old URLs");
                    return;
            }
        }
        catch (Exception e)
        {
            _loggingService.LogError($"Failed to remove old URLs:\n{JsonConvert.SerializeObject(e)}");
        }
    }
}