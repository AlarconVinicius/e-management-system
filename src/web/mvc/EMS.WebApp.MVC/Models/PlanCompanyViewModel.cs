namespace EMS.WebApp.MVC.Models;

public class PlanCompanyViewModel
{
    public PlanViewModel Plan { get; set; } = new PlanViewModel();
    public RegisterCompanyViewModel RegisterCompany { get; set; } = new RegisterCompanyViewModel();
}
