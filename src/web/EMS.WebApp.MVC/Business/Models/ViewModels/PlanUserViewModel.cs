namespace EMS.WebApp.MVC.Business.Models.ViewModels;

public class PlanUserViewModel
{
    public PlanViewModel Plan { get; set; } = new PlanViewModel();
    public RegisterUser RegisterUser { get; set; } = new RegisterUser();
}
