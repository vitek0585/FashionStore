using System.Web;
using FashionStore.Core.AppValue;
using FashionStore.WorkFlow.ViewedStorage.Provider.Interface;

namespace FashionStore.WorkFlow.ViewedStorage.Provider
{
    public class RecentlyViewedProvider:IRecentlyViewedProvider
    {
        public RecentlyViewedStorage TryGet(string key,short size)
        {
            var session = HttpContext.Current.Session;
            var viewed = (RecentlyViewedStorage)session[ValuesApp.RecentlyViewed];
            if (viewed == null)
            {
                session[ValuesApp.RecentlyViewed] = viewed = new RecentlyViewedStorage(size);
            }
            return viewed;
        }
        
    }
}