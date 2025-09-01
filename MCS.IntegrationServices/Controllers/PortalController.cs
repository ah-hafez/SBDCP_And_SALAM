using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Services;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;

namespace MCS.IntegrationServices.Controllers
{
    public class PortalController : ApiController
    {
        // GET: Portal

        class TransactionDetailsDTO
        {
            public long Number { get; set; }
            public string Subject { get; set; }
            public string Priority { get; set; }
            public string Confidentiality { get; set; }
        }
        public HttpResponseMessage GetTransaction(int year, int transactionNumber)
        {
            var transactionDetailsDTO = HttpClientWrapper<GetResult<TransactionDetailsDTO>>.GetItemRequest($"api/Transaction/GetTransactionByNumberAndYear?year={year}&transactionNumber={transactionNumber}");

            return Request.CreateResponse(HttpStatusCode.OK, transactionDetailsDTO.Result);
        }
        public HttpResponseMessage GetTransactionsByNationalId(string nationalId)
        {
            var transactionDetailsDTOs = HttpClientWrapper<GetResult<List<TransactionDetailsDTO>>>.GetItemRequest($"api/Transaction/GetTransactionsByNationalId?nationalId={nationalId}");
            return Request.CreateResponse(HttpStatusCode.OK, transactionDetailsDTOs.Result);
        }

    }
}