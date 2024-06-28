using EMS.Core.Handlers;
using EMS.Core.Notifications;
using EMS.Core.Requests.Dashboards;
using EMS.Core.Responses.Dashboards;
using EMS.Core.User;
using EMS.WebApi.Business.Interfaces.Repositories;

namespace EMS.WebApi.Business.Handlers;

public class DashboardHandler : BaseHandler, IDashboardHandler
{
    public readonly IServiceAppointmentRepository _serviceAppointmentRepository;
    public readonly ICompanyRepository _companyRepository;
    public readonly IEmployeeRepository _employeeRepository;
    public readonly IClientRepository _clientRepository;
    public readonly IServiceRepository _serviceRepository;

    public DashboardHandler(INotifier notifier, IAspNetUser appUser, IServiceAppointmentRepository serviceAppointmentRepository, ICompanyRepository companyRepository, IEmployeeRepository employeeRepository, IClientRepository clientRepository, IServiceRepository serviceRepository) : base(notifier, appUser)
    {
        _serviceAppointmentRepository = serviceAppointmentRepository;
        _companyRepository = companyRepository;
        _employeeRepository = employeeRepository;
        _clientRepository = clientRepository;
        _serviceRepository = serviceRepository;
    }

    public async Task<DashboardResponse> GetDashboardDetailsAsync(GetDashboardDetailsRequest request)
    {
        if (TenantIsEmpty()) return null;
        try
        {
            var clients = await _clientRepository.SearchAsync(c => c.CompanyId == TenantId);
            var employees = await _employeeRepository.SearchAsync(c => c.CompanyId == TenantId);
            var services = await _serviceRepository.SearchAsync(c => c.CompanyId == TenantId);
            var serviceAppointments = await _serviceAppointmentRepository.SearchAsync(c => c.CompanyId == TenantId);

            var appointmentRetention = (await _serviceAppointmentRepository.GetAppointmentRetentionDataAsync(TenantId, request.SelectedYear)).Select(a => new AppointmentRetentionDataResult(a.Month, a.Realized, a.Canceled)).ToList();
            List<int> employeeStatus = new() { 0, 1};
            var availableYears = await _serviceAppointmentRepository.GetAvailableYearsAsync(TenantId);
            var response = new DashboardResponse(clients.Count(), employees.Count(), services.Count(), serviceAppointments.Count(), appointmentRetention, employeeStatus, availableYears);
            return response;
        }
        catch (Exception)
        {
            Notify("Não foi possível recuperar os dados do Dashboard.");
            return null;
        }
    }
}
