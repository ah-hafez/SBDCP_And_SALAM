using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
    public class ReportController : ApiBaseController
    {
        // POST: TransactionReportSearch
        [HttpPost]
        public HttpResponseMessage TransactionReportSearch(SearchCriteriaTransactionReportDTO searchCriteriaTransactionReportDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionReportResultDTO>> getResult = null;
            int TotalCount = 0;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    SearchCriteriaTransactionReport searchCriteriaTransactionReport = ReportMapper.Map(searchCriteriaTransactionReportDTO);
                    List<TransactionReportResult> TransactionReportResults = ReportBL.TransactionReportSearch(searchCriteriaTransactionReport, out TotalCount);
                    var result = ReportMapper.Map(TransactionReportResults);
                    getResult = GetResult<List<TransactionReportResultDTO>>.Create(statusCode, result, null);
                    getResult.RowsCount = TotalCount;
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<TransactionReportResultDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<TransactionReportResultDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage SecretaryTransactionReportSearch(SearchCriteriaTransactionReportDTO searchCriteriaTransactionReportDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionReportResultDTO>> getResult = null;
            int TotalCount = 0;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    SearchCriteriaTransactionReport searchCriteriaTransactionReport = ReportMapper.Map(searchCriteriaTransactionReportDTO);
                    List<TransactionReportResult> TransactionReportResults = ReportBL.SecretaryTransactionReportSearch(searchCriteriaTransactionReport, out TotalCount);
                    var result = ReportMapper.Map(TransactionReportResults);
                    getResult = GetResult<List<TransactionReportResultDTO>>.Create(statusCode, result, null);
                    getResult.RowsCount = TotalCount;
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<TransactionReportResultDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<TransactionReportResultDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage SentTransactionReportSearch(SearchCriteriaTransactionReportDTO searchCriteriaTransactionReportDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<SentTransactionReportResultDTO>> getResult = null;
            int TotalCount = 0;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    SearchCriteriaTransactionReport searchCriteriaTransactionReport = ReportMapper.Map(searchCriteriaTransactionReportDTO);
                    List<SentTransactionReportResult> TransactionReportResults = ReportBL.SentTransactionReportSearch(searchCriteriaTransactionReport, out TotalCount);
                    var result = ReportMapper.Map(TransactionReportResults);
                    getResult = GetResult<List<SentTransactionReportResultDTO>>.Create(statusCode, result, null);
                    getResult.RowsCount = TotalCount;
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<SentTransactionReportResultDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<SentTransactionReportResultDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage SentTransactionReportStatusSearch(SearchCriteriaTransactionReportDTO searchCriteriaTransactionReportDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<SentTransactionReportResultDTO>> getResult = null;
            int TotalCount = 0;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    SearchCriteriaTransactionReport searchCriteriaTransactionReport = ReportMapper.Map(searchCriteriaTransactionReportDTO);
                    List<SentTransactionReportResult> TransactionReportResults = ReportBL.SentTransactionReportStautsSearch(searchCriteriaTransactionReport, out TotalCount);
                    var result = ReportMapper.Map(TransactionReportResults);
                    getResult = GetResult<List<SentTransactionReportResultDTO>>.Create(statusCode, result, null);
                    getResult.RowsCount = TotalCount;
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<SentTransactionReportResultDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<SentTransactionReportResultDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage TasksReportSearch(SearchCriteriaTransactionReportDTO searchCriteriaTransactionReportDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TaskReportResultDTO>> getResult = null;
            int TotalCount = 0;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    SearchCriteriaTransactionReport searchCriteriaTransactionReport = ReportMapper.Map(searchCriteriaTransactionReportDTO);
                    List<TaskReportResult> TransactionReportResults = ReportBL.TasksReportSearch(searchCriteriaTransactionReport, out TotalCount);
                    var result = ReportMapper.Map(TransactionReportResults);
                    getResult = GetResult<List<TaskReportResultDTO>>.Create(statusCode, result, null);
                    getResult.RowsCount = TotalCount;
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<TaskReportResultDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<TaskReportResultDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage FollowupReportSearch(SearchCriteriaTransactionReportDTO searchCriteriaTransactionReportDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<FollowupReportResultDTO>> getResult = null;
            int TotalCount = 0;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    SearchCriteriaTransactionReport searchCriteriaTransactionReport = ReportMapper.Map(searchCriteriaTransactionReportDTO);
                    List<FollowupReportResult> TransactionReportResults = ReportBL.FollowupReportSearch(searchCriteriaTransactionReport, out TotalCount);
                    var result = ReportMapper.Map(TransactionReportResults);
                    getResult = GetResult<List<FollowupReportResultDTO>>.Create(statusCode, result, null);
                    getResult.RowsCount = TotalCount;
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<FollowupReportResultDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<FollowupReportResultDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        // POST: PerformanceMeasurementReportSearch
        [HttpPost]
        public HttpResponseMessage PerformanceMeasurementReportSearch(SearchCriteriaPerformanceMeasurementDTO searchCriteriaPerformanceMeasurementDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<PerformanceMeasurementReportResultDTO>> getResult = null;
            int TotalCount = 0;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    SearchCriteriaPerformanceMeasurementReport searchCriteriaTransactionReport = ReportMapper.Map(searchCriteriaPerformanceMeasurementDTO);
                    List<PerformanceMeasurementReportResult> TransactionReportResults = ReportBL.PerformanceMeasurementReportSearch(searchCriteriaTransactionReport, out TotalCount);
                    var result = ReportMapper.Map(TransactionReportResults);
                    getResult = GetResult<List<PerformanceMeasurementReportResultDTO>>.Create(statusCode, result, null);
                    getResult.RowsCount = TotalCount;
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<PerformanceMeasurementReportResultDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<PerformanceMeasurementReportResultDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
    }
}