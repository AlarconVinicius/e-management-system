using AutoMapper;
using EMS.WebApp.Business.Models;
using EMS.WebApp.MVC.Models;

namespace EMS.WebApp.MVC.Configuration;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Employee, EmployeeViewModel>().ReverseMap();
        CreateMap<Product, ProductViewModel>().ReverseMap();
        CreateMap<Plan, PlanViewModel>().ReverseMap();
        CreateMap<Company, CompanyViewModel>().ReverseMap();
        //CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
    }
}
