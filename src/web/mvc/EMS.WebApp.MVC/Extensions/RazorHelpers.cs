using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace EMS.WebApp.MVC.Extensions;

public static class RazorHelpers
{
    public static string HashEmailForGravatar(this RazorPage page, string email)
    {
        var md5Hasher = MD5.Create();
        var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(email));
        var sBuilder = new StringBuilder();
        foreach (var t in data)
        {
            sBuilder.Append(t.ToString("x2"));
        }
        return sBuilder.ToString();
    }
    public static string FormatCurrency(this RazorPage page, decimal value)
    {
        return value > 0 ? string.Format(Thread.CurrentThread.CurrentCulture, "{0:C}", value) : "Gratuito";
    }

    public static string StockMessage(this RazorPage page, int quantity)
    {
        return quantity > 0 ? $"Apenas {quantity} em estoque!" : "Produto esgotado!";
    }

    public static IEnumerable<string> SplitBenefits(this RazorPage page, string benefits)
    {
        return benefits.Split(',').ToList();
    }

    public static string FormatDate(this RazorPage page, DateTime value)
    {
        return value.ToString("dd/MM/yyyy");
    }

    public static string FormatTimeSpan(this RazorPage page, TimeSpan value)
    {
        if (value.Hours > 0)
        {
            return $"{value.Hours}h {value.Minutes}min";
        }
        return $"{value.Minutes}min";
    }

    public static string FormatTimeSpanString(this RazorPage page, string timeSpanString)
    {
        if (TimeSpan.TryParseExact(timeSpanString, @"hh\:mm\:ss", CultureInfo.InvariantCulture, out TimeSpan timeSpan))
        {
            if (timeSpan.Hours > 0)
            {
                return $"{timeSpan.Hours}h {timeSpan.Minutes}min";
            }
            return $"{timeSpan.Minutes}min";
        }
        return "Formato inválido";
    }
}