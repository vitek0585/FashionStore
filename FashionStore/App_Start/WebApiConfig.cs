using System.Web.Http;
using FashionStore.Configuration.JSON;
using FashionStore.Core.HttpParameterBinding;
using FashionStore.Core.WebApiFormatters;
using FashionStore.Models.MediaFormatter;

namespace FashionStore
{

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            JsonConfiguration.Setup(config);
            config.Formatters.Add(new FormDataFormatter<GoodsFileModel>());
            config.Formatters.Add(new FormDataOnlyFileFormatter());
            config.ParameterBindingRules.Insert(0, PostPutVariableParameterBinding.HookupParameterBinding);

        }
    }
}
