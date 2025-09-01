using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using MCS.Tenants.Service.Mappers;
using MCS.Tenants.Service.Results;
using MCS.Business;
using MCS.DTO;
using MCS.Tenants.Service.Service.Filters;
using MCS.DTO.Tenants;
using MCS.Framework.Persistence;
using System.Net.Http;
using MCS.Common.ApiControllerResults;
using System.Net;
using System;
using MCS.Common;

namespace MCS.Tenants.Service.Controllers.API
{
    [Authorization]
    [RoutePrefix("api/tenant")]
    public class TenantController : BaseApiController
    {

        [HttpGet, Route("getAllTenants")]
        public HttpResponseMessage GetAllTenants()
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TenantDTO>> getResult = null;
            try
            {
                TenantBL tenantBL = new TenantBL();
                var result = tenantBL.GetAllTenants("ar").Select(x => x.ToTenantDTO()).ToList();
                getResult = GetResult<List<TenantDTO>>.Create(statusCode, result, result.Count);
                return Request.CreateResponse(HttpStatusCode.OK, getResult.Result);
            }
            catch (Exception ex)
            {
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<TenantDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost, Route("search")]
        public HttpResponseMessage GetTenants(SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TenantDTO>> getResult = null;
            try
            {
                TenantBL tenantBL = new TenantBL();
                var result = tenantBL.GetTenants(searchCriteria, "ar", out int rowsCount);
                var tenantDTOs = result.Select(x => x.ToTenantDTO()).ToList();
                getResult = GetResult<List<TenantDTO>>.Create(statusCode, tenantDTOs, rowsCount);
                return Request.CreateResponse(HttpStatusCode.OK, tenantDTOs);
            }
            catch (Exception ex)
            {
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<TenantDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost, Route("save")]
        public HttpResponseMessage AddTenant(TenantDTO model, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            try
            {
                TenantBL tenantBL = new TenantBL();
                var result = tenantBL.AddTenant(model.ToTenant(), cultureName);
                getResult = GetResult<int>.Create(statusCode, result, null);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<int>.Create(statusCode, -1, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost, Route("AddEditUserTenant")]
        public HttpResponseMessage AddEditUserTenant(UserTenantDTO model)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TenantDTO> getResult = null;
            try
            {
                TenantBL tenantBL = new TenantBL();
                var tenantId = tenantBL.AddEditUserTenant(model.ToTenant());
                getResult = GetResult<TenantDTO>.Create(statusCode, new TenantDTO { Id = tenantId }, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<TenantDTO>.Create(statusCode, new TenantDTO { Id = 0 }, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet, Route("getAllUserTenants")]
        public HttpResponseMessage GetAllUserTenants()
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserTenantDTO>> getResult = null;
            try
            {
                TenantBL tenantBL = new TenantBL();
                var result = tenantBL.GetAllUserTenants().Select(x => x.ToTenantDTO()).ToList();
                getResult = GetResult<List<UserTenantDTO>>.Create(statusCode, result, result.Count);
                return Request.CreateResponse(HttpStatusCode.OK, getResult.Result);
            }
            catch (Exception ex)
            {
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<UserTenantDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost, Route("update")]
        public HttpResponseMessage UpdateTenant(TenantDTO model)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            try
            {
                TenantBL tenantBL = new TenantBL();
                tenantBL.UpdateTenant(model.ToTenant());
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<int>.Create(statusCode, -1, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet, Route("byId/{id}")]
        public HttpResponseMessage GetTenantById(int id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TenantDTO> getResult = null;
            try
            {
                TenantBL tenantBL = new TenantBL();
                var resutl = tenantBL.GetTenantById(id).ToTenantDTO();
                getResult = GetResult<TenantDTO>.Create(statusCode, resutl, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult.Result);
            }
            catch (Exception ex)
            {
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<TenantDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet, Route("userTenantById/{id}")]
        public HttpResponseMessage GetUserTenantById(int id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<UserTenantDTO> getResult = null;
            try
            {
                TenantBL tenantBL = new TenantBL();
                var userTenantDTO = tenantBL.GetUserTenantById(id).ToTenantDTO();
                getResult = GetResult<UserTenantDTO>.Create(statusCode, userTenantDTO, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult.Result);
            }
            catch (Exception ex)
            {
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<UserTenantDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost, Route("activate")]
        public HttpResponseMessage ActivateTenant(TenantDTO model)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            try
            {
                TenantBL tenantBL = new TenantBL();
                tenantBL.ActivateTenant(model.Id, model.IsActive);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<int>.Create(statusCode, -1, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost, Route("delete")]
        public HttpResponseMessage DeleteTenants(IList<int> ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            try
            {
                TenantBL tenantBL = new TenantBL();
                tenantBL.DeleteTenants(ids);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, -1, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<int>.Create(statusCode, -1, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost, Route("DeleteUserTenant")]
        public HttpResponseMessage DeleteUserTenant(int id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            try
            {
                TenantBL tenantBL = new TenantBL();
                tenantBL.DeleteUserTenant(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<int>.Create(statusCode, -1, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<int>.Create(statusCode, -1, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost, Route("sendResetPasswordEmail")]
        public HttpResponseMessage SendTanentResetPasswordEmail(TenantDTO tenantDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            try
            {
                TenantBL tenantBL = new TenantBL();
                tenantBL.SendTanentResetPasswordEmail(tenantDTO.Id, "ar");
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<int>.Create(statusCode, -1, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet, Route("allCultures")]
        public IHttpActionResult GetTenantCultures()
        {
            TenantBL tenantBL = new TenantBL();
            var data = tenantBL.GetTenantCultures();
            var result = data.Select(x => x.ToTenantCultureDTO()).ToList();
            result.ForEach(d =>
            {
                var x = data.FirstOrDefault(a => a.Id == d.Id);
                if (x != null)
                {
                    d.Name = x.Name?.ToTenantLookupDTO();
                }
            });
            return Ok(result);
        }
    }
}
