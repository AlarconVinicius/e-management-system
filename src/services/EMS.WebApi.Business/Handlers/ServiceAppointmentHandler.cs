using EMS.Core.Extensions;
using EMS.Core.Handlers;
using EMS.Core.Notifications;
using EMS.Core.Requests.ServiceAppointments;
using EMS.Core.Responses;
using EMS.Core.Responses.ServiceAppointments;
using EMS.Core.User;
using EMS.WebApi.Business.Interfaces.Repositories;
using EMS.WebApi.Business.Mappings;

namespace EMS.WebApi.Business.Handlers;

public class ServiceAppointmentHandler : BaseHandler, IServiceAppointmentHandler
{
    public readonly IServiceAppointmentRepository _serviceAppointmentRepository;
    public readonly ICompanyRepository _companyRepository;
    public readonly IEmployeeRepository _employeeRepository;
    public readonly IClientRepository _clientRepository;
    public readonly IServiceRepository _serviceRepository;

    public ServiceAppointmentHandler(INotifier notifier, IAspNetUser appUser, IServiceAppointmentRepository serviceAppointmentRepository, ICompanyRepository companyRepository, IEmployeeRepository employeeRepository, IClientRepository clientRepository, IServiceRepository serviceRepository) : base(notifier, appUser)
    {
        _serviceAppointmentRepository = serviceAppointmentRepository;
        _companyRepository = companyRepository;
        _employeeRepository = employeeRepository;
        _clientRepository = clientRepository;
        _serviceRepository = serviceRepository;
    }

    public async Task<ServiceAppointmentResponse> GetByIdAsync(GetServiceAppointmentByIdRequest request)
    {
        if (TenantIsEmpty()) return null;
        try
        {
            var serviceAppointment = await _serviceAppointmentRepository.GetByIdAsync(request.Id, TenantId);

            if (serviceAppointment is null)
            {
                Notify("Agendamento não encontrado.");
                return null;
            }
            return serviceAppointment.MapServiceAppointmentToServiceAppointmentResponse();
        }
        catch
        {
            Notify("Não foi possível recuperar o agendamento.");
            return null;
        }
    }

    public async Task<PagedResponse<ServiceAppointmentResponse>> GetAllAsync(GetAllServiceAppointmentsRequest request)
    {
        if (TenantIsEmpty()) return null;
        try
        {
            return (await _serviceAppointmentRepository.GetAllPagedAsync(request.PageSize, request.PageNumber, TenantId, request.Query)).MapPagedServiceAppointmentsToPagedResponseServiceAppointments();
        }
        catch
        {
            Notify("Não foi possível consultar os agendamentos.");
            return null;
        }
    }

    public async Task CreateAsync(CreateServiceAppointmentRequest request)
    {
        //if (!ExecuteValidation(new ServiceAppointmentValidation(), serviceAppointment)) return;
        if (TenantIsEmpty()) return;
        if (!CompanyExists(TenantId)) return;
        if (!ServiceExists(request.ServiceId, TenantId)) return;
        if (!EmployeeExists(request.EmployeeId, TenantId)) return;
        if (!ClientExists(request.ClientId, TenantId)) return;

        try
        {
            var service = await _serviceRepository.GetByIdAsync(request.ServiceId, TenantId);

            request.CompanyId = TenantId;
            var appointmentStart = request.AppointmentStart;
            var appointmentEnd = appointmentStart.Add(service.Duration);
            request.AppointmentEnd = appointmentEnd;

            var conflictingAppointments = await _serviceAppointmentRepository.SearchAsync(sa =>
                (sa.EmployeeId == request.EmployeeId || sa.ClientId == request.ClientId) &&
                ((sa.AppointmentStart >= appointmentStart && sa.AppointmentStart < appointmentEnd) ||
                (sa.AppointmentEnd > appointmentStart && sa.AppointmentEnd <= appointmentEnd)));

            if (conflictingAppointments.Any())
            {
                var conflictingAppointment = conflictingAppointments.First();
                Notify($"Conflito de agendamento!");
                Notify($"Serviço: {conflictingAppointment.Service.Name}, Agendado com: {conflictingAppointment.Employee.Name}  {conflictingAppointment.Employee.LastName}, para o cliente: {conflictingAppointment.Client.Name} {conflictingAppointment.Client.LastName}, de {conflictingAppointment.AppointmentStart.ToFormattedString()} até {conflictingAppointment.AppointmentEnd.ToFormattedString()}.");
                return;
            }
            var serviceAppointmentMapped = request.MapCreateServiceAppointmentRequestToServiceAppointment();
            await _serviceAppointmentRepository.AddAsync(serviceAppointmentMapped);
            return;
        }
        catch
        {
            Notify("Não foi possível criar o agendamento.");
            return;
        }
    }

