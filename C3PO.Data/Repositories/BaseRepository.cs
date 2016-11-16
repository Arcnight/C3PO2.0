using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;

using C3PO.Data.Models;
using C3PO.Data.Interfaces;

namespace C3PO.Data.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected postgresEntities DBContext { private set; get; }

        protected DbSet<T> Repository { private set; get; }

        public BaseRepository()
        {
            DBContext = new postgresEntities();
            Repository = DBContext.Set<T>();
        }

        public virtual T Get(int id)
        {
            return Repository.Find(id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return Repository;
        }
    }
}