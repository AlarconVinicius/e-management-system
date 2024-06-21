namespace EMS.Core.Extensions;

public static class StringExtensions
{
    public static string OnlyNumbers(this string str, string input)
    {
        return new string(input.Where(char.IsDigit).ToArray());
    }
}
