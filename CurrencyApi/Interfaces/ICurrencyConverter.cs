namespace CurrencyApi.Interfaces
{
    public interface ICurrencyConverter
    {
        decimal ConvertUsdTo(decimal value, string convertTo);
        decimal ConvertWithCeiling(decimal value, string convertTo);
    }
}