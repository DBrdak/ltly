using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Ltly.Lambda.Domain.Urls;
using Newtonsoft.Json;

namespace Ltly.Lambda.Data.Repositories;

internal sealed class UrlRepository : Repository<Url>, IUrlRepository
{
    public UrlRepository(DbContext context) : base(context)
    {
    }

    public async Task<Url> GetOrAddAsync(Url url, CancellationToken cancellationToken = default)
    {
        var doc = await Table.GetItemAsync(new Primitive(url.OriginalValue), cancellationToken);

        if (doc is null)
        {
            var json = JsonConvert.SerializeObject(url);
            doc = Document.FromJson(json);

            doc = await Table.PutItemAsync(doc, cancellationToken);
        }

        return JsonConvert.DeserializeObject<Url>(doc.ToJson()) ??
               throw new InvalidOperationException("Cannot convert document to Url");
    }
}