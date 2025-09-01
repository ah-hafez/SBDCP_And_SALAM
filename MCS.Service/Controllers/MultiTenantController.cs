using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using MCS.Framework;
using MCS.Framework.Exceptions;
using MCS.Business;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Domain;
using MCS.DTO;
using MCS.Service.Mappers;

namespace MCS.Service.Controllers
{
    [CustomAuthenticationAttribute]
    public class MultiTenantController : ApiBaseController
    {
        public HttpResponseMessage GetTenantInfo(string username, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TenantDTO> getResult = null;
            try
            {
                ITenantBL tenantBL = IoC.Resolve<ITenantBL>();
                var tenant = tenantBL.GetTenantByUserName(username, cultureName);
                TenantDTO tenantDTO = TenantMapper.MapTenant(tenant);
                getResult = GetResult<TenantDTO>.Create(statusCode, tenantDTO, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<TenantDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<TenantDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
    }
}