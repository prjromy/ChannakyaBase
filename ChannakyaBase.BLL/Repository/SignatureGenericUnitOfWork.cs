using ChannakyaBase.DAL.SignatureModel;
//using global::ChannakyaBase.DAL.SignatureModel;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ChannakyaBase.BLL.Repository
{
    public sealed class SignatureGenericUnitOfWork : IGenericUnitOfWork, IDisposable
    {
        private SignatureEntities entities = null;
        public SignatureGenericUnitOfWork()
        {

            entities = new SignatureEntities();
        }
        public Dictionary<Type, object> reprositories = new Dictionary<Type, object>();
        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (reprositories.Keys.Contains(typeof(T)) == true)
            {
                return reprositories[typeof(T)] as IGenericRepository<T>;
            }
            IGenericRepository<T> repo = new GenericRepository<T>(entities);
            reprositories.Add(typeof(T), repo);
            return repo;
        }
        public int ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return entities.Database.ExecuteSqlCommand("EXEC " + query, parameters);
        }

        public int Commit()
        {
            return entities.SaveChanges();
        }



        private bool disposed = false;
        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    entities.Dispose();

                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IQueryable<DAL.DatabaseModel.FGetCustList_Result> GetCustList()
        {
            throw new NotImplementedException();
        }

        public EntityDatabaseTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }
    }
}


