namespace FashionStore.WorkFlow.Cart.Interfaces.Provider
{
    public interface ICartProvider<TItem>
    {
        ICart<TItem> GetCart();
    }

    
}