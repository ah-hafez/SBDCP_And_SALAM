using MobileApi.Models;
using Newtonsoft.Json;

namespace MobileApi.Common
{
    public class JsonConverter
    {
        public static string JsonSerializer(AccessToken token)
        {
            return JsonConvert.SerializeObject(token, Formatting.Indented, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            });
        }
        public static AccessToken JsonDeserializer(string token)
        {
            return JsonConvert.DeserializeObject<AccessToken>(token);
        }
    }
}