using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using Task = System.Threading.Tasks.Task;

namespace MCS.Business
{
    public static class ReportBL
    {
        public static List<TransactionReportResult> TransactionReportSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount)
        {
            try
            {
                TotalCount = 0;
                List<TransactionReportResult> result = new List<TransactionReportResult>();
                IReportWrapper searchWrapper = IoC.Resolve<IReportWrapper>();
                if (searchCriteriaTransactionReport.IsPrint.HasValue)
                {
                    int numberOfLoop = Convert.ToInt32(Math.Round(Convert.ToDouble(searchCriteriaTransactionReport.TotalCount) / Convert.ToDouble(searchCriteriaTransactionReport.PageSize)));
                    if (numberOfLoop == 0)
                    {
                        numberOfLoop = 1;
                    }
                    for (int pageIndex = 0; pageIndex < numberOfLoop; pageIndex++)
                    {
                        searchCriteriaTransactionReport.PageIndex = pageIndex;
                        var newResult = searchWrapper.TransactionReportSearch(searchCriteriaTransactionReport, out TotalCount);
                        result.AddRange(newResult);
                    }
                }
                else
                {
                    searchCriteriaTransactionReport.PageIndex -= 1;
                    result = searchWrapper.TransactionReportSearch(searchCriteriaTransactionReport, out TotalCount);
                }
                return result;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public static List<TransactionReportResult> SecretaryTransactionReportSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount)
        {
            try
            {
                TotalCount = 0;
                List<TransactionReportResult> result = new List<TransactionReportResult>();
                IReportWrapper searchWrapper = IoC.Resolve<IReportWrapper>();
                if (searchCriteriaTransactionReport.IsPrint.HasValue)
                {
                    int numberOfLoop = Convert.ToInt32(Math.Round(Convert.ToDouble(searchCriteriaTransactionReport.TotalCount) / Convert.ToDouble(searchCriteriaTransactionReport.PageSize)));
                    if (numberOfLoop == 0)
                    {
                        numberOfLoop = 1;
                    }
                    for (int pageIndex = 0; pageIndex < numberOfLoop; pageIndex++)
                    {
                        searchCriteriaTransactionReport.PageIndex = pageIndex;
                        var newResult = searchWrapper.SecretaryTransactionReportSearch(searchCriteriaTransactionReport, out TotalCount);
                        result.AddRange(newResult);
                    }
                }
                else
                {
                    searchCriteriaTransactionReport.PageIndex -= 1;
                    result = searchWrapper.SecretaryTransactionReportSearch(searchCriteriaTransactionReport, out TotalCount);
                }
                return result;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }


        public static List<SentTransactionReportResult> SentTransactionReportSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount)
        {
            try
            {
                TotalCount = 0;
                List<SentTransactionReportResult> result = new List<SentTransactionReportResult>();
                IReportWrapper searchWrapper = IoC.Resolve<IReportWrapper>();
                if (searchCriteriaTransactionReport.IsPrint.HasValue)
                {
                    int numberOfLoop = Convert.ToInt32(Math.Round(Convert.ToDouble(searchCriteriaTransactionReport.TotalCount) / Convert.ToDouble(searchCriteriaTransactionReport.PageSize)));
                    if (numberOfLoop == 0)
                    {
                        numberOfLoop = 1;
                    }
                    for (int pageIndex = 0; pageIndex < numberOfLoop; pageIndex++)
                    {
                        searchCriteriaTransactionReport.PageIndex = pageIndex;
                        var newResult = searchWrapper.SentTransactionReportSearch(searchCriteriaTransactionReport, out TotalCount);
                        result.AddRange(newResult);
                    }
                }
                else
                {
                    searchCriteriaTransactionReport.PageIndex -= 1;
                    result = searchWrapper.SentTransactionReportSearch(searchCriteriaTransactionReport, out TotalCount);
                }
                return result;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public static List<SentTransactionReportResult> SentTransactionReportStautsSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount)
        {
            try
            {
                TotalCount = 0;
                List<SentTransactionReportResult> result = new List<SentTransactionReportResult>();
                IReportWrapper searchWrapper = IoC.Resolve<IReportWrapper>();
                if (searchCriteriaTransactionReport.IsPrint.HasValue)
                {
                    int numberOfLoop = Convert.ToInt32(Math.Round(Convert.ToDouble(searchCriteriaTransactionReport.TotalCount) / Convert.ToDouble(searchCriteriaTransactionReport.PageSize)));
                    if (numberOfLoop == 0)
                    {
                        numberOfLoop = 1;
                    }
                    for (int pageIndex = 0; pageIndex < numberOfLoop; pageIndex++)
                    {
                        searchCriteriaTransactionReport.PageIndex = pageIndex;
                        var newResult = searchWrapper.SentTransactionReportStatusSearch(searchCriteriaTransactionReport, out TotalCount);
                        result.AddRange(newResult);
                    }
                }
                else
                {
                    searchCriteriaTransactionReport.PageIndex -= 1;
                    result = searchWrapper.SentTransactionReportStatusSearch(searchCriteriaTransactionReport, out TotalCount);
                }
                return result;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public static List<TaskReportResult> TasksReportSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount)
        {
            try
            {
                TotalCount = 0;
                List<TaskReportResult> result = new List<TaskReportResult>();
                IReportWrapper searchWrapper = IoC.Resolve<IReportWrapper>();
                if (searchCriteriaTransactionReport.IsPrint.HasValue)
                {
                    int numberOfLoop = Convert.ToInt32(Math.Round(Convert.ToDouble(searchCriteriaTransactionReport.TotalCount) / Convert.ToDouble(searchCriteriaTransactionReport.PageSize)));
                    if (numberOfLoop == 0)
                    {
                        numberOfLoop = 1;
                    }
                    for (int pageIndex = 0; pageIndex < numberOfLoop; pageIndex++)
                    {
                        searchCriteriaTransactionReport.PageIndex = pageIndex;
                        var newResult = searchWrapper.TasksReportSearch(searchCriteriaTransactionReport, out TotalCount);
                        result.AddRange(newResult);
                    }
                }
                else
                {
                    searchCriteriaTransactionReport.PageIndex -= 1;
                    result = searchWrapper.TasksReportSearch(searchCriteriaTransactionReport, out TotalCount);
                }
                return result;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public static List<FollowupReportResult> FollowupReportSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount)
        {
            try
            {
                TotalCount = 0;
                List<FollowupReportResult> result = new List<FollowupReportResult>();
                IReportWrapper searchWrapper = IoC.Resolve<IReportWrapper>();
                if (searchCriteriaTransactionReport.IsPrint.HasValue)
                {
                    int numberOfLoop = Convert.ToInt32(Math.Round(Convert.ToDouble(searchCriteriaTransactionReport.TotalCount) / Convert.ToDouble(searchCriteriaTransactionReport.PageSize)));
                    if (numberOfLoop == 0)
                    {
                        numberOfLoop = 1;
                    }
                    for (int pageIndex = 0; pageIndex < numberOfLoop; pageIndex++)
                    {
                        searchCriteriaTransactionReport.PageIndex = pageIndex;
                        var newResult = searchWrapper.FollowupReportSearch(searchCriteriaTransactionReport, out TotalCount);
                        result.AddRange(newResult);
                    }
                }
                else
                {
                    searchCriteriaTransactionReport.PageIndex -= 1;
                    result = searchWrapper.FollowupReportSearch(searchCriteriaTransactionReport, out TotalCount);
                }
                return result;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public static List<PerformanceMeasurementReportResult> PerformanceMeasurementReportSearch(SearchCriteriaPerformanceMeasurementReport searchCriteriaPerformanceMeasurementReport, out int TotalCount)
        {
            try
            {
                TotalCount = 0;
                List<PerformanceMeasurementReportResult> result = new List<PerformanceMeasurementReportResult>();
                IReportWrapper searchWrapper = IoC.Resolve<IReportWrapper>();
                if (searchCriteriaPerformanceMeasurementReport.IsPrint.HasValue)
                {
                    int numberOfLoop = Convert.ToInt32(Math.Round(Convert.ToDouble(searchCriteriaPerformanceMeasurementReport.TotalCount) / Convert.ToDouble(searchCriteriaPerformanceMeasurementReport.PageSize)));
                    if (numberOfLoop == 0)
                    {
                        numberOfLoop = 1;
                    }
                    for (int pageIndex = 0; pageIndex < numberOfLoop; pageIndex++)
                    {
                        searchCriteriaPerformanceMeasurementReport.PageIndex = pageIndex;
                        var newResult = searchWrapper.PerformanceMeasurementReportSearch(searchCriteriaPerformanceMeasurementReport, out TotalCount);
                        result.AddRange(newResult);
                    }
                }
                else
                {
                    searchCriteriaPerformanceMeasurementReport.PageIndex -= 1;
                    result = searchWrapper.PerformanceMeasurementReportSearch(searchCriteriaPerformanceMeasurementReport, out TotalCount);
                }

                return result;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
    }
}
