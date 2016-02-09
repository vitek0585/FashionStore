namespace FashionStore.WorkFlow.Cart.Interfaces.Provider
{
    public interface IClearUserCart
    {
        void ClearCart();
    }
    public interface ICartProvider<TItem>
    {
        ICart<TItem> GetCart();
        
    }

    
}