namespace EMS.WebApi.Business.Models;

public class ServiceAppointment : Entity
{
    public Guid CompanyId { get; private set; }
    public Guid EmployeeId { get; private set; }
    public Guid ClientId { get; private set; }
    public Guid ServiceId { get; private set; }
    public DateTime AppointmentStart { get; private set; }
    public DateTime AppointmentEnd { get; private set; }
    public EServiceStatus Status { get; private set; }
    public Company Company { get; set; }
    public Employee Employee { get; set; }
    public Client Client { get; set; }
    public Service Service { get; set; }

    public ServiceAppointment() { }

    public ServiceAppointment(Guid companyId, Guid employeeId, Guid clientId, Guid serviceId, DateTime appointmentStart, DateTime appointmentEnd, EServiceStatus status)
    {
        CompanyId = companyId;
        EmployeeId = employeeId;
        ClientId = clientId;
        ServiceId = serviceId;
        AppointmentStart = appointmentStart;
        AppointmentEnd = appointmentEnd;
        Status = status;
    }

    public ServiceAppointment(Guid id, Guid companyId, Guid employeeId, Guid clientId, Guid serviceId, DateTime appointmentStart, DateTime appointmentEnd, EServiceStatus status)
    {
        SetId(id);
        CompanyId = companyId;
        EmployeeId = employeeId;
        ClientId = clientId;
        ServiceId = serviceId;
        AppointmentStart = appointmentStart;
        AppointmentEnd = appointmentEnd;
        Status = status;
    }

    #region Setters
    public void SetEmployeeId(Guid id)
    {
        EmployeeId = id;
    }

    public void SetClientId(Guid id)
    {
        ClientId = id;
    }

    public void SetServiceId(Guid id)
    {
        ServiceId = id;
    }

    public void SetAppointmentStart(DateTime appointmentStart)
    {
        AppointmentStart = appointmentStart;
    }

    public void SetAppointmentEnd(DateTime appointmentEnd)
    {
        AppointmentEnd = appointmentEnd;
    }
    #endregion
}
