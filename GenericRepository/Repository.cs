using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OWS.GR
{
    public class Repository<T> : IRepository<T> where T : class
    {
        //private readonly OrderBy<T> DefaultOrderBy = null;// new OrderBy<T>(qry => qry.OrderBy(e => e.Id));

        protected DbContext _context;
        //protected readonly DbSet<T> _dbset;

        public Repository(DbContext context)
        {
            _context = context;
           // _dbset = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes);
            return result.ToList();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes);
            return await result.ToListAsync();
        }

        public virtual void Load(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes);
            result.Load();
        }

        public virtual async Task LoadAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            var result = QueryDb(null, orderBy, includes);
            await result.LoadAsync();
        }

        public virtual IEnumerable<T> GetPage(int startRow, int pageLength, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            //if (orderBy == null) orderBy = DefaultOrderBy?.Expression;

            var result = QueryDb(null, orderBy, includes).AsNoTracking();
            return result.Skip(startRow).Take(pageLength).ToList();
        }

        public virtual async Task<IEnumerable<T>> GetPageAsync(int startRow, int pageLength, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            //if (orderBy == null) orderBy = DefaultOrderBy?.Expression;

            var result = QueryDb(null, orderBy, includes).AsNoTracking();
            return await result.Skip(startRow).Take(pageLength).ToListAsync();
        }

        public virtual T Get(params object[] keyValues)
        {
            var existing = _context.Set<T>().Find(keyValues.ToArray());
            return existing;
        }
        public virtual async Task<T> GetAsync(params object[] keyValues)
        {
            var existing = await _context.Set<T>().FindAsync(keyValues.ToArray());
            return existing;
        }
        public virtual T Get(object id, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
            {
                query = includes(query);
            }

            //if (typeof(T).IsSubclassOf(typeof(Entity<>)))
            //    return query.SingleOrDefaultAsync(x => x.Id.Equals(id));
            var properties = GetKeyProperties();
            if (properties.Count() != 1 || !(properties.First().PropertyType == id.GetType()))
                throw new Exception(string.Format("Invalid key type {0}.", id == null ? null : id.GetType().Name));
            var valueType = id.GetType();
            return query.Where(PropertyEquals<T, object>(typeof(T).GetProperty(properties.First().Name), id)).SingleOrDefault();
            //return query.SingleOrDefault(x => (x.GetType().GetProperty(properties.First().Name).GetValue(x, null)).Equals(id));

        }
        public virtual async Task<T> GetAsync(object id, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
            {
                query = includes(query);
            }

            //if (typeof(T).IsSubclassOf(typeof(Entity<>)))
            //    return query.SingleOrDefaultAsync(x => x.Id.Equals(id));
            var properties = GetKeyProperties();
            if (properties.Count() != 1 || !(properties.First().PropertyType == id.GetType()))
                throw new Exception(string.Format("Invalid key type {0}.", id == null ? null : id.GetType().Name));
            return await query.Where(PropertyEquals<T, object>(typeof(T).GetProperty(properties.First().Name), id)).SingleOrDefaultAsync();
            //return await query.SingleOrDefaultAsync(x => (x.GetType().GetProperty(properties.First().Name).GetValue(x, null)).Equals(id));

        }

        public virtual IEnumerable<T> Query(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes);
            return result.ToList();
        }

        public virtual async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes);
            return await result.ToListAsync();
        }

        public virtual void Load(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes);
            result.Load();
        }

        public virtual async Task LoadAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            var result = QueryDb(filter, orderBy, includes);
            await result.LoadAsync();
        }

        public virtual IEnumerable<T> QueryPage(int startRow, int pageLength, Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            //if (orderBy == null) orderBy = DefaultOrderBy?.Expression;

            var result = QueryDb(filter, orderBy, includes).AsNoTracking();
            return result.Skip(startRow).Take(pageLength).ToList();
        }

        public virtual async Task<IEnumerable<T>> QueryPageAsync(int startRow, int pageLength, Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> includes = null)
        {
            //if (orderBy == null) orderBy = DefaultOrderBy?.Expression;

            var result = QueryDb(filter, orderBy, includes).AsNoTracking();
            return await result.Skip(startRow).Take(pageLength).ToListAsync();
        }

        public virtual void Add(T entity)
        {
            if (entity == null) throw new InvalidOperationException("Unable to add a null entity to the repository.");
            _context.Set<T>().Add(entity);
        }

        public virtual T Update(object entity)
        {
            List<object> keyValues = new List<object>();
            var properties = GetKeyProperties();
            if (properties.Count() == 0)
                throw new Exception(string.Format("No Key for entity {0}.", typeof(T).Name));
            foreach (var key in properties)
            {
                keyValues.Add(entity.GetType().GetProperty(key.Name).GetValue(entity));
            }

            var existing = _context.Set<T>().Find(keyValues.ToArray());
            if (existing == null) throw new Exception(string.Format("Cannot find entity type {0} with key {1}", typeof(T).Name, string.Join(",", keyValues.ToArray())));
            _context.Entry(existing).CurrentValues.SetValues(entity);
            return existing;
        }
        public virtual void Remove(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Deleted;
            _context.Set<T>().Remove(entity);
        }

        public virtual void Remove(params object[] keyValues)
        {
            var entity = new T();
            var properties = GetKeyProperties();
            //if (properties.Count() != 1 || !(properties.First().PropertyType == id.GetType()))
            //    throw new Exception(string.Format("Invalid key type {0}.", id == null ? null : id.GetType().Name));
            if (properties.Count() != keyValues.Count())
                throw new Exception("Wrong number of key values.");
            for (int i = 0; i < properties.Count(); i++)
            {
                var key = properties.ElementAt(i);
                entity.GetType().GetProperty(key.Name).SetValue(entity, keyValues[i]);
            }

            this.Remove(entity);
        }

        public virtual bool Any(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Any();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.AnyAsync();
        }

        public virtual int Count(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.Count();
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        protected IQueryable<T> QueryDb(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, Func<IQueryable<T>, IQueryable<T>> includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }

        public void SetUnchanged(T entity)
        {
            _context.Entry(entity).State = EntityState.Unchanged;
        }
        public void Attach(T entity)
        {
            _context.Set<T>().Attach(entity);
        }
        protected virtual IEnumerable<PropertyInfo> GetKeyProperties()
        {
            var type = typeof(T);
            var properties = typeof(T).GetProperties().Where(prop => prop.IsDefined(typeof(KeyAttribute), true));
            return properties;
        }
        private Expression<Func<TItem, bool>> PropertyEquals<TItem, TValue>(PropertyInfo property, TValue value)
        {
            var param = Expression.Parameter(typeof(TItem));
            var body = Expression.Equal(Expression.Property(param, property),
                Expression.Constant(value));
            return Expression.Lambda<Func<TItem, bool>>(body, param);
        }

    }

}