using System;
using System.Drawing;
using System.IO;
using System.Web;

namespace MCS.UI.Handlers
{
    /// <summary>
    /// Summary description for AuthSign
    /// </summary>
    public class AuthSign : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            byte[] data = null;
            var binaryData = context.Request.Params["binaryData"].Trim();
             data = Convert.FromBase64String(binaryData);
            var token = Guid.NewGuid().ToString();
            context.Cache.Add(token, data, null, DateTime.Now.AddMinutes(15),
                System.Web.Caching.Cache.NoSlidingExpiration,
                System.Web.Caching.CacheItemPriority.Normal, null);
            context.Response.Write(token);
            context.Response.ContentType = "text/plain";
        }

        public Image Base64ToImage(string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}