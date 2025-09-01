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
using MCS.IntegrationServices.Models.Sharepoints;

namespace MCS.IntegrationServices.Controllers
{
    [BasicAuthentication]
    public class TransactionController : BaseApiController
    {

        [HttpGet]
        public IHttpActionResult GetTransactions([FromUri] TransactionBaseRequest request)
        {

            GetTransactionResponse response = new GetTransactionResponse();
            try
            {
                var signature = Request.Headers.Where(x => x.Key == "Signature").FirstOrDefault();
                if (signature.Value == null)
                {
                    response.ResponseCode = ResponseCodeConst.SignatureMismatch;
                    response.ResponseMessage = "UnAuthorized Request";
                    return Json(response);
                }

                if (!ModelState.IsValid)
                {
                    response.ResponseCode = ResponseCodeConst.ValidationError;
                    response.ResponseMessage = string.Join("<br/> ", ModelState.Values
                           .SelectMany(x => x.Errors)
                           .Select(x => x.ErrorMessage));

                    return Json(response);
                }


             
                if (!IsValidDate(request.RequestDate))
                {

                    response.ResponseCode = ResponseCodeConst.UnAuthorizedRequest;
                    response.ResponseMessage = "Expired Request or Invalid Datetime";
                    return Json(response);
                }


                if (IsDuplicateSignature(signature.Value.FirstOrDefault()))
                {

                    response.ResponseCode = ResponseCodeConst.UnAuthorizedRequest;
                    response.ResponseMessage = "Duplicate Signature";
                    return Json(response);
                }

                
                var targetUserProfile = GetUserProfile(request.UserName);
                if (targetUserProfile.Result == null || targetUserProfile.Result.Id == 0)
                {
                    response.ResponseCode = ResponseCodeConst.UserNotFound;
                    response.ResponseMessage = "user not found ! ";
                    return Json(response);

                }
                var userProfile = GetUserProfile("");
                var requestBody = request.PageIndex.ToString() + request.PageSize.ToString() + request.RequestDate + request.UserName;

                var isValidKey = AESThenHMAC.IsValidateKey(requestBody, signature.Value.FirstOrDefault(), userProfile.Result.ApiKey);
                if (!isValidKey)
                {
                    response.ResponseCode = ResponseCodeConst.SignatureMismatch;
                    response.ResponseMessage = "UnAuthorized Request";
                    return Json(response);
                }



                GetResult<List<BasicTransactionDto>> transactionDtos = HttpClientWrapper<GetResult<List<BasicTransactionDto>>>.
                    GetItemRequest(string.Format("api/SharepointIntegration/GetTransactionsByUsername?PageIndex={0}&PageSize={1}&CultureName={2}&UserId={3}",
                request.PageIndex, request.PageSize, SessionInfo.CultureShortName, targetUserProfile.Result.Id)).Result;
                response.Transactions = TransactionMapper.Map(transactionDtos.Result, targetUserProfile.Result);
                response.TotalRecord = transactionDtos.RowsCount ?? 0;
                response.PageIndex = request.PageIndex;
                response.PageSize = request.PageSize;
                response.ResponseCode = ResponseCodeConst.Success;
                return Json(response);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                response.ResponseCode = ResponseCodeConst.InternalServerError;
                response.ResponseMessage = "Internal Server Error";
                return Json(response);
            }

        }


    }


}
