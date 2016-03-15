namespace FashionStore.WorkFlow.ViewedStorage.Provider.Interface
{
    public interface IRecentlyViewedProvider
    {
        RecentlyViewedStorage TryGet(string key, short size);
    }
}