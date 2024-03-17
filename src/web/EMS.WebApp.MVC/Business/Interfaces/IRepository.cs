using EMS.WebApp.MVC.Business.Models;

namespace EMS.WebApp.MVC.Business.Interfaces;

public interface IRepository<T> : IDisposable where T : Entity
{
    IUnitOfWork UnitOfWork { get; }
}