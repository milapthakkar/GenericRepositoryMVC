using GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OWS.GR
{
    public class UnitOfWork : IDisposable
    {
        protected DbContext _context;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        //public IGenericRepository<T> GetRepository<T>() where T : class
        //{
        //    if (repositories.Keys.Contains(typeof(T)) == true)
        //    {
        //        return repositories[typeof(T)] as IGenericRepository<T>;
        //    }
        //    IGenericRepository<T> repo = new GenericRepository<T>(_context);
        //    repositories.Add(typeof(T), repo);
        //    return repo;
        //}

        public IRepository<T,Tkey> GetRepository<T, Tkey>() where T : class
        {
            if (repositories.Keys.Contains(typeof(T)) == true)
            {
                return repositories[typeof(T)] as IRepository<T, Tkey>;
            }
            IRepository<T, Tkey> repo = new Repository<T, Tkey>(_context);
            repositories.Add(typeof(T), repo);
            return repo;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
