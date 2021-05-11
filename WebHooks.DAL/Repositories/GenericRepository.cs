using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebHooks.DAL.EF;
using WebHooks.DAL.Interfaces;
using WebHooks.DAL.Models;

namespace WebHooks.DAL.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {

        DbContext _context;
        DbSet<T> _dbSet;


        public GenericRepository(WebHooksContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Create(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            item.IsActual = true;
            if(item.CreateTime == null)
            {
                item.CreateTime = DateTime.UtcNow;
            }
            _dbSet.Add(item);
        }

        public void Delete(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            _dbSet.Remove(item);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return GetQuery(includes).Where(predicate);
        }

        public T Get(Int64 id, params Expression<Func<T, object>>[] includes)
        {
            return GetQuery(includes).Where(T => T.Id == id).FirstOrDefault();
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            return GetQuery(includes).AsNoTracking();
        }

        public IEnumerable<T> GetActual(params Expression<Func<T, object>>[] includes)
        {
            return GetQuery(includes).Where(T => T.IsActual).AsNoTracking();
        }

        public IEnumerable<T> GetQuery(string queryString, params Expression<Func<T, object>>[] includes)
        {
            var resultList = _dbSet.FromSqlRaw(queryString);
            includes.ToList().ForEach(prop => resultList = resultList.Include(prop));
            return resultList;
        }

        public void Update(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            _context.Entry(item).State = EntityState.Modified;
            _dbSet.Update(item);
        }

        public void ExecuteQuery(string queryString, params object[] parameters)
        {
            _context.Database.ExecuteSqlRaw(queryString, parameters);
        }

        public IQueryable<T> GetQuery(params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            includes.ToList().ForEach(prop => query = query.Include(prop));
            return query;
        }

        public void Deactualize(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (item.IsActual)
            {
                item.IsActual = false;
                item.DeactualizeTime = DateTime.UtcNow;
                _context.Entry(item).State = EntityState.Modified;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void ExecuteQuery(string queryString, params string[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
