using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace MCS.UI.Helpers
{
    public static class JsonHelper
    {
        public static string ToJson(this HtmlHelper html, object obj)
        {
            JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Error
            };
            var scriptSerializer = JsonSerializer.Create(Settings);
            StringWriter strWriter = new StringWriter(new StringBuilder());
            // Serialize the data to the Output stream of the response
            scriptSerializer.Serialize(strWriter, obj);
            return strWriter.ToString();
        }
    }
}