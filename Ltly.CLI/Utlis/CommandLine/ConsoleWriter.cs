namespace Ltly.CLI.Utlis.CommandLine;

public static class ConsoleWriter
{
    public static void PrintLine(string message) => Console.WriteLine(message);

    public static void PrintLine(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    public static void Print(string message) => Console.Write(message);

    public static void Print(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(message);
        Console.ResetColor();
    }
}