namespace EMS.WebApp.MVC.Business.Interfaces;

public interface IUnitOfWork
{
    Task<bool> Commit();
}