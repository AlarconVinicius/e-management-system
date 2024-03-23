using System.Text.Json.Serialization;

namespace EMS.WebApp.MVC.Business.Models.Subscription;

public class PlanSubscriber : Entity
{
    public Guid PlanId { get; set; }
    public Guid SubscriberId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string UserCpf { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    [JsonIgnore]
    public Plan Plan { get; set; }

    public PlanSubscriber() { }

    public PlanSubscriber(Guid planId, Guid subscriberId, string username, string userEmail, string userCpf, bool isActive)
    {
        PlanId = planId;
        SubscriberId = subscriberId;
        UserName = username;
        UserEmail = userEmail;
        UserCpf = userCpf;
        IsActive = isActive;
    }
}