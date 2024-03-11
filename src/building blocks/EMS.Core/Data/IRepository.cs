using EMS.Core.DomainObjects;

namespace EMS.Core.Data;

public interface IRepository<T> : IDisposable where T : Entity
{
    IUnitOfWork UnitOfWork { get; }
}