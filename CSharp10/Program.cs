using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CSharp10;

internal class Program
{
    private const string A = "A";
    private const string B = "B";
    private const string X = $"{A}{B}";

    private static void Main(string[] args)
    {
        Console.WriteLine($"Hello, World! {X}");
        PrintDate(DateTime.Now);
    }

    private static void PrintDate(DateTime date, [CallerArgumentExpression(nameof(date))] string? expression = null)
    {
        Debug.WriteLine(expression);
        Console.WriteLine(date.ToString());
    }
}