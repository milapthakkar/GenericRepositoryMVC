using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OWS.GR
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DbContext _context;
        protected readonly DbSet<T> _dbset;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbset = context.Set<T>();
        }

        #region basicmethods
        public IEnumerable<T> GetAll(Func<T, bool> whereCondition = null)
        {
            if (whereCondition != null)
            {
                return _dbset.Where(whereCondition);
            }

            return _dbset.AsEnumerable();
        }

        public T Get(Func<T, bool> whereCondition)
        {
            return _dbset.First(whereCondition);
        }

        public void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbset.Remove(entity);
        }
        #endregion basicmethods
        #region ExtraMethods

        public T SingleOrDefaultOrderBy(Expression<Func<T, bool>> whereCondition, Expression<Func<T, int>> orderBy, string direction)
        {
            if (direction == "ASC")
            {
                return _dbset.Where(whereCondition).OrderBy(orderBy).FirstOrDefault();

            }
            else
            {
                return _dbset.Where(whereCondition).OrderByDescending(orderBy).FirstOrDefault();
            }
        }

        public bool Exists(Expression<Func<T, bool>> whereCondition)
        {
            return _dbset.Any(whereCondition);
        }

        public int Count(Expression<Func<T, bool>> whereCondition = null)
        {
            if (whereCondition != null)
            {
                return _dbset.Where(whereCondition).Count();
            }
            return _dbset.AsEnumerable().Count();
        }

        public IEnumerable<T> GetPagedRecords(Expression<Func<T, bool>> whereCondition, Expression<Func<T, string>> orderBy, int pageNo, int pageSize)
        {
            return (_dbset.Where(whereCondition).OrderBy(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize)).AsEnumerable();
        }

        public IEnumerable<T> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return _dbset.SqlQuery(query, parameters);
        }
        #endregion ExtraMethods
    }
}
