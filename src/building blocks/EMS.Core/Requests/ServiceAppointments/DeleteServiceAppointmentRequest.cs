namespace EMS.Core.Requests.ServiceAppointments;

public class DeleteServiceAppointmentRequest : CompaniesRequest
{
    public Guid Id { get; set; }

    public DeleteServiceAppointmentRequest() { }

    public DeleteServiceAppointmentRequest(Guid id)
    {
        Id = id;
    }

    public DeleteServiceAppointmentRequest(Guid id, Guid companyId) : base(companyId)
    {
        Id = id;
    }
}
