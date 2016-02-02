using System.Web;
using FashionStore.Application.Cart.Interfaces;
using FashionStore.Application.Cart.Interfaces.Provider;
using FashionStore.Core.AppValue;

namespace FashionStore.WorkFlow.Cart
{
    class CartProvider : ICartProvider<UserOrder>
    {
        public ICart<UserOrder> GetCart()
        {
            var cart = HttpContext.Current.Session[ValuesApp.Cart];
            if (cart == null)
            {
                cart = new UserCart();
                HttpContext.Current.Session[ValuesApp.Cart] = cart;
            }
            return (UserCart)cart;
        }
    }
    
}