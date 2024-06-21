using EMS.Core.Requests.Services;
using EMS.Core.Responses;
using EMS.Core.Responses.Services;

namespace EMS.Core.Handlers;

public interface IServiceHandler
{
    Task CreateAsync(CreateServiceRequest request);
    Task UpdateAsync(UpdateServiceRequest request);
    Task DeleteAsync(DeleteServiceRequest request);
    Task<ServiceResponse> GetByIdAsync(GetServiceByIdRequest request);
    Task<PagedResponse<ServiceResponse>> GetAllAsync(GetAllServicesRequest request);
}