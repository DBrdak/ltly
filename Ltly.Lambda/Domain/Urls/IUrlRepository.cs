using Shared.Kernel.Primitives;

namespace Ltly.Lambda.Domain.Urls;

public interface IUrlRepository
{
    Task<Url> GetOrAddAsync(Url url, CancellationToken cancellationToken = default);

    Task<Result<string>> GetOriginalUrlAsync(string shoretenedUrl, CancellationToken cancellationToken = default);

    Task<Result> RemoveOldUrls(CancellationToken cancellationToken = default);
}