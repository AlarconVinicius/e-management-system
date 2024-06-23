using EMS.Core.Enums;
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

    [DisplayName("Nome do Colaborador")]
    public string EmployeeName { get; set; }

    [DisplayName("Sobrenome do Colaborador")]
    public string EmployeeLastname { get; set; }

    [DisplayName("Id do Cliente")]
    public Guid ClientId { get; set; }

    [DisplayName("Nome do Cliente")]
    public string ClientName { get; set; }

    [DisplayName("Sobrenome do Cliente")]
    public string ClientLastname { get; set; }

    [DisplayName("Id do Serviço")]
    public Guid ServiceId { get; set; }

    [DisplayName("Nome do Serviço")]
    public string ServiceName { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    [DisplayName("Início do Agendamento")]
    public DateTime AppointmentStart { get; set; }

    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    [DisplayName("Fim do Agendamento")]
    public DateTime AppointmentEnd { get; set; }

    [DisplayName("Status")]
    public EServiceStatusCore Status { get; set; }

    [DisplayName("Data de Cadastro")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Data de Modificação")]
    public DateTime UpdatedAt { get; set; }

    public ServiceAppointmentResponse() { }

    public ServiceAppointmentResponse(Guid id, Guid companyId, Guid employeeId, Guid clientId, Guid serviceId, string employeeName, string employeeLastname, string clientName, string clientLastname, string serviceName, DateTime appointmentStart, DateTime appointmentEnd, EServiceStatusCore status, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        CompanyId = companyId;
        EmployeeId = employeeId;
        ClientId = clientId;
        ServiceId = serviceId;
        EmployeeName = employeeName;
        EmployeeLastname = employeeLastname;
        ClientName = clientName;
        ClientLastname = clientLastname;
        ServiceName = serviceName;
        AppointmentStart = appointmentStart;
        AppointmentEnd = appointmentEnd;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}
