using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Interfaces.Repositories;

public interface IServiceAppointmentRepository : IRepository<ServiceAppointment>
{
    Task<ServiceAppointment> GetByIdAsync(Guid id, Guid tenantId);
    Task<PagedResult<ServiceAppointment>> GetAllPagedAsync(int pageSize, int pageIndex, Guid tenantId, string query = null);
}
