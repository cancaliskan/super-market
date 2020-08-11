﻿using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Supermarket.Common.Helpers;
using Supermarket.DataAccess.Contracts;
using Supermarket.Domain.Entities;

namespace Supermarket.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected DbContext Context;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            Context = context;
            _dbSet = Context.Set<T>();
        }

        public T GetById(Guid id)
        {
            return _dbSet.FirstOrDefault(x=> x.Id == id && x.IsActive);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList().Where(x=>x.IsActive);
        }

        public void Add(T entity)
        {
            entity.Id = new Guid(GuidHelper.GetNewUid());
            entity.IsActive = true;
            entity.CreatedDate=DateTime.Now;
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            entity.UpdateDate=DateTime.Now;
            _dbSet.Update(entity);
        }

        public void Remove(Guid id)
        {
            _dbSet.Remove(GetById(id));
        }
        
        public void Remove(T entity)
        {
            entity.DeletedDate=DateTime.Now;
            entity.IsActive = false;
            Update(entity);
        }
    }
}