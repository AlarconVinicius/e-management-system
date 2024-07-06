using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Interfaces.Repositories;

public interface IServiceAppointmentRepository : IRepository<ServiceAppointment>
{
    Task<ServiceAppointment> GetByIdAsync(Guid id, Guid tenantId);
    Task<PagedResult<ServiceAppointment>> GetAllPagedAsync(int pageSize, int pageIndex, Guid tenantId, string query = null);
    Task<List<AppointmentRetentionData>> GetAppointmentRetentionDataAsync(Guid tenantId, int selectedYear);
    Task<List<int>> GetAvailableYearsAsync(Guid tenantId);
    Task UpdateEmployeeIdAsync(Guid oldEmployeeId, Guid newEmployeeId);
}
