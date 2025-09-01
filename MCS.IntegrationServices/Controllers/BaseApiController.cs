using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Common.Utility;
using MCS.DTO;
using MCS.IntegrationServices.Common;
using MCS.IntegrationServices.Mappers;
using MCS.IntegrationServices.Models.IAM.User;
using MCS.IntegrationServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using MCS.Framework.Logging;
using MCS.Framework.Exceptions;
using Swashbuckle.Swagger;
using System.Web.Services.Description;
using MCS.IntegrationServices.Models.IAM.Role;

namespace MCS.IntegrationServices.Controllers
{
    [BasicAuthentication]
    public class BaseApiController : ApiController
    {

        protected GetResult<UserProfileDTO> GetUserProfile(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                AuthenticationIdentity authenticationIdentity = Thread.CurrentPrincipal.Identity as AuthenticationIdentity;
                username = authenticationIdentity.UserName;
            }
            GetResult<UserProfileDTO> userprofileDto = HttpClientWrapper<GetResult<UserProfileDTO>>.
           GetItemRequest(string.Format("api/IAM/GetUserProfileByName?username={0}", username)).Result;

            return userprofileDto;

        }


        protected bool IsDuplicateSignature(string signature)
        {

            GetResult<int> userprofileDto = HttpClientWrapper<GetResult<int>>.
           GetItemRequest(string.Format("api/AuditLog/GetLogBySignature?signature={0}", signature)).Result;

            return userprofileDto.Result > 0;

        }
        protected bool IsValidDate(string requestDate)
        {
            DateTime dateTime = DateTime.MinValue;
            DateTime.TryParseExact(requestDate, "dd-MM-yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out dateTime);
            if (dateTime == DateTime.MinValue || dateTime.AddMinutes(5) < DateTime.Now || dateTime > DateTime.Now)
            {

                return false;
            }

            return true;
        }



    }


}
