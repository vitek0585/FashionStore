namespace FashionStore.WorkFlow.UserSession.Interfaces
{
    public interface IClearUserSession
    {
        void ClearByKey(params string[] keys);
        void ClearAll();
    }
}