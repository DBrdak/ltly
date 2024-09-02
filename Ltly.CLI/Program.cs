using Ltly.CLI.Arguments;
using Ltly.CLI.Models;
using Ltly.CLI.Services;
using Shared.Kernel.Primitives;
using static Ltly.CLI.Messages.Messages;
using static Ltly.CLI.Utlis.CommandLine.ConsoleWriter;

namespace Ltly.CLI;

internal class Program
{
    public static async Task Main(string[] args)
    {
        //TODO adjust on prod
        args = [Console.ReadLine()];
        switch (args)
        {
            case var _ when args.Length != 1 || !ArgumentPatterns.DoesMatchAny(args[0]):
                PrintLine(InvalidArguments, ConsoleColor.Red);
                break;
            case var _ when ArgumentPatterns.Help.IsMatch(args[0]):
                PrintLine(HelpMessage, ConsoleColor.Blue);
                break;
            case var _ when ArgumentPatterns.Url.IsMatch(args[0]):
                var urlPairGetResult = await UrlShortenerService.GetShortenedUrlAsync(args[0]);
                DisplayShorteningResult(urlPairGetResult);
                break;
            default:
                PrintLine(InvalidArguments, ConsoleColor.Red);
                break;
        }
    }

    private static void DisplayShorteningResult(Result<UrlPair>? result)
    {
        switch (result)
        {
            case null:
                PrintLine(ShorteningUnknownError);
                break;
            case { IsFailure: true }:
                PrintLine(ShorteningErrorResponse(result.Error));
                break;
            default: 
                PrintLine(ShorteningResponse(result.Value));
                break;
        }
    }
}