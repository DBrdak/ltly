using Shared.Kernel.Primitives;

namespace Ltly.Lambda.Domain.Urls;

internal class UrlErrors
{
    public static readonly Error InvalidUrlError = new("Provided invalid URL");
    public static readonly Error InvalidTokenError = new("Provided invalid token");
}