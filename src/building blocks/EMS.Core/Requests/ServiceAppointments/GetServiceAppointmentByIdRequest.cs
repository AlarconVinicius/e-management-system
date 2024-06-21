namespace EMS.Core.Requests.ServiceAppointments;

public class GetServiceAppointmentByIdRequest : Request
{
    public Guid Id { get; set; }

    public GetServiceAppointmentByIdRequest() { }

    public GetServiceAppointmentByIdRequest(Guid id)
    {
        Id = id;
    }
}