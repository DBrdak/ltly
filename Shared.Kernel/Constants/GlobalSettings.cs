using System.Text.RegularExpressions;

namespace Shared.Kernel.Constants;

public static class GlobalSettings
{
    public const string Domain = "https://ltly.link";
    public static readonly Regex UrlPattern = new(
        @"^(https?:\/\/)?(www\.)?([a-zA-Z0-9\p{L}\p{M}-]+\.)+[a-zA-Z\p{L}]{2,}((\/[\w\p{L}\p{M}\-.~:\/?#\[\]@!$&'()*+,;=%]*)?)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
}