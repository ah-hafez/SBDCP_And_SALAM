using MCS.Common.ApiControllerResults;
using MCS.Common;
using MCS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI;
using MCS.Business;
using MCS.Framework;
using MCS.Service.Mappers;
using MCS.Domain;
using MCS.Framework.Exceptions;
using MCS.Framework.Persistence;
using MCS.Domain.Search.SearchCriteria;
using System.Runtime.Remoting.Contexts;
using MCS.Common.TransactionContext;

namespace MCS.Service.Controllers
{
    [CustomAuthenticationAttribute]
    public class SharepointIntegrationController : ApiController
    {
        protected readonly ITransactionContextScopeFactory context = IoC.Resolve<ITransactionContextScopeFactory>();

        [HttpGet]
        public HttpResponseMessage GetTransactionsByUsername([FromUri] BaseSearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<IList<BasicTransactionDto>> getResult = null;
            int rowsCount = 0;
            if (searchCriteria.OrderBy == null || searchCriteria.OrderBy == "")
            {
                searchCriteria.OrderBy = "Id";
            }

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFileBL fileBL = new FileBL();

                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                    IList<Transaction> transactions = TransactionBL.GetTransactionByUsername(searchCriteria, out rowsCount);

                    List<BasicTransactionDto> basicTransactionDtos = TransactionMapper.BasicMap(transactions);

                    getResult = GetResult<IList<BasicTransactionDto>>.Create(statusCode, basicTransactionDtos, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<IList<BasicTransactionDto>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<IList<BasicTransactionDto>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }





    }
}