    public async Task UpdateAsync(UpdateServiceAppointmentRequest request)
    {
        //if (!ExecuteValidation(new ServiceAppointmentValidation(), serviceAppointment)) return;

        if (!CompanyExists(TenantId)) return;
        if (!ServiceAppointmentExists(request.Id, TenantId)) return;
        if (!ServiceExists(request.ServiceId, TenantId)) return;
        if (!EmployeeExists(request.EmployeeId, TenantId)) return;
        if (!ClientExists(request.ClientId, TenantId)) return;

        var serviceAppointmentDb = await _serviceAppointmentRepository.GetByIdAsync(request.Id, TenantId);

        try
        {
            var service = await _serviceRepository.GetByIdAsync(request.ServiceId, TenantId);

            var appointmentStart = request.AppointmentStart;
            var appointmentEnd = appointmentStart.Add(service.Duration);
            request.AppointmentEnd = appointmentEnd;

            var conflictingAppointments = await _serviceAppointmentRepository.SearchAsync(sa =>
                sa.Id != request.Id &&
                (sa.EmployeeId == request.EmployeeId || sa.ClientId == request.ClientId) &&
                ((sa.AppointmentStart >= appointmentStart && sa.AppointmentStart < appointmentEnd) ||
                (sa.AppointmentEnd > appointmentStart && sa.AppointmentEnd <= appointmentEnd)));

            if (conflictingAppointments.Any())
            {
                var conflictingAppointment = conflictingAppointments.First();
                Notify($"Conflito de agendamento!");
                Notify($"Serviço: {conflictingAppointment.Service.Name}, Agendado com: {conflictingAppointment.Employee.Name + conflictingAppointment.Employee.LastName}, para o cliente: {conflictingAppointment.Client.Name + conflictingAppointment.Client.LastName}, de {conflictingAppointment.AppointmentStart} até {conflictingAppointment.AppointmentEnd}");
                return;
            }
            serviceAppointmentDb.SetServiceId(request.ServiceId);
            serviceAppointmentDb.SetEmployeeId(request.EmployeeId);
            serviceAppointmentDb.SetClientId(request.ClientId);
            serviceAppointmentDb.SetStatus(request.Status.MapEServiceStatusCoreToEServiceStatus());
            serviceAppointmentDb.SetAppointmentStart(request.AppointmentStart);
            serviceAppointmentDb.SetAppointmentEnd(request.AppointmentEnd);
            serviceAppointmentDb.Employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            serviceAppointmentDb.Client = await _clientRepository.GetByIdAsync(request.ClientId);
            serviceAppointmentDb.Service = await _serviceRepository.GetByIdAsync(request.ServiceId);

            await _serviceAppointmentRepository.UpdateAsync(serviceAppointmentDb);

            return;
        }
        catch
        {
            Notify("Não foi possível atualizar o agendamento.");
            return;
        }
    }

    public async Task DeleteAsync(DeleteServiceAppointmentRequest request)
    {
        if (TenantIsEmpty()) return;
        try
        {
            if (!ServiceAppointmentExists(request.Id, TenantId)) return;

            await _serviceAppointmentRepository.DeleteAsync(request.Id);

            return;
        }
        catch
        {
            Notify("Não foi possível deletar o agendamento.");
            return;
        }
    }

    private bool ServiceAppointmentExists(Guid id, Guid companyId)
    {
        if (_serviceAppointmentRepository.SearchAsync(f => f.Id == id && f.CompanyId == companyId).Result.Any())
        {
            return true;
        };

        Notify("Agendamento não encontrado.");
        return false;
    }

    private bool EmployeeExists(Guid employeeId, Guid companyId)
    {
        if (_employeeRepository.GetByIdAsync(employeeId, companyId).Result is not null)
        {
            return true;
        };

        Notify("Colaborador não encontrado.");
        return false;
    }

    private bool ClientExists(Guid clientId, Guid companyId)
    {
        if (_clientRepository.GetByIdAsync(clientId, companyId).Result is not null)
        {
            return true;
        };

        Notify("Cliente não encontrado.");
        return false;
    }

    private bool ServiceExists(Guid serviceId, Guid companyId)
    {
        if (_serviceRepository.GetByIdAsync(serviceId, companyId).Result is not null)
        {
            return true;
        };

        Notify("Serviço não encontrado.");
        return false;
    }

    private bool CompanyExists(Guid companyId)
    {
        if (_companyRepository.GetByIdAsync(companyId).Result is not null)
        {
            return true;
        };

        Notify("TenantId não encontrado.");
        return false;
    }
}
