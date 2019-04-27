using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Entity;

namespace OWS.GR
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Func<T, bool> whereCondition = null);
        T Get(Func<T, bool> whereCondition);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        //Additional Methods
        T SingleOrDefaultOrderBy(Expression<Func<T, bool>> whereCondition, Expression<Func<T, int>> orderBy, string direction);
        bool Exists(Expression<Func<T, bool>> whereCondition);
        int Count(Expression<Func<T, bool>> whereCondition = null);
        IEnumerable<T> GetPagedRecords(Expression<Func<T, bool>> whereCondition, Expression<Func<T, string>> orderBy, int pageNo, int pageSize);
        IEnumerable<T> ExecWithStoreProcedure(string query, params object[] parameters);

        //Async      
    }
}
