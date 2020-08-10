using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Supermarket.DataAccess.Contracts;

namespace Supermarket.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext Context;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            Context = context;
            _dbSet = Context.Set<T>();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Remove(int id)
        {
            _dbSet.Remove(GetById(id));
        }
    }
}