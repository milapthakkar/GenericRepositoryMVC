using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GenericRepository
{    
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbset;

        public Repository(DbContext context)
        {
            _context = context;
            _dbset = context.Set<TEntity>();
        }

        public TEntity Get(TKey id)
        {
            return _dbset.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {            
            return _dbset.ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbset.Where(predicate);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbset.SingleOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            _dbset.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _dbset.AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            _dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbset.RemoveRange(entities);
        }

        public TEntity SingleOrDefaultOrderBy(Expression<Func<TEntity, bool>> whereCondition, Expression<Func<TEntity, int>> orderBy, string direction)
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

        public bool Exists(Expression<Func<TEntity, bool>> whereCondition)
        {
            return _dbset.Any(whereCondition);
        }

        public int Count(Expression<Func<TEntity, bool>> whereCondition = null)
        {
            if (whereCondition != null)
            {
                return _dbset.Where(whereCondition).Count();
            }
            return _dbset.AsEnumerable().Count();
        }

        public IEnumerable<TEntity> GetPagedRecords(Expression<Func<TEntity, bool>> whereCondition, Expression<Func<TEntity, string>> orderBy, int pageNo, int pageSize)
        {
            return (_dbset.Where(whereCondition).OrderBy(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize)).AsEnumerable();
        }

        public IEnumerable<TEntity> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return _dbset.SqlQuery(query, parameters);
        }
        //var data = deptRepo.ExecWithStoreProcedure("usp_get_department").ToList();
        //var data = deptRepo.ExecWithStoreProcedure("usp_get_department @p0", 1).ToList();
    }
}
