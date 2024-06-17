using EMS.Core.Requests.Companies;
using EMS.Core.Responses.Plans;

namespace EMS.WebApp.MVC.Models;

public class PlanCompanyViewModel
{
    public PlanResponse Plan { get; set; }
    public CreateCompanyAndUserRequest CreateCompanyAndUserRequest { get; set; }
}
