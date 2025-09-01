using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace MCS.Framework.Controls.Mvc
{
    /// <summary>
    /// <c>BarcodeImageRouteHandler</c> implements an HTTP route handler that
    /// allows barcode rendering to be used in extension-less scenarios.
    /// </summary>
    /// <remarks>
    /// To register a route using this handler add the following code
    /// 
    /// </remarks>
    public class BarcodeImageRouteHandler : IRouteHandler
    {
        /// <summary>
        /// Provides the object that processes the request.
        /// </summary>
        /// <param name="requestContext">
        /// An object that encapsulates information about the request.
        /// </param>
        /// <returns>An object that processes the request.</returns>
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new BarcodeImageHandler();
        }
    }
}
