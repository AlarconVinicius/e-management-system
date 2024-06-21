using EMS.Core.Extensions;
using EMS.Core.Handlers;
using EMS.Core.Notifications;
using EMS.Core.Requests.Services;
using EMS.Core.Responses;
using EMS.Core.Responses.Services;
using EMS.Core.User;
using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Business.Mappings;

namespace EMS.WebApi.Business.Handlers;

public class ServiceHandler : BaseHandler, IServiceHandler
{
    public readonly IServiceRepository _serviceRepository;
    public readonly ICompanyRepository _companyRepository;

    public ServiceHandler(INotifier notifier, IAspNetUser appUser, IServiceRepository serviceRepository, ICompanyRepository companyRepository) : base(notifier, appUser)
    {
        _serviceRepository = serviceRepository;
        _companyRepository = companyRepository;
    }

    public async Task<ServiceResponse> GetByIdAsync(GetServiceByIdRequest request)
    {
        if (TenantIsEmpty()) return null;
        try
        {
            var service = await _serviceRepository.GetByIdAsync(request.Id, TenantId);

            if (service is null)
            {
                Notify("Serviço não encontrado.");
                return null;
            }
            return service.MapServiceToServiceResponse();
        }
        catch
        {
            Notify("Não foi possível recuperar o serviço.");
            return null;
        }
    }

    public async Task<PagedResponse<ServiceResponse>> GetAllAsync(GetAllServicesRequest request)
    {
        if (TenantIsEmpty()) return null;
        try
        {
            return (await _serviceRepository.GetAllPagedAsync(request.PageSize, request.PageNumber, TenantId, request.Query)).MapPagedServicesToPagedResponseServices();
        }
        catch
        {
            Notify("Não foi possível consultar os serviços.");
            return null;
        }
    }

    public async Task CreateAsync(CreateServiceRequest request)
    {
        //if (!ExecuteValidation(new ServiceValidation(), service)) return;
        if (TenantIsEmpty()) return;
        if (!CompanyExists(TenantId)) return;

        request.CompanyId = TenantId;
        var serviceMapped = request.MapCreateServiceRequestToService();
        try
        {
            await _serviceRepository.AddAsync(serviceMapped);
            return;
        }
        catch
        {
            Notify("Não foi possível criar o serviço.");
            return;
        }
    }

    public async Task UpdateAsync(UpdateServiceRequest request)
    {
        //if (!ExecuteValidation(new ServiceValidation(), service)) return;
        if (TenantIsEmpty()) return;
        if (!ServiceExists(request.Id, TenantId)) return;
        var serviceDb = await _serviceRepository.GetByIdAsync(request.Id, TenantId);

        try
        {
            serviceDb.SetName(request.Name);
            serviceDb.SetPrice(request.Price);
            serviceDb.SetDuration(request.Duration.ToTimeSpan());
            serviceDb.SetIsActive(request.IsActive);

            await _serviceRepository.UpdateAsync(serviceDb);

            return;
        }
        catch
        {
            Notify("Não foi possível atualizar o servicee.");
            return;
        }
    }

    public async Task DeleteAsync(DeleteServiceRequest request)
    {
        if (TenantIsEmpty()) return;
        try
        {
            if (!ServiceExists(request.Id, TenantId)) return;

            await _serviceRepository.DeleteAsync(request.Id);

            return;
        }
        catch
        {
            Notify("Não foi possível deletar o servicee.");
            return;
        }
    }

    private bool ServiceExists(Guid id, Guid companyId)
    {
        if (_serviceRepository.SearchAsync(f => f.Id == id && f.CompanyId == companyId).Result.Any())
        {
            return true;
        };

        Notify("Serviço não encontrado.");
        return false;
    }

    private bool CompanyExists(Guid companyId)
    {
        if (_companyRepository.GetByIdAsync(companyId).Result is not null)
        {
            return true;
        };

        Notify("TenantId não encontrado.");
        return false;
    }
}
