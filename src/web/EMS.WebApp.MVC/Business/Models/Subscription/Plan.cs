using System.Text.Json.Serialization;

namespace EMS.WebApp.MVC.Business.Models.Subscription;

public class Plan : Entity
{
    public string Title { get; set; } = string.Empty;
    public string SubTitle { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Benefits { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    [JsonIgnore]
    public ICollection<PlanSubscriber> PlanSubscribers { get; set; } = new List<PlanSubscriber>();

    public Plan() { }
}