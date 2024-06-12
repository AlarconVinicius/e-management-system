using EMS.Core.Requests.Identities;
using EMS.Core.Responses.Plans;

namespace EMS.WebApp.MVC.Models;

public class PlanCompanyViewModel
{
    public PlanResponse Plan { get; set; } = new PlanResponse();
    public CreateUserRequest CreateUserRequest { get; set; } = new CreateUserRequest();
    public RegisterCompanyViewModel RegisterCompany { get; set; } = new RegisterCompanyViewModel();
}
