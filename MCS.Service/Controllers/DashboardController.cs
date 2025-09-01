using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MCS.Framework;
using MCS.Framework.Exceptions;
using MCS.Business;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Domain;
using MCS.DTO;
using MCS.Service.Mappers;
using MCS.DTO.Shared;

namespace MCS.Service.Controllers
{
    [CustomAuthenticationAttribute]
    public class DashboardController : ApiBaseController
    {
        [HttpGet]
        public HttpResponseMessage GetDashboardData(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<DashboardDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    List<DashboardDTO> dashboardDTOs = DashboardMapper.Map(TransactionBL.GetDashboardData(cultureName));

                    getResult = GetResult<List<DashboardDTO>>.Create(statusCode, dashboardDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<DashboardDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<DashboardDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetDashboardHomeData(DashboardFilterCriteria dashboardFilterCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<DashboardHomeDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    DateTime dateTimeFrom = DateTime.ParseExact(dashboardFilterCriteria.fromDate.Split(' ')[0], "dd/MM/yyyy", null);
                    DateTime dateTimeTo = DateTime.ParseExact(dashboardFilterCriteria.toDate.Split(' ')[0], "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).AddSeconds(59);
                    IDashboardHomeBL dashboardHomeBL = IoC.Resolve<DashboardHomeBL>();
                    DashboardHomeDTO dashboardHomeDTO = DashboardHomeMapper.Map(dashboardHomeBL.GetDashboardHome(dateTimeFrom, dateTimeTo, dashboardFilterCriteria.entityId, dashboardFilterCriteria.userId, dashboardFilterCriteria.level));
                    getResult = GetResult<DashboardHomeDTO>.Create(statusCode, dashboardHomeDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<DashboardHomeDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<DashboardHomeDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage GetDashboardDetails(DashboardFilterCriteria dashboardFilterCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionDetailsDTO>> getResult = null;
            int TotalCount = 0;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    DateTime dateTimeFrom = DateTime.ParseExact(dashboardFilterCriteria.fromDate.Split(' ')[0], "dd/MM/yyyy", null);
                    DateTime dateTimeTo = DateTime.ParseExact(dashboardFilterCriteria.toDate.Split(' ')[0], "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).AddSeconds(59);
                    IDashboardHomeBL dashboardHomeBL = IoC.Resolve<IDashboardHomeBL>();

                    List<DashboardTransactionDetails> dashboardTransactionDetailsList = dashboardHomeBL.GetDashboardDetails(dateTimeFrom, dateTimeTo, dashboardFilterCriteria.entityId, dashboardFilterCriteria.userId, dashboardFilterCriteria.level, dashboardFilterCriteria.itemId, dashboardFilterCriteria.cultureId, dashboardFilterCriteria.pageIndex, dashboardFilterCriteria.pageSize, out TotalCount);
                    List<TransactionDetailsDTO> TransactionDetailsDTOList = TransactionDetailsMapper.Map(dashboardTransactionDetailsList);
                    getResult = GetResult<List<TransactionDetailsDTO>>.Create(statusCode, TransactionDetailsDTOList, null);
                    getResult.RowsCount = TotalCount;

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionDetailsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionDetailsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage LateTransactionsDetails(DashboardFilterCriteria dashboardFilterCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionDetailsDTO>> getResult = null;
            int TotalCount = 0;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    DateTime dateTimeFrom = DateTime.ParseExact(dashboardFilterCriteria.fromDate.Split(' ')[0], "dd/MM/yyyy", null);
                    DateTime dateTimeTo = DateTime.ParseExact(dashboardFilterCriteria.toDate.Split(' ')[0], "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).AddSeconds(59);
                    IDashboardHomeBL dashboardHomeBL = IoC.Resolve<IDashboardHomeBL>();

                    List<DashboardTransactionDetails> dashboardTransactionDetailsList = dashboardHomeBL.LateTransactionsDetails(dateTimeFrom, dateTimeTo, dashboardFilterCriteria.entityId, dashboardFilterCriteria.userId, dashboardFilterCriteria.level, dashboardFilterCriteria.itemId, dashboardFilterCriteria.cultureId, dashboardFilterCriteria.pageIndex, dashboardFilterCriteria.pageSize, out TotalCount);
                    List<TransactionDetailsDTO> TransactionDetailsDTOList = TransactionDetailsMapper.Map(dashboardTransactionDetailsList);
                    getResult = GetResult<List<TransactionDetailsDTO>>.Create(statusCode, TransactionDetailsDTOList, null);
                    getResult.RowsCount = TotalCount;

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionDetailsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionDetailsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        public HttpResponseMessage InProgressTransactionsDetails(DashboardFilterCriteria dashboardFilterCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionDetailsDTO>> getResult = null;
            int TotalCount = 0;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    DateTime dateTimeFrom = DateTime.ParseExact(dashboardFilterCriteria.fromDate.Split(' ')[0], "dd/MM/yyyy", null);
                    DateTime dateTimeTo = DateTime.ParseExact(dashboardFilterCriteria.toDate.Split(' ')[0], "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).AddSeconds(59);
                    IDashboardHomeBL dashboardHomeBL = IoC.Resolve<IDashboardHomeBL>();

                    List<DashboardTransactionDetails> dashboardTransactionDetailsList = dashboardHomeBL.InProgressTransactionsDetails(dateTimeFrom, dateTimeTo, dashboardFilterCriteria.entityId, dashboardFilterCriteria.userId, dashboardFilterCriteria.level, dashboardFilterCriteria.itemId, dashboardFilterCriteria.cultureId, dashboardFilterCriteria.pageIndex, dashboardFilterCriteria.pageSize, out TotalCount);
                    List<TransactionDetailsDTO> TransactionDetailsDTOList = TransactionDetailsMapper.Map(dashboardTransactionDetailsList);
                    getResult = GetResult<List<TransactionDetailsDTO>>.Create(statusCode, TransactionDetailsDTOList, null);
                    getResult.RowsCount = TotalCount;

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionDetailsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionDetailsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetDashboardHomeReport(DashboardFilterCriteria dashboardFilterCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<DashboardHomeReportDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    DateTime? dateTimeFrom = null;
                    DateTime? dateTimeTo = null;


                    if (string.IsNullOrEmpty(dashboardFilterCriteria.toDate) == false)
                    {
                        dateTimeFrom = DateTime.ParseExact(dashboardFilterCriteria.fromDate.Split(' ')[0], "dd/MM/yyyy", null);
                    }
                    if (string.IsNullOrEmpty(dashboardFilterCriteria.fromDate) == false)
                    {
                        dateTimeTo = DateTime.ParseExact(dashboardFilterCriteria.toDate.Split(' ')[0], "dd/MM/yyyy", null);
                    }



                    IDashboardHomeBL dashboardHomeBL = IoC.Resolve<DashboardHomeBL>();
                    DashboardHomeReportDTO dashboardHomeDTO = DashboardHomeMapper.Map(dashboardHomeBL.GetDashboardReport(dateTimeFrom, dateTimeTo, dashboardFilterCriteria.entityId, dashboardFilterCriteria.userId));
                    dashboardHomeDTO.DashboardReportBottomList = DashboardHomeMapper.Map(dashboardHomeBL.GetDashboardReportBottom(dateTimeFrom, dateTimeTo, dashboardFilterCriteria.entityId, dashboardFilterCriteria.userId));

                    getResult = GetResult<DashboardHomeReportDTO>.Create(statusCode, dashboardHomeDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<DashboardHomeReportDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<DashboardHomeReportDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetCounterDetails(int Id, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<CounterDetailDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ICounterBL counterBL = IoC.Resolve<CounterBL>();

                    List<CounterDetailDTO> counterDetail = CounterMapper.Map(counterBL.GetCounterById(Id).CounterDetails, cultureName);

                    getResult = GetResult<List<CounterDetailDTO>>.Create(statusCode, counterDetail, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<CounterDetailDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<CounterDetailDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
    }
}
