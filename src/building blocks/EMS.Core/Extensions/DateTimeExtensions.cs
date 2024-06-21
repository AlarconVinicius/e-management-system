using System.Globalization;

namespace EMS.Core.Extensions;

public static class DateTimeExtensions
{
    public static DateTime ToDateTime(this string value, string format = "dd/MM/yyyy HH:mm")
    {
        if (!DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
        {
            throw new FormatException($"The DateTime field must be in the format {format}.");
        }
        return result;
    }

    public static string ToFormattedString(this DateTime value, string format = "dd/MM/yyyy HH:mm")
    {
        return value.ToString(format, CultureInfo.InvariantCulture);
    }
}