using EMS.Core.Requests.Services;
using EMS.Core.Responses;
using EMS.Core.Responses.Services;
using EMS.Core.Utils;
using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Mappings;

public static class ServiceMappings
{
    public static ServiceResponse MapServiceToServiceResponse(this Service service)
    {
        if (service == null)
        {
            return null;
        }

        return new ServiceResponse(service.Id, service.CompanyId, service.Name, service.Price, service.Duration, service.IsActive, service.CreatedAt, service.UpdatedAt);
    }

    public static Service MapServiceResponseToService(this ServiceResponse serviceResponse)
    {
        if (serviceResponse == null)
        {
            return null;
        }
        //Guid id, string name, decimal price, TimeSpan duration, bool isActive, DateTime createdAt, DateTime updatedAt
        return new Service(serviceResponse.Id, serviceResponse.CompanyId, serviceResponse.Name, serviceResponse.Price, serviceResponse.Duration, serviceResponse.IsActive);
    }

    public static PagedResponse<ServiceResponse> MapPagedServicesToPagedResponseServices(this PagedResult<Service> service)
    {
        if (service == null)
        {
            return null;
        }

        return new PagedResponse<ServiceResponse>(service.List.Select(x => x.MapServiceToServiceResponse()).ToList(), service.TotalResults, service.PageIndex, service.PageSize);
    }

    public static Service MapCreateServiceRequestToService(this CreateServiceRequest serviceRequest)
    {
        if (serviceRequest == null)
        {
            return null;
        }
        
        return new Service(serviceRequest.CompanyId, serviceRequest.Name, serviceRequest.Price, serviceRequest.Duration.ToTimeSpan(), serviceRequest.IsActive);
    }
}