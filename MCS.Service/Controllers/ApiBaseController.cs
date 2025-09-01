using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using MCS.Framework;
using MCS.Framework.MultiTenants;
using MCS.Framework.Security;
using MCS.Framework.Web;
using MCS.Business;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;
using MCS.Service.Mappers;
using Constants = MCS.Common.Constants;

namespace MCS.Service.Controllers
{
    public class ApiBaseController : ApiController
    {
        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            if (CurrentUserIdentity != null && UserContext.LoggedInUser == null)
            {
                var transactionContextScope = context.Create();

                IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                UserProfile userProfile = userManagementBL.GetUserByIdentity(CurrentUserIdentity, Language);

                User user = new User()
                {
                    Id = userProfile.Id,
                    IPAddress = GetUserIP(),
                    UserName = userProfile.UserName,
                    Email = userProfile.Email,
                    Claims = LoginUserMapper.MapClaimsGroup(userProfile.UserGroups[0].Group.Permissions)
                };

                UserContext.SetLoggedInUserInWebSession(user);



                if (UserContext.LoggedInUser != null)
                    UserContext.LoggedInUser.RequestId = Guid.NewGuid().ToString();
            }
            return base.ExecuteAsync(controllerContext, cancellationToken);
        }
        public string CurrentUserIdentity => User.Identity.GetUserId();

        public string Language
        {
            get
            {
                var culture = GetFirstHeaderValueOrDefault("Language", h => "ar", s => s);

                return culture;

            }
        }

        public int LoggedInOrgUnitId
        {
            get
            {
                int orgUnitId = GetFirstHeaderValueOrDefault<int>("OrgUnitId", h => "-1", s => int.Parse(s));
                return orgUnitId;
            }
        }

        protected T GetFirstHeaderValueOrDefault<T>(string headerKey, Func<HttpRequestMessage, string> defaultValue, Func<string, T> valueTransform)
        {
            IEnumerable<string> headerValues;
            HttpRequestMessage message = Request ?? new HttpRequestMessage();
            if (!message.Headers.TryGetValues(headerKey, out headerValues))
                return valueTransform(defaultValue(message));
            string firstHeaderValue = headerValues.FirstOrDefault() ?? defaultValue(message);
            return valueTransform(firstHeaderValue);
        }

        private string GetUserIP()
        {
            string ipAddress = string.Empty;

            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Request != null)
                {
                    HttpCookie userHostIPCookie = HttpContext.Current.Request.Cookies[Common.Constants.UserHostIPAddressKey];

                    ipAddress = (userHostIPCookie != null) ? userHostIPCookie.Value : string.Empty;
                }
            }

            return ipAddress;
        }

        protected readonly ITransactionContextScopeFactory context = IoC.Resolve<ITransactionContextScopeFactory>();
        public class UserLoggingInfo
        {
            public int UserId { get; set; }
            public int TransactionId { get; set; }
            public AuditingActionCode AuditingActionCode { get; set; }
        }

        public void LogAction(AuditingActionCode auditingActionCode, int transactionId)
        {
            ITransactionLoggingBL transactionLoggingBL = new TransactionLoggingBL();

            ILookupBL lookupBL = IoC.Resolve<ILookupBL>();

            TransactionLog transactionLog = new TransactionLog
            {
                UserId = UserContext.LoggedInUser.Id,
                AuditingActionCode = lookupBL.GetLookupItem(auditingActionCode.LookupIdentity(LookupCategory.AuditingActionCode, string.Empty)),
                TransactionId = transactionId,
                Date = DateTime.Now,
                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now)
            };

            transactionLoggingBL.Log(transactionLog);
        }
        public void SetViewdTransactionCopy(int transactionCopyId)
        {

            ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);
            transactionBL.SetViewedTransactionCopy(transactionCopyId);



        }

        public ApiBaseController()
        {
        }
    }
}
