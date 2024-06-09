using EMS.WebApp.Business.Models;

namespace EMS.WebApp.Business.Interfaces.Services;

public interface IPlanService
{
    Task Add(Plan plan);
    Task Update(Plan plan);
    Task Delete(Guid id);
}
