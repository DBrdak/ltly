using Amazon.DynamoDBv2.DocumentModel;

namespace Ltly.Lambda.Data.Repositories;

public abstract class Repository<TEntity>
{
    protected readonly Table Table;

    protected Repository(DbContext context)
    {
        Table = context.Set<TEntity>();
    }
}