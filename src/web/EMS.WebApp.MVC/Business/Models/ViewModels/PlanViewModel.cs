using EMS.WebApp.MVC.Business.Models.Subscription;

namespace EMS.WebApp.MVC.Business.Models.ViewModels;

public class PlanViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string SubTitle { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Benefits { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public PlanViewModel ToViewModel(Plan plan)
    {
        return new PlanViewModel
        {
            Id = plan.Id,
            Title = plan.Title,
            SubTitle = plan.SubTitle,
            Price = plan.Price,
            Benefits = plan.Benefits,
            IsActive = plan.IsActive
        };
    }

    public Plan ToDomain(PlanViewModel planViewModel)
    {
        return new Plan
        {
            Id = planViewModel.Id,
            Title = planViewModel.Title,
            SubTitle = planViewModel.SubTitle,
            Price = planViewModel.Price,
            Benefits = planViewModel.Benefits,
            IsActive = planViewModel.IsActive
        };
    }
}