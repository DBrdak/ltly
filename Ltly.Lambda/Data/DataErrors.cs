using Ltly.Lambda.Domain.Primitives;

namespace Ltly.Lambda.Data;

public sealed class DataErrors<TEntity>
{
    public static readonly Error AddError = new(
        $"Problem while adding {typeof(TEntity).Name} to the database");
    public static readonly Error RemoveError = new(
        $"Problem while removing {typeof(TEntity).Name} to the database");
    public static readonly Error UpdateError = new(
        $"Problem while updating {typeof(TEntity).Name} to the database");
    public static readonly Error GetError = new(
        $"Problem while getting {typeof(TEntity).Name}");
    public static readonly Error NotFound = new(
        $"{typeof(TEntity).Name} not found");
    public static readonly Error TransactionError = new(
        $"Transaction commit failure");
    public static readonly Error MustBeUnique = new(
        $"Record must be unique");
}