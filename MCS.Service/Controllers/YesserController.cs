//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using MCS.Framework.Exceptions;
//using MCS.Business;
//using MCS.Common;
//using MCS.Common.ApiControllerResults;
//using MCS.Domain;
//using MCS.DTO.HubTransaction;
//using MCS.Integration.HUB;
//using MCS.Service.Hubs;

//namespace MCS.Service.Controllers
//{

//    [CustomAuthenticationAttribute]
//    public class YESSERController : ApiBaseController
//    {
//        [HttpGet]
//        public HttpResponseMessage GetHubTransactions(int userId)
//        {
//            StatusCode statusCode = Common.StatusCode.Ok;
//            GetResult<List<HubTransactionDTO>> getResult = null;
//            try
//            {
//                using (var transactionContextScope = context.CreateReadOnly())
//                {
//                    IHubTransactionBL hubTransactionBL = new HubTransactionBL();

//                    IList<HubTransaction> transactions = hubTransactionBL.GetHubTransactions(userId);

//                    List<HubTransactionDTO> transactionsDTOs = HubTransactionMapper.Map(transactions);

//                    getResult = GetResult<List<HubTransactionDTO>>.Create(statusCode, transactionsDTOs, null);

//                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
//                }

//            }
//            catch (BusinessException BEX)
//            {
//                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), BEX.Message);

//                getResult = GetResult<List<HubTransactionDTO>>.Create(statusCode, null, null);

//                return Request.CreateResponse(HttpStatusCode.OK, getResult);
//            }
//            catch (Exception ex)
//            {
//                ExceptionHelper.HandleException(ex);

//                statusCode = Common.StatusCode.GeneralError;

//                getResult = GetResult<List<HubTransactionDTO>>.Create(statusCode, null, null);

//                return Request.CreateResponse(HttpStatusCode.OK, getResult);
//            }
//        }
//        [HttpGet]
//        public HttpResponseMessage RejectHubTransaction(int hubTransactionId, string rejectReason)
//        {
//            StatusCode statusCode = Common.StatusCode.Ok;
//            GetResult<HubTransactionDTO> getResult = null;
//            try
//            {
//                using (var transactionContextScope = context.CreateReadOnly())
//                {
//                    IHubTransactionBL hubTransactionBL = new HubTransactionBL();

//                    hubTransactionBL.RejectHubTransaction(hubTransactionId, rejectReason);

//                    getResult = GetResult<HubTransactionDTO>.Create(statusCode, null, null);

//                    HUBService service = new HUBService();
//                    // commented until the service UP 
//                    // service.SendReject(hubTransactionId , null , rejectReason); 

//                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
//                }

//            }
//            catch (BusinessException BEX)
//            {
//                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), BEX.Message);

//                getResult = GetResult<HubTransactionDTO>.Create(statusCode, null, null);

//                return Request.CreateResponse(HttpStatusCode.OK, getResult);
//            }
//            catch (Exception ex)
//            {
//                ExceptionHelper.HandleException(ex);

//                statusCode = Common.StatusCode.GeneralError;

//                getResult = GetResult<HubTransactionDTO>.Create(statusCode, null, null);

//                return Request.CreateResponse(HttpStatusCode.OK, getResult);
//            }
//        }
//        [HttpGet]
//        public HttpResponseMessage AcceptHubTransaction(int hubTransactionId, int userId)
//        {
//            StatusCode statusCode = Common.StatusCode.Ok;
//            GetResult<HubTransactionDTO> getResult = null;
//            try
//            {
//                using (var transactionContextScope = context.CreateReadOnly())
//                {
//                    IHubTransactionBL hubTransactionBL = new HubTransactionBL();

//                    hubTransactionBL.AcceptHubTransaction(hubTransactionId, userId);

//                    getResult = GetResult<HubTransactionDTO>.Create(statusCode, null, null);

//                    HUBService service = new HUBService();

//                    service.SendConfirm(hubTransactionId);

//                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
//                }

//            }
//            catch (BusinessException BEX)
//            {
//                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), BEX.Message);

//                getResult = GetResult<HubTransactionDTO>.Create(statusCode, null, null);

//                return Request.CreateResponse(HttpStatusCode.OK, getResult);
//            }
//            catch (Exception ex)
//            {
//                ExceptionHelper.HandleException(ex);

//                statusCode = Common.StatusCode.GeneralError;

//                getResult = GetResult<HubTransactionDTO>.Create(statusCode, null, null);

//                return Request.CreateResponse(HttpStatusCode.OK, getResult);
//            }
//        }
//    }
//}
