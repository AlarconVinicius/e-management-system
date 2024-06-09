using EMS.WebApi.Business.Models;

namespace EMS.WebApi.Business.Interfaces.Services;

public interface IPlanService
{
    Task Add(Plan plan);
    Task Update(Plan plan);
    Task Delete(Guid id);
}
