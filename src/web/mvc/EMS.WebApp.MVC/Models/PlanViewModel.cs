using EMS.WebApp.Business.Models;

namespace EMS.WebApp.MVC.Models;

public class PlanViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Benefits { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public PlanViewModel ToViewModel(Plan plan)
    {
        return new PlanViewModel
        {
            Id = plan.Id,
            Title = plan.Title,
            Subtitle = plan.Subtitle,
            Price = plan.Price,
            Benefits = plan.Benefits,
            IsActive = plan.IsActive
        };
    }

    public Plan ToDomain(PlanViewModel planViewModel)
    {
        return new Plan(planViewModel.Title, planViewModel.Subtitle, planViewModel.Price, planViewModel.Benefits, planViewModel.IsActive);
    }
}