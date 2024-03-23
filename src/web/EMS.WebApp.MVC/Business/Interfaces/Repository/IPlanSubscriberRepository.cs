using EMS.WebApp.MVC.Business.Models.Subscription;

namespace EMS.WebApp.MVC.Business.Interfaces.Repository;

public interface IPlanSubscriberRepository : IRepository<PlanSubscriber>
{
    Task<PlanSubscriber> GetById(Guid id);
    Task<PlanSubscriber> GetByUserCpf(string cpf);
    Task<IEnumerable<PlanSubscriber>> GetAll();
    void AddPlanUser(PlanSubscriber planUser);
    void UpdatePlanUser(PlanSubscriber planUser);
    Task DisablePlanUser(Guid id, bool disable);
    Task EnablePlanUser(Guid id, bool enable);
}