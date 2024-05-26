using AutoMapper;
using EMS.Core.DomainObjects;
using EMS.WebApp.Business.Models;
using EMS.WebApp.MVC.Models;

namespace EMS.WebApp.MVC.Configuration;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Employee, EmployeeViewModel>()
           .ForMember(dest => dest.Document, opt => opt.MapFrom(src => src.Document.Number))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address))
           .ReverseMap()
           .ForMember(dest => dest.Document, opt => opt.MapFrom(src => new Cpf(src.Document)))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)));
        CreateMap<Client, ClientViewModel>()
          .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.Document.Number))
          .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address))
          .ReverseMap()
          .ForMember(dest => dest.Document, opt => opt.MapFrom(src => new Cpf(src.Cpf)))
          .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)));
        //CreateMap<Employee, EmployeeViewModel>()
        //    .ForMember(dest => dest.Document, opt => opt.MapFrom(src => src.Document.Number))
        //    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address))
        //    .ReverseMap()
        //    .ForMember(dest => dest.Document.Number, opt => opt.MapFrom(src => src.Document))
        //    .ForMember(dest => dest.Email.Address, opt => opt.MapFrom(src => src.Email));
        CreateMap<Product, ProductViewModel>().ReverseMap();
        CreateMap<Plan, PlanViewModel>().ReverseMap();
        CreateMap<Company, CompanyViewModel>().ReverseMap();
        //CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
    }
}
