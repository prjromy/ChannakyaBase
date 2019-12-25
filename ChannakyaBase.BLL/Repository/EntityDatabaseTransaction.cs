using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.BLL.Repository
{
   
    public class EntityDatabaseTransaction : IDisposable
    {
        private DbContextTransaction _transaction;
      
        public EntityDatabaseTransaction(DbContext context)
        {
            
            _transaction = context.Database.BeginTransaction();
        }
        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }
        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}
