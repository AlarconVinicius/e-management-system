using EMS.WebApp.MVC.Business.Models.Subscription;

namespace EMS.WebApp.MVC.Business.Interfaces.Repository;

public interface IPlanRepository : IRepository<Plan>
{
    Task<Plan> GetById(Guid id);
    Task<IEnumerable<Plan>> GetAll();
    void AddPlan(Plan plan);
    void UpdatePlan(Plan plan);
    Task DisablePlan(Guid id, bool disable);
    Task EnablePlan(Guid id, bool enable);
}