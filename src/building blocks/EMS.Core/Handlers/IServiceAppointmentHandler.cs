using EMS.Core.Requests.ServiceAppointments;
using EMS.Core.Responses;
using EMS.Core.Responses.ServiceAppointments;

namespace EMS.Core.Handlers;

public interface IServiceAppointmentHandler
{
    Task CreateAsync(CreateServiceAppointmentRequest request);
    Task UpdateAsync(UpdateServiceAppointmentRequest request);
    Task DeleteAsync(DeleteServiceAppointmentRequest request);
    Task<ServiceAppointmentResponse> GetByIdAsync(GetServiceAppointmentByIdRequest request);
    Task<PagedResponse<ServiceAppointmentResponse>> GetAllAsync(GetAllServiceAppointmentsRequest request);
}