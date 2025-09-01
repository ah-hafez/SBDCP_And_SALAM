using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MCS.Framework.Web
{
    public class HttpApplicationBase : HttpApplication
    {
        public HttpApplicationBase()
        {
            
            BeginRequest += new EventHandler(HttpApplicationBase_BeginRequest);
            EndRequest += new EventHandler(HttpApplicationBase_EndRequest);
        }

        public virtual void HttpApplicationBase_BeginRequest(object sender, EventArgs e)
        {
           
        }

        public virtual void HttpApplicationBase_EndRequest(object sender, EventArgs e)
        {
        }

        public virtual void Application_Start(object sender, EventArgs e)
        {
            
        }

        public virtual void Application_End(object sender, EventArgs e)
        {
            
        }

        public override void Dispose()
        {
        }
    }
}
