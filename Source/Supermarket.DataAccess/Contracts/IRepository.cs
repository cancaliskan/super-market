using System.Collections.Generic;

namespace Supermarket.DataAccess.Contracts
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Remove(int id);
    }
}