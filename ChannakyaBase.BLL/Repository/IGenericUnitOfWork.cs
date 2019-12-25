using ChannakyaBase.DAL.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.BLL.Repository
{
    public interface IGenericUnitOfWork:IDisposable
    {
        int Commit();
        IQueryable<FGetCustList_Result> GetCustList();
        int ExecWithStoreProcedure(string query, params object[] parameters);
         EntityDatabaseTransaction BeginTransaction();
    }

}
