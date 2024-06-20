using EMS.Core.Enums;
using System.ComponentModel;

namespace EMS.Core.Responses.ServiceAppointments;

public class ServiceAppointmentResponse
{
    [DisplayName("Id")]
    public Guid Id { get; set; }

    [DisplayName("Id do Colaborador")]
    public Guid EmployeeId { get; set; }

    [DisplayName("Id do Cliente")]
    public Guid ClientId { get; set; }

    [DisplayName("Id do Serviço")]
    public Guid ServiceId { get; set; }

    [DisplayName("Início do Agendamento")]
    public DateTime AppointmentStart { get; set; }

    [DisplayName("Fim do Agendamento")]
    public DateTime AppointmentEnd { get; set; }
    public EServiceStatusCore Status { get; set; }

    [DisplayName("Data de Cadastro")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Data de Modificação")]
    public DateTime UpdatedAt { get; set; }

    public ServiceAppointmentResponse() { }

    public ServiceAppointmentResponse(Guid id, Guid employeeId, Guid clientId, Guid serviceId, DateTime appointmentStart, DateTime appointmentEnd, EServiceStatusCore status, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        EmployeeId = employeeId;
        ClientId = clientId;
        ServiceId = serviceId;
        AppointmentStart = appointmentStart;
        AppointmentEnd = appointmentEnd;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}
