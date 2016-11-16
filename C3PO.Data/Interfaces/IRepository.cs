using System;
using System.Linq;
using System.Collections.Generic;

namespace C3PO.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        IQueryable<T> GetAll();
    }
}
