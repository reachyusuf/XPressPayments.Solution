using XPressPayments.Data.EFRepository;

namespace XPressPayments.Data.UnitOfWork
{
    public interface IUnitofWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        Task<int> SaveChanges();
        void Rollback();
    }
}
