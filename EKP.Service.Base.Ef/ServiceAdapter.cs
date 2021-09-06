using System.Data.Entity;
using System.Transactions;
using EKP.Repository;
using EKP.Repository.Ef;

namespace EKP.Service.Base.Ef
{
    public delegate object ExecuteServiceCallBackHandler();

    internal class ServiceAdapter<TDbEntitie> : IServiceAdapter where TDbEntitie : DbContext, new()
    {
        internal DbContextAdapter<TDbEntitie> Adapter;

        public ServiceAdapter()
        {
            Adapter = new DbContextAdapter<TDbEntitie>();
        }

        public object ExecuteService(ExecuteServiceCallBackHandler callBackHandler, bool isOpenTransaction = true)
        {
            object ret = null;
            if (isOpenTransaction)
            {
                using (var ts = new TransactionScope())
                {
                    if (callBackHandler != null)
                        ret = callBackHandler();
                    ts.Complete();
                }
            }
            else
            {
                ret = callBackHandler();
            }

            return ret;
        }
    }
}