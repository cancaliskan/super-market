using System;
using System.Collections.Generic;

namespace Supermarket.DataAccess.Contracts
{
    public interface IRepository<T> where T : class
    {
        T GetById(Guid id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Remove(Guid id);
        void Remove(T entity);
    }
}