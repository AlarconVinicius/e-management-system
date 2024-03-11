using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class ExpiryCardDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
            return false;

        var monthYear = value.ToString()!.Split('/');
        if (monthYear.Length != 2)
            return false;

        var monthStr = monthYear[0];
        var yearStr = $"20{monthYear[1]}";

        if (int.TryParse(monthStr, out var month) &&
            int.TryParse(yearStr, out var year))
        {
            try
            {
                var expiryDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                return expiryDate > DateTime.UtcNow;
            }
            catch (ArgumentOutOfRangeException)
            {
                // Handle invalid date (e.g., February 30)
                return false;
            }
        }

        return false;
    }
}