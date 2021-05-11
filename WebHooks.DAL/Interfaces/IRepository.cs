using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetActual(params Expression<Func<T, object>>[] includes);
        T Get(Int64 id, params Expression<Func<T, object>>[] includes);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        void Create(T item);
        void Update(T item);
        void Delete(T item);
        void Deactualize(T item);
        void Save();
        IQueryable<T> GetQuery(params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetQuery(string queryString, params Expression<Func<T, object>>[] includes);
        void ExecuteQuery(string queryString, params object[] parameters);
    }
}
