using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class DocumentRepository : BaseRepository<DocumentInfo>, IDocumentRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public DocumentRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods  

        public DocumentInfo GetDocumentById(int documentId, int? userWeight)
        {
            try
            {
                bool hasPermission = false;
                DocumentInfo dInfo = _oMCSDbContext.DocumentsInfo.Include(x => x.Document).Where(d => d.Id == documentId).FirstOrDefault();
                if (dInfo?.Document != null)
                {
                    int documentID = dInfo.Document.Id;
                    if (dInfo.TransactionId.HasValue && dInfo.TransactionId.Value > 0)
                    {
                        var transaction = _oMCSDbContext.Transactions.Include(x => x.Confidentiality).Where(x => x.Id == dInfo.TransactionId && x.Confidentiality.Weight <= userWeight).FirstOrDefault();
                        if (transaction != null)
                        {
                            hasPermission = true;
                        }
                    }
                    else
                    {
                        hasPermission = true;
                    }
                    if (hasPermission)
                        dInfo.Document.Content = (from d in _oMCSDbContext.Documents
                                                  where d.Id == documentID
                                                  select d.Content).FirstOrDefault();
                    else
                        dInfo = null;

                }


                return dInfo;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void DeleteDocument(int documentId)
        {
            try
            {
                DocumentInfo documentInfo = this.FindBy(d => d.Id == documentId);

                if (documentInfo?.Document != null)
                {
                    _oMCSDbContext.Entry(documentInfo.Document).State = System.Data.Entity.EntityState.Deleted;
                    _oMCSDbContext.Entry(documentInfo).State = System.Data.Entity.EntityState.Deleted;

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int AddDocument(DocumentInfo documentInfo)
        {
            try
            {
                _oMCSDbContext.DocumentsInfo.Add(documentInfo);
                _oMCSDbContext.SaveChanges();
                return documentInfo.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateMainDocumentContent(int documentId, int TransactionId, byte[] content, string memType)
        {
            try
            {

                DocumentInfo oDocumentInfoOld = this.Get(documentId);
                oDocumentInfoOld.Document.Content = content;
                oDocumentInfoOld.MimeType = memType;
                oDocumentInfoOld.TransactionId = TransactionId;
                _oMCSDbContext.Entry(oDocumentInfoOld).State = EntityState.Modified;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateDocumentByECMId(string ECMId, int documentId)
        {
            try
            {
                DocumentInfo oDocumentInfoOld = this.Get(documentId);
                oDocumentInfoOld.ECMId = ECMId;
                _oMCSDbContext.Entry(oDocumentInfoOld).State = EntityState.Modified;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public string GetECMIdByDocumentId(int documentId)
        {
            try
            {
                DocumentInfo oDocumentInfoOld = this.FindBy(d => d.Id == documentId);
                return oDocumentInfoOld.ECMId;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public byte[] GetMainDocument(int documentId, int? userWeight)
        {
            try
            {

                DocumentInfo oDocumentInfoOld = GetDocumentById(documentId, userWeight);
                return oDocumentInfoOld?.Document?.Content;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void ClearMigratedDocumentBinary(int documentId)
        {
            try
            {
                DocumentInfo oDocumentInfoOld = this.Get(documentId);
                if (!string.IsNullOrWhiteSpace(oDocumentInfoOld.ECMId))
                {
                    oDocumentInfoOld.Document.Content = null;
                }

                _oMCSDbContext.Entry(oDocumentInfoOld).State = EntityState.Modified;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<DocumentInfo> GetAllDocuments(int pageSize, int? userWeight)
        {
            try
            {
                //var documents = _oMCSDbContext.DocumentsInfo.Include(c => c.Document).Where(d => d.Document.Content != null && d.Transaction.Confidentiality.Weight <= userWeight);

                var documents = (from di in _oMCSDbContext.DocumentsInfo
                                 join t in _oMCSDbContext.Transactions on di.TransactionId equals t.Id
                                 where di.Document.Content != null && t.Confidentiality.Weight <= userWeight
                                 select new DocumentInfo
                                 {
                                     Document = di.Document,
                                     FromEntity = di.FromEntity,
                                     FromEntityId = di.FromEntityId,
                                     ModefiedOn = di.ModefiedOn,
                                     CreatedBy = di.CreatedBy,
                                     Id = di.Id,
                                     TransactionId = di.TransactionId,
                                     CreatedOn = di.CreatedOn,
                                     DocumentType = di.DocumentType,
                                     ECMId = di.ECMId,
                                     FromUser = di.FromUser,
                                     FromUserId = di.FromUserId,
                                     IsDeleted = di.IsDeleted,
                                     MimeType = di.MimeType,
                                     ModefiedBy = di.ModefiedBy,
                                     Name = di.Name,
                                     Size = di.Size,

                                 });


                return documents.Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateMainDocumentContentWithDigitalSign(int documentId, byte[] content, bool IsDigitallySigned, string MimeContent)
        {
            try
            {
                DocumentInfo oDocumentInfoOld = this.FindBy(d => d.Id == documentId);
                oDocumentInfoOld.Document.Content = content;
                // oDocumentInfoOld.IsDigitallySigned = IsDigitallySigned;
                oDocumentInfoOld.MimeType = MimeContent;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateDocumentContentByTransaction(int transactionId, byte[] content)
        {
            try
            {
                var mainTransactionId = _oMCSDbContext.Transactions.Where(x => x.Id == transactionId).Select(x => x.MainDocumentId).FirstOrDefault();

                DocumentInfo oDocumentInfoOld = this.Get(mainTransactionId.Value);
                oDocumentInfoOld.Document.Content = content;
                _oMCSDbContext.Entry(oDocumentInfoOld).State = EntityState.Modified;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        #endregion Methods
    }

}
