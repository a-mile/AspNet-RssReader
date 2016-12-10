using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AspNet_RssReader_Domain.Abstract;

namespace AspNet_RssReader_Domain.Concrete
{
    public class GenericRepository<T> : IRepository<T> where T : class 
    {
        readonly DbContext _context;
        readonly IDbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public IEnumerable<T> SelectAll(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
            {
                return _dbSet.Where(predicate);
            }

            return _dbSet.AsEnumerable();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public T GetByStringId(string id)
        {
            return _dbSet.Find(id);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public T Delete(int id)
        {
            T entityToDelete = GetById(id);
            _dbSet.Remove(entityToDelete);
            return entityToDelete;
        }

        public T Delete(T entity)
        {
            _dbSet.Remove(entity);
            return entity;
        }       
    }
}
