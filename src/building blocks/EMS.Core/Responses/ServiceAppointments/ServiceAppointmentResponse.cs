using EMS.Core.Enums;
using EMS.Core.Utils;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Responses.ServiceAppointments;

public class ServiceAppointmentResponse
{
    [DisplayName("Id")]
    public Guid Id { get; set; }

    [DisplayName("Id da Empresa")]
    public Guid CompanyId { get; set; }

    [DisplayName("Id do Colaborador")]
    public Guid EmployeeId { get; set; }

    [DisplayName("Id do Cliente")]
    public Guid ClientId { get; set; }

    [DisplayName("Id do Serviço")]
    public Guid ServiceId { get; set; }


    [RegularExpression(RegexUtils.DateTimeWithHourPattern, ErrorMessage = "Formato inválido. Use dd/MM/yyyy HH:mm:ss")]
    [DisplayName("Início do Agendamento")]
    public string AppointmentStart { get; set; }

    [RegularExpression(RegexUtils.DateTimeWithHourPattern, ErrorMessage = "Formato inválido. Use dd/MM/yyyy HH:mm:ss")]
    [DisplayName("Fim do Agendamento")]
    public string AppointmentEnd { get; set; }
    public EServiceStatusCore Status { get; set; }

    [DisplayName("Data de Cadastro")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Data de Modificação")]
    public DateTime UpdatedAt { get; set; }

    public ServiceAppointmentResponse() { }

    public ServiceAppointmentResponse(Guid id, Guid companyId, Guid employeeId, Guid clientId, Guid serviceId, string appointmentStart, string appointmentEnd, EServiceStatusCore status, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        CompanyId = companyId;
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
