using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.BLL.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IQueryable<T> Queryable();
        IQueryable<T> FindByMany(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        T GetSingle(Expression<Func<T, bool>> predicate);
        void Attach(T entity);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void AddRange(IEnumerable<T> entity);
        IEnumerable<T> SqlQuery(string query, params object[] parameters);

        void Save();
    }
}
