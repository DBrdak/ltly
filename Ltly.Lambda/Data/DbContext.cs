using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;

namespace Ltly.Lambda.Data;

public sealed class DbContext
{
    private readonly AmazonDynamoDBClient _client = new();

    private readonly AmazonDynamoDBException _connectionException =
        new("Could not connect to DynamoDB");
    private AmazonDynamoDBException InvalidTableException(string typeName) =>
        new($"Table for {typeName} does not exist");

    private Table Urls => Table.TryLoadTable(_client, nameof(Urls), out var table) ?
        table : throw _connectionException;

    public Table Set<TEntity>() =>
        typeof(TEntity) switch
        {
            { Name: "Url" } => Urls,
            var type => throw InvalidTableException(type.Name)
        };
}