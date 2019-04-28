using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GenericRepository
{    
    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        TEntity Get(TKey id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        bool Exists(Expression<Func<TEntity, bool>> whereCondition);
        int Count(Expression<Func<TEntity, bool>> whereCondition = null);
        IEnumerable<TEntity> GetPagedRecords(Expression<Func<TEntity, bool>> whereCondition, Expression<Func<TEntity, string>> orderBy, int pageNo, int pageSize);
        IEnumerable<TEntity> ExecWithStoreProcedure(string query, params object[] parameters);
    }    
}
