using System;
using System.Collections.Specialized;
using System.IO;

namespace FashionStore.Core.AppValue
{

    public static class ValuesApp
    {
        private const string _productsImagePath = "~/Image/Products";

        public const string Cart = "userCart";
        public const string RecentlyViewed = "recentlyViewed";
        public const string RandomCategory = "randomCategory";
        public const string Language = "lang";
        public const string Currency = "currency";
        public const string IsAutorize = "isAutorize";

        public static string[] Currencies = { "uah", "usd" };
        public static string[] Languages = { "ru", "en" };

        public static string LanguageDefault
        {
            get { return Languages[0]; }
        }
        public static string CurrencyDefault
        {
            get { return Currencies[0]; }
        }


        public static ListDictionary CurrencySymbol = new ListDictionary(StringComparer.InvariantCultureIgnoreCase)
        {
            {new Tuple<string,string>(Languages[0],Currencies[0]),"грн"},
            {new Tuple<string,string>(Languages[1],Currencies[0]),"grn"},

            {new Tuple<string,string>(Languages[0],Currencies[1]),"$"},
            {new Tuple<string,string>(Languages[1],Currencies[1]),"$"}
        };

        public static string GetCurrencySymbol(string lang, string currency)
        {
            return CurrencySymbol[Tuple.Create(lang, currency)].ToString();
        }
        public static string ConvertImageNameToAbsolutePath(string name)
        {
            return Path.Combine(_productsImagePath, name);
        }

    }
}