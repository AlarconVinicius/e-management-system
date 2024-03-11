namespace EMS.Core.Data;

public interface IUnitOfWork
{
    Task<bool> Commit();
}