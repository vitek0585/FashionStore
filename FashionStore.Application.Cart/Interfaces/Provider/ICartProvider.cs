namespace FashionStore.Application.Cart.Interfaces.Provider
{
    public interface ICartProvider<TItem>
    {
        ICart<TItem> GetCart();
    }

    
}