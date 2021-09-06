namespace EKP.Service.Base.Ef
{
    internal interface IServiceAdapter
    {
        object ExecuteService(ExecuteServiceCallBackHandler callBackHandler, bool isOpenTransaction = true);
    }
}