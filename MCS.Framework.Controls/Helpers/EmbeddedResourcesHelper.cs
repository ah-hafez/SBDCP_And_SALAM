using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Handlers;

namespace MCS.Framework.Controls
{
    public sealed class EmbeddedResourcesHelper
    {
        public static string WebResourceUrl(string resourceName, Type type)
        {
            string resourceUrl = string.Empty;

            List<MemberInfo> methodCandidates =
                typeof(AssemblyResourceLoader).GetMember("GetWebResourceUrlInternal",
                BindingFlags.NonPublic | BindingFlags.Static).ToList();

            foreach (var methodCandidate in methodCandidates)
            {
                var method = methodCandidate as MethodInfo;

                if (method == null || method.GetParameters().Length != 5)
                    continue;

                resourceUrl = string.Format("{0}", method.Invoke
                (
                    null,
                    new object[] { Assembly.GetAssembly(type), resourceName, false, false, null })
                );
                break;
            }

            return HttpContext.Current.Request.Url.Host + resourceUrl;
        }      
    }
}
