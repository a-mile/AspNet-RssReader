using System;
using System.Collections.Generic;
using System.Linq;
using AspNet_RssReader_Domain.Abstract;

namespace AspNet_RssReader_Domain.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        readonly Dictionary<Type, object> _repositoriesDict = new Dictionary<Type, object>();

        public IRepository<T> Repository<T>() where T : class
        {
            if (_repositoriesDict.Keys.Contains(typeof(T)))
            {
                return _repositoriesDict[typeof(T)] as IRepository<T>;
            }

            IRepository<T> repo = new GenericRepository<T>(_context);
            _repositoriesDict.Add(typeof(T), repo);
            return repo;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
