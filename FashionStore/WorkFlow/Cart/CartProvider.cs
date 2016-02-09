using System.Web;
using FashionStore.Core.AppValue;
using FashionStore.Models.Order;
using FashionStore.WorkFlow.Cart.Interfaces;
using FashionStore.WorkFlow.Cart.Interfaces.Provider;

namespace FashionStore.WorkFlow.Cart
{
    class CartProvider : ICartProvider<UserOrderModel>
    {
        public ICart<UserOrderModel> GetCart()
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