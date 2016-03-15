using System.Web;
using FashionStore.Core.AppValue;
using FashionStore.WorkFlow.Cart.Interfaces.Provider;
using FashionStore.WorkFlow.UserSession.Interfaces;

namespace FashionStore.WorkFlow.UserSession
{
    public class UserSession : IClearUserSession
    {
        public void ClearByKey(params string[] keys)
        {
            lock (HttpContext.Current.Session.SyncRoot)
            {
                foreach (var key in keys)
                {
                    var session = HttpContext.Current.Session;
                    var cart = session[key];
                    if (cart != null)
                    {
                        session[key] = null;
                    }
                }
            }
        }

        public void ClearAll()
        {
            lock (HttpContext.Current.Session.SyncRoot)
            {
                HttpContext.Current.Session.Clear();
            }
        }

    }
}