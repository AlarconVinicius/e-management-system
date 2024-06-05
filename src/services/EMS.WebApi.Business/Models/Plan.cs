namespace EMS.WebApi.Business.Models;

public class Plan : Entity
{
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public decimal Price { get; set; }
    public string Benefits { get; set; }
    public bool IsActive { get; set; }

    public ICollection<Company> Companies { get; set; } = new List<Company>();  

    public Plan() { }

    public Plan(string title, string subtitle, decimal price, string benefits, bool isActive)
    {
        Title = title;
        Subtitle = subtitle;
        Price = price;
        Benefits = benefits;
        IsActive = isActive;
    }
}