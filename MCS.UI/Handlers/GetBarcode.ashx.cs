using System;
using System.Web;

namespace MCS.UI.Handlers
{
    /// <summary>
    /// Summary description for GetBarcode
    /// </summary>
    public class GetBarcode : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            byte[] data = null;

            var token = context.Request.QueryString["token"].Trim();
            if (Guid.Parse(token) != Guid.Empty)
                data = context.Cache[token] as byte[];

            if (data != null)
            {
                context.Response.BinaryWrite(data);
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.ContentType = "image/png";
            }
            else
            {
                context.Response.StatusCode = 404;
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