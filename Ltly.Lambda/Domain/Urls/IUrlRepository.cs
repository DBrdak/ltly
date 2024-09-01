namespace Ltly.Lambda.Domain.Urls;

public interface IUrlRepository
{
    Task<Url> GetOrAddAsync(Url url, CancellationToken cancellationToken = default);
}