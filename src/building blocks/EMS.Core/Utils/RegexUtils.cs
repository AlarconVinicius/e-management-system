namespace EMS.Core.Utils;

public class RegexUtils
{
    public const string TimeSpanWithHourPattern = @"^(?:[01]\d|2[0-3]):(?:[0-5]\d):(?:[0-5]\d)$";

    public const string TimeSpanWithoutHourPattern = @"^(?:[01]\d|2[0-3]):(?:[0-5]\d)$";

    public const string DateTimeWithHourPattern = @"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4} (?:[01]\d|2[0123]):(?:[012345]\d):(?:[012345]\d)$";

    public const string DateTimeWithoutHourPattern = @"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$";
}
