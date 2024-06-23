using EMS.Core.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMS.Core.Requests.ServiceAppointments;

public class UpdateServiceAppointmentRequest : CompaniesRequest
{
    public Guid Id { get; set; }

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
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    [DisplayName("Início do Agendamento")]
    public DateTime AppointmentStart { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
    [DisplayName("Fim do Agendamento")]
    public DateTime AppointmentEnd { get; set; }

    [DisplayName("Status")]
    public EServiceStatusCore Status { get; set; }

    public UpdateServiceAppointmentRequest() { }

    public UpdateServiceAppointmentRequest(Guid id, Guid companyId, Guid employeeId, Guid clientId, Guid serviceId, DateTime appointmentStart, DateTime appointmentEnd, EServiceStatusCore status) : base(companyId)
    {
        Id = id;
        EmployeeId = employeeId;
        ClientId = clientId;
        ServiceId = serviceId;
        AppointmentStart = appointmentStart;
        AppointmentEnd = appointmentEnd;
        Status = status;
    }
}
