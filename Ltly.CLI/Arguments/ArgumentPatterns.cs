using System.Text.RegularExpressions;

namespace Ltly.CLI.Arguments;

public static class ArgumentPatterns
{
    public static Regex Help = new("help");
    public static Regex Url = new(
        @"^(https?:\/\/)?(www\.)?([a-zA-Z0-9\p{L}\p{M}-]+\.)+[a-zA-Z\p{L}]{2,}((\/[\w\p{L}\p{M}\-.~:\/?#\[\]@!$&'()*+,;=%]*)?)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    private static readonly IReadOnlyCollection<Regex> All = [Help, Url];

    public static bool DoesMatchAny(string arg) => All.Any(pattern => pattern.IsMatch(arg));
}