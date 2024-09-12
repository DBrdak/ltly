using System.Text.Json.Serialization;
using Shared.Kernel.Constants;
using Shared.Kernel.Primitives;

namespace Ltly.Lambda.Domain.Urls;

public sealed class Url
{
    public string OriginalValue { get; init; }
    public string ShortenedValue { get; private set; }
    public long ExpireOnTicks { get; init; } = DateTime.UtcNow.AddMonths(6).Ticks;
    private const int tokenLength = 4;
    private const string httpsPrefix = "https://";
    private const string httpPrefix = "http://";

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

        if (!longUrl.StartsWith(httpPrefix) && !longUrl.StartsWith(httpsPrefix))
        {
            longUrl = string.Concat(httpsPrefix, longUrl);
        }

        var shortenedUrl = string.Concat(GlobalSettings.Domain, "/", GenerateToken());

        return new Url(longUrl, shortenedUrl);
    }

    private static string GenerateToken()
    {
        var ulid = Ulid.NewUlid().ToString();

        var initToken = ulid.Substring(ulid.Length - 4, tokenLength);

        var token = string.Empty;

        for (var i = 0; i < initToken.Length; i++)
        {
            var rng = new Random().Next(tokenLength);
            token += rng == i ? 
                initToken[i].ToString().ToLower() :
                initToken[i].ToString().ToUpper();
        }

        return token;
    }

    public static Result<string> GetShortenedUrlFromToken(string token)
    {
        var isTokenValid = token.Length == tokenLength;

        if (!isTokenValid)
        {
            return UrlErrors.InvalidTokenError;
        }

        return $"{GlobalSettings.Domain}/{token}";
    }

    public void ReGenerateToken()
    {
        ShortenedValue = GenerateToken();
    }
}