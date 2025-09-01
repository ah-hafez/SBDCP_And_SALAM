using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using MCS.Framework;
using MCS.Framework.Exceptions;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Framework.Security;
using MCS.Framework.Web;
using MCS.Business;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DocRepository.DataDef;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;
using MCS.DTO;
using MCS.DTO.ExternalParties;
using MCS.DTO.Transaction;
using MCS.Service.Mappers;
using System.Text;
using System.Web;
using YESSER.NCS.MCS.Service.Helpers;
using HashMechanism;
using static MCS.Service.Controllers.TransactionController.CertificationClient;
using MCS.DTO.AdminAudit;
using static MCS.Common.UserClaims;

namespace MCS.Service.Controllers
{
    [CustomAuthenticationAttribute]
    public class AuditLogController : ApiBaseController
    {

        [HttpGet]
        public HttpResponseMessage GetAuditLog(string cultureName, bool IsForPrint, [FromUri] SearchCriteriaCustom searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<AuditLogDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IAuditLogBL AuditLogBL = IoC.Resolve<AuditLogBL>();
                        List<AuditLog> AuditLogs = AuditLogBL.GetAuditLog(cultureName, IsForPrint, searchCriteria, out int itemsCount).ToList();
                        List<AuditLogDTO> AuditLogDTOs = AuditLogMapper.Map(AuditLogs);
                        getResult = GetResult<List<AuditLogDTO>>.Create(statusCode, AuditLogDTOs, itemsCount);
                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    getResult = GetResult<List<AuditLogDTO>>.Create(statusCode, null, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<AuditLogDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<AuditLogDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }




        [HttpPost]
        public HttpResponseMessage AddApiLog(ApiAuditLogDto apiAuditLogDto)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IAuditingBL auditingBL = IoC.Resolve<AuditingBL>();

                        ApiAuditLog apiAuditLog = AuditLogMapper.Map(apiAuditLogDto);
                        auditingBL.AddApiAuditLog(apiAuditLog);
                        postResult = PostResult.Create(statusCode, apiAuditLog.Id);

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    postResult = PostResult.Create(statusCode, null);
                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage GetLogBySignature(string signature)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    IAuditingBL auditingBL = IoC.Resolve<AuditingBL>();


                    var result = auditingBL.GetLogBySignature(signature);

                    getResult = GetResult<int>.Create(statusCode, result, null);
                    return Request.CreateResponse(HttpStatusCode.Created, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
    }

}

