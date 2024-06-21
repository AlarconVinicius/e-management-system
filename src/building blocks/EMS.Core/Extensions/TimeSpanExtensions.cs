using System.Globalization;

namespace EMS.Core.Extensions;

public static class TimeSpanExtensions
{
    public static TimeSpan ToTimeSpan(this string value, string format = "hh\\:mm")
    {
        if (!TimeSpan.TryParseExact(value, format, null, out TimeSpan duration))
        {
            throw new FormatException($"The Duration field must be in the format {format}.");
        }
        return duration;
    }

    public static string ToFormattedString(this TimeSpan value, string format = "hh\\:mm")
    {
        if (format.Contains("hh") && format.Contains("mm") && !format.Contains("ss"))
        {
            return value.ToString(@"hh\:mm", CultureInfo.InvariantCulture);
        }
        else if (format.Contains("hh") && format.Contains("mm") && format.Contains("ss"))
        {
            return value.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture);
        }
        else if (format.Contains("mm") && format.Contains("ss") && !format.Contains("hh"))
        {
            return value.ToString(@"mm\:ss", CultureInfo.InvariantCulture);
        }
        else
        {
            return value.ToString();
        }
    }
}