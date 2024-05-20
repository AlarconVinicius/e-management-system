namespace EMS.WebApp.MVC.Business.Models.ViewModels;

public class PlanCompanyViewModel
{
    public PlanViewModel Plan { get; set; } = new PlanViewModel();
    public RegisterCompanyViewModel RegisterCompany { get; set; } = new RegisterCompanyViewModel();
}
