using EMS.Core.Extensions;
using EMS.Core.Requests.ServiceAppointments;
using EMS.Core.Responses;
using EMS.Core.Responses.ServiceAppointments;
using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Mappings;

public static class ServiceAppointmentMappings
{
    public static ServiceAppointmentResponse MapServiceAppointmentToServiceAppointmentResponse(this ServiceAppointment serviceAppointment)
    {
        if (serviceAppointment == null)
        {
            return null;
        }

        return new ServiceAppointmentResponse(serviceAppointment.Id, serviceAppointment.CompanyId, serviceAppointment.EmployeeId, serviceAppointment.ClientId, serviceAppointment.ServiceId, serviceAppointment.AppointmentStart.ToFormattedString(), serviceAppointment.AppointmentEnd.ToFormattedString(), serviceAppointment.Status.MapEServiceStatusToEServiceStatusCore(), serviceAppointment.CreatedAt, serviceAppointment.UpdatedAt);
    }

    public static ServiceAppointment MapServiceAppointmentResponseToServiceAppointment(this ServiceAppointmentResponse serviceAppointmentResponse)
    {
        if (serviceAppointmentResponse == null)
        {
            return null;
        }

        return new ServiceAppointment(serviceAppointmentResponse.Id, serviceAppointmentResponse.CompanyId, serviceAppointmentResponse.EmployeeId, serviceAppointmentResponse.ClientId, serviceAppointmentResponse.ServiceId, serviceAppointmentResponse.AppointmentStart.ToDateTime(), serviceAppointmentResponse.AppointmentEnd.ToDateTime(), serviceAppointmentResponse.Status.MapEServiceStatusCoreToEServiceStatus());
    }

    public static PagedResponse<ServiceAppointmentResponse> MapPagedServiceAppointmentsToPagedResponseServiceAppointments(this PagedResult<ServiceAppointment> serviceAppointment)
    {
        if (serviceAppointment == null)
        {
            return null;
        }

        return new PagedResponse<ServiceAppointmentResponse>(serviceAppointment.List.Select(x => x.MapServiceAppointmentToServiceAppointmentResponse()).ToList(), serviceAppointment.TotalResults, serviceAppointment.PageIndex, serviceAppointment.PageSize);
    }

    public static ServiceAppointment MapCreateServiceAppointmentRequestToServiceAppointment(this CreateServiceAppointmentRequest serviceAppointmentRequest)
    {
        if (serviceAppointmentRequest == null)
        {
            return null;
        }

        return new ServiceAppointment(serviceAppointmentRequest.CompanyId, serviceAppointmentRequest.EmployeeId, serviceAppointmentRequest.ClientId, serviceAppointmentRequest.ServiceId, serviceAppointmentRequest.AppointmentStart.ToDateTime(), serviceAppointmentRequest.AppointmentEnd.ToDateTime(), serviceAppointmentRequest.Status.MapEServiceStatusCoreToEServiceStatus());
    }
}