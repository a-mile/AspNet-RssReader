using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AspNet_RssReader_Domain.Abstract
{
    public interface IRepository<T> where T : class 
    {
        IEnumerable<T> SelectAll(Expression<Func<T, bool>> predicate = null);
        T GetById(int id);
        T GetByStringId(string id);
        void Add(T entity);
        void Update(T entity);
        T Delete(int id);
        T Delete(T entity);
    }
}
