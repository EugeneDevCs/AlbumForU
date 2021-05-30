using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLayer.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Create(T item);
        IEnumerable<T> GetAll();
        T Get(string id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);        
        void Update(T item);
        void Delete(string id);
    }
}
