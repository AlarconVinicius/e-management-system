namespace EMS.Core.Extensions;

public static class TimeSpanExtensions
{
    public static TimeSpan ToTimeSpan(this string str, string format = "hh\\:mm\\:ss")
    {
        if (!TimeSpan.TryParseExact(str, format, null, out TimeSpan duration))
        {
            throw new FormatException($"The Duration field must be in the format {format}.");
        }
        return duration;
    }
}