using Ltly.CLI.Models;
using Shared.Kernel.Primitives;

namespace Ltly.CLI.Messages;

public static class Messages
{
    public static string InvalidArguments = 
        """
        Specified invalid argument, see all available arguments by typing help
        """;

    public static string HelpMessage =
        """
        Currently available arguments:
            -> <url> - valid url that matches this regex: ^(https?:\/\/)?(www\.)?([a-zA-Z0-9\p{L}\p{M}-]+\.)+[a-zA-Z\p{L}]{2,}((\/[\w\p{L}\p{M}\-.~:\/?#\[\]@!$&'()*+,;=%]*)?)$. Invokes a function to shorten the URL. In result, returns both original and shortened URL.
            -> help - bro you're here :)
        """;

    public static string ShorteningResponse(UrlPair urlPair) =>
        $"""
        Your shortened URL is ready for use:
            -> Long URL: {urlPair.OriginalValue}
            -> Short URL: {urlPair.ShortenedValue}
        """;

    public static string ShorteningErrorResponse(Error error) =>
        $"""
        Error has occured when shortening the URL, if you believe that the URL is valid and the error persist after many tries, please contact me on GitHub or raise an Issue: https://github.com/DBrdak/ltly.
        Error message: {error.Message}
        """;

    public static string ShorteningUnknownError =>
        $"""
        Unknown error has occured when shortening the URL, if you believe that the URL is valid and the error persist after many tries, please contact me on GitHub or raise an Issue: https://github.com/DBrdak/ltly
        """;
}