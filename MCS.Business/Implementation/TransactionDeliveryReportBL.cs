using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class TransactionDeliveryReportBL : BaseBL, ITransactionDeliveryReportBL
    {
        public int AddTransactionDeliveryReport(TransactionDeliveryReport transactionDeliveryReport)
        {
            try
            {
                ITransactionDeliveryReportRepository transactionDeliveryReportRepository = IoC.Resolve<TransactionDeliveryReportRepository>();
                return transactionDeliveryReportRepository.AddTransactionDeliveryReport(transactionDeliveryReport);
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
        public void UpdateTransactionDeliveryReportCopies(int transactionId, int? reporterId)
        {
            try
            {
                ITransactionDeliveryReportRepository transactionDeliveryReportRepository = IoC.Resolve<TransactionDeliveryReportRepository>();
                transactionDeliveryReportRepository.UpdateTransactionDeliveryReportCopies(transactionId, reporterId);
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
        public IList<TransactionDeliveryReport> GetTransactionDeliveryReportByTransactionId(int transcationId, bool? isCopy = false, bool? all = false)
        {
            try
            {
                ITransactionDeliveryReportRepository transactionDeliveryReportRepository = IoC.Resolve<TransactionDeliveryReportRepository>();

                if (isCopy.HasValue && isCopy.Value == true)
                {
                    return transactionDeliveryReportRepository.GetTransactionDeliveryReport(r => r.TransactionId == transcationId && (r.TransactionExternalCopyId != null || r.TransactionCopyId !=null));
                }
                else if (all.HasValue && all.Value == true)
                {
                    return transactionDeliveryReportRepository.GetTransactionDeliveryReport(r => r.TransactionId == transcationId);
                }
                else 
                {
                    return transactionDeliveryReportRepository.GetTransactionDeliveryReport(r => r.TransactionId == transcationId && r.TransactionExternalCopyId == null && r.TransactionCopyId == null);
                }
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
        public IList<TransactionDeliveryReport> GetTransactionDeliveryReportByTransactionIds(List<int> transcationIds)
        {
            try
            {
                ITransactionDeliveryReportRepository transactionDeliveryReportRepository = IoC.Resolve<TransactionDeliveryReportRepository>();
                return transactionDeliveryReportRepository.GetTransactionDeliveryReport(r => transcationIds.Contains(r.TransactionId));
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
        public IList<TransactionDeliveryReport> GetDeliveryReport(List<int> deliveryReportIds)
        {
            try
            {
                ITransactionDeliveryReportRepository transactionDeliveryReportRepository = IoC.Resolve<TransactionDeliveryReportRepository>();
                return transactionDeliveryReportRepository.GetTransactionDeliveryReportByIds(deliveryReportIds);
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

        public IList<TransactionDeliveryReport> GetDeliveryReport(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                ITransactionDeliveryReportRepository transactionDeliveryReportRepository = IoC.Resolve<TransactionDeliveryReportRepository>();
                return transactionDeliveryReportRepository.GetTransactionDeliveryReport(searchCriteria, out rowsCount);
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

        public IList<TransactionDeliveryReport> GetLastDeliveryReport(int transcationId, int userId)
        {
            try
            {
                ITransactionDeliveryReportRepository transactionDeliveryReportRepository = IoC.Resolve<TransactionDeliveryReportRepository>();
                return transactionDeliveryReportRepository.GetTransactionDeliveryReport(r => r.TransactionId == transcationId && r.UserId == userId);
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
        public IList<TransactionDeliveryReport> GetTransactionDeliveryReportByNumber(DateTime? date, string cultureName)
        {
            try
            {
                ITransactionDeliveryReportRepository transactionDeliveryReportRepository = IoC.Resolve<TransactionDeliveryReportRepository>();

                return transactionDeliveryReportRepository.GetTransactionDeliveryReportByNumber(date, cultureName);
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
        public IList<TransactionDeliveryReport> GetTransactionDeliveryReportByNumber(DateTime? date, int? transactionId, string number, string cultureName)
        {
            try
            {
                ITransactionDeliveryReportRepository transactionDeliveryReportRepository = IoC.Resolve<TransactionDeliveryReportRepository>();

                return transactionDeliveryReportRepository.GetTransactionDeliveryReportByNumber(date,  transactionId, number, cultureName);
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
        public IList<TransactionDeliveryReport> GetTransactionDeliveryReportByNumber(string number)
        {
            try
            {
                ITransactionDeliveryReportRepository transactionDeliveryReportRepository = IoC.Resolve<TransactionDeliveryReportRepository>();
                return transactionDeliveryReportRepository.GetTransactionDeliveryReport(r => r.Number == number);
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
        public int UpdateDeliveryReportDocumentByNumber(DocumentInfo document, string Number)
        {
            try
            {
                ITransactionDeliveryReportRepository transactionDeliveryReportRepository = IoC.Resolve<TransactionDeliveryReportRepository>();

                return transactionDeliveryReportRepository.UpdateDeliveryReportsDocumentByNumber(document, Number);
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

        public int UpdateDeliveryReportsDocumentByDate(DocumentInfo document, string DateH , string DeliveryReportNumber)
        {
            try
            {
                ISignedDeliveryReportRepository signedDeliveryReportRepository = IoC.Resolve<SignedDeliveryReportRepository>();
                SignedDeliveryReport signedDeliveryReport = new SignedDeliveryReport();

                ITransactionDeliveryReportRepository transactionDeliveryReportRepository = IoC.Resolve<TransactionDeliveryReportRepository>();
                TransactionDeliveryReport transactionDeliveryReport = transactionDeliveryReportRepository.GetTransactionDeliveryReport(r => r.Number == DeliveryReportNumber).FirstOrDefault();
                signedDeliveryReport.TransactionDeliveryReportId = transactionDeliveryReport.Id;
                signedDeliveryReport.Document  = new DocumentInfo()
                {
                    MimeType = document.MimeType,
                    Size = document.Size,
                    CreatedBy = document.CreatedBy,
                    Document = new Document()
                    {
                        CreatedBy = document.CreatedBy,
                    }
                };
                signedDeliveryReport.Date = DateTimeUtility.HijriToGreg(DateH);
                signedDeliveryReport.DateH = DateH;
                return signedDeliveryReportRepository.AddSignedDeliveryReport(signedDeliveryReport);
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

        public int UpdateDeliveryReportsDocumentByDeliveryReportId(DocumentInfo document, string DateH, int Id)
        {
            try
            {
                ISignedDeliveryReportRepository signedDeliveryReportRepository = IoC.Resolve<SignedDeliveryReportRepository>();
                SignedDeliveryReport signedDeliveryReport = new SignedDeliveryReport();

                ITransactionDeliveryReportRepository transactionDeliveryReportRepository = IoC.Resolve<TransactionDeliveryReportRepository>();
                TransactionDeliveryReport transactionDeliveryReport = transactionDeliveryReportRepository.GetTransactionDeliveryReport(r => r.Id == Id).FirstOrDefault();
                signedDeliveryReport.TransactionDeliveryReportId = transactionDeliveryReport.Id;
                signedDeliveryReport.Document = new DocumentInfo()
                {
                    MimeType = document.MimeType,
                    Size = document.Size,
                    CreatedBy = document.CreatedBy,
                    Document = new Document()
                    {
                        CreatedBy = document.CreatedBy,
                    }
                };
                signedDeliveryReport.Date = DateTimeUtility.HijriToGreg(DateH);
                signedDeliveryReport.DateH = DateH;
                return signedDeliveryReportRepository.AddSignedDeliveryReport(signedDeliveryReport);
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
        public TransactionDeliveryReport GetTransactionDeliveryReportByHistoryId(int historyId)
        {
            try
            {
                ITransactionDeliveryReportRepository transactionDeliveryReportRepository = IoC.Resolve<TransactionDeliveryReportRepository>();
                return transactionDeliveryReportRepository.GetTransactionDeliveryReport(r => r.TransactionHistoryId == historyId).FirstOrDefault();
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

        public void UpdateTransactionDeliveryReport(TransactionDeliveryReport transactionDeliveryReport)
        {
            try
            {
                ITransactionDeliveryReportRepository transactionDeliveryReportRepository = IoC.Resolve<TransactionDeliveryReportRepository>();
                transactionDeliveryReportRepository.UpdateTransactionDeliveryReport(transactionDeliveryReport);
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

        public TransactionDeliveryReport GetTransactionDeliveryReportByAssignmentHistoryId(int assignmentHistoryId)
        {
            try
            {
                ITransactionDeliveryReportRepository transactionDeliveryReportRepository = IoC.Resolve<TransactionDeliveryReportRepository>();
                return transactionDeliveryReportRepository.GetTransactionDeliveryReport(r => r.TransactionAssignmentHistoryId == assignmentHistoryId).FirstOrDefault();
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
