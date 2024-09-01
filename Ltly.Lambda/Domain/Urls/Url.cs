using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ltly.Lambda.Domain.Constants;
using Ltly.Lambda.Domain.Primitives;

namespace Ltly.Lambda.Domain.Urls;

public sealed class Url
{
    public string OriginalValue { get; init; }
    public string ShortenedValue { get; init; }

    private static readonly Regex _urlPattern = new(
        @"^(https?:\/\/)?(([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,})((\/[a-zA-Z0-9-._~:\/?#\[\]@!$&'()*+,;=%]*)?)$");

    [JsonConstructor]
    [Newtonsoft.Json.JsonConstructor]
    private Url(string originalValue, string shortenedValue)
    {
        OriginalValue = originalValue;
        ShortenedValue = shortenedValue;
    }

    public static Result<Url> Generate(string longUrl)
    {
        longUrl = longUrl.ToLower();
        var isValidUrl = _urlPattern.IsMatch(longUrl);

        if (isValidUrl)
        {
            return UrlErrors.InvalidUrlError;
        }

        var shortenedUrl = string.Concat(GlobalSettings.Domain, "/", GenerateToken());

        return new Url(longUrl, shortenedUrl);
    }

    private static string GenerateToken() => new Ulid().ToString().ToLower();
}