using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FashionStore.Configuration.JSON
{
    public class JsonConfiguration
    {
        public static void Setup(HttpConfiguration configuration)
        {
            configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                //DateFormatString = "dd.MM.yyyy",
                
#if DEBUG
                Formatting = Formatting.Indented
#endif
                
            };
           
        }
        
    }
}