using Ltly.CLI.Models;
using Newtonsoft.Json;
using Shared.Kernel.Constants;
using Shared.Kernel.Primitives;

namespace Ltly.CLI.Services;
public sealed class UrlShortenerService
{
    private const string apiShortenPath = $"{GlobalSettings.Domain}/api/v1/shorten?url=";

    public static async Task<Result<UrlPair>?> GetShortenedUrlAsync(string url)
    {
        using var client = new HttpClient();
        var path = new Uri($"{apiShortenPath}{Uri.EscapeDataString(url)}");

        try
        {
            var response = await client.PostAsync(path, null);

            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Result<UrlPair>>(responseBody);
        }
        catch (Exception e)
        {
            return null;
        }
    }
}