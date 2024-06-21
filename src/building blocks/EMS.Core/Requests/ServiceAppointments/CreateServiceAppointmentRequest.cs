using EMS.Core.Enums;
using EMS.Core.Extensions;
using EMS.Core.Utils;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Requests.ServiceAppointments;

public class CreateServiceAppointmentRequest : CompaniesRequest
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Id do Colaborador")]
    public Guid EmployeeId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Id do Cliente")]
    public Guid ClientId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayName("Id do Serviço")]
    public Guid ServiceId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [RegularExpression(RegexUtils.DateTimeWithHourPattern, ErrorMessage = "Formato inválido. Use dd/MM/yyyy HH:mm:ss")]
    [DisplayName("Início do Agendamento")]
    public string AppointmentStart { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [RegularExpression(RegexUtils.DateTimeWithHourPattern, ErrorMessage = "Formato inválido. Use dd/MM/yyyy HH:mm:ss")]
    [DisplayName("Fim do Agendamento")]
    public string AppointmentEnd { get; set; }
    public EServiceStatusCore Status { get; set; }

    public CreateServiceAppointmentRequest() { }

    public CreateServiceAppointmentRequest(Guid companyId, Guid employeeId, Guid clientId, Guid serviceId, string appointmentStart, string appointmentEnd, EServiceStatusCore status) : base(companyId)
    {
        EmployeeId = employeeId;
        ClientId = clientId;
        ServiceId = serviceId;
        AppointmentStart = appointmentStart;
        AppointmentEnd = appointmentEnd;
        Status = status;
    }
}
