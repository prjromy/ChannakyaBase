using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;

namespace ChannakyaBase.BLL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DbContext _entities;
        protected readonly IDbSet<T> _dbset;
      
        public GenericRepository(DbContext context)
        {
            
            _entities = context;
            _dbset = context.Set<T>();
        }
        
        public void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public void Attach(T entity)
        {
            _dbset.Attach(entity);
        }

        public void Delete(T entity)
        {
            _dbset.Remove(entity);
        }

        public void Edit(T entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> query = _dbset.Where(predicate).AsEnumerable();
            return query;
        }

        public IEnumerable<T> GetAll()
        {
           //var ab = _entities.Set<CertificateDef>().AsEnumerable();
            return _dbset.AsEnumerable();
        }

        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            T query = _dbset.Where(predicate).FirstOrDefault();
            return query;
        }

     

        public void Save()
        {
            _entities.SaveChanges();
        }

        public void AddRange(IEnumerable<T> entity)
        {
            _entities.Set<T>().AddRange(entity);
        }

        public IEnumerable<T> SqlQuery(string query, params object[] parameters)
        {
            return _entities.Database.SqlQuery<T>(query, parameters);
        }

        public IQueryable<T> Queryable()
        {
            return _dbset.AsQueryable();
        }

        public IQueryable<T> FindByMany(Expression<Func<T, bool>> predicate)
        {
          return _dbset.Where(predicate).AsQueryable();
        }
    }
}