using Amazon.DynamoDBv2.DocumentModel;
using Ltly.Lambda.Domain.Urls;
using Newtonsoft.Json;
using Shared.Kernel.Primitives;

namespace Ltly.Lambda.Data.Repositories;

internal sealed class UrlRepository : Repository<Url>, IUrlRepository
{

    public UrlRepository(DbContext context) : base(context)
    {
    }

    public async Task<Url> GetOrAddAsync(Url url, CancellationToken cancellationToken = default)
    {
        var doc = await Table.GetItemAsync(new Primitive(url.OriginalValue), cancellationToken);

        if (doc is null || !doc.Any())
        {
            await EnsureUniqueness(url, cancellationToken);
            var json = JsonConvert.SerializeObject(url);
            doc = Document.FromJson(json);

            await Table.PutItemAsync(doc, cancellationToken);

            doc = await Table.GetItemAsync(new Primitive(url.OriginalValue), cancellationToken);
        }

        return JsonConvert.DeserializeObject<Url>(doc.ToJson()) ??
               throw new InvalidOperationException("Cannot convert document to Url");
    }

    private async Task EnsureUniqueness(Url url, CancellationToken cancellationToken)
    {
        var counter = 0;

        while (counter < 15)
        {
            var existingUrl = await GetOriginalUrlAsync(url.ShortenedValue, cancellationToken);

            if (existingUrl.IsFailure)
            {
                return;
            }

            url.ReGenerateToken();

            counter++;
        }
    }

    public async Task<Result<string>> GetOriginalUrlAsync(string shoretenedUrl, CancellationToken cancellationToken = default)
    {
        var filter = new ScanFilter();
        filter.AddCondition(nameof(Url.ShortenedValue), ScanOperator.Equal, shoretenedUrl);

        var scanConfig = new ScanOperationConfig
        {
            Filter = filter,
            Select = SelectValues.AllAttributes,
            Limit = 1000
        };

        var scanner = Table.Scan(scanConfig);
        List<Document> docs = [];

        do
        {
            docs.AddRange(await scanner.GetNextSetAsync(cancellationToken));
        }
        while (!scanner.IsDone);

        var docJson = docs.SingleOrDefault()?.ToJson();

        if (string.IsNullOrWhiteSpace(docJson))
        {
            return DataErrors<Url>.NotFound;
        }

        return JsonConvert.DeserializeObject<Url>(docJson)?.OriginalValue;
    }

    public async Task<Result<int>> RemoveOldUrls(CancellationToken cancellationToken = default)
    {
        var filter = new ScanFilter();
        filter.AddCondition(nameof(Url.ExpireOnTicks), ScanOperator.LessThan, DateTime.UtcNow.Ticks);

        var scanConfig = new ScanOperationConfig
        {
            Filter = filter,
            Select = SelectValues.AllAttributes,
            Limit = 1000
        };

        var scanner = Table.Scan(scanConfig);
        List<Document> docs = [];

        do
        {
            docs.AddRange(await scanner.GetNextSetAsync(cancellationToken));
        }
        while (!scanner.IsDone);

        var docJson = docs.ToJson();
        var oldUrls = JsonConvert.DeserializeObject<List<Url>>(docJson);

        var batch = Table.CreateBatchWrite();

        oldUrls?.Select(url => url.OriginalValue).ToList().ForEach(
            originalValue => batch.AddKeyToDelete(new Primitive(originalValue)));

        await batch.ExecuteAsync(cancellationToken);

        return Result.Success(oldUrls?.Count ?? 0);
    }
}