using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using MCS.Framework.Encryption;

namespace MCS.UI.Common
{
    public class CustomActionAttribute : FilterAttribute, IActionFilter
    {
        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Dictionary<string, object> decryptedParameters = new Dictionary<string, object>();

            if (((ReflectedActionDescriptor)filterContext.ActionDescriptor).ActionName == "PendingExecuting")
            {
                List<int> transactionIds = new List<int>();
                foreach (var item in filterContext.HttpContext.Request.QueryString["transactionsIds"].Split(','))
                {
                    transactionIds.Add(Convert.ToInt32(AESEncrytDecry.Base64Decode(item)));
                }

                filterContext.ActionParameters["transactionsIds"] = transactionIds;
                if (filterContext.HttpContext.Request.QueryString["type"] != null)
                {
                    filterContext.ActionParameters["type"] = AESEncrytDecry.Base64Decode(filterContext.HttpContext.Request.QueryString["type"]);
                }
                if (filterContext.HttpContext.Request.QueryString["tabId"] != null)
                {
                    filterContext.ActionParameters["tabId"] = AESEncrytDecry.Base64Decode(filterContext.HttpContext.Request.QueryString["tabId"]);
                }
            }
            else
            {
                foreach (string key in filterContext.HttpContext.Request.QueryString.AllKeys)
                {
                    if (key.ToLower() == "returnurl")
                    {
                        continue;
                    }
                    decryptedParameters.Add(key, AESEncrytDecry.Base64Decode(filterContext.HttpContext.Request.QueryString[key]));
                }

                for (int i = 0; i < filterContext.RouteData.Values.Keys.Count; i++)
                {
                    if (filterContext.RouteData.Values.Keys.ElementAt(i).ToString() != "controller" &&
                        filterContext.RouteData.Values.Keys.ElementAt(i).ToString() != "action")
                    {
                        decryptedParameters.Add(filterContext.RouteData.Values.Keys.ElementAt(i).ToString(), AESEncrytDecry.Base64Decode(filterContext.RouteData.Values.Values.ElementAt(i).ToString()));
                    }
                }
                try { 
              
                for (int i = 0; i < decryptedParameters.Count; i++)
                {
                    filterContext.ActionParameters[decryptedParameters.Keys.ElementAt(i)] = Convert.ChangeType(decryptedParameters.Values.ElementAt(i), filterContext.ActionDescriptor.GetParameters().First(x => x.ParameterName == decryptedParameters.Keys.ElementAt(i).ToString()).ParameterType);
                }

                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}