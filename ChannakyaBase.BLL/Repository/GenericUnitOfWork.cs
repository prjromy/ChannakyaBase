using ChannakyaBase.DAL.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ChannakyaBase.BLL.Repository
{
    public sealed class GenericUnitOfWork : IGenericUnitOfWork, IDisposable
    {
        private ChannakyaBaseEntities entities = null;
        public GenericUnitOfWork()
        {

            entities = new ChannakyaBaseEntities();
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
        public IQueryable<FGetCustList_Result> GetCustList()
        {
            var listCust = entities.FGetCustList();
            return listCust;
        }
        public IQueryable<FGetLocationTB_Result> GetLocationList()
        {
            var locationList = entities.FGetLocationTB();
            return locationList;
        }
        public EntityDatabaseTransaction BeginTransaction()
        {
            return new EntityDatabaseTransaction(entities);
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
        public ChannakyaBaseEntities GetContext()
        {
            ChannakyaBaseEntities chaBase = new ChannakyaBaseEntities();
            return chaBase;

        }
    }
}