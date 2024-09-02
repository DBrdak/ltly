using System.Text.Json.Serialization;
using Shared.Kernel.Constants;
using Shared.Kernel.Primitives;

namespace Ltly.Lambda.Domain.Urls;

public sealed class Url
{
    public string OriginalValue { get; init; }
    public string ShortenedValue { get; init; }

    [JsonConstructor]
    [Newtonsoft.Json.JsonConstructor]
    private Url(string originalValue, string shortenedValue)
    {
        OriginalValue = originalValue;
        ShortenedValue = shortenedValue;
    }

    public static Result<Url> Generate(string longUrl)
    {
        var isInvalidUrl = !GlobalSettings.UrlPattern.IsMatch(longUrl);

        if (isInvalidUrl)
        {
            return UrlErrors.InvalidUrlError;
        }

        var shortenedUrl = string.Concat(GlobalSettings.Domain, "/", GenerateToken());

        return new Url(longUrl, shortenedUrl);
    }

    private static string GenerateToken()
    {
        var ulid = Ulid.NewUlid().ToString();

        return ulid.Substring(ulid.Length - 4, 4).ToLower();
    }

    public static Result<string> GetShortenedUrlFromToken(string token)
    {
        var isTokenValid = Ulid.TryParse(token.ToUpper(), out var ulid);

        if (!isTokenValid)
        {
            return UrlErrors.InvalidTokenError;
        }

        token = ulid.ToString().ToLower();

        return $"{GlobalSettings.Domain}/{token}";
    }
}