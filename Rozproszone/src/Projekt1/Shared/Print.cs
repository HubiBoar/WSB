using System.Text;

namespace Shared;

public static class Print
{
    public static void Input()
    {
        Console.WriteLine();
        Console.WriteLine("<--[Input]-->");
        Console.WriteLine();
    }

    public static void Output(StringBuilder builder)
    {
        Console.WriteLine();
        Console.WriteLine("<--[Output]-->");
        Console.WriteLine();
        Console.WriteLine(builder);
    }

    public static void Output(string value)
    {
        Output(new StringBuilder(value));
    }

    public static void Line()
    {
        Console.WriteLine();
    }

    public static void Separator()
    {
        Console.WriteLine("----------------------------------------------------------------------------");
    }

    public static void Line(string value)
    {
        Console.WriteLine(value);
    }

    public static T ReadValue<T>(string value)
    {
        Console.WriteLine(value);
        return ReadValue<T>();
    }

    public static T ReadValue<T>()
    {
        return (T)Convert.ChangeType(Console.ReadLine(), typeof(T))!;
    }
}