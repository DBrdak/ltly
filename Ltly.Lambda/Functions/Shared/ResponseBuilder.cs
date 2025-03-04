﻿using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shared.Kernel.Primitives;

namespace Ltly.Lambda.Functions.Shared;

public static class ResponseBuilder
{
    private static readonly DefaultContractResolver contractResolver = new()
    {
        NamingStrategy = new CamelCaseNamingStrategy
        {
            OverrideSpecifiedNames = false
        }
    };

    private static readonly JsonSerializerSettings serializerSettings =
        new()
        {
            ContractResolver = contractResolver,
            Formatting = Formatting.Indented
        };

    private static APIGatewayHttpApiV2ProxyResponse Respond(Result result, int statusCode, Dictionary<string, string>? headers = null) =>
        new()
        {
            StatusCode = statusCode,
            Headers = headers ?? new Dictionary<string, string>(),
            Body = JsonConvert.SerializeObject(result, serializerSettings)
        };

    private static APIGatewayHttpApiV2ProxyResponse Respond<T>(Result<T> result, int statusCode, Dictionary<string, string>? headers = null) =>
        new()
        {
            StatusCode = statusCode,
            Headers = headers ?? new Dictionary<string, string>(),
            Body = JsonConvert.SerializeObject(result, serializerSettings)
        };

    private static APIGatewayHttpApiV2ProxyResponse Redirect(Result<string> result) =>
        new()
        {
            StatusCode = 302,
            Headers = new Dictionary<string, string>
            {
                {
                    "Location", result.IsSuccess ?
                        result.Value :
                        string.Empty
                }
            }
        };

    public static APIGatewayHttpApiV2ProxyResponse ReturnAPIResponse(
        this Result result,
        int? successStatusCode = null,
        int? failureStatusCode = null,
        Dictionary<string, string>? headers = null)
    {
        return result.IsSuccess ?
            Respond(result, successStatusCode ?? 200, headers) :
            Respond(result, failureStatusCode ?? 400, headers);
    }

    public static APIGatewayHttpApiV2ProxyResponse ReturnAPIResponse<T>(
        this Result<T> result,
        int? successStatusCode = null,
        int? failureStatusCode = null,
        Dictionary<string, string>? headers = null)
    {
        return result.IsSuccess ?
            Respond(result, successStatusCode ?? 200, headers) :
            Respond(result, failureStatusCode ?? 400, headers);
    }

    public static APIGatewayHttpApiV2ProxyResponse ReturnAPIRedirectResponse(
        this Result<string> result) =>
        Redirect(result);
}