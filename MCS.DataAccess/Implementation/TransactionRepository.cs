using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Common.Utility;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;
using Action = MCS.Domain.Action;
using SystemDataForSPs = System.Data;
using System.Globalization;

namespace MCS.DataAccess
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public TransactionRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        public int AddTransaction(Transaction transaction)
        {
            try
            {
                _oMCSDbContext.Transactions.Add(transaction);
                _oMCSDbContext.SaveChanges();

                return transaction.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<ExternalPartyAttachment> GetExternalPartiesAttach(int transactionID, int externalPartyId, string cultureName)
        {
            try
            {


                IList<ExternalPartyAttachment> externalPartyAttachments = _oMCSDbContext.ExternalPartyAttachments
                                                                                        .Where(epa => epa.Id == externalPartyId)
                                                                                        .Include(epa => epa.DocumentInfo)
                                                                                        .Include(epa => epa.DocumentInfo.Document)
                                                                                        .ToList();

                //IList<ExternalPartyAttachment> externalPartyAttachments = (from attachment in _oMCSDbContext.ExternalPartyAttachments.ToList()
                //                                                           where (attachment.PartyId == externalPartyId)
                //                                                           select new
                //                                                           {
                //                                                               attachment.DocumentInfo,
                //                                                               attachment.CreatedBy,
                //                                                               attachment.CreatedOn,
                //                                                               attachment.Id,
                //                                                               attachment.PartyId

                //                                                           }
                //                     ).ToList().Select(p => new ExternalPartyAttachment
                //                     {
                //                         DocumentInfo = new DocumentInfo
                //                         {
                //                             Document = new Document
                //                             {
                //                                 Id = p.DocumentInfo.Document.Id,
                //                                 Content = p.DocumentInfo.Document.Content
                //                             },

                //                             Id = p.DocumentInfo.Id,
                //                             MimeType = p.DocumentInfo.MimeType,
                //                             Name = p.DocumentInfo.Name,
                //                             Size = p.DocumentInfo.Size,
                //                             IsDeleted = p.DocumentInfo.IsDeleted,
                //                             ECMId = p.DocumentInfo.ECMId
                //                         },

                //                         CreatedBy = p.CreatedBy,
                //                         CreatedOn = p.CreatedOn,
                //                         Id = p.Id,
                //                         PartyId = p.PartyId
                //                     }).ToList();
                return externalPartyAttachments;
            }

            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public void UpdateTransaction(Transaction transaction)
        {
            try
            {
                Transaction transactionOld = GetTransactionById(transaction.Id);

                if (transactionOld != null)
                {
                    transactionOld.Subject = transaction.Subject;
                    transactionOld.Remarks = transaction.Remarks;
                    transactionOld.Priority = transaction.Priority;
                    transactionOld.LetterType = transaction.LetterType;
                    transactionOld.Confidentiality = transaction.Confidentiality;
                    transactionOld.SignedByUser = transaction.SignedByUser;
                    transactionOld.ToUser = transaction.ToUser;
                    transactionOld.Entity = transaction.Entity;
                    transactionOld.DocumentNumber = transaction.DocumentNumber;
                    transactionOld.TransactionType = transaction.TransactionType;
                    transactionOld.PrintedDeliveryReport = transaction.PrintedDeliveryReport;
                    transactionOld.ExternalParty = transaction.ExternalParty;
                    transactionOld.ExternalPartyManager = transaction.ExternalPartyManager;
                    transactionOld.DeliveryReportNumber = transaction.DeliveryReportNumber;
                    transactionOld.OutboundDraftId = transaction.OutboundDraftId;
                    transactionOld.RemindDate = transaction.RemindDate;
                    transactionOld.RemindDateH = transaction.RemindDateH;
                    transactionOld.IsDeleted = transaction.IsDeleted;
                    transactionOld.SuggestedTopic = transaction.SuggestedTopic;
                    transactionOld.IsSigned = transaction.IsSigned;
                    transactionOld.Copies = transaction.Copies;
                    transactionOld.ExternalCopies = transaction.ExternalCopies;
                    transactionOld.DeliveryMethodId = transaction.DeliveryMethodId;
                    transactionOld.POBox = transaction.POBox;
                    transactionOld.PostCode = transaction.PostCode;
                    transactionOld.IsForIndividual = transaction.IsForIndividual;
                    transactionOld.ProcessPeriodTransaction = transaction.ProcessPeriodTransaction;
                    transactionOld.SubjectClassificationsId = transaction.SubjectClassificationsId;
                    transactionOld.SideContactExternalEntityID = transaction.SideContactExternalEntityID;
                    transactionOld.NumberContact = transaction.NumberContact;

                    if (transactionOld.SubjectClassifications != null)
                    {
                        transactionOld.SubjectClassifications.ToList().ForEach(item => _oMCSDbContext.TransactionSubjectClassifications.Remove(item));
                    }

                    transactionOld.SubjectClassifications = transaction.SubjectClassifications;

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateTransaction(Transaction transaction, bool updateDocument, bool isReserved = false)
        {
            try
            {
                Transaction transactionOld = GetTransactionById(transaction.Id);

                if (transactionOld != null)
                {
                    transactionOld.Subject = transaction.Subject;
                    transactionOld.Remarks = transaction.Remarks;
                    transactionOld.PriorityId = transaction.PriorityId;
                    transactionOld.LetterTypeId = transaction.LetterTypeId;
                    transactionOld.ConfidentialityId = transaction.ConfidentialityId;
                    transactionOld.SignedByUserId = transaction.SignedByUserId;
                    transactionOld.ToUserId = transaction.ToUserId;
                    transactionOld.EntityId = transaction.EntityId > 0 ? transaction.EntityId : (int?)null;
                    transactionOld.DocumentNumber = transaction.DocumentNumber;
                    transactionOld.TransactionTypeId = transaction.TransactionTypeId;
                    transactionOld.PrintedDeliveryReport = transaction.PrintedDeliveryReport;
                    transactionOld.ExternalPartyId = transaction.ExternalPartyId > 0 ? transaction.ExternalPartyId : null;
                    transactionOld.ExternalPartyManagerId = transaction.ExternalPartyManagerId;
                    transactionOld.DeliveryReportNumber = transaction.DeliveryReportNumber;
                    transactionOld.OutboundDraftId = transaction.OutboundDraftId;
                    transactionOld.OutboundDraftEditorType = transaction.OutboundDraftEditorType;
                    transactionOld.RemindDate = transaction.RemindDate;
                    transactionOld.RemindDateH = transaction.RemindDateH;
                    transactionOld.SuggestedTopicId = transaction.SuggestedTopicId != -1 ? transaction.SuggestedTopicId : transactionOld.SuggestedTopicId;
                    transactionOld.IsSigned = transaction.IsSigned;
                    transactionOld.Copies = transactionOld.Copies;
                    transactionOld.ExternalCopies = transactionOld.ExternalCopies;
                    transactionOld.DeliveryMethodId = transaction.DeliveryMethodId;
                    transactionOld.PostCode = transaction.PostCode;
                    transactionOld.POBox = transaction.POBox;
                    transactionOld.IsForIndividual = transaction.IsForIndividual;
                    transactionOld.InboundDateH = transaction.InboundDateH;
                    transactionOld.ReporterId = transaction.ReporterId;
                    transactionOld.NumberContact = transaction.NumberContact;
                    transactionOld.SideContactExternalEntityID = transaction.SideContactExternalEntityID;
                    transactionOld.Attachments = transactionOld.Attachments;
                    transactionOld.CityId = transaction.CityId;
                    transactionOld.Summary = transaction.Summary;
                    transactionOld.Encrypted = transaction.Encrypted;

                    //if (updateDocument)
                    //{
                    //    _oMCSDbContext.Entry(transactionOld).Reference(r => r.MainDocument).CurrentValue = transaction.MainDocument;
                    //}

                    if (isReserved || (transactionOld.MainDocument == null && transaction.MainDocument != null))
                    {
                        transactionOld.MainDocument = transaction.MainDocument;
                    }

                    if (transactionOld.SubjectClassifications != null && transactionOld.SubjectClassifications.Count > 0)
                    {
                        transactionOld.SubjectClassifications.ToList().ForEach(item => _oMCSDbContext.TransactionSubjectClassifications.Remove(item));
                    }

                    transactionOld.SubjectClassifications = transaction.SubjectClassifications;
                    transactionOld.LetterNumber = transaction.LetterNumber;

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateTransactionNames(int transactionId, IList<TransactionName> transactionNames)
        {
            try
            {
                Transaction transaction = GetTransactionById(transactionId);
                //Add all names if there is no names added before
                if (transaction != null && (transaction.Names == null || transaction.Names.Count == 0))
                {
                    if (transactionNames != null && transactionNames.Count > 0)
                    {
                        foreach (var item in transactionNames)
                        {
                            item.TransactionId = transactionId;
                            var Name = _oMCSDbContext.Names.Where(n => n.Id == item.Name.Id).FirstOrDefault();
                            if (Name != null)
                            {
                                var NameEntry = _oMCSDbContext.Entry(Name);
                                NameEntry.CurrentValues.SetValues(item.Name);
                                NameEntry.Entity.Id = item.Name.Id;
                                item.Name = null;
                                _oMCSDbContext.TransactionNames.Add(item);
                            }
                            else
                            {
                                _oMCSDbContext.TransactionNames.Add(item);
                            }
                        }
                        _oMCSDbContext.SaveChanges();
                        return;
                    }
                }

                //There are names added before
                if (transaction != null && transaction.Names != null && transaction.Names.Count > 0)
                {
                    if (transactionNames != null)
                    {
                        foreach (var item in transactionNames)
                        {
                            var originalTransactionName = transaction.Names
                                                                    .Where(c => c.Name.Id == item.Name.Id && c.Name.Id != 0)
                                                                    .SingleOrDefault();
                            //Updated Item
                            if (originalTransactionName != null)
                            {
                                item.Id = originalTransactionName.Id;
                                var NameEntry = _oMCSDbContext.Entry(originalTransactionName.Name);
                                NameEntry.CurrentValues.SetValues(item.Name);
                                NameEntry.Entity.Id = item.Name.Id;
                                var TransactionNameEntry = _oMCSDbContext.Entry(originalTransactionName);
                                TransactionNameEntry.CurrentValues.SetValues(item);
                                TransactionNameEntry.Entity.TransactionId = transactionId;
                            }
                            //Added item
                            else
                            {
                                item.TransactionId = transactionId;
                                var Name = _oMCSDbContext.Names.Where(n => n.Id == item.Name.Id).FirstOrDefault();
                                if (Name != null)
                                {
                                    var NameEntry = _oMCSDbContext.Entry(Name);
                                    NameEntry.CurrentValues.SetValues(item.Name);
                                    NameEntry.Entity.Id = item.Name.Id;
                                    item.Name = null;
                                    _oMCSDbContext.TransactionNames.Add(item);
                                }
                                else
                                {
                                    _oMCSDbContext.TransactionNames.Add(item);
                                }
                            }
                        }
                        foreach (var originalTransactionName in transaction.Names.Where(c => c.Id != 0).ToList())
                        {
                            if (!transactionNames.Any(c => c.Id == originalTransactionName.Id))
                            {
                                _oMCSDbContext.TransactionNames.Remove(originalTransactionName);
                            }
                        }
                    }
                    else
                    {
                        foreach (var originalTransactionName in transaction.Names.Where(c => c.Id != 0).ToList())
                        {
                            _oMCSDbContext.TransactionNames.Remove(originalTransactionName);
                        }
                    }
                    _oMCSDbContext.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateAssignmentPaperCopies(int transactionId, IList<TransactionCopy> transactionCopies)
        {
            try
            {
                Transaction transaction = GetTransactionById(transactionId);
                List<TransactionCopy> copies = transaction.Copies.Where(c => c.TransactionId == transactionId).ToList();
                foreach (var item in transactionCopies)
                {
                    if (!copies.Any(copy => copy.EntityId == item.EntityId && copy.UserId == item.UserId))
                    {

                        if (!copies.Any(copy => copy.EntityId == item.EntityId && copy.UserId == item.UserId))
                        {
                            item.TransactionId = transactionId;
                            _oMCSDbContext.TransactionCopies.Add(item);

                        }

                        //if ((item.UserId != null && !copies.Any(copy => copy.EntityId == item.EntityId && copy.UserId == null)) ||
                        //    (item.UserId == null && !copies.Any(copy => copy.EntityId == item.EntityId && copy.UserId != null)))
                        //{
                        //    item.TransactionId = transactionId;
                        //    _oMCSDbContext.TransactionCopies.Add(item);
                        //}
                    }
                }

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateTransactionCopies(int transactionId, IList<TransactionCopy> transactionCopies)
        {
            try
            {
                Transaction transaction = GetTransactionById(transactionId);
                //Add all copies if there is no copies added before
                if (transaction != null && (transaction.Copies == null || transaction.Copies.Count == 0))
                {
                    bool isReserved = transaction.StatusId == TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    foreach (var item in transactionCopies)
                    {
                        item.IsSent = isReserved ? 1 : item.IsSent;
                        if (item.IsSent == 1)
                        {
                            item.SentDate = DateTime.Now;
                        }
                        item.TransactionId = transactionId;
                        _oMCSDbContext.TransactionCopies.Add(item);
                    }
                    _oMCSDbContext.SaveChanges();
                    return;
                }

                //There are copies added before
                if (transaction != null && transaction.Copies != null && transaction.Copies.Count > 0)
                {
                    foreach (var item in transactionCopies)
                    {
                        var originalTransactionCopy = transaction.Copies
                                                                .Where(c => c.Id == item.Id && c.Id != 0)
                                                                .SingleOrDefault();
                        //Updated Item
                        if (originalTransactionCopy != null)
                        {
                            var TransactionCopyEntry = _oMCSDbContext.Entry(originalTransactionCopy);
                            if (originalTransactionCopy.IsSent != 1 && item.IsSent == 1)
                            {
                                TransactionCopyEntry.CurrentValues.SetValues(item);
                                TransactionCopyEntry.Entity.TransactionId = transactionId;
                            }

                        }
                        //Added item
                        else
                        {
                            item.TransactionId = transactionId;
                            _oMCSDbContext.TransactionCopies.Add(item);
                        }
                    }
                    //foreach (var originalTransactionCopy in transaction.Copies.Where(c => c.Id != 0).ToList())
                    //{
                    //    if (!transactionCopies.Any(c => c.Id == originalTransactionCopy.Id))
                    //    {
                    //        _oMCSDbContext.TransactionCopies.Remove(originalTransactionCopy);
                    //    }
                    //}
                }
                _oMCSDbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateCopy(int CopyId)
        {
            try
            {
                TransactionCopy transactionCopy = _oMCSDbContext.TransactionCopies.Where(a => a.Id == CopyId).FirstOrDefault();
                transactionCopy.SentDate = DateTime.Now;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Transaction UpdateVipInbound(List<TransactionFollowUp> transactionFollowUps, List<TransactionCopy> transactionCopies, int transactionId, int? ConfidentialityId, byte[] documentContent, string summary)
        {
            try
            {
                Transaction transaction = _oMCSDbContext.Transactions.Include(x => x.Copies).Include(x => x.FollowUp).Include("MainDocument.Document").Where(a => a.Id == transactionId).FirstOrDefault();
                if (ConfidentialityId.HasValue && ConfidentialityId.Value > 0 && transaction.ConfidentialityId != ConfidentialityId.Value)
                    transaction.ConfidentialityId = ConfidentialityId.Value;
                transaction.Summary = summary;
                if (transactionFollowUps != null && transactionFollowUps.Count > 0)
                {
                    foreach (var followup in transactionFollowUps)
                    {
                        followup.DateTo = followup.FollowUpExpireDate;
                        followup.DateToH = followup.FollowUpExpireDateHj;
                        if (!(transaction.FollowUp != null && transaction.FollowUp.Any(x => x.FollowUpTypeId == followup.FollowUpTypeId && x.Active
                        && x.FollowUpExpireDate < DateTime.Now)))
                            transaction.FollowUp.Add(followup);
                    }

                }
                if (transactionCopies != null && transactionCopies.Count > 0)
                {

                    foreach (var copy in transactionCopies)
                    {
                        var oldCopy = transaction.Copies.Where(x => x.UserId == copy.UserId && x.EntityId == copy.EntityId).FirstOrDefault();
                        if (oldCopy == null)
                        {
                            transaction.Copies.Add(copy);
                        }
                        else
                        {
                            oldCopy.Viewed = false;
                            oldCopy.ModefiedOn = DateTime.Now;
                            oldCopy.ModefiedBy = copy.FromUserId;
                        }

                    }

                }
                transaction.MainDocument.Document.Content = documentContent;

                _oMCSDbContext.SaveChanges();
                return transaction;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Transaction UpdateVipOutboundInternal(List<TransactionFollowUp> transactionFollowUps, List<TransactionCopy> transactionCopies, int transactionId, int? ConfidentialityId, byte[] documentContent, string summary)
        {
            try
            {
                Transaction transaction = _oMCSDbContext.Transactions.Include(x => x.Copies).Include(x => x.FollowUp).Include("MainDocument.Document").Where(a => a.Id == transactionId).FirstOrDefault();
                if (ConfidentialityId.HasValue && ConfidentialityId.Value > 0 && transaction.ConfidentialityId != ConfidentialityId.Value)
                    transaction.ConfidentialityId = ConfidentialityId.Value;
                transaction.Summary = summary;
                if (transactionFollowUps != null && transactionFollowUps.Count > 0)
                {
                    foreach (var followup in transactionFollowUps)
                    {
                        followup.DateTo = followup.FollowUpExpireDate;
                        followup.DateToH = followup.FollowUpExpireDateHj;

                        if (!(transaction.FollowUp != null && transaction.FollowUp.Any(x => x.FollowUpTypeId == followup.FollowUpTypeId && x.Active
                        && x.FollowUpExpireDate < DateTime.Now)))
                            transaction.FollowUp.Add(followup);
                    }

                }
                if (transactionCopies != null && transactionCopies.Count > 0)
                {
                    foreach (var copy in transactionCopies)
                    {
                        var oldCopy = transaction.Copies.Where(x => x.UserId == copy.UserId && x.EntityId == copy.EntityId).FirstOrDefault();
                        if (oldCopy == null)
                        {
                            transaction.Copies.Add(copy);
                        }
                        else
                        {
                            oldCopy.Viewed = false;
                            oldCopy.ModefiedOn = DateTime.Now;
                            oldCopy.ModefiedBy = copy.FromUserId;
                        }

                    }

                }
                transaction.MainDocument.Document.Content = documentContent;
                _oMCSDbContext.SaveChanges();
                return transaction;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Transaction UpdateVipOutboundDraft(List<TransactionFollowUp> transactionFollowUps, List<TransactionCopy> transactionCopies, int transactionId, int? ConfidentialityId, string mainDocumentContent, string pdfDocumentContent, bool isSigned)
        {
            try
            {
                Transaction transaction = _oMCSDbContext.Transactions.Include(x => x.Copies).Include(x => x.FollowUp).Where(a => a.Id == transactionId)
                    .Include(x => x.MainDocument).Include(x => x.MainDocument.Document).FirstOrDefault();
                if (ConfidentialityId.HasValue && ConfidentialityId.Value > 0 && transaction.ConfidentialityId != ConfidentialityId.Value)
                    transaction.ConfidentialityId = ConfidentialityId.Value;

                if (transactionFollowUps != null && transactionFollowUps.Count > 0)
                {
                    foreach (var followup in transactionFollowUps)
                    {
                        followup.DateTo = followup.FollowUpExpireDate;
                        followup.DateToH = followup.FollowUpExpireDateHj;
                        if (!(transaction.FollowUp != null && transaction.FollowUp.Any(x => x.FollowUpTypeId == followup.FollowUpTypeId && x.Active
                        && x.FollowUpExpireDate < DateTime.Now)))
                            transaction.FollowUp.Add(followup);
                    }

                }
                if (transactionCopies != null && transactionCopies.Count > 0)
                {
                    foreach (var copy in transactionCopies)
                    {
                        var oldCopy = transaction.Copies.Where(x => x.UserId == copy.UserId && x.EntityId == copy.EntityId).FirstOrDefault();
                        if (oldCopy == null)
                        {
                            transaction.Copies.Add(copy);
                        }
                        else
                        {
                            oldCopy.Viewed = false;
                            oldCopy.ModefiedOn = DateTime.Now;
                            oldCopy.ModefiedBy = copy.FromUserId;
                        }

                    }

                }
                transaction.MainDocument.Document.Content = Convert.FromBase64String(mainDocumentContent);
                if (!string.IsNullOrWhiteSpace(pdfDocumentContent))
                    transaction.OldWordDocumnt.Document.Content = Convert.FromBase64String(pdfDocumentContent);
                _oMCSDbContext.SaveChanges();
                return transaction;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public void UpdateProcessPeriodTransaction(int trnsId, int? ProcessPeriod)
        {
            try
            {
                Transaction transaction = GetTransactionById(trnsId);
                if (transaction != null)
                {
                    transaction.ProcessPeriodTransaction = ProcessPeriod;
                }


                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateTransactionExternalCopies(int transactionId, IList<TransactionExternalCopy> transactionExternalCopies)
        {
            try
            {
                Transaction transaction = GetTransactionById(transactionId);
                //Add all copies if there is no external copies added before
                if (transaction != null && (transaction.ExternalCopies == null || transaction.ExternalCopies.Count == 0))
                {
                    foreach (var item in transactionExternalCopies)
                    {
                        item.TransactionId = transactionId;
                        _oMCSDbContext.TransactionExternalCopies.Add(item);
                    }
                    _oMCSDbContext.SaveChanges();
                    return;
                }

                //There are external copies added before
                if (transaction != null && transaction.ExternalCopies != null && transaction.ExternalCopies.Count > 0)
                {
                    foreach (var item in transactionExternalCopies)
                    {
                        var originalTransactionExCopy = transaction.ExternalCopies
                                                                .Where(c => c.Id == item.Id && c.Id != 0)
                                                                .SingleOrDefault();
                        //Updated Item
                        if (originalTransactionExCopy != null)
                        {
                            var TransactionExCopyEntry = _oMCSDbContext.Entry(originalTransactionExCopy);
                            TransactionExCopyEntry.CurrentValues.SetValues(item);
                            TransactionExCopyEntry.Entity.TransactionId = transactionId;
                            if (item.ExternalPartyAttachment != null && item.ExternalPartyAttachment.Any())
                            {
                                List<int> ids = item.ExternalPartyAttachment.Where(epa => epa.IsDeleted == true).Select(ex => ex.Id).ToList();
                                var DocInfoIds = _oMCSDbContext.ExternalPartyAttachments.Where(Att => ids.Contains(Att.Id)).Select(exa => exa.DocumentInfo.Id).ToList();
                                var DocIds = _oMCSDbContext.DocumentsInfo.Where(doc => DocInfoIds.Contains(doc.Id)).Select(di => di.Document.Id).ToList();

                                _oMCSDbContext.Documents.RemoveRange(_oMCSDbContext.Documents.Where(doc => DocIds.Contains(doc.Id)));
                                _oMCSDbContext.DocumentsInfo.RemoveRange(_oMCSDbContext.DocumentsInfo.Where(docInfo => DocInfoIds.Contains(docInfo.Id)));
                                _oMCSDbContext.ExternalPartyAttachments.RemoveRange(_oMCSDbContext.ExternalPartyAttachments.Where(exa => ids.Contains(exa.Id)));
                                item.ExternalPartyAttachment.ForEach(e => e.TransactionExternalCopyId = item.Id);
                                originalTransactionExCopy.ExternalPartyAttachment.AddRange(item.ExternalPartyAttachment.Where(e => e.IsDeleted == false));
                            }
                        }
                        //Added item
                        else
                        {
                            item.TransactionId = transactionId;
                            //to add new attachment  to old attachment  in extCopy
                            var originalTransactionExCopy2 = transaction.ExternalCopies
                                                                            .Where(c => c.EntityId == item.EntityId && c.Id != 0)
                                                                            .SingleOrDefault();

                            if (originalTransactionExCopy2 != null)
                            {
                                foreach (var oldItem in originalTransactionExCopy2.ExternalPartyAttachment)
                                {
                                    if (item.ExternalPartyAttachment == null)
                                    {
                                        item.ExternalPartyAttachment = new List<ExternalPartyAttachment>();
                                    }
                                    item.ExternalPartyAttachment.Add(oldItem);
                                }
                            }
                            _oMCSDbContext.TransactionExternalCopies.Add(item);
                        }
                    }
                    //deleted items
                    foreach (var originalTransactionExCopy in transaction.ExternalCopies.Where(c => c.Id != 0).ToList())
                    {
                        if (!transactionExternalCopies.Any(c => c.Id == originalTransactionExCopy.Id))
                        {
                            //delete related attachment to externalCopy
                            if (originalTransactionExCopy.ExternalPartyAttachment != null)
                            {
                                foreach (ExternalPartyAttachment externalParty in originalTransactionExCopy.ExternalPartyAttachment.ToList())
                                {
                                    _oMCSDbContext.ExternalPartyAttachments.Remove(externalParty);
                                }
                            }
                            _oMCSDbContext.TransactionExternalCopies.Remove(originalTransactionExCopy);
                        }
                    }
                }
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateTransactionLinks(int transactionId, IList<TransactionLink> transactionLinks)
        {
            try
            {
                Transaction transaction = GetTransactionById(transactionId);

                //Add all links if there is no links added before
                if (transaction != null && (transaction.Links == null || transaction.Links.Count == 0))
                {
                    transaction.Links = transactionLinks;
                    _oMCSDbContext.SaveChanges();
                    return;
                }


                //There are links added before
                if (transaction != null && transaction.Links != null && transaction.Links.Count > 0)
                {
                    foreach (var item in transactionLinks)
                    {
                        var originalTransactionLink = transaction.Links
                                                                .Where(c => c.Id == item.Id && c.Id != 0)
                                                                .SingleOrDefault();

                        //Added item
                        if (originalTransactionLink == null)
                        {
                            transaction.Links.Add(item);
                        }
                    }

                    foreach (var originalTransactionLink in transaction.Links.Where(c => c.Id != 0).ToList())
                    {
                        if (!transactionLinks.Any(c => c.Id == originalTransactionLink.Id))
                        {
                            _oMCSDbContext.TransactionLinks.Remove(originalTransactionLink);
                        }
                    }
                }


                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void FollowUpAddTransactionLinks(int transactionId, IList<TransactionLink> transactionLinks)
        {
            try
            {
                Transaction transaction = GetTransactionById(transactionId);

                //Add all links if there is no links added before
                if (transaction != null && (transaction.Links == null || transaction.Links.Count == 0))
                {
                    transaction.Links = transactionLinks;
                    _oMCSDbContext.SaveChanges();
                    return;
                }


                //There are links added before
                if (transaction != null && transaction.Links != null && transaction.Links.Count > 0)
                {
                    foreach (var item in transactionLinks)
                    {
                        var originalTransactionLink = transaction.Links
                                                                .Where(c => c.Id == item.Id && c.Id != 0)
                                                                .SingleOrDefault();

                        //Added item
                        if (originalTransactionLink == null)
                        {
                            transaction.Links.Add(item);
                        }
                    }


                }


                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateTransactionContactDate(int transactionId, string ContactDateH)
        {
            try
            {
                Transaction transaction = GetTransactionById(transactionId);
                transaction.ContactDateH = ContactDateH;
                _oMCSDbContext.SaveChanges();
                return;


            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public void UpdateTransactionSubjectClassifications(int transactionId, IList<TransactionSubjectClassification> transactionSubjectClassifications)
        {
            try
            {
                Transaction transaction = GetTransactionById(transactionId);

                if (transaction != null && transaction.SubjectClassifications != null &&
                    transaction.SubjectClassifications.Count > 0)
                {
                    transaction.SubjectClassifications.ToList().ForEach(item => _oMCSDbContext.TransactionSubjectClassifications.Remove(item));

                    _oMCSDbContext.SaveChanges();
                }

                transaction.SubjectClassifications = transactionSubjectClassifications;

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateTransactionAttachments(int transactionId, IList<Attachment> attachments)
        {
            try
            {
                attachments.ToList().ForEach(a => a.TransactionId = transactionId);
                Transaction transaction = GetTransactionById(transactionId);

                //Add all attachments if there is no copies added before
                if (transaction != null && (transaction.Attachments == null || transaction.Attachments.Count == 0))
                {
                    foreach (var item in attachments)
                    {
                        item.TransactionId = transactionId;
                        _oMCSDbContext.Attachments.Add(item);
                    }
                    _oMCSDbContext.SaveChanges();
                    return;
                }

                //There are attachments added before
                if (transaction != null && transaction.Attachments != null && transaction.Attachments.Count > 0)
                {
                    foreach (var item in attachments)
                    {
                        var originalTransactionAttachment = transaction.Attachments
                                                                .Where(c => c.Id == item.Id && c.Id != 0)
                                                                .SingleOrDefault();
                        //Updated Item
                        if (originalTransactionAttachment != null)
                        {
                            var TransactionAttachmentEntry = _oMCSDbContext.Entry(originalTransactionAttachment);
                            TransactionAttachmentEntry.CurrentValues.SetValues(item);
                            if (item.DocumentInfo != null)
                            {
                                originalTransactionAttachment.DocumentInfo = item.DocumentInfo;
                                originalTransactionAttachment.DocumentInfo.TransactionId = transactionId;
                            }

                            TransactionAttachmentEntry.Entity.TransactionId = transactionId;

                            //if (originalTransactionAttachment.DocumentInfo != null)
                            //{
                            //    item.DocumentInfo.Id = originalTransactionAttachment.DocumentInfo.Id;
                            //    item.DocumentInfo.Document.Id = originalTransactionAttachment.DocumentInfo.Document.Id;
                            //    var documentInfoEntry = _oMCSDbContext.Entry(originalTransactionAttachment.DocumentInfo);
                            //    documentInfoEntry.CurrentValues.SetValues(item.DocumentInfo);

                            //    var documentEntry = _oMCSDbContext.Entry(originalTransactionAttachment.DocumentInfo.Document);
                            //    documentEntry.CurrentValues.SetValues(item.DocumentInfo.Document);
                            //}
                        }
                        //Added item
                        else
                        {
                            item.TransactionId = transactionId;
                            _oMCSDbContext.Attachments.Add(item);
                        }
                    }
                    foreach (var originalTransactionAttachment in transaction.Attachments.Where(c => c.Id != 0).ToList())
                    {
                        if (!attachments.Any(c => c.Id == originalTransactionAttachment.Id))
                        {
                            _oMCSDbContext.Attachments.Remove(originalTransactionAttachment);
                        }
                    }
                }
                _oMCSDbContext.SaveChanges();
                /////////////////

                //if (transaction != null && transaction.Attachments != null && transaction.Attachments.Count > 0)
                //{
                //    transaction.Attachments.ToList().ForEach(a =>
                //    {
                //        if (a.DocumentInfo != null)
                //        {
                //            if (a.DocumentInfo.Document != null)
                //            {
                //                _oMCSDbContext.Entry(a.DocumentInfo.Document).State = EntityState.Deleted;
                //            }

                //            _oMCSDbContext.Entry(a.DocumentInfo).State = EntityState.Deleted;

                //        }

                //        _oMCSDbContext.Attachments.Remove(a);

                //    });

                //    _oMCSDbContext.SaveChanges();
                //}

                //transaction.Attachments = attachments;

                //_oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void AddDeliveryReportToAttachment(Attachment attachment)
        {
            try
            {
                _oMCSDbContext.Attachments.Add(attachment);
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Transaction GetTransactionById(int transactionId)
        {
            try
            {
                return FindBy(t => t.Id == transactionId && !t.IsDeleted);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Transaction GetTransactionById(int TransactionId, string cultureName, bool isNotification = false)
        {
            try
            {
                Transaction transaction = _oMCSDbContext.Transactions.Where(t => t.Id == TransactionId).FirstOrDefault();

                if (transaction == null || transaction.IsDeleted)
                {
                    return null;
                }

                Transaction result = new Transaction
                {
                    Id = transaction.Id,
                    CreatedBy = transaction.CreatedBy,
                    Date = transaction.Date,
                    DateH = transaction.DateH,
                    Status = transaction.Status,
                    TransactionCategory = transaction.TransactionCategory,
                    TransactionCategoryId = transaction.TransactionCategoryId,
                    OrgUnitId = transaction.OrgUnitId,
                    ToUserId = transaction.ToUserId,
                    EntityId = transaction.EntityId,
                    ExternalPartyId = transaction.ExternalPartyId,
                    Names = transaction.Names,
                    Number = transaction.Number,
                    DocumentNumber = transaction.DocumentNumber,
                    Remarks = transaction.Remarks,
                    Subject = transaction.Subject,
                    PrintedDeliveryReport = transaction.PrintedDeliveryReport,
                    DeliveryReportNumber = transaction.DeliveryReportNumber,
                    MainDocument = transaction.MainDocument,
                    OldWordDocumnt = transaction.OldWordDocumnt,
                    RemindDate = transaction.RemindDate,
                    RemindDateH = transaction.RemindDateH,
                    OutboundDraftEditorType = transaction.OutboundDraftEditorType,
                    IsSigned = transaction.IsSigned,
                    OutboundDraftId = transaction.OutboundDraftId,
                    DeliveryMethodId = transaction.DeliveryMethodId,
                    InboundDateH = transaction.InboundDateH,
                    IsDraft = transaction.IsDraft,
                    ExternalPartyManagerId = transaction.ExternalPartyManagerId,
                    LetterTypeId = transaction.LetterTypeId,
                    RejectionReason = transaction.RejectionReason,
                    Year = transaction.Year,
                    YearH = transaction.YearH,
                    TransactionTypeId = transaction.TransactionTypeId,
                    SuggestedTopicId = transaction.SuggestedTopicId,
                    UserId = transaction.UserId,
                    SignedByUserId = transaction.SignedByUserId,
                    PostCode = transaction.PostCode,
                    POBox = transaction.POBox,
                    PriorityId = transaction.PriorityId,
                    StatusId = transaction.StatusId,
                    MainDocumentId = transaction.MainDocumentId,
                    ConfidentialityId = transaction.ConfidentialityId,
                    IsForIndividual = transaction.IsForIndividual,
                    ReporterId = transaction.ReporterId,
                    DeliveryNumber = transaction.DeliveryNumber,
                    SubjectClassificationsId = transaction.SubjectClassificationsId,
                    RecordNumber = transaction.RecordNumber,
                    // SideContactExternalEntity = transaction.SideContactExternalEntity,
                    SideContactExternalEntityID = transaction.SideContactExternalEntityID,
                    NumberContact = transaction.NumberContact,
                    ContactDateH = transaction.ContactDateH,
                    IsPresentationDraft = transaction.IsPresentationDraft,
                    PresentationDraftNumber = transaction.PresentationDraftNumber,
                    OutBoundDraftNumber = transaction.OutBoundDraftNumber,
                    IsElcOutBound = transaction.IsElcOutBound,
                    NeedAcknowled = transaction.NeedAcknowled,
                    OldWordDocumntId = transaction.OldWordDocumntId,
                    IsAppointment = transaction.IsAppointment,
                    ProcessPeriodTransaction = transaction.ProcessPeriodTransaction,
                    IsDecisionDraft = transaction.IsDecisionDraft,
                    Summary = transaction.Summary,
                    InboundIntendedPerson = transaction.InboundIntendedPerson,
                    ComplaintNumber = transaction.ComplaintNumber,
                    DeliveryMethod = (transaction.DeliveryMethod != null) ? new Lookup
                    {
                        Id = transaction.DeliveryMethodId,
                        Text = transaction.DeliveryMethod.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    SuggestedTopic = (transaction.SuggestedTopic != null) ? new SuggestedTopic
                    {
                        Id = transaction.SuggestedTopic.Id,
                        Text = transaction.SuggestedTopic.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()

                    } : null,

                    SignedByUser = (transaction.SignedByUser != null) ? new UserProfile
                    {
                        Id = transaction.SignedByUser.Id,
                        LocalName = transaction.SignedByUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()

                    } : null,

                    User = (transaction.User != null) ? new UserProfile
                    {
                        Id = transaction.User.Id,
                        LocalName = transaction.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    OrgUnit = (transaction.OrgUnit != null) ? new OrgUnit
                    {
                        Id = transaction.OrgUnit.Id,
                        LocalName = transaction.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Assignments = transaction.Assignments.Select(a => new TransactionAssignment
                    {
                        Description = a.Description,
                        Date = a.Date,
                        DateH = a.DateH,
                        Id = a.Id,
                        TransactionPathId = a.TransactionPathId,
                        CurrentPathStep = a.CurrentPathStep,
                        Tray = (a.Tray != null) ? new Tray
                        {
                            Id = a.Tray.Id,
                            LocalName = a.Tray.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        ToUser = (a.ToUser != null) ? new UserProfile
                        {
                            Id = a.ToUser.Id,
                            LocalName = a.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        Transaction = (a.Transaction != null) ? new Transaction
                        {
                            Id = a.Transaction.Id
                        } : null,

                        Action = (a.Action != null) ? new Action
                        {
                            Id = a.Action.Id,
                            LocalName = a.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                            Type = a.Action.Type
                        } : null,

                        FromEntity = (a.FromEntity != null) ? new OrgUnit
                        {
                            Id = a.FromEntity.Id,
                            LocalName = a.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        ToEntity = (a.ToEntity != null) ? new OrgUnit
                        {
                            Id = a.ToEntity.Id,
                            LocalName = a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        FromUser = (a.FromUser != null) ? new UserProfile
                        {
                            Id = a.FromUser.Id,
                            LocalName = a.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null
                    }
                    ).ToList(),


                    ExternalCopies = transaction.ExternalCopies.Select(c => new TransactionExternalCopy
                    {
                        Id = c.Id,
                        Date = c.Date,
                        DateH = c.DateH,
                        ActionId = c.ActionId,
                        TransactionId = c.TransactionId,
                        UserId = c.UserId,
                        Viewed = c.Viewed,
                        Action = (c.Action != null) ? new Action
                        {
                            Id = c.Action.Id,
                            LocalName = c.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        User = (c.User != null) ? new ExternalPartyManager
                        {
                            Id = c.User.Id,
                            LocalName = c.User.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        Entity = (c.Entity != null) ? new ExternalParty
                        {
                            Id = c.Entity.Id,
                            LocalName = c.Entity.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                            YasserRegistered = c.Entity.YasserRegistered
                        } : null,
                        EntityId = c.EntityId,

                        Status = c.Status,
                        SendEmail = c.SendEmail,
                        ExternalPartyAttachment = c.ExternalPartyAttachment.ToList().Select(o => new ExternalPartyAttachment
                        {
                            Id = o.Id,
                            PartyId = o.PartyId,
                            Name = o.Name,
                            DocumentInfo = new DocumentInfo
                            {
                                Document = new Document
                                {
                                    Id = o.DocumentInfo.Document.Id,
                                    Content = o.DocumentInfo.Document.Content
                                },

                                Id = o.DocumentInfo.Id,
                                MimeType = o.DocumentInfo.MimeType,
                                Name = o.DocumentInfo.Name,
                                Size = o.DocumentInfo.Size,
                                IsDeleted = o.DocumentInfo.IsDeleted,
                                ECMId = o.DocumentInfo.ECMId,
                                FromEntity = o.DocumentInfo.FromEntity,
                                FromUser = o.DocumentInfo.FromUser,
                                FromEntityId = o.DocumentInfo.FromEntityId,
                                FromUserId = o.DocumentInfo.FromUserId
                            },

                        }).ToList(),
                        FromEntityId = c.FromEntityId,
                        FromUserId = c.FromUserId,
                        FromEntity = (c.FromEntity != null) ? new OrgUnit
                        {
                            Id = c.FromEntity.Id,
                            LocalName = c.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        FromUser = (c.FromUser != null) ? new UserProfile
                        {
                            Id = c.FromUser.Id,
                            LocalName = c.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null
                    }
                    ).ToList(),
                    Links = transaction.Links.Select(tl => new TransactionLink
                    {
                        Id = tl.Id,
                        TransactionId = tl.TransactionId,


                        Type = (tl.Type != null) ? new Link
                        {
                            Id = tl.Type.Id,
                            Text = tl.Type.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        ToTransaction = (tl.ToTransaction != null) ? new Transaction
                        {
                            Id = tl.ToTransaction.Id,
                            Number = tl.ToTransaction.Number,
                            Subject = tl.ToTransaction.Subject,
                            DateH = tl.ToTransaction.DateH,
                            Date = tl.ToTransaction.Date,
                            TransactionCategoryId = tl.ToTransaction.TransactionCategoryId,
                            TransactionType = (transaction.TransactionType != null) ? new TransactionType
                            {
                                Id = transaction.TransactionType.Id,
                                Text = transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            } : null,
                            YearH = tl.ToTransaction.YearH,
                            Year = tl.ToTransaction.Year,
                            TransactionCategory = new Lookup()
                            {
                                Id = tl.ToTransaction.TransactionCategoryId,
                                Text = tl.ToTransaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            },
                            ConfidentialityId = tl.ToTransaction.ConfidentialityId,
                            Assignments = tl.ToTransaction.Assignments.Select(a => new TransactionAssignment
                            {
                                Description = a.Description,
                                Date = a.Date,
                                DateH = a.DateH,
                                Id = a.Id,
                                TransactionPathId = a.TransactionPathId,
                                CurrentPathStep = a.CurrentPathStep,
                                Tray = (a.Tray != null) ? new Tray
                                {
                                    Id = a.Tray.Id,
                                    LocalName = a.Tray.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                } : null,

                                ToUser = (a.ToUser != null) ? new UserProfile
                                {
                                    Id = a.ToUser.Id,
                                    LocalName = a.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                } : null,

                                Transaction = (a.Transaction != null) ? new Transaction
                                {
                                    Id = a.Transaction.Id
                                } : null,

                                Action = (a.Action != null) ? new Action
                                {
                                    Id = a.Action.Id,
                                    LocalName = a.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                                    Type = a.Action.Type
                                } : null,

                                FromEntity = (a.FromEntity != null) ? new OrgUnit
                                {
                                    Id = a.FromEntity.Id,
                                    LocalName = a.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                } : null,

                                ToEntity = (a.ToEntity != null) ? new OrgUnit
                                {
                                    Id = a.ToEntity.Id,
                                    LocalName = a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                } : null,
                                FromUser = (a.FromUser != null) ? new UserProfile
                                {
                                    Id = a.FromUser.Id,
                                    LocalName = a.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                } : null
                            }).ToList()

                        } : null,


                    }
                    ).ToList(),

                    SubjectClassifications = transaction.SubjectClassifications.Select(ts => new TransactionSubjectClassification
                    {
                        Id = ts.Id,
                        TransactionId = ts.TransactionId,

                        SubjectClassification = (ts.SubjectClassification != null) ? new SubjectClassification
                        {
                            Id = ts.SubjectClassification.Id,
                            Text = ts.SubjectClassification.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                    }
                   ).ToList(),

                    Attachments = transaction.Attachments.Select(a => new Attachment
                    {
                        DocumentInfo = (a.DocumentInfo != null) ? new DocumentInfo
                        {
                            Document = (a.DocumentInfo.Document != null) ? new Document
                            {
                                Id = a.DocumentInfo.Document.Id
                            } : null,

                            Id = a.DocumentInfo.Id,
                            MimeType = a.DocumentInfo.MimeType,
                            Name = a.DocumentInfo.Name,
                            Size = a.DocumentInfo.Size,
                            FromEntityId = a.DocumentInfo.FromEntityId,
                            FromUserId = a.DocumentInfo.FromUserId,
                            FromEntity = (a.DocumentInfo.FromEntity != null) ? new OrgUnit
                            {
                                Id = a.DocumentInfo.FromEntity.Id,
                                LocalName = a.DocumentInfo.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            } : null,
                            FromUser = (a.DocumentInfo.FromUser != null) ? new UserProfile
                            {
                                Id = a.DocumentInfo.FromUser.Id,
                                LocalName = a.DocumentInfo.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            } : null

                        } : null,

                        Type = (a.Type != null) ? new AttachmentType
                        {
                            Archivable = a.Type.Archivable,
                            Id = a.Type.Id,
                            Text = a.Type.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        Description = a.Description,
                        Count = a.Count,
                        Id = a.Id,
                        TypeId = a.TypeId,
                        AttachmentSource = a.AttachmentSource,
                        CreatedBy = a.CreatedBy,
                    }).ToList(),

                    Entity = (transaction.Entity != null) ? new OrgUnit
                    {
                        Id = transaction.Entity.Id,
                        LocalName = transaction.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    ToUser = (transaction.ToUser != null) ? new UserProfile
                    {
                        Id = transaction.ToUser.Id,
                        LocalName = transaction.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Priority = (transaction.Priority != null) ? new Priority
                    {
                        Id = transaction.Priority.Id,
                        Text = transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Confidentiality = (transaction.Confidentiality != null) ? new Permission
                    {
                        Id = transaction.Confidentiality.Id,
                        LocalName = transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                        Code = transaction.Confidentiality.Code
                    } : null,

                    TransactionType = (transaction.TransactionType != null) ? new TransactionType
                    {
                        Id = transaction.TransactionType.Id,
                        Text = transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    LetterType = (transaction.LetterType != null) ? new LetterType
                    {
                        Id = transaction.LetterType.Id,
                        Text = transaction.LetterType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    City = (transaction.City != null) ? new City
                    {
                        Id = transaction.City.Id,
                        Text = transaction.City.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    ExternalParty = (transaction.ExternalParty != null) ? new ExternalParty
                    {
                        Id = transaction.ExternalParty.Id,
                        Number = transaction.ExternalParty.Number,
                        LocalName = transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                        LocalAddress = transaction.ExternalParty.LocalAddress
                    } : null,


                    ExternalPartyManager = (transaction.ExternalPartyManager != null) ? new ExternalPartyManager
                    {
                        Id = transaction.ExternalPartyManager.Id,
                        LocalName = transaction.ExternalPartyManager.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    FollowUp = transaction.FollowUp.Where(f => !f.IsDeleted).Select(f => new TransactionFollowUp
                    {
                        Id = f.Id,
                        TransactionId = f.TransactionId,
                        DateTo = f.DateTo,
                        DateToH = f.DateToH,
                        IsDeleted = f.IsDeleted,
                        CreatedOn = f.CreatedOn,
                        CreatedBy = f.CreatedBy,
                        ModefiedOn = f.ModefiedOn,
                        ModefiedBy = f.ModefiedBy,
                        CreatingUserId = f.CreatingUserId,
                        CreatingUser = (f.CreatingUser != null) ? new UserProfile
                        {
                            Id = f.CreatingUserId,
                            LocalName = f.CreatingUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        CreatingEntityId = f.CreatingEntityId,
                        CreatingEntity = (f.CreatingEntity != null) ? new OrgUnit
                        {
                            Id = f.CreatingEntity.Id,
                            LocalName = f.CreatingEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        FollowUpEntityId = f.FollowUpEntityId,
                        FollowUpEntity = (f.FollowUpEntity != null) ? new OrgUnit
                        {
                            Id = f.FollowUpEntity.Id,
                            LocalName = f.FollowUpEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        FollowUpUserId = f.FollowUpUserId,
                        FollowUpUser = (f.FollowUpUser != null) ? new UserProfile
                        {
                            Id = f.FollowUpUserId.Value,
                            LocalName = f.FollowUpUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        CreationDate = f.CreationDate,
                        FollowUpExpireDate = f.FollowUpExpireDate,
                        Notes = f.Notes,
                        Active = f.Active,
                        FollowUpProccessId = f.FollowUpProccessId,
                        ProccessPeriod = f.ProccessPeriod,
                        ProccessPeriodDate = f.ProccessPeriodDate,
                        FollowUpProccessNote = f.FollowUpProccessNote,
                        FollowUpCompletionDate = f.FollowUpCompletionDate,
                        FollowUpCompletionDateHj = f.FollowUpCompletionDateHj,
                        FollowUpExpireDateHj = f.FollowUpExpireDateHj,
                        FollowUpReceiveDate = f.FollowUpReceiveDate,
                        FollowUpReason = f.FollowUpReason,
                        FollowUpTypeId = f.FollowUpTypeId,
                        FollowUpStatusId = f.FollowUpStatusId,
                        FollowUpMethodId = f.FollowUpMethodId,
                        FollowUpPriortyId = f.FollowUpPriortyId,
                        FollowUpSourceId = f.FollowUpSourceId,
                        FollowUpProgressId = f.FollowUpProgressId,
                        IsCopy = f.IsCopy,
                        IsReminder = f.IsReminder,
                        IsEscalated = f.IsEscalated,
                        IsImportant = f.IsImportant,
                        HasChild = f.HasChild,
                        ParentId = f.ParentId,


                    }).ToList(),
                    LetterNumber = transaction.LetterNumber
                };

                if (!isNotification && result.MainDocument != null && result.MainDocument.Document != null)
                    result.MainDocument.Document.Content = null;

                if (result.OldWordDocumnt != null && result.OldWordDocumnt.Document != null)
                    result.OldWordDocumnt.Document.Content = null;

                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool CheckUserHasPermission(List<int> transactionId, int? userId)
        {
            try
            {
                if (!userId.HasValue)
                {

                    return true;
                }
                int? userWeight = _oMCSDbContext.Permissions.Where(p => p.PermissionGroups.Any(pg => pg.GroupUsers.Any(gu => gu.UserId == userId))).Max(p => p.Weight);

                return !_oMCSDbContext.Transactions.Any(t => transactionId.Any(tid => tid == t.Id) && (t.Confidentiality.Weight > userWeight
                && !t.SpecialAuthorizations.Any(ts => ts.UserProfileId == userId && (!ts.ExpiredDate.HasValue || ts.ExpiredDate > DateTime.Now))));


            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void AddTransactionSpecialAuthorize(int transactionId, int userId)
        {
            try
            {

                int? userWeight = _oMCSDbContext.Permissions.Where(p => p.PermissionGroups.Any(pg => pg.GroupUsers.Any(gu => gu.UserId == userId))).Max(p => p.Weight);

                bool hasNotViewPermission = !_oMCSDbContext.Transactions.Any(t => transactionId == t.Id && (t.Confidentiality.Weight > userWeight
                && !t.SpecialAuthorizations.Any(ts => ts.UserProfileId == userId && (!ts.ExpiredDate.HasValue || ts.ExpiredDate > DateTime.Now))));

                if (!hasNotViewPermission)
                {
                    TransactionSpecialAuthorize transactionSpecialAuthorize = new TransactionSpecialAuthorize
                    {
                        TransactionId = transactionId,
                        UserProfileId = userId,
                        CreatedOn = DateTime.Now
                    };
                    _oMCSDbContext.TransactionSpecialAuthorizes.Add(transactionSpecialAuthorize);
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Transaction GetTransactionByIdAsNotacking(int transactionId)
        {
            try
            {
                return _oMCSDbContext.Transactions.Where(t => t.Id == transactionId && !t.IsDeleted).AsNoTracking().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Transaction GetTransactionByIdForNotification(int transactionId)
        {
            try
            {
                return _oMCSDbContext.Transactions
                    .Include(a => a.TransactionCategory)
                    .Include(c => c.Priority)
                    .Include(c => c.Confidentiality)
                    .FirstOrDefault(t => t.Id == transactionId && !t.IsDeleted);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Transaction GetTransactionByTransactionNumber(long transactionNumber)
        {
            try
            {
                int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                return FindBy(t =>
                t.Number == transactionNumber
                && (t.TransactionCategoryId == ExternalOutbound)
                && !t.IsDraft
                && !t.IsPresentationDraft
                && !t.IsDeleted
                && t.Year == DateTime.Now.Year);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Transaction GetTransactionByTransactionId(long transactionId)
        {
            try
            {
                return FindBy(t => t.Id == transactionId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool CheckIfTransactionSigned(int transactionId)
        {
            try
            {
                Transaction transaction = FindBy(t => t.Id == transactionId && !t.IsDeleted);

                if (transaction != null)
                {
                    return transaction.IsSigned;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Transaction LoadTransaction(int transactionId)
        {
            try
            {
                return _oMCSDbContext.Transactions.Single(t => t.Id == transactionId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int GetTransactionByIdAndOrgUnit(int transactionId, int OrgUnitId)
        {
            try
            {
                return _oMCSDbContext.TransactionAssignmentHistories.Where(t => t.TransactionId == transactionId && (t.ToEntityId == OrgUnitId || OrgUnitId == -1)).Count();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public TransactionCertificateInfo GetTransactionCertificate(int transactionId, string cultureName, int? userWeight)
        {
            try
            {
                Transaction transaction = _oMCSDbContext.Transactions.Where(t => t.Id == transactionId && t.Confidentiality.Weight <= userWeight).FirstOrDefault();

                TransactionCertificateInfo transactionCertificate = new TransactionCertificateInfo();

                transactionCertificate.Id = transaction.Id;
                transactionCertificate.Date = transaction.Date;
                transactionCertificate.DateH = transaction.DateH;
                transactionCertificate.Number = transaction.Number;
                transactionCertificate.DocumentNumber = transaction.DocumentNumber;
                transactionCertificate.Subject = transaction.Subject;
                transactionCertificate.Status = transaction.Status.Localizations.Where(s => s.Culture.ShortName == cultureName).LocalText();
                transactionCertificate.UserCreatedBy = transaction.User.LocalizationIdentifier.Localizations.Where(u => u.Culture.ShortName == cultureName).LocalText();
                transactionCertificate.OrgUnitCreatedBy = transaction.OrgUnit.LocalizationIdentifier.Localizations.Where(o => o.Culture.ShortName == cultureName).LocalText();
                transactionCertificate.Priority = transaction.Priority.LocalizationIdentifier.Localizations.Where(s => s.Culture.ShortName == cultureName).LocalText();
                transactionCertificate.Confidentiality = transaction.Confidentiality.Name.Localizations.Where(s => s.Culture.ShortName == cultureName).LocalText();
                transactionCertificate.ConfidentialityId = transaction.ConfidentialityId;
                transactionCertificate.TransactionType = transaction.TransactionType.LocalizationIdentifier.Localizations.Where(s => s.Culture.ShortName == cultureName).LocalText();
                transactionCertificate.LetterType = transaction.LetterType != null ? transaction.LetterType.LocalizationIdentifier.Localizations.Where(s => s.Culture.ShortName == cultureName).LocalText() : null;
                transactionCertificate.ExternalParty = transaction.ExternalParty != null ? transaction.ExternalParty.Name.Localizations.Where(p => p.Culture.ShortName == cultureName).LocalText() : null;
                transactionCertificate.MainDocument = transaction.MainDocument;
                transactionCertificate.RemindDateH = transaction.RemindDate != null ? transaction.RemindDateH : null;
                transactionCertificate.RemindTime = transaction.RemindDate != null ? transaction.RemindDate.Value.ToString("hh:mm tt") : null;
                transactionCertificate.CurrentAssignment = transaction.Assignments.FirstOrDefault();
                transactionCertificate.InboundIntendedPerson = transaction.InboundIntendedPerson;
                transactionCertificate.DeliveryMethod = transaction.DeliveryMethod.Localizations.Where(d => d.Culture.ShortName == cultureName).FirstOrDefault().Text;
                transactionCertificate.HasDate = transaction.RemindDate != null ? true : false;
                transactionCertificate.IsForIndividual = transaction.IsForIndividual;
                transactionCertificate.Remarks = transaction.Remarks;
                transactionCertificate.SignedBy = transaction.SignedByUser?.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                transactionCertificate.ToUser = transaction.ToUser?.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                transactionCertificate.ToEntity = transaction.Entity?.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                transactionCertificate.ProcessPeriodTransaction = (int)transaction.ProcessPeriodTransaction;
                transactionCertificate.CurrentAssignment.ToEntity.LocalName = transactionCertificate.CurrentAssignment.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                transactionCertificate.NumberContact = transaction.NumberContact;
                transactionCertificate.SideContactExternalEntityName = transaction.Sidecontactexternalentity != null ? transaction.Sidecontactexternalentity?.Name.Localizations.Where(p => p.Culture.ShortName == cultureName).LocalText() : null;
                transactionCertificate.RecordNumber = transaction.RecordNumber;
                transactionCertificate.LetterNumber = transaction.LetterNumber;
                transactionCertificate.Encrypted = transaction.Encrypted;
               
                transactionCertificate.FileNumber = transaction?.SubjectTransactions.FirstOrDefault()?.Number ?? 0;
                transactionCertificate.FileDescription = transaction?.SubjectTransactions.FirstOrDefault()?.Description;
                string fullClassificationName = "";
                if (transaction?.SubjectTransactions != null && transaction?.SubjectTransactions.Count > 0)
                {

                    fullClassificationName = transaction?.SubjectTransactions.FirstOrDefault().IC_SUBJECTS.ITEM_DISPLAY;
                    bool hasParent = transaction?.SubjectTransactions.FirstOrDefault().IC_SUBJECTS.PARENT_ID > 0;
                    var parent = transaction?.SubjectTransactions.FirstOrDefault().IC_SUBJECTS.Parent;
                    while (hasParent)
                    {
                      
                        fullClassificationName += " - " + parent.ITEM_DISPLAY;
                        if (parent.PARENT_ID > 0)
                        {
                            parent = parent.Parent;
                        }
                        else
                        {
                            hasParent = false;
                        }

                    }


                    transactionCertificate.ClassificationName = fullClassificationName;


                }
                //transactionCertificate.Links = transaction.Links.Select(tl => new TransactionLink
                //{
                //    Id = tl.Id,
                //    TransactionId = tl.TransactionId,




                //    Type = (tl.Type != null) ? new Link
                //    {
                //        Id = tl.Type.Id,
                //        Text = tl.Type.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                //    } : null,



                //    ToTransaction = (tl.ToTransaction != null) ? new Transaction
                //    {
                //        Id = tl.ToTransaction.Id,
                //        Number = tl.ToTransaction.Number,
                //        Subject = tl.ToTransaction.Subject,
                //        DateH = tl.ToTransaction.DateH,
                //        Date = tl.ToTransaction.Date,
                //        TransactionCategoryId = tl.ToTransaction.TransactionCategoryId,
                //        TransactionType = (transaction.TransactionType != null) ? new TransactionType
                //        {
                //            Id = transaction.TransactionType.Id,
                //            Text = transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                //        } : null,
                //        YearH = tl.ToTransaction.YearH,
                //        Year = tl.ToTransaction.Year,
                //        TransactionCategory = new Lookup()
                //        {
                //            Id = tl.ToTransaction.TransactionCategoryId,
                //            Text = tl.ToTransaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                //        },
                //        ConfidentialityId = tl.ToTransaction.ConfidentialityId
                //    } : null,
                //}).ToList();

                switch ((TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionStatus, string.Empty))
                {
                    case TransactionCategory.Inbound:
                    case TransactionCategory.InternalOutbound:
                        {
                            if (transaction.ExternalPartyManager != null)
                            {
                                Localization localization = transaction.ExternalPartyManager.Name.Localizations
                                    .Where(m => m.Culture.ShortName == cultureName).FirstOrDefault();

                                if (localization != null)
                                {
                                    transactionCertificate.Manager = localization.Text;
                                }

                                if (transaction.ToUser != null)
                                {
                                    localization = transaction.ToUser.LocalizationIdentifier.Localizations
                                        .Where(m => m.Culture.ShortName == cultureName).FirstOrDefault();
                                }

                                if (localization != null)
                                {
                                    transactionCertificate.ToUser = localization.Text;
                                }
                            }
                        }
                        break;
                    case TransactionCategory.ExternalOutbound:
                        {
                            Localization localization = null;
                            if (transaction.ExternalPartyManager != null)
                            {
                                localization = transaction.ExternalPartyManager.Name.Localizations
                                    .Where(m => m.Culture.ShortName == cultureName).FirstOrDefault();

                                if (localization != null)
                                {
                                    transactionCertificate.Manager = localization.Text;
                                }
                            }
                            if (transaction.SignedByUser != null)
                            {
                                localization = transaction.SignedByUser.LocalizationIdentifier.Localizations
                                        .Where(m => m.Culture.ShortName == cultureName).FirstOrDefault();
                            }

                            if (localization != null)
                            {
                                transactionCertificate.ToUser = localization.Text;
                            }
                        }
                        break;
                }

                return transactionCertificate;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Transaction GetTransactionByNumberAndYear(Expression<Func<Transaction, bool>> @where, string cultureName)
        {
            try
            {
                Transaction transaction = _oMCSDbContext.Transactions.Where(@where).FirstOrDefault();
                if (transaction == null || transaction.IsDeleted)
                {
                    return null;
                }
                Transaction result = new Transaction
                {
                    Number = transaction.Number,
                    Subject = transaction.Subject,
                    Priority = (transaction.Priority != null) ? new Priority
                    {
                        Id = transaction.Priority.Id,
                        Text = transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    Confidentiality = (transaction.Confidentiality != null) ? new Permission
                    {
                        Id = transaction.Confidentiality.Id,
                        LocalName = transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                        Code = transaction.Confidentiality.Code
                    } : null,
                };
                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public List<Transaction> GetTransactionsByNationalId(Expression<Func<Transaction, bool>> @where, string cultureName)
        {
            try
            {
                List<Transaction> transaction = _oMCSDbContext.Transactions.Where(@where).ToList();
                if (transaction == null)
                {
                    return new List<Transaction>();
                }

                transaction = transaction.Select(t => new Transaction
                {
                    Number = t.Number,
                    Subject = t.Subject,
                    Priority = (t.Priority != null) ? new Priority
                    {
                        Id = t.Priority.Id,
                        Text = t.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    Confidentiality = (t.Confidentiality != null) ? new Permission
                    {
                        Id = t.Confidentiality.Id,
                        LocalName = t.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                        Code = t.Confidentiality.Code
                    } : null
                }).ToList();

                return transaction;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Transaction GetTransaction(Expression<Func<Transaction, bool>> @where, int userid, string cultureName, bool isNotification = false)
        {
            try
            {
                Transaction transaction = _oMCSDbContext.Transactions.Where(@where).FirstOrDefault();

                if (transaction == null || transaction.IsDeleted)
                {
                    return null;
                }

                Transaction result = new Transaction
                {
                    Id = transaction.Id,
                    CreatedBy = transaction.CreatedBy,
                    Date = transaction.Date,
                    DateH = transaction.DateH,
                    Status = transaction.Status,
                    TransactionCategory = transaction.TransactionCategory,
                    TransactionCategoryId = transaction.TransactionCategoryId,
                    OrgUnitId = transaction.OrgUnitId,
                    ToUserId = transaction.ToUserId,
                    EntityId = transaction.EntityId,
                    ExternalPartyId = transaction.ExternalPartyId,
                    Names = transaction.Names,
                    Number = transaction.Number,
                    DocumentNumber = transaction.DocumentNumber,
                    Remarks = transaction.Remarks,
                    Subject = transaction.Subject,
                    PrintedDeliveryReport = transaction.PrintedDeliveryReport,
                    DeliveryReportNumber = transaction.DeliveryReportNumber,
                    MainDocument = transaction.MainDocument,
                    OldWordDocumnt = transaction.OldWordDocumnt,
                    RemindDate = transaction.RemindDate,
                    RemindDateH = transaction.RemindDateH,
                    OutboundDraftEditorType = transaction.OutboundDraftEditorType,
                    IsSigned = transaction.IsSigned,
                    OutboundDraftId = transaction.OutboundDraftId,
                    DeliveryMethodId = transaction.DeliveryMethodId,
                    InboundDateH = transaction.InboundDateH,
                    IsDraft = transaction.IsDraft,
                    ExternalPartyManagerId = transaction.ExternalPartyManagerId,
                    LetterTypeId = transaction.LetterTypeId,
                    RejectionReason = transaction.RejectionReason,
                    Year = transaction.Year,
                    YearH = transaction.YearH,
                    TransactionTypeId = transaction.TransactionTypeId,
                    SuggestedTopicId = transaction.SuggestedTopicId,
                    UserId = transaction.UserId,
                    SignedByUserId = transaction.SignedByUserId,
                    PostCode = transaction.PostCode,
                    POBox = transaction.POBox,
                    PriorityId = transaction.PriorityId,
                    StatusId = transaction.StatusId,
                    MainDocumentId = transaction.MainDocumentId,
                    ConfidentialityId = transaction.ConfidentialityId,
                    IsForIndividual = transaction.IsForIndividual,
                    ReporterId = transaction.ReporterId,
                    DeliveryNumber = transaction.DeliveryNumber,
                    SubjectClassificationsId = transaction.SubjectClassificationsId,
                    RecordNumber = transaction.RecordNumber,
                    // SideContactExternalEntity = transaction.SideContactExternalEntity,
                    SideContactExternalEntityID = transaction.SideContactExternalEntityID,
                    NumberContact = transaction.NumberContact,
                    ContactDateH = transaction.ContactDateH,
                    IsPresentationDraft = transaction.IsPresentationDraft,
                    PresentationDraftNumber = transaction.PresentationDraftNumber,
                    OutBoundDraftNumber = transaction.OutBoundDraftNumber,
                    IsElcOutBound = transaction.IsElcOutBound,
                    NeedAcknowled = transaction.NeedAcknowled,
                    OldWordDocumntId = transaction.OldWordDocumntId,
                    IsAppointment = transaction.IsAppointment,
                    ProcessPeriodTransaction = transaction.ProcessPeriodTransaction,
                    IsDecisionDraft = transaction.IsDecisionDraft,
                    Summary = transaction.Summary,
                    Encrypted = transaction.Encrypted,
                    InboundIntendedPerson = transaction.InboundIntendedPerson,
                    ComplaintNumber = transaction.ComplaintNumber,
                    SavedTransactionAssignments = transaction.SavedTransactionAssignments != null ? transaction.SavedTransactionAssignments.Select(x => new SavedTransactionAssignment
                    {

                        AssignmentList = x.AssignmentList,
                        TransactionId = x.TransactionId,

                    }).ToList() : null,
                    DeliveryMethod = (transaction.DeliveryMethod != null) ? new Lookup
                    {
                        Id = transaction.DeliveryMethodId,
                        Text = transaction.DeliveryMethod.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    SuggestedTopic = (transaction.SuggestedTopic != null) ? new SuggestedTopic
                    {
                        Id = transaction.SuggestedTopic.Id,
                        Text = transaction.SuggestedTopic.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()

                    } : null,

                    SignedByUser = (transaction.SignedByUser != null) ? new UserProfile
                    {
                        Id = transaction.SignedByUser.Id,
                        LocalName = transaction.SignedByUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()

                    } : null,

                    User = (transaction.User != null) ? new UserProfile
                    {
                        Id = transaction.User.Id,
                        LocalName = transaction.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    OrgUnit = (transaction.OrgUnit != null) ? new OrgUnit
                    {
                        Id = transaction.OrgUnit.Id,
                        LocalName = transaction.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Assignments = transaction.Assignments.Select(a => new TransactionAssignment
                    {
                        Description = a.Description,
                        Date = a.Date,
                        DateH = a.DateH,
                        Id = a.Id,
                        TransactionPathId = a.TransactionPathId,
                        CurrentPathStep = a.CurrentPathStep,
                        Tray = (a.Tray != null) ? new Tray
                        {
                            Id = a.Tray.Id,
                            LocalName = a.Tray.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        ToUser = (a.ToUser != null) ? new UserProfile
                        {
                            Id = a.ToUser.Id,
                            LocalName = a.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        Transaction = (a.Transaction != null) ? new Transaction
                        {
                            Id = a.Transaction.Id
                        } : null,

                        Action = (a.Action != null) ? new Action
                        {
                            Id = a.Action.Id,
                            LocalName = a.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                            Type = a.Action.Type
                        } : null,

                        FromEntity = (a.FromEntity != null) ? new OrgUnit
                        {
                            Id = a.FromEntity.Id,
                            LocalName = a.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        ToEntity = (a.ToEntity != null) ? new OrgUnit
                        {
                            Id = a.ToEntity.Id,
                            LocalName = a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        FromUser = (a.FromUser != null) ? new UserProfile
                        {
                            Id = a.FromUser.Id,
                            LocalName = a.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null
                    }
                    ).ToList(),

                    Copies = transaction.Copies.Where(c => !c.IsBcc || (c.IsBcc && c.FromUserId == userid || c.UserId == userid)).Select(c => new TransactionCopy
                    {
                        Id = c.Id,
                        Date = c.Date,
                        DateH = c.DateH,
                        ActionId = c.ActionId,
                        TransactionId = c.TransactionId,
                        UserId = c.UserId,
                        Status = c.Status,
                        SentDate = c.SentDate,
                        IsOpr = c.IsOpr,
                        IsBcc = c.IsBcc,
                        Action = (c.Action != null) ? new Action
                        {
                            Id = c.Action.Id,
                            LocalName = c.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                            Type = c.Action.Type
                        } : null,

                        User = (c.User != null) ? new UserProfile
                        {
                            Id = c.User.Id,
                            LocalName = c.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        Entity = (c.Entity != null) ? new OrgUnit
                        {
                            Id = c.Entity.Id,
                            LocalName = c.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        FromEntityId = c.FromEntityId,
                        FromUserId = c.FromUserId,
                        FromEntity = (c.FromEntity != null) ? new OrgUnit
                        {
                            Id = c.FromEntity.Id,
                            LocalName = c.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        FromUser = (c.FromUser != null) ? new UserProfile
                        {
                            Id = c.FromUser.Id,
                            LocalName = c.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null
                    }
                    ).ToList(),

                    ExternalCopies = transaction.ExternalCopies.Select(c => new TransactionExternalCopy
                    {
                        Id = c.Id,
                        Date = c.Date,
                        DateH = c.DateH,
                        ActionId = c.ActionId,
                        TransactionId = c.TransactionId,
                        UserId = c.UserId,
                        Viewed = c.Viewed,
                        Action = (c.Action != null) ? new Action
                        {
                            Id = c.Action.Id,
                            LocalName = c.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        User = (c.User != null) ? new ExternalPartyManager
                        {
                            Id = c.User.Id,
                            LocalName = c.User.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        Entity = (c.Entity != null) ? new ExternalParty
                        {
                            Id = c.Entity.Id,
                            LocalName = c.Entity.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                            YasserRegistered = c.Entity.YasserRegistered
                        } : null,
                        EntityId = c.EntityId,

                        Status = c.Status,
                        SendEmail = c.SendEmail,
                        ExternalPartyAttachment = c.ExternalPartyAttachment.ToList().Select(o => new ExternalPartyAttachment
                        {
                            Id = o.Id,
                            PartyId = o.PartyId,
                            Name = o.Name,
                            DocumentInfo = new DocumentInfo
                            {
                                Document = new Document
                                {
                                    Id = o.DocumentInfo.Document.Id,
                                    Content = o.DocumentInfo.Document.Content
                                },

                                Id = o.DocumentInfo.Id,
                                MimeType = o.DocumentInfo.MimeType,
                                Name = o.DocumentInfo.Name,
                                Size = o.DocumentInfo.Size,
                                IsDeleted = o.DocumentInfo.IsDeleted,
                                ECMId = o.DocumentInfo.ECMId,
                                FromEntity = o.DocumentInfo.FromEntity,
                                FromUser = o.DocumentInfo.FromUser,
                                FromEntityId = o.DocumentInfo.FromEntityId,
                                FromUserId = o.DocumentInfo.FromUserId
                            },

                        }).ToList(),
                        FromEntityId = c.FromEntityId,
                        FromUserId = c.FromUserId,
                        FromEntity = (c.FromEntity != null) ? new OrgUnit
                        {
                            Id = c.FromEntity.Id,
                            LocalName = c.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        FromUser = (c.FromUser != null) ? new UserProfile
                        {
                            Id = c.FromUser.Id,
                            LocalName = c.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null
                    }
                    ).ToList(),
                    Links = transaction.Links.Select(tl => new TransactionLink
                    {
                        Id = tl.Id,
                        TransactionId = tl.TransactionId,


                        Type = (tl.Type != null) ? new Link
                        {
                            Id = tl.Type.Id,
                            Text = tl.Type.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        ToTransaction = (tl.ToTransaction != null) ? new Transaction
                        {
                            Id = tl.ToTransaction.Id,
                            Number = tl.ToTransaction.Number,
                            Subject = tl.ToTransaction.Subject,
                            DateH = tl.ToTransaction.DateH,
                            Date = tl.ToTransaction.Date,
                            TransactionCategoryId = tl.ToTransaction.TransactionCategoryId,
                            TransactionType = (transaction.TransactionType != null) ? new TransactionType
                            {
                                Id = transaction.TransactionType.Id,
                                Text = transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            } : null,
                            YearH = tl.ToTransaction.YearH,
                            Year = tl.ToTransaction.Year,
                            TransactionCategory = new Lookup()
                            {
                                Id = tl.ToTransaction.TransactionCategoryId,
                                Text = tl.ToTransaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            },
                            ConfidentialityId = tl.ToTransaction.ConfidentialityId,
                            Assignments = tl.ToTransaction.Assignments.Select(a => new TransactionAssignment
                            {
                                Description = a.Description,
                                Date = a.Date,
                                DateH = a.DateH,
                                Id = a.Id,
                                TransactionPathId = a.TransactionPathId,
                                CurrentPathStep = a.CurrentPathStep,
                                Tray = (a.Tray != null) ? new Tray
                                {
                                    Id = a.Tray.Id,
                                    LocalName = a.Tray.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                } : null,

                                ToUser = (a.ToUser != null) ? new UserProfile
                                {
                                    Id = a.ToUser.Id,
                                    LocalName = a.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                } : null,

                                Transaction = (a.Transaction != null) ? new Transaction
                                {
                                    Id = a.Transaction.Id
                                } : null,

                                Action = (a.Action != null) ? new Action
                                {
                                    Id = a.Action.Id,
                                    LocalName = a.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                                    Type = a.Action.Type
                                } : null,

                                FromEntity = (a.FromEntity != null) ? new OrgUnit
                                {
                                    Id = a.FromEntity.Id,
                                    LocalName = a.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                } : null,

                                ToEntity = (a.ToEntity != null) ? new OrgUnit
                                {
                                    Id = a.ToEntity.Id,
                                    LocalName = a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                } : null,
                                FromUser = (a.FromUser != null) ? new UserProfile
                                {
                                    Id = a.FromUser.Id,
                                    LocalName = a.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                } : null,

                            }).ToList()

                        } : null,


                    }
                    ).ToList(),

                    SubjectClassifications = transaction.SubjectClassifications.Select(ts => new TransactionSubjectClassification
                    {
                        Id = ts.Id,
                        TransactionId = ts.TransactionId,

                        SubjectClassification = (ts.SubjectClassification != null) ? new SubjectClassification
                        {
                            Id = ts.SubjectClassification.Id,
                            Text = ts.SubjectClassification.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                    }
                   ).ToList(),

                    Attachments = transaction.Attachments.Select(a => new Attachment
                    {
                        DocumentInfo = (a.DocumentInfo != null) ? new DocumentInfo
                        {
                            Document = (a.DocumentInfo.Document != null) ? new Document
                            {
                                Id = a.DocumentInfo.Document.Id
                            } : null,

                            Id = a.DocumentInfo.Id,
                            MimeType = a.DocumentInfo.MimeType,
                            Name = a.DocumentInfo.Name,
                            Size = a.DocumentInfo.Size,
                            FromEntityId = a.DocumentInfo.FromEntityId,
                            FromUserId = a.DocumentInfo.FromUserId,
                            FromEntity = (a.DocumentInfo.FromEntity != null) ? new OrgUnit
                            {
                                Id = a.DocumentInfo.FromEntity.Id,
                                LocalName = a.DocumentInfo.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            } : null,
                            FromUser = (a.DocumentInfo.FromUser != null) ? new UserProfile
                            {
                                Id = a.DocumentInfo.FromUser.Id,
                                LocalName = a.DocumentInfo.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            } : null

                        } : null,

                        Type = (a.Type != null) ? new AttachmentType
                        {
                            Archivable = a.Type.Archivable,
                            Id = a.Type.Id,
                            Text = a.Type.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        Description = a.Description,
                        Count = a.Count,
                        Id = a.Id,
                        TypeId = a.TypeId,
                        AttachmentSource = a.AttachmentSource,
                        CreatedBy = a.CreatedBy,
                    }).ToList(),

                    Entity = (transaction.Entity != null) ? new OrgUnit
                    {
                        Id = transaction.Entity.Id,
                        LocalName = transaction.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    ToUser = (transaction.ToUser != null) ? new UserProfile
                    {
                        Id = transaction.ToUser.Id,
                        LocalName = transaction.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Priority = (transaction.Priority != null) ? new Priority
                    {
                        Id = transaction.Priority.Id,
                        Text = transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Confidentiality = (transaction.Confidentiality != null) ? new Permission
                    {
                        Id = transaction.Confidentiality.Id,
                        LocalName = transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                        Code = transaction.Confidentiality.Code
                    } : null,

                    TransactionType = (transaction.TransactionType != null) ? new TransactionType
                    {
                        Id = transaction.TransactionType.Id,
                        Text = transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    LetterType = (transaction.LetterType != null) ? new LetterType
                    {
                        Id = transaction.LetterType.Id,
                        Text = transaction.LetterType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    City = (transaction.City != null) ? new City
                    {
                        Id = transaction.City.Id,
                        Text = transaction.City.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    ExternalParty = (transaction.ExternalParty != null) ? new ExternalParty
                    {
                        Id = transaction.ExternalParty.Id,
                        Number = transaction.ExternalParty.Number,
                        LocalName = transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                        LocalAddress = transaction.ExternalParty.LocalAddress
                    } : null,


                    ExternalPartyManager = (transaction.ExternalPartyManager != null) ? new ExternalPartyManager
                    {
                        Id = transaction.ExternalPartyManager.Id,
                        LocalName = transaction.ExternalPartyManager.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    FollowUp = transaction.FollowUp.Where(f => !f.IsDeleted).Select(f => new TransactionFollowUp
                    {
                        Id = f.Id,
                        TransactionId = f.TransactionId,
                        DateTo = f.DateTo,
                        DateToH = f.DateToH,
                        IsDeleted = f.IsDeleted,
                        CreatedOn = f.CreatedOn,
                        CreatedBy = f.CreatedBy,
                        ModefiedOn = f.ModefiedOn,
                        ModefiedBy = f.ModefiedBy,
                        CreatingUserId = f.CreatingUserId,
                        CreatingUser = (f.CreatingUser != null) ? new UserProfile
                        {
                            Id = f.CreatingUserId,
                            LocalName = f.CreatingUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        CreatingEntityId = f.CreatingEntityId,
                        CreatingEntity = (f.CreatingEntity != null) ? new OrgUnit
                        {
                            Id = f.CreatingEntity.Id,
                            LocalName = f.CreatingEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        FollowUpEntityId = f.FollowUpEntityId,
                        FollowUpEntity = (f.FollowUpEntity != null) ? new OrgUnit
                        {
                            Id = f.FollowUpEntity.Id,
                            LocalName = f.FollowUpEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        FollowUpUserId = f.FollowUpUserId,
                        FollowUpUser = (f.FollowUpUser != null) ? new UserProfile
                        {
                            Id = f.FollowUpUserId.Value,
                            LocalName = f.FollowUpUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        CreationDate = f.CreationDate,
                        FollowUpExpireDate = f.FollowUpExpireDate,
                        Notes = f.Notes,
                        Active = f.Active,
                        FollowUpProccessId = f.FollowUpProccessId,
                        ProccessPeriod = f.ProccessPeriod,
                        ProccessPeriodDate = f.ProccessPeriodDate,
                        FollowUpProccessNote = f.FollowUpProccessNote,
                        FollowUpCompletionDate = f.FollowUpCompletionDate,
                        FollowUpCompletionDateHj = f.FollowUpCompletionDateHj,
                        FollowUpExpireDateHj = f.FollowUpExpireDateHj,
                        FollowUpReceiveDate = f.FollowUpReceiveDate,
                        FollowUpReason = f.FollowUpReason,
                        FollowUpTypeId = f.FollowUpTypeId,
                        FollowUpStatusId = f.FollowUpStatusId,
                        FollowUpMethodId = f.FollowUpMethodId,
                        FollowUpPriortyId = f.FollowUpPriortyId,
                        FollowUpSourceId = f.FollowUpSourceId,
                        FollowUpProgressId = f.FollowUpProgressId,
                        IsCopy = f.IsCopy,
                        IsReminder = f.IsReminder,
                        IsEscalated = f.IsEscalated,
                        IsImportant = f.IsImportant,
                        HasChild = f.HasChild,
                        ParentId = f.ParentId,


                    }).ToList(),
                    LetterNumber = transaction.LetterNumber
                };

                if (!isNotification && result.MainDocument != null && result.MainDocument.Document != null)
                    result.MainDocument.Document.Content = null;

                if (result.OldWordDocumnt != null && result.OldWordDocumnt.Document != null)
                    result.OldWordDocumnt.Document.Content = null;

                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Transaction GetTransactionLight(Expression<Func<Transaction, bool>> @where, string cultureName)
        {
            try
            {
                Transaction transaction = _oMCSDbContext.Transactions.Include(a => a.Assignments).Where(@where).FirstOrDefault();

                if (transaction == null || transaction.IsDeleted)
                {
                    return null;
                }

                Transaction result = new Transaction
                {
                    Id = transaction.Id,
                    TransactionCategory = transaction.TransactionCategory,
                    TransactionCategoryId = transaction.TransactionCategoryId,
                    TransactionTypeId = transaction.TransactionTypeId,
                    UserId = transaction.UserId,
                    StatusId = transaction.StatusId,
                    ReporterId = transaction.ReporterId,
                    DeliveryNumber = transaction.DeliveryNumber,
                    DeliveryMethodId = transaction.DeliveryMethodId,
                    Assignments = transaction.Assignments.Select(a => new TransactionAssignment
                    {
                        Id = a.Id,
                        Viewed = a.Viewed,
                        ToUser = (a.ToUser != null) ? new UserProfile
                        {
                            Id = a.ToUser.Id,
                            LocalName = a.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        ToEntity = (a.ToEntity != null) ? new OrgUnit
                        {
                            Id = a.ToEntity.Id,
                            LocalName = a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        DeliveryMethodId = a.DeliveryMethodId,

                    }).OrderBy(a => a.Id).ToList()
                };
                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Transaction GetTransaction(Expression<Func<Transaction, bool>> @where)
        {
            try
            {
                Transaction transaction = _oMCSDbContext.Transactions.Where(@where).FirstOrDefault();

                if (transaction != null && transaction.IsDeleted)
                {
                    return null;
                }

                return transaction;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Transaction GetTransaction(Expression<Func<Transaction, bool>> @where, string cultureName)
        {
            try
            {
                Transaction transaction = _oMCSDbContext.Transactions.Where(@where).FirstOrDefault();

                if (transaction == null || transaction.IsDeleted)
                {
                    return null;
                }

                Transaction result = new Transaction
                {
                    Id = transaction.Id,
                    CreatedBy = transaction.CreatedBy,
                    Date = transaction.Date,
                    DateH = transaction.DateH,
                    Status = transaction.Status,
                    TransactionCategory = transaction.TransactionCategory,
                    TransactionCategoryId = transaction.TransactionCategoryId,
                    OrgUnitId = transaction.OrgUnitId,
                    ToUserId = transaction.ToUserId,
                    EntityId = transaction.EntityId,
                    ExternalPartyId = transaction.ExternalPartyId,
                    Names = transaction.Names,
                    Number = transaction.Number,
                    DocumentNumber = transaction.DocumentNumber,
                    Remarks = transaction.Remarks,
                    Subject = transaction.Subject,
                    PrintedDeliveryReport = transaction.PrintedDeliveryReport,
                    DeliveryReportNumber = transaction.DeliveryReportNumber,
                    MainDocument = transaction.MainDocument,
                    OldWordDocumnt = transaction.OldWordDocumnt,
                    RemindDate = transaction.RemindDate,
                    RemindDateH = transaction.RemindDateH,
                    OutboundDraftEditorType = transaction.OutboundDraftEditorType,
                    IsSigned = transaction.IsSigned,
                    OutboundDraftId = transaction.OutboundDraftId,
                    DeliveryMethodId = transaction.DeliveryMethodId,
                    InboundDateH = transaction.InboundDateH,
                    IsDraft = transaction.IsDraft,
                    ExternalPartyManagerId = transaction.ExternalPartyManagerId,
                    LetterTypeId = transaction.LetterTypeId,
                    RejectionReason = transaction.RejectionReason,
                    Year = transaction.Year,
                    YearH = transaction.YearH,
                    TransactionTypeId = transaction.TransactionTypeId,
                    SuggestedTopicId = transaction.SuggestedTopicId,
                    UserId = transaction.UserId,
                    SignedByUserId = transaction.SignedByUserId,
                    PostCode = transaction.PostCode,
                    POBox = transaction.POBox,
                    PriorityId = transaction.PriorityId,
                    StatusId = transaction.StatusId,
                    MainDocumentId = transaction.MainDocumentId,
                    ConfidentialityId = transaction.ConfidentialityId,
                    IsForIndividual = transaction.IsForIndividual,
                    ReporterId = transaction.ReporterId,
                    DeliveryNumber = transaction.DeliveryNumber,
                    SubjectClassificationsId = transaction.SubjectClassificationsId,
                    RecordNumber = transaction.RecordNumber,
                    // SideContactExternalEntity = transaction.SideContactExternalEntity,
                    SideContactExternalEntityID = transaction.SideContactExternalEntityID,
                    NumberContact = transaction.NumberContact,
                    ContactDateH = transaction.ContactDateH,
                    IsPresentationDraft = transaction.IsPresentationDraft,
                    PresentationDraftNumber = transaction.PresentationDraftNumber,
                    OutBoundDraftNumber = transaction.OutBoundDraftNumber,
                    IsElcOutBound = transaction.IsElcOutBound,
                    NeedAcknowled = transaction.NeedAcknowled,
                    OldWordDocumntId = transaction.OldWordDocumntId,
                    IsAppointment = transaction.IsAppointment,
                    ProcessPeriodTransaction = transaction.ProcessPeriodTransaction,
                    IsDecisionDraft = transaction.IsDecisionDraft,
                    Summary = transaction.Summary,
                    Encrypted = transaction.Encrypted,
                    InboundIntendedPerson = transaction.InboundIntendedPerson,
                    ComplaintNumber = transaction.ComplaintNumber,
                    SavedTransactionAssignments = transaction.SavedTransactionAssignments != null ? transaction.SavedTransactionAssignments.Select(x => new SavedTransactionAssignment
                    {

                        AssignmentList = x.AssignmentList,
                        TransactionId = x.TransactionId,

                    }).ToList() : null,
                    DeliveryMethod = (transaction.DeliveryMethod != null) ? new Lookup
                    {
                        Id = transaction.DeliveryMethodId,
                        Text = transaction.DeliveryMethod.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    SuggestedTopic = (transaction.SuggestedTopic != null) ? new SuggestedTopic
                    {
                        Id = transaction.SuggestedTopic.Id,
                        Text = transaction.SuggestedTopic.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()

                    } : null,

                    SignedByUser = (transaction.SignedByUser != null) ? new UserProfile
                    {
                        Id = transaction.SignedByUser.Id,
                        LocalName = transaction.SignedByUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()

                    } : null,

                    User = (transaction.User != null) ? new UserProfile
                    {
                        Id = transaction.User.Id,
                        LocalName = transaction.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    OrgUnit = (transaction.OrgUnit != null) ? new OrgUnit
                    {
                        Id = transaction.OrgUnit.Id,
                        LocalName = transaction.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Assignments = transaction.Assignments.Select(a => new TransactionAssignment
                    {
                        Description = a.Description,
                        Date = a.Date,
                        DateH = a.DateH,
                        Id = a.Id,
                        TransactionPathId = a.TransactionPathId,
                        CurrentPathStep = a.CurrentPathStep,
                        Tray = (a.Tray != null) ? new Tray
                        {
                            Id = a.Tray.Id,
                            LocalName = a.Tray.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        ToUser = (a.ToUser != null) ? new UserProfile
                        {
                            Id = a.ToUser.Id,
                            LocalName = a.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        Transaction = (a.Transaction != null) ? new Transaction
                        {
                            Id = a.Transaction.Id
                        } : null,

                        Action = (a.Action != null) ? new Action
                        {
                            Id = a.Action.Id,
                            LocalName = a.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                            Type = a.Action.Type
                        } : null,

                        FromEntity = (a.FromEntity != null) ? new OrgUnit
                        {
                            Id = a.FromEntity.Id,
                            LocalName = a.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        ToEntity = (a.ToEntity != null) ? new OrgUnit
                        {
                            Id = a.ToEntity.Id,
                            LocalName = a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        FromUser = (a.FromUser != null) ? new UserProfile
                        {
                            Id = a.FromUser.Id,
                            LocalName = a.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null
                    }
                    ).ToList(),

                    Copies = transaction.Copies.Where(c => !c.IsBcc || (c.IsBcc)).Select(c => new TransactionCopy
                    {
                        Id = c.Id,
                        Date = c.Date,
                        DateH = c.DateH,
                        ActionId = c.ActionId,
                        TransactionId = c.TransactionId,
                        UserId = c.UserId,
                        Status = c.Status,
                        SentDate = c.SentDate,
                        IsOpr = c.IsOpr,
                        IsBcc = c.IsBcc,
                        Action = (c.Action != null) ? new Action
                        {
                            Id = c.Action.Id,
                            LocalName = c.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                            Type = c.Action.Type
                        } : null,

                        User = (c.User != null) ? new UserProfile
                        {
                            Id = c.User.Id,
                            LocalName = c.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        Entity = (c.Entity != null) ? new OrgUnit
                        {
                            Id = c.Entity.Id,
                            LocalName = c.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        FromEntityId = c.FromEntityId,
                        FromUserId = c.FromUserId,
                        FromEntity = (c.FromEntity != null) ? new OrgUnit
                        {
                            Id = c.FromEntity.Id,
                            LocalName = c.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        FromUser = (c.FromUser != null) ? new UserProfile
                        {
                            Id = c.FromUser.Id,
                            LocalName = c.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null
                    }
                    ).ToList(),

                    ExternalCopies = transaction.ExternalCopies.Select(c => new TransactionExternalCopy
                    {
                        Id = c.Id,
                        Date = c.Date,
                        DateH = c.DateH,
                        ActionId = c.ActionId,
                        TransactionId = c.TransactionId,
                        UserId = c.UserId,
                        Viewed = c.Viewed,
                        Action = (c.Action != null) ? new Action
                        {
                            Id = c.Action.Id,
                            LocalName = c.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        User = (c.User != null) ? new ExternalPartyManager
                        {
                            Id = c.User.Id,
                            LocalName = c.User.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        Entity = (c.Entity != null) ? new ExternalParty
                        {
                            Id = c.Entity.Id,
                            LocalName = c.Entity.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                            YasserRegistered = c.Entity.YasserRegistered
                        } : null,
                        EntityId = c.EntityId,

                        Status = c.Status,
                        SendEmail = c.SendEmail,
                        ExternalPartyAttachment = c.ExternalPartyAttachment.ToList().Select(o => new ExternalPartyAttachment
                        {
                            Id = o.Id,
                            PartyId = o.PartyId,
                            Name = o.Name,
                            DocumentInfo = new DocumentInfo
                            {
                                Document = new Document
                                {
                                    Id = o.DocumentInfo.Document.Id,
                                    Content = o.DocumentInfo.Document.Content
                                },

                                Id = o.DocumentInfo.Id,
                                MimeType = o.DocumentInfo.MimeType,
                                Name = o.DocumentInfo.Name,
                                Size = o.DocumentInfo.Size,
                                IsDeleted = o.DocumentInfo.IsDeleted,
                                ECMId = o.DocumentInfo.ECMId,
                                FromEntity = o.DocumentInfo.FromEntity,
                                FromUser = o.DocumentInfo.FromUser,
                                FromEntityId = o.DocumentInfo.FromEntityId,
                                FromUserId = o.DocumentInfo.FromUserId
                            },

                        }).ToList(),
                        FromEntityId = c.FromEntityId,
                        FromUserId = c.FromUserId,
                        FromEntity = (c.FromEntity != null) ? new OrgUnit
                        {
                            Id = c.FromEntity.Id,
                            LocalName = c.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        FromUser = (c.FromUser != null) ? new UserProfile
                        {
                            Id = c.FromUser.Id,
                            LocalName = c.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null
                    }
                    ).ToList(),
                    Links = transaction.Links.Select(tl => new TransactionLink
                    {
                        Id = tl.Id,
                        TransactionId = tl.TransactionId,


                        Type = (tl.Type != null) ? new Link
                        {
                            Id = tl.Type.Id,
                            Text = tl.Type.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        ToTransaction = (tl.ToTransaction != null) ? new Transaction
                        {
                            Id = tl.ToTransaction.Id,
                            Number = tl.ToTransaction.Number,
                            Subject = tl.ToTransaction.Subject,
                            DateH = tl.ToTransaction.DateH,
                            Date = tl.ToTransaction.Date,
                            TransactionCategoryId = tl.ToTransaction.TransactionCategoryId,
                            TransactionType = (transaction.TransactionType != null) ? new TransactionType
                            {
                                Id = transaction.TransactionType.Id,
                                Text = transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            } : null,
                            YearH = tl.ToTransaction.YearH,
                            Year = tl.ToTransaction.Year,
                            TransactionCategory = new Lookup()
                            {
                                Id = tl.ToTransaction.TransactionCategoryId,
                                Text = tl.ToTransaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            },
                            ConfidentialityId = tl.ToTransaction.ConfidentialityId,
                            Assignments = tl.ToTransaction.Assignments.Select(a => new TransactionAssignment
                            {
                                Description = a.Description,
                                Date = a.Date,
                                DateH = a.DateH,
                                Id = a.Id,
                                TransactionPathId = a.TransactionPathId,
                                CurrentPathStep = a.CurrentPathStep,
                                Tray = (a.Tray != null) ? new Tray
                                {
                                    Id = a.Tray.Id,
                                    LocalName = a.Tray.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                } : null,

                                ToUser = (a.ToUser != null) ? new UserProfile
                                {
                                    Id = a.ToUser.Id,
                                    LocalName = a.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                } : null,

                                Transaction = (a.Transaction != null) ? new Transaction
                                {
                                    Id = a.Transaction.Id
                                } : null,

                                Action = (a.Action != null) ? new Action
                                {
                                    Id = a.Action.Id,
                                    LocalName = a.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                                    Type = a.Action.Type
                                } : null,

                                FromEntity = (a.FromEntity != null) ? new OrgUnit
                                {
                                    Id = a.FromEntity.Id,
                                    LocalName = a.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                } : null,

                                ToEntity = (a.ToEntity != null) ? new OrgUnit
                                {
                                    Id = a.ToEntity.Id,
                                    LocalName = a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                } : null,
                                FromUser = (a.FromUser != null) ? new UserProfile
                                {
                                    Id = a.FromUser.Id,
                                    LocalName = a.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                } : null,

                            }).ToList()

                        } : null,


                    }
                    ).ToList(),

                    SubjectClassifications = transaction.SubjectClassifications.Select(ts => new TransactionSubjectClassification
                    {
                        Id = ts.Id,
                        TransactionId = ts.TransactionId,

                        SubjectClassification = (ts.SubjectClassification != null) ? new SubjectClassification
                        {
                            Id = ts.SubjectClassification.Id,
                            Text = ts.SubjectClassification.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                    }
                   ).ToList(),

                    Attachments = transaction.Attachments.Select(a => new Attachment
                    {
                        DocumentInfo = (a.DocumentInfo != null) ? new DocumentInfo
                        {
                            Document = (a.DocumentInfo.Document != null) ? new Document
                            {
                                Id = a.DocumentInfo.Document.Id
                            } : null,

                            Id = a.DocumentInfo.Id,
                            MimeType = a.DocumentInfo.MimeType,
                            Name = a.DocumentInfo.Name,
                            Size = a.DocumentInfo.Size,
                            FromEntityId = a.DocumentInfo.FromEntityId,
                            FromUserId = a.DocumentInfo.FromUserId,
                            FromEntity = (a.DocumentInfo.FromEntity != null) ? new OrgUnit
                            {
                                Id = a.DocumentInfo.FromEntity.Id,
                                LocalName = a.DocumentInfo.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            } : null,
                            FromUser = (a.DocumentInfo.FromUser != null) ? new UserProfile
                            {
                                Id = a.DocumentInfo.FromUser.Id,
                                LocalName = a.DocumentInfo.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            } : null

                        } : null,

                        Type = (a.Type != null) ? new AttachmentType
                        {
                            Archivable = a.Type.Archivable,
                            Id = a.Type.Id,
                            Text = a.Type.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        Description = a.Description,
                        Count = a.Count,
                        Id = a.Id,
                        TypeId = a.TypeId,
                        AttachmentSource = a.AttachmentSource,
                        CreatedBy = a.CreatedBy,
                    }).ToList(),

                    Entity = (transaction.Entity != null) ? new OrgUnit
                    {
                        Id = transaction.Entity.Id,
                        LocalName = transaction.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    ToUser = (transaction.ToUser != null) ? new UserProfile
                    {
                        Id = transaction.ToUser.Id,
                        LocalName = transaction.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Priority = (transaction.Priority != null) ? new Priority
                    {
                        Id = transaction.Priority.Id,
                        Text = transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Confidentiality = (transaction.Confidentiality != null) ? new Permission
                    {
                        Id = transaction.Confidentiality.Id,
                        LocalName = transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                        Code = transaction.Confidentiality.Code
                    } : null,

                    TransactionType = (transaction.TransactionType != null) ? new TransactionType
                    {
                        Id = transaction.TransactionType.Id,
                        Text = transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    LetterType = (transaction.LetterType != null) ? new LetterType
                    {
                        Id = transaction.LetterType.Id,
                        Text = transaction.LetterType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    City = (transaction.City != null) ? new City
                    {
                        Id = transaction.City.Id,
                        Text = transaction.City.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    ExternalParty = (transaction.ExternalParty != null) ? new ExternalParty
                    {
                        Id = transaction.ExternalParty.Id,
                        Number = transaction.ExternalParty.Number,
                        LocalName = transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                        LocalAddress = transaction.ExternalParty.LocalAddress
                    } : null,


                    ExternalPartyManager = (transaction.ExternalPartyManager != null) ? new ExternalPartyManager
                    {
                        Id = transaction.ExternalPartyManager.Id,
                        LocalName = transaction.ExternalPartyManager.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    FollowUp = transaction.FollowUp.Where(f => !f.IsDeleted).Select(f => new TransactionFollowUp
                    {
                        Id = f.Id,
                        TransactionId = f.TransactionId,
                        DateTo = f.DateTo,
                        DateToH = f.DateToH,
                        IsDeleted = f.IsDeleted,
                        CreatedOn = f.CreatedOn,
                        CreatedBy = f.CreatedBy,
                        ModefiedOn = f.ModefiedOn,
                        ModefiedBy = f.ModefiedBy,
                        CreatingUserId = f.CreatingUserId,
                        CreatingUser = (f.CreatingUser != null) ? new UserProfile
                        {
                            Id = f.CreatingUserId,
                            LocalName = f.CreatingUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        CreatingEntityId = f.CreatingEntityId,
                        CreatingEntity = (f.CreatingEntity != null) ? new OrgUnit
                        {
                            Id = f.CreatingEntity.Id,
                            LocalName = f.CreatingEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        FollowUpEntityId = f.FollowUpEntityId,
                        FollowUpEntity = (f.FollowUpEntity != null) ? new OrgUnit
                        {
                            Id = f.FollowUpEntity.Id,
                            LocalName = f.FollowUpEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        FollowUpUserId = f.FollowUpUserId,
                        FollowUpUser = (f.FollowUpUser != null) ? new UserProfile
                        {
                            Id = f.FollowUpUserId.Value,
                            LocalName = f.FollowUpUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        CreationDate = f.CreationDate,
                        FollowUpExpireDate = f.FollowUpExpireDate,
                        Notes = f.Notes,
                        Active = f.Active,
                        FollowUpProccessId = f.FollowUpProccessId,
                        ProccessPeriod = f.ProccessPeriod,
                        ProccessPeriodDate = f.ProccessPeriodDate,
                        FollowUpProccessNote = f.FollowUpProccessNote,
                        FollowUpCompletionDate = f.FollowUpCompletionDate,
                        FollowUpCompletionDateHj = f.FollowUpCompletionDateHj,
                        FollowUpExpireDateHj = f.FollowUpExpireDateHj,
                        FollowUpReceiveDate = f.FollowUpReceiveDate,
                        FollowUpReason = f.FollowUpReason,
                        FollowUpTypeId = f.FollowUpTypeId,
                        FollowUpStatusId = f.FollowUpStatusId,
                        FollowUpMethodId = f.FollowUpMethodId,
                        FollowUpPriortyId = f.FollowUpPriortyId,
                        FollowUpSourceId = f.FollowUpSourceId,
                        FollowUpProgressId = f.FollowUpProgressId,
                        IsCopy = f.IsCopy,
                        IsReminder = f.IsReminder,
                        IsEscalated = f.IsEscalated,
                        IsImportant = f.IsImportant,
                        HasChild = f.HasChild,
                        ParentId = f.ParentId,


                    }).ToList(),
                    LetterNumber = transaction.LetterNumber
                };


                if (result.OldWordDocumnt != null && result.OldWordDocumnt.Document != null)
                    result.OldWordDocumnt.Document.Content = null;

                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Transaction> GetTransactions(int orgUnitId, int year)
        {
            try
            {
                IList<Transaction> transactions = (from transaction in _oMCSDbContext.Transactions.ToList()
                                                   where transaction.OrgUnit.Id == orgUnitId &&
                                                   DateTimeUtility.GetHijriYear(transaction.Date) == year
                                                   select new
                                                   {
                                                       transaction.TransactionCategory,
                                                       transaction.User,
                                                       transaction.Date
                                                   }).ToList().Select(t => new Transaction
                                                   {
                                                       TransactionCategory = t.TransactionCategory,
                                                       User = t.User,
                                                       Date = t.Date
                                                   }).ToList();

                return transactions.Where(t => !t.IsDeleted).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Transaction> GetTransactions(Expression<Func<Transaction, bool>> @where)
        {
            try
            {
                IList<Transaction> transactions = (from transaction in _oMCSDbContext.Transactions.Where(@where)
                                                   select new
                                                   {
                                                       transaction.TransactionCategory,
                                                       transaction.User,
                                                       transaction.Date
                                                   }).ToList().Select(t => new Transaction
                                                   {
                                                       TransactionCategory = t.TransactionCategory,
                                                       User = t.User,
                                                       Date = t.Date
                                                   }).ToList();

                return transactions.Where(t => !t.IsDeleted).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<Transaction> GetLateTransactions()
        {
            try
            {
                IList<Transaction> transactions = (from transaction in _oMCSDbContext.Transactions
                                                   where (
                                                             (transaction.Assignments[0].TransactionAssignmentProcessPeriod == null && transaction.Assignments[0].ToUser.TransactionProcessingPeriod > 0
                                                           &&
                                                           transaction.Assignments[0].Date < DbFunctions.AddDays(DateTime.Now, -1 * transaction.Assignments[0].ToUser.TransactionProcessingPeriod))
                                                         || (transaction.Assignments[0].TransactionAssignmentProcessPeriod != null
                                                         && transaction.Assignments[0].TransactionAssignmentProcessPeriod < DateTime.Now
                                                          )
                                                          ||
                                                          transaction.RemindDate < DateTime.Now)
                                                   select new
                                                   {
                                                       transaction.Assignments,
                                                       transaction.Id,
                                                       transaction.Date,
                                                       transaction.Number
                                                   }).ToList().Select(t => new Transaction
                                                   {
                                                       Assignments = t.Assignments,
                                                       Id = t.Id,
                                                       Date = t.Date,
                                                       Number = t.Number
                                                   }).ToList();

                return transactions.Where(t => !t.IsDeleted).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Transaction GetPreviousTransaction(int userId, int orgUnitId, Common.TransactionCategory transactionCategory, string cultureName, bool IsForIndividual)
        {
            try
            {
                int TransactionCategoryId = (int)(TransactionCategory)transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, cultureName);
                Transaction transaction = _oMCSDbContext.Transactions.Where(t => t.UserId == userId & t.IsForIndividual == IsForIndividual & t.OrgUnitId == orgUnitId & t.TransactionCategoryId == TransactionCategoryId & !t.IsDeleted)
                                              .OrderByDescending(t => t.Id).FirstOrDefault();

                if (transaction == null)
                {
                    return null;
                }
                Transaction result = new Transaction
                {
                    Date = transaction.Date,
                    DateH = transaction.DateH,
                    Status = transaction.Status,
                    TransactionCategory = transaction.TransactionCategory,
                    DocumentNumber = transaction.DocumentNumber,
                    Remarks = transaction.Remarks,
                    Subject = transaction.Subject,
                    RemindDate = transaction.RemindDate,
                    RemindDateH = transaction.RemindDateH,
                    DeliveryMethodId = transaction.DeliveryMethodId,
                    POBox = transaction.POBox,
                    PostCode = transaction.PostCode,
                    EntityId = transaction.EntityId,
                    ToUserId = transaction.ToUserId,
                    IsForIndividual = transaction.IsForIndividual,
                    ReporterId = transaction.ReporterId,
                    ExternalPartyId = transaction.ExternalPartyId,
                    SideContactExternalEntityID = transaction.SideContactExternalEntityID,
                    SuggestedTopic = (transaction.SuggestedTopic != null) ? new SuggestedTopic
                    {
                        Id = transaction.SuggestedTopic.Id,
                        Text = transaction.SuggestedTopic.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    SubjectClassifications = transaction.SubjectClassifications.Select(ts => new TransactionSubjectClassification
                    {
                        Id = ts.Id,
                        TransactionId = ts.TransactionId,

                        SubjectClassification = (ts.SubjectClassification != null) ? new SubjectClassification
                        {
                            Id = ts.SubjectClassification.Id,
                            Text = ts.SubjectClassification.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                    }
                ).ToList(),
                    SignedByUser = (transaction.SignedByUser != null) ? new UserProfile
                    {
                        Id = transaction.SignedByUser.Id,
                        LocalName = transaction.SignedByUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Entity = (transaction.Entity != null) ? new OrgUnit
                    {
                        Id = transaction.Entity.Id,
                        LocalName = transaction.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    ToUser = (transaction.ToUser != null) ? new UserProfile
                    {
                        Id = transaction.ToUser.Id,
                        LocalName = transaction.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Priority = new Priority
                    {
                        Id = transaction.Priority.Id,
                        Text = transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },

                    Confidentiality = new Permission
                    {
                        Id = transaction.Confidentiality.Id,
                        LocalName = transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },


                    TransactionType = (transaction.TransactionType != null) ? new TransactionType
                    {
                        Id = transaction.TransactionType.Id,
                        Text = transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    LetterType = (transaction.LetterType != null) ? new LetterType
                    {
                        Id = transaction.LetterType.Id,
                        Text = transaction.LetterType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,


                };
                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Transaction GetPreviousTransactionByID(int transactionsId, int orgUnitId, Common.TransactionCategory transactionCategory, string cultureName, bool IsForIndividual)
        {
            try
            {
                int TransactionCategoryId = (int)(TransactionCategory)transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, cultureName);
                Transaction transaction = _oMCSDbContext.Transactions.Where(t => t.Id == transactionsId & t.IsForIndividual == IsForIndividual & t.OrgUnitId == orgUnitId & t.TransactionCategoryId == TransactionCategoryId & !t.IsDeleted)
                                              .OrderByDescending(t => t.Id).FirstOrDefault();

                if (transaction == null)
                {
                    return null;
                }
                Transaction result = new Transaction
                {
                    Date = transaction.Date,
                    DateH = transaction.DateH,
                    Status = transaction.Status,
                    TransactionCategory = transaction.TransactionCategory,
                    DocumentNumber = transaction.DocumentNumber,
                    Remarks = transaction.Remarks,
                    Subject = transaction.Subject,
                    RemindDate = transaction.RemindDate,
                    RemindDateH = transaction.RemindDateH,
                    DeliveryMethodId = transaction.DeliveryMethodId,
                    POBox = transaction.POBox,
                    PostCode = transaction.PostCode,
                    EntityId = transaction.EntityId,
                    ToUserId = transaction.ToUserId,
                    IsForIndividual = transaction.IsForIndividual,
                    ReporterId = transaction.ReporterId,
                    ExternalPartyId = transaction.ExternalPartyId,
                    SuggestedTopic = (transaction.SuggestedTopic != null) ? new SuggestedTopic
                    {
                        Id = transaction.SuggestedTopic.Id,
                        Text = transaction.SuggestedTopic.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    SubjectClassifications = transaction.SubjectClassifications.Select(ts => new TransactionSubjectClassification
                    {
                        Id = ts.Id,
                        TransactionId = ts.TransactionId,

                        SubjectClassification = (ts.SubjectClassification != null) ? new SubjectClassification
                        {
                            Id = ts.SubjectClassification.Id,
                            Text = ts.SubjectClassification.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                    }
                ).ToList(),
                    SignedByUser = (transaction.SignedByUser != null) ? new UserProfile
                    {
                        Id = transaction.SignedByUser.Id,
                        LocalName = transaction.SignedByUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Entity = (transaction.Entity != null) ? new OrgUnit
                    {
                        Id = transaction.Entity.Id,
                        LocalName = transaction.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    ToUser = (transaction.ToUser != null) ? new UserProfile
                    {
                        Id = transaction.ToUser.Id,
                        LocalName = transaction.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Priority = new Priority
                    {
                        Id = transaction.Priority.Id,
                        Text = transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },

                    Confidentiality = new Permission
                    {
                        Id = transaction.Confidentiality.Id,
                        LocalName = transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    },


                    TransactionType = (transaction.TransactionType != null) ? new TransactionType
                    {
                        Id = transaction.TransactionType.Id,
                        Text = transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    LetterType = (transaction.LetterType != null) ? new LetterType
                    {
                        Id = transaction.LetterType.Id,
                        Text = transaction.LetterType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                };
                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Transaction GetTransactionBasicInfo(int transactionId, string cultureName)
        {
            try
            {
                Transaction transaction =
                    _oMCSDbContext.Transactions.Where(t => t.Id == transactionId & !t.IsDeleted).FirstOrDefault();

                if (transaction == null)
                {
                    return null;
                }

                Transaction result = new Transaction
                {
                    Date = transaction.Date,
                    DateH = transaction.DateH,
                    OutboundDraftId = transaction.OutboundDraftId,
                    Year = transaction.Year,
                    YearH = transaction.YearH,
                    Number = transaction.Number,
                    DocumentNumber = transaction.DocumentNumber,
                    Remarks = transaction.Remarks,
                    Subject = transaction.Subject,
                    RemindDate = transaction.RemindDate,
                    RemindDateH = transaction.RemindDateH,
                    IsSigned = transaction.IsSigned,
                    OutboundDraftEditorType = transaction.OutboundDraftEditorType,
                    POBox = transaction.POBox,
                    PostCode = transaction.PostCode,
                    IsForIndividual = transaction.IsForIndividual,
                    DeliveryMethodId = transaction.DeliveryMethodId,
                    DeliveryMethod = (transaction.DeliveryMethod != null) ? new Lookup
                    {
                        Id = transaction.DeliveryMethodId,
                        Text = transaction.DeliveryMethod.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    TransactionCategory = (transaction.TransactionCategory != null) ? new Lookup
                    {
                        Id = transaction.TransactionCategory.Id,
                        Text = transaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    SuggestedTopic = (transaction.SuggestedTopic != null) ? new SuggestedTopic
                    {
                        Id = transaction.SuggestedTopic.Id,
                        Text = transaction.SuggestedTopic.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    SubjectClassifications = transaction.SubjectClassifications.Select(ts => new TransactionSubjectClassification
                    {
                        Id = ts.Id,
                        TransactionId = ts.TransactionId,

                        SubjectClassification = (ts.SubjectClassification != null) ? new SubjectClassification
                        {
                            Id = ts.SubjectClassification.Id,
                            Text = ts.SubjectClassification.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                    }
                        ).ToList(),
                    SignedByUser = (transaction.SignedByUser != null) ? new UserProfile
                    {
                        Id = transaction.SignedByUser.Id,
                        LocalName = transaction.SignedByUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Entity = (transaction.Entity != null) ? new OrgUnit
                    {
                        Id = transaction.Entity.Id,
                        LocalName = transaction.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    ToUser = (transaction.ToUser != null) ? new UserProfile
                    {
                        Id = transaction.ToUser.Id,
                        LocalName = transaction.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Priority = (transaction.Priority != null) ? new Priority
                    {
                        Id = transaction.Priority.Id,
                        Text = transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Confidentiality = (transaction.Confidentiality != null) ? new Permission
                    {
                        Id = transaction.Confidentiality.Id,
                        LocalName = transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    TransactionType = (transaction.TransactionType != null) ? new TransactionType
                    {
                        Id = transaction.TransactionType.Id,
                        Text = transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    LetterType = (transaction.LetterType != null) ? new LetterType
                    {
                        Id = transaction.LetterType.Id,
                        Text = transaction.LetterType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    ExternalParty = (transaction.ExternalParty != null) ? new ExternalParty
                    {
                        Id = transaction.ExternalParty.Id,
                        Number = transaction.ExternalParty.Number,
                        LocalName = (transaction.ExternalParty.Name != null) ? transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : string.Empty,
                        LocalAddress = (transaction.ExternalParty.Address != null) ? transaction.ExternalParty.Address.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : string.Empty
                    } : null,

                    ExternalPartyManager = (transaction.ExternalPartyManager != null) ? new ExternalPartyManager
                    {
                        Id = transaction.ExternalPartyManager.Id,
                        LocalName = transaction.ExternalPartyManager.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Links = transaction.Links.Select(tl => new TransactionLink
                    {
                        Id = tl.Id,
                        TransactionId = tl.TransactionId,




                        Type = (tl.Type != null) ? new Link
                        {
                            Id = tl.Type.Id,
                            Text = tl.Type.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,



                        ToTransaction = (tl.ToTransaction != null) ? new Transaction
                        {
                            Id = tl.ToTransaction.Id,
                            Number = tl.ToTransaction.Number,
                            Subject = tl.ToTransaction.Subject,
                            DateH = tl.ToTransaction.DateH,
                            Date = tl.ToTransaction.Date,
                            TransactionCategoryId = tl.ToTransaction.TransactionCategoryId,
                            TransactionType = (transaction.TransactionType != null) ? new TransactionType
                            {
                                Id = transaction.TransactionType.Id,
                                Text = transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            } : null,
                            YearH = tl.ToTransaction.YearH,
                            Year = tl.ToTransaction.Year,
                            TransactionCategory = new Lookup()
                            {
                                Id = tl.ToTransaction.TransactionCategoryId,
                                Text = tl.ToTransaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            },
                            ConfidentialityId = tl.ToTransaction.ConfidentialityId
                        } : null,



                    }
                    ).ToList(),

                    Attachments = transaction.Attachments.Select(a => new Attachment
                    {
                        DocumentInfo = (a.DocumentInfo != null) ? new DocumentInfo
                        {
                            Document = (a.DocumentInfo.Document != null) ? new Document
                            {
                                Id = a.DocumentInfo.Document.Id
                            } : null,



                            Id = a.DocumentInfo.Id,
                            MimeType = a.DocumentInfo.MimeType,
                            Name = a.DocumentInfo.Name,
                            Size = a.DocumentInfo.Size,
                            FromEntityId = a.DocumentInfo.FromEntityId,
                            FromUserId = a.DocumentInfo.FromUserId,
                            FromEntity = (a.DocumentInfo.FromEntity != null) ? new OrgUnit
                            {
                                Id = a.DocumentInfo.FromEntity.Id,
                                LocalName = a.DocumentInfo.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            } : null,
                            FromUser = (a.DocumentInfo.FromUser != null) ? new UserProfile
                            {
                                Id = a.DocumentInfo.FromUser.Id,
                                LocalName = a.DocumentInfo.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            } : null



                        } : null,



                        Type = (a.Type != null) ? new AttachmentType
                        {
                            Archivable = a.Type.Archivable,
                            Id = a.Type.Id,
                            Text = a.Type.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,



                        Description = a.Description,
                        Count = a.Count,
                        Id = a.Id,
                        TypeId = a.TypeId,
                        AttachmentSource = a.AttachmentSource,
                        CreatedBy = a.CreatedBy,
                    }).ToList(),
                };

                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionLink> GetTransactionLinks(int transactionId, string cultureName)
        {
            try
            {
                IList<TransactionLink> transactionLinks = (from transactionLink in _oMCSDbContext.TransactionLinks
                                                           join transaction in _oMCSDbContext.Transactions on
                                                           transactionLink.TransactionId equals transaction.Id
                                                           where transaction.Id == transactionId && transaction.IsDeleted == false
                                                           select new
                                                           {
                                                               transactionLink.Id,
                                                               transactionLink.TransactionId,
                                                               transactionLink.Type,
                                                               transactionLink.ToTransaction
                                                           }).ToList().Select(tl => new TransactionLink
                                                           {
                                                               Id = tl.Id,
                                                               TransactionId = tl.TransactionId,

                                                               Type = (tl.Type != null) ? new Link
                                                               {
                                                                   Id = tl.Type.Id,
                                                                   Text = tl.Type.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                                               } : null,

                                                               ToTransaction = (tl.ToTransaction != null) ? new Transaction
                                                               {
                                                                   Id = tl.ToTransaction.Id,
                                                                   Number = tl.ToTransaction.Number,
                                                                   TransactionCategory = (tl.ToTransaction.TransactionCategory != null) ? new Lookup
                                                                   {
                                                                       Id = tl.ToTransaction.TransactionCategory.Id,
                                                                       Text = tl.ToTransaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                                                   } : null,
                                                                   TransactionType = tl.ToTransaction.TransactionType,
                                                                   DateH = tl.ToTransaction.DateH,
                                                                   Date = tl.ToTransaction.Date,
                                                                   Links = tl.ToTransaction.Links,
                                                                   ConfidentialityId = tl.ToTransaction.ConfidentialityId,
                                                                   Assignments = tl.ToTransaction.Assignments.Select(a => new TransactionAssignment
                                                                   {
                                                                       Description = a.Description,
                                                                       Date = a.Date,
                                                                       DateH = a.DateH,
                                                                       Id = a.Id,
                                                                       TransactionPathId = a.TransactionPathId,
                                                                       CurrentPathStep = a.CurrentPathStep,
                                                                       Tray = (a.Tray != null) ? new Tray
                                                                       {
                                                                           Id = a.Tray.Id,
                                                                           LocalName = a.Tray.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                                                       } : null,

                                                                       ToUser = (a.ToUser != null) ? new UserProfile
                                                                       {
                                                                           Id = a.ToUser.Id,
                                                                           LocalName = a.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                                                       } : null,

                                                                       Transaction = (a.Transaction != null) ? new Transaction
                                                                       {
                                                                           Id = a.Transaction.Id
                                                                       } : null,

                                                                       Action = (a.Action != null) ? new Action
                                                                       {
                                                                           Id = a.Action.Id,
                                                                           LocalName = a.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                                                                           Type = a.Action.Type
                                                                       } : null,

                                                                       FromEntity = (a.FromEntity != null) ? new OrgUnit
                                                                       {
                                                                           Id = a.FromEntity.Id,
                                                                           LocalName = a.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                                                       } : null,

                                                                       ToEntity = (a.ToEntity != null) ? new OrgUnit
                                                                       {
                                                                           Id = a.ToEntity.Id,
                                                                           LocalName = a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                                                       } : null,
                                                                       FromUser = (a.FromUser != null) ? new UserProfile
                                                                       {
                                                                           Id = a.FromUser.Id,
                                                                           LocalName = a.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                                                       } : null
                                                                   }).ToList()
                                                               } : null
                                                           }
                                                                ).ToList();

                return transactionLinks;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionLink> GetTransactionLinksForCertificate(int transactionId, string cultureName)
        {
            try
            {
                IList<TransactionLink> transactionLinks = (from transactionLink in _oMCSDbContext.TransactionLinks
                                                           join transaction in _oMCSDbContext.Transactions on
                                                           transactionLink.TransactionId equals transaction.Id into trans
                                                           from t in trans.DefaultIfEmpty()
                                                           join transaction2 in _oMCSDbContext.Transactions on
                                                           transactionLink.ToTransactionId equals transaction2.Id into toTrans
                                                           from toT in toTrans.DefaultIfEmpty()
                                                           where (transactionLink.TransactionId == transactionId || transactionLink.ToTransactionId == transactionId)
                                                           select new
                                                           {
                                                               transactionLink.Id,
                                                               transactionLink.TransactionId,
                                                               transactionLink.Type,
                                                               transactionLink.Transaction,
                                                               transactionLink.ToTransaction,
                                                               transactionLink.ToTransactionId
                                                           }).ToList().Select(tl => new TransactionLink
                                                           {
                                                               Id = tl.Id,
                                                               TransactionId = tl.TransactionId,
                                                               ToTransactionId = tl.ToTransactionId,
                                                               Type = (tl.Type != null) ? new Link
                                                               {
                                                                   Id = tl.Type.Id,
                                                                   Text = tl.Type.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                                               } : null,

                                                               ToTransaction = (tl.ToTransaction != null) ? new Transaction
                                                               {
                                                                   Id = tl.ToTransaction.Id,
                                                                   Number = tl.ToTransaction.Number,
                                                                   TransactionCategory = tl.ToTransaction.TransactionCategory,
                                                                   TransactionType = tl.ToTransaction.TransactionType,
                                                                   DateH = tl.ToTransaction.DateH,
                                                                   Date = tl.ToTransaction.Date,
                                                                   Links = tl.ToTransaction.Links
                                                               } : null,
                                                               Transaction = (tl.Transaction != null) ? new Transaction
                                                               {
                                                                   Id = tl.Transaction.Id,
                                                                   Number = tl.Transaction.Number,
                                                                   TransactionCategory = tl.Transaction.TransactionCategory,
                                                                   TransactionType = tl.Transaction.TransactionType,
                                                                   DateH = tl.Transaction.DateH,
                                                                   Date = tl.Transaction.Date,
                                                                   Links = tl.Transaction.Links
                                                               } : null
                                                           }
                                                                ).ToList();

                return transactionLinks;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int GetTransactionCopiesCount(Expression<Func<TransactionCopy, bool>> where)
        {
            try
            {
                return _oMCSDbContext.TransactionCopies.Where(where).Where(t => !t.Transaction.IsDeleted).ToList().Count;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public int GetTransactionExternalCopiesCount(Expression<Func<TransactionExternalCopy, bool>> where)
        {
            try
            {
                return _oMCSDbContext.TransactionExternalCopies.Where(where).Where(t => !t.Transaction.IsDeleted).ToList().Count;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<TransactionCopy> GetTransactionCopiesByTransactionId(int transactionId, int userId, string cultureName)
        {
            try
            {
                IList<TransactionCopy> transactionCopies = _oMCSDbContext.TransactionCopies
                    .Where(tc => tc.TransactionId == transactionId && (!tc.IsBcc || (tc.IsBcc && (tc.FromUserId == userId || tc.UserId == userId)))).
                    Select(c => new
                    {
                        c.Id,
                        c.Date,
                        c.DateH,
                        c.TransactionId,
                        c.IsSent,
                        c.Status,

                        User = c.User ?? null,
                        userName = c.User != null ? c.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : "",
                        c.UserId,


                        Action = (c.Action ?? null),
                        c.ActionId,
                        ActionName = c.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                        ActionType = c.Action.Type,

                        Entity = c.Entity ?? null,
                        c.EntityId,
                        EntityName = c.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                        FromEntityName = c.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                        FromUserName = c.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                        c.GeneralExplanation,
                        c.SpecialExplanation,
                        c.FromEntityId,
                        c.FromUserId,
                        c.IsOpr,
                        c.IsBcc,
                        OprEntity = c.OprEntity ?? null,
                        c.OprEntityId,
                        c.SpecialCopy,
                        OprEntityName = c.OprEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                        c.CreatedOn,
                        c.SentDate,
                        ViewedByUsername = c.ViewedBy != null ? c.ViewedBy.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : null,
                        c.ViewedOnDate,
                        c.ViewedOnDateH,
                        c.ViewedById



                    }).ToList().Select(c => new TransactionCopy
                    {
                        Id = c.Id,
                        Date = c.Date,
                        DateH = c.DateH,
                        TransactionId = c.TransactionId,
                        IsSent = c.IsSent,
                        Status = c.Status,
                        Action = (c.Action != null) ? new Domain.Action
                        {
                            Id = c.Action.Id,
                            LocalName = c.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                            Type = c.Action.Type
                        } : null,

                        User = (c.User != null) ? new UserProfile
                        {
                            Id = c.UserId.Value,
                            LocalName = c.userName
                        } : null,

                        Entity = (c.Entity != null) ? new OrgUnit
                        {
                            Id = c.EntityId.Value,
                            LocalName = c.EntityName
                        } : null,
                        FromUser = (c.FromUserId.HasValue) ? new UserProfile
                        {
                            Id = c.FromUserId.Value,
                            LocalName = c.FromUserName
                        } : null,
                        FromEntity = (c.FromEntityId.HasValue) ? new OrgUnit
                        {
                            Id = c.FromEntityId.Value,
                            LocalName = c.FromEntityName
                        } : null,
                        GeneralExplanation = c.GeneralExplanation,
                        SpecialExplanation = c.SpecialExplanation,
                        FromUserId = c.FromUserId,
                        FromEntityId = c.FromEntityId,
                        IsOpr = c.IsOpr,
                        IsBcc = c.IsBcc,
                        OprEntityId = c.OprEntityId,
                        OprEntity = (c.OprEntity != null) ? new OrgUnit
                        {
                            Id = c.OprEntityId.Value,
                            LocalName = c.OprEntityName
                        } : null,
                        CreatedOn = c.CreatedOn,
                        SentDate = c.SentDate,
                        ViewedById = c.ViewedById,
                        ViewedOnDate = c.ViewedOnDate,
                        ViewedOnDateH = c.ViewedOnDateH,
                        ViewedBy = (c.ViewedById.HasValue) ? new UserProfile
                        {
                            Id = c.ViewedById.Value,
                            LocalName = c.ViewedByUsername
                        } : null,

                    }
                    ).ToList();
                return transactionCopies;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public TransactionCopy GetTransactionCopyById(int id)
        {
            try
            {
                return _oMCSDbContext.TransactionCopies.Where(c => c.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionExternalCopy> GetTransactionExternalCopiesByTransactionId(int transactionId, string cultureName)
        {
            try
            {
                IList<TransactionExternalCopy> transactionCopies = _oMCSDbContext.TransactionExternalCopies
                    .Where(tc => tc.TransactionId == transactionId)
                    .Select(c => new
                    {
                        c.Id,
                        c.Date,
                        c.DateH,
                        c.ActionId,
                        c.TransactionId,
                        c.Viewed,
                        UserName = c.User.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                        c.UserId,


                        Action = (c.Action ?? null),
                        ActionsId = c.Action.Id,
                        ActionName = c.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                        ActionType = c.Action.Type,
                        c.EntityId,
                        EntityName = c.Entity.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                        FromEntityName = c.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                        c.FromEntityId,
                        c.FromUserId,
                        FromUsername = c.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                        ExternalAttachment = c.ExternalPartyAttachment.Select(o => new
                        {
                            id = o.Id,
                            DocumentInfoId = o.DocumentInfo.Id,
                            o.DocumentInfo.MimeType,
                            o.DocumentInfo.Name,
                            o.DocumentInfo.Size,
                            o.DocumentInfo.ECMId,
                            Document = new
                            {
                                Document = o.DocumentInfo.Document.Id,
                                o.DocumentInfo.Document.Content
                            },
                        })

                    }).ToList().Select(c => new TransactionExternalCopy
                    {
                        Id = c.Id,
                        Date = c.Date,
                        DateH = c.DateH,
                        ActionId = c.ActionId,
                        TransactionId = c.TransactionId,
                        Viewed = c.Viewed,
                        Action = (c.Action != null) ? new Domain.Action
                        {
                            Id = c.ActionId,
                            LocalName = c.ActionName,
                            Type = c.ActionType
                        } : null,

                        User = (c.UserId != null) ? new ExternalPartyManager
                        {
                            Id = c.UserId.Value,
                            LocalName = c.UserName
                        } : null,

                        Entity = (c.EntityId.HasValue) ? new ExternalParty
                        {
                            Id = c.EntityId.Value,
                            LocalName = c.EntityName
                        } : null,
                        FromEntity = (c.FromEntityId.HasValue) ? new OrgUnit
                        {
                            Id = c.FromEntityId.Value,
                            LocalName = c.FromEntityName
                        } : null,
                        FromUser = (c.FromUserId.HasValue) ? new UserProfile
                        {
                            Id = c.FromUserId.Value,
                            LocalName = c.FromUsername
                        } : null,
                        ExternalPartyAttachment = c.ExternalAttachment.ToList().Select(o => new ExternalPartyAttachment
                        {
                            Id = o.id,
                            DocumentInfo = new DocumentInfo
                            {
                                Document = new Document
                                {
                                    Id = o.Document.Document,
                                    Content = o.Document.Content
                                },
                                Id = o.DocumentInfoId,
                                MimeType = o.MimeType,
                                Name = o.Name,
                                Size = o.Size,
                                IsDeleted = false,
                                ECMId = o.ECMId
                            },

                        }).ToList(),
                    }
                    ).ToList();

                return transactionCopies;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionCopy> GetTransactionCopies(Expression<Func<TransactionCopy, bool>> where, TrayType trayType, SearchCriteriaCustom searchCriteria, out int rowsCount, int? UserWeight, int currentUserId)
        {
            try
            {
                int Inbound = Common.TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int InternalOutbound = Common.TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int ExternalOutbound = Common.TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int Viewed = TransCopyStatus.Viewed.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);
                int deletedStatus = TransactionStatus.Deleted.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                IQueryable<TransactionCopy> transactions = _oMCSDbContext.TransactionCopies
                    .Where(where).Where(tr => !tr.Transaction.IsDeleted && //tr.Status != Delete &&
                    ((trayType == TrayType.Copies && tr.Transaction.TransactionCategoryId == Inbound && tr.Status != Viewed && tr.Status != deletedStatus && !tr.SpecialCopy) ||
                    (trayType == TrayType.SavedCopies && tr.Status == Viewed && tr.Status != deletedStatus) ||
                    (trayType == TrayType.SpecialCopies && tr.Status != Viewed && tr.Status != deletedStatus && tr.SpecialCopy) ||
                    (trayType == TrayType.InternalInboundCopies && tr.Transaction.TransactionCategoryId == InternalOutbound && tr.Status != Viewed && tr.Status != deletedStatus && !tr.SpecialCopy) ||
                    (trayType == TrayType.CopiesOutbound && tr.Transaction.TransactionCategoryId == ExternalOutbound && tr.Status != Viewed && tr.Status != deletedStatus && !tr.SpecialCopy)));

                if (searchCriteria.SearchColunms != null && searchCriteria.SearchColunms.Count > 0)
                {
                    foreach (SearchColunm searchColunm in searchCriteria.SearchColunms)
                    {
                        if (typeof(int).IsAssignableFrom(typeof(TransactionCopy).GetProperty(searchColunm.ColunmName).PropertyType) & searchColunm.ColunmName == "Number")
                        {
                            transactions = transactions.Where(p => p.Transaction.Number.ToString().Equals(searchColunm.ColunmValue));
                        }
                    }
                }

                if (searchCriteria.FromDateTime.HasValue)
                {
                    transactions = transactions.Where(p => p.Transaction.Date >= searchCriteria.FromDateTime.Value);
                }

                if (searchCriteria.ToDateTime.HasValue)
                {
                    transactions = transactions.Where(p => p.Transaction.Date <= searchCriteria.ToDateTime.Value);
                }

                //TODO:To Modify It To Be Dynamic Using Dynamic Linq Library Instead Of Static Values
                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        if (filter.Value == "-1")
                        {
                            continue;
                        }
                        if (filter.ColumnName == "FromDateTime")
                        {

                            transactions = SortTextByFromDateTime(transactions, filter.Value, filter.Type, searchCriteria.CultureName);


                        }
                        else if (filter.ColumnName == "ToDateTime")
                        {
                            transactions = SortTextByToDateTime(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (filter.ColumnName == "ToEntity" || filter.ColumnName == "FromEntity")
                        {
                            if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(TransactionAssignment).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "FromEntity")
                            {
                                transactions = SortTextByFromEntity(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                            }
                            //else if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "ToEntity")
                            //{
                            //    transactions = SortTextByToEntity(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                            //}
                        }

                        else if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "ToUser")
                        {
                            transactions = SortTextByToUser(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "Status")
                        {
                            transactions = SortTextByStatus(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "TransactionType")
                        {
                            transactions = SortTextByTransactionCategory(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(Permission).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "Confidentiality")
                        {
                            transactions = SortTextByConfidentialityLevel(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(ILocalizeEntity).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "Priority")
                        {
                            transactions = SortTextByPriorityLevel(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(long).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "Number")
                        {
                            transactions = SortTextByNumber(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(string).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "DocumentNumber")
                        {
                            transactions = SortTextByDocumentNumber(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(DateTime).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "ReminderDate")
                        {
                            transactions = SortTextByReminderDate(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (typeof(string).IsAssignableFrom(typeof(Transaction).GetProperty(filter.ColumnName).PropertyType) & filter.ColumnName == "Subject")
                        {
                            transactions = SortTextBySubject(transactions, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                    }
                }

                rowsCount = transactions.Count();
                if (searchCriteria.MultipleOrderBy != null)
                {
                    searchCriteria.MultipleOrderBy = searchCriteria.MultipleOrderBy.OrderBy(a => a.Index).ToList();
                    foreach (var orderBy in searchCriteria.MultipleOrderBy)
                    {
                        if (orderBy.ColumnName == "ToEntity")
                            transactions = OrderByToEntity(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "FromEntity")
                            transactions = OrderByFromEntity(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "ToUser")
                            transactions = OrderByToUser(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Status")
                            transactions = OrderByStatus(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Priority")
                            transactions = OrderByPriorityLevel(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Confidentiality")
                            transactions = OrderByConfidentialityLevel(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Id")
                            transactions = OrderById(transactions, searchCriteria, orderBy.IsAscending);

                        else if (orderBy.ColumnName == "Number")
                            transactions = OrderByNumber(transactions, searchCriteria, orderBy.IsAscending);

                    }
                }
                else
                {
                    // transactions = OrderByNumber(transactions, searchCriteria, false);
                    transactions = OrderByDate(transactions, searchCriteria, false);
                }


                transactions = transactions.Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                          .Take(searchCriteria.PageSize);

                return transactions.ToList().Select(t => new TransactionCopy
                {
                    Transaction = new Transaction
                    {
                        Id = t.Transaction.Id,
                        Date = t.Transaction.Date,
                        DateH = t.Transaction.DateH,
                        Entity = (t.Transaction.Entity != null) ? new OrgUnit
                        {
                            Id = t.Transaction.Entity.Id,
                            LocalName = t.Transaction.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        } : null,
                        TransactionCategoryId = t.Transaction.TransactionCategoryId,
                        DeliveryMethodId = t.Transaction.DeliveryMethodId,
                        Priority = (t.Transaction.Priority != null) ? new Priority
                        {
                            Id = t.Transaction.Priority.Id,
                            Text = t.Transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        } : null,
                        Confidentiality = (t.Transaction.Confidentiality != null) ? new Permission
                        {
                            Id = t.Transaction.Confidentiality.Id,
                            LocalName = t.Transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        } : null,
                        LetterType = (t.Transaction.LetterType != null) ? new LetterType
                        {
                            Id = t.Transaction.LetterType.Id,
                            Text = t.Transaction.LetterType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        } : null,
                        Status = (t.Transaction.Status != null) ? new Lookup
                        {
                            Id = t.Transaction.Status.Id,
                            Text = t.Transaction.Status.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        } : null,
                        Subject = t.Transaction.Subject,
                        DocumentNumber = t.Transaction.DocumentNumber,
                        TransactionCategory = (t.Transaction.TransactionCategory != null) ? new Lookup
                        {
                            Id = t.Transaction.TransactionCategory.Id,
                            Text = t.Transaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        } : null,
                        TransactionType = (t.Transaction.TransactionType != null) ? new TransactionType
                        {
                            Id = t.Transaction.TransactionType.Id,
                            Text = t.Transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText(),
                            Color = (t.Transaction.TransactionType.Color != null) ? new Lookup
                            {
                                Id = t.Transaction.TransactionType.Color.Id,
                            } : null
                        } : null,
                        ToUser = (t.Transaction.ToUser != null) ? new UserProfile
                        {
                            Id = t.Transaction.ToUser.Id,
                            LocalName = t.Transaction.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        } : null,
                        User = (t.Transaction.User != null) ? new UserProfile
                        {
                            Id = t.Transaction.User.Id,
                            LocalName = t.Transaction.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        } : null,
                        Number = t.Transaction.Number,
                        OrgUnit = (t.Transaction.OrgUnit != null) ? new OrgUnit
                        {
                            Id = t.Transaction.OrgUnit.Id,
                            LocalName = t.Transaction.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                        } : null,
                        RemindDate = t.Transaction.RemindDate,
                        RemindDateH = t.Transaction.RemindDateH,
                        StatusId = t.Status,
                        RejectionReason = t.Transaction.RejectionReason,
                        HasPermission = t.Transaction.SpecialAuthorizations.Any(sa => sa.UserProfileId == currentUserId && (!sa.ExpiredDate.HasValue || sa.ExpiredDate > DateTime.Now))
                        ? true : UserWeight == null ? false : t.Transaction.Confidentiality.Weight <= UserWeight ? true : false,
                    },
                    IsOpr = t.IsOpr,
                    IsBcc = t.IsBcc,
                    OprEntityId = t.OprEntityId,
                    SpecialCopy = t.SpecialCopy,
                    OprEntity = new OrgUnit
                    {
                        LocalName = (t.OprEntity != null) ? t.OprEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText() : null
                    },
                    Id = t.Id,
                }).OrderByDescending(t => t.Date).ToList();

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public int? GetTransactionId(Expression<Func<Transaction, bool>> @where)
        {
            try
            {
                Transaction transaction = _oMCSDbContext.Transactions.Where(@where).FirstOrDefault();

                if (transaction != null && !transaction.IsDeleted)
                {
                    return transaction.Id;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public DocumentInfo GetMainDocumentByTransactionId(int transactionId)
        {
            try
            {
                Transaction transaction =
                    _oMCSDbContext.Transactions.Where(t => t.Id == transactionId & !t.IsDeleted).FirstOrDefault();

                if (transaction == null)
                {
                    return null;
                }

                return transaction.MainDocument;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public DocumentInfo GetOldMainDocumentByTransactionId(int transactionId)
        {
            try
            {
                Transaction transaction =
                    _oMCSDbContext.Transactions.Where(t => t.Id == transactionId & !t.IsDeleted).FirstOrDefault();

                if (transaction == null)
                {
                    return null;
                }

                return transaction.OldWordDocumnt;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void SetTransactionCopyToViewed(TransactionCopy transactionCopy)
        {
            try
            {
                transactionCopy.Status = TransCopyStatus.Viewed.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void SetTransactionCopyToDelete(TransactionCopy transactionCopy)
        {
            try
            {
                transactionCopy.Status = TransCopyStatus.Delete.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void SetTransactionCopyToUndo(TransactionCopy transactionCopy)
        {
            try
            {
                transactionCopy.Status = TransCopyStatus.NotViewed.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void SetTransactionExternalCopyToViewed(TransactionExternalCopy transactionExternalCopy)
        {
            try
            {
                transactionExternalCopy.Viewed = true;

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionName> GetTransactionNames(int transactionId, string cultureName)
        {

            try
            {
                List<TransactionName> transactionNames = _oMCSDbContext.Transactions.Where(t => t.Id == transactionId).FirstOrDefault().Names.ToList();
                return transactionNames;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IList<Attachment> GetTransactionAttachments(int transactionId, string cultureName)
        {

            try
            {
                Transaction transaction = _oMCSDbContext.Transactions.Where(t => t.Id == transactionId && !t.IsDeleted).FirstOrDefault();

                List<Attachment> attachments = new List<Attachment>();

                attachments = transaction.Attachments.Where(a => a.DocumentInfo != null).Where(a => a.DocumentInfo.Document != null).Select(a => new Attachment
                {
                    DocumentInfo = new DocumentInfo
                    {
                        Document = new Document
                        {
                            Id = a.DocumentInfo.Document.Id
                        },

                        Id = a.DocumentInfo.Id,
                        MimeType = a.DocumentInfo.MimeType,
                        Name = a.DocumentInfo.Name,
                        Size = a.DocumentInfo.Size,
                        FromUserId = a.DocumentInfo.FromUserId,
                        FromUser = (a.DocumentInfo.FromUser != null) ? new UserProfile
                        {
                            Id = a.DocumentInfo.FromUser.Id,
                            LocalName = a.DocumentInfo.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null
                    },

                    Type = (a.Type != null) ? new AttachmentType
                    {
                        Archivable = a.Type.Archivable,
                        Id = a.Type.Id,
                        Text = a.Type.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Description = a.Description,
                    Count = a.Count,
                    Id = a.Id,
                }).ToList();

                return attachments;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void TransactionElcOutBoundAdd(TransactionElcOutBound transactionElcOutBound)
        {
            var ElcOutBound = _oMCSDbContext.TransactionElcOutBounds.FirstOrDefault(f => f.UserId == transactionElcOutBound.UserId && f.TransactionId == transactionElcOutBound.TransactionId && f.EntityId == transactionElcOutBound.EntityId);
            if (ElcOutBound == null)
            {
                _oMCSDbContext.TransactionElcOutBounds.Add(transactionElcOutBound);
                _oMCSDbContext.SaveChanges();
            }
        }
        public void AddConfidentialityAcknowledgment(int TransactionId, int UserId, int OrgUnitId, DateTime CreatedDate)
        {
            var TransactionConfidentialityAcknowledgment = _oMCSDbContext.TransactionConfidAcknowledgeds.FirstOrDefault(f => f.UserId == UserId && f.TransactionId == TransactionId && f.EntityId == OrgUnitId);
            if (TransactionConfidentialityAcknowledgment == null)
            {
                TransactionConfidAcknowledged transactionConfidentialityAcknowledgment = new TransactionConfidAcknowledged();
                transactionConfidentialityAcknowledgment.TransactionId = TransactionId;
                transactionConfidentialityAcknowledgment.UserId = UserId;
                transactionConfidentialityAcknowledgment.EntityId = OrgUnitId;
                transactionConfidentialityAcknowledgment.CreatedBy = UserId;
                transactionConfidentialityAcknowledgment.CreatedOn = CreatedDate;

                _oMCSDbContext.TransactionConfidAcknowledgeds.Add(transactionConfidentialityAcknowledgment);
                _oMCSDbContext.SaveChanges();
            }
        }

        public void TransactionElcOutBoundUpdate(int userId, int orgUnitId, bool ishidden, int transactionId)
        {
            try
            {

                List<TransactionElcOutBound> TransactionElcOutBound = _oMCSDbContext.TransactionElcOutBounds.Where(f => f.TransactionId == transactionId).ToList();

                if (TransactionElcOutBound.Count > 0)//Hide all
                {
                    foreach (var item in TransactionElcOutBound)
                    {
                        item.Ishidden = ishidden;
                        item.ModefiedBy = userId;
                        item.ModefiedOn = DateTime.Now;
                        _oMCSDbContext.Entry(item).Property(x => x.Ishidden).IsModified = true;
                    }
                    _oMCSDbContext.SaveChanges();

                }

                return;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void AcknowledgeElcOutBound(int userId, int orgUnitId, bool ishidden, int transactionId)
        {
            try
            {

                TransactionElcOutBound usertransactionElcOutBound = _oMCSDbContext.TransactionElcOutBounds.
                    Where(f => f.TransactionId == transactionId && f.EntityId == orgUnitId && f.UserId == userId).SingleOrDefault();
                if (usertransactionElcOutBound != null)
                {

                    usertransactionElcOutBound.Ishidden = ishidden;
                    usertransactionElcOutBound.ModefiedBy = userId;
                    usertransactionElcOutBound.ModefiedOn = DateTime.Now;
                    usertransactionElcOutBound.ModefiedOn = DateTime.Now;

                    _oMCSDbContext.SaveChanges();
                }
                else
                {
                    TransactionElcOutBound OrgunitTransactionElcOutBound = _oMCSDbContext.TransactionElcOutBounds.
                                       Where(f => f.TransactionId == transactionId && f.EntityId == orgUnitId).SingleOrDefault();

                    OrgunitTransactionElcOutBound.Ishidden = ishidden;
                    OrgunitTransactionElcOutBound.ModefiedBy = userId;
                    OrgunitTransactionElcOutBound.ModefiedOn = DateTime.Now;
                    OrgunitTransactionElcOutBound.ModefiedOn = DateTime.Now;

                    _oMCSDbContext.SaveChanges();

                }
                Transaction transaction = GetTransactionById(transactionId);

                foreach (TransactionCopy transactionCopy in transaction.Copies)
                {
                    transactionCopy.IsSent = 1;
                    transactionCopy.SentDate = DateTime.Now;
                }


                if (transaction.NeedAcknowled)
                {
                    transaction.NeedAcknowled = false;
                    _oMCSDbContext.SaveChanges();

                }
                return;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region SearchCriteria

        private IQueryable<Transaction> SortTextByToEntity(IQueryable<Transaction> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.Entity.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.Entity.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.Entity.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.Entity.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }
        private IQueryable<TransactionCopy> SortTextByFromEntity(IQueryable<TransactionCopy> source, string textValue, FilterType filterType, string culureName)
        {
            var id = Convert.ToInt32(textValue);
            //if (textValue != "" && textValue != null)
            //{
            //    var id = Convert.ToInt32(textValue);
            //    where = ExpressionUtility.AndAlso(where, dr => dr.FromEntity.Id == id);
            //}
            //  return _oMCSDbContext.TransactionAssignments.Where(where).Select(t => t.Transaction).Where(tr => !tr.IsDeleted);
            return source.Where(p => p.Transaction.Assignments.Select(a => a.FromEntity.Id == id).FirstOrDefault());

        }

        private IQueryable<TransactionCopy> SortTextByToUser(IQueryable<TransactionCopy> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.Transaction.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.Transaction.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.Transaction.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.Transaction.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }

        private IQueryable<TransactionCopy> SortTextByStatus(IQueryable<TransactionCopy> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.Transaction.Status.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.Transaction.Status.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.Transaction.Status.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    int id = Convert.ToInt32(textValue);
                    return source.Where(p => p.Status == id);//.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }

        private IQueryable<TransactionCopy> SortTextByTransactionCategory(IQueryable<TransactionCopy> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != null || textValue != "")
            {
                int id = Convert.ToInt32(textValue);
                return source.Where(p => p.Transaction.TransactionCategoryId == id);
            }

            return source;
        }

        private IQueryable<TransactionCopy> SortTextByConfidentialityLevel(IQueryable<TransactionCopy> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != null || textValue != "")
            {
                int id = Convert.ToInt32(textValue);
                return source.Where(p => p.Transaction.Confidentiality.Id == id);
            }

            return source;
        }

        private IQueryable<TransactionCopy> SortTextByPriorityLevel(IQueryable<TransactionCopy> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != null || textValue != "")
            {
                int id = Convert.ToInt32(textValue);
                return source.Where(p => p.Transaction.Priority.Id == id);
            }

            return source;
        }

        private IQueryable<TransactionCopy> SortTextByNumber(IQueryable<TransactionCopy> source, string textValue, FilterType filterType, string culureName)
        {
            if (SystemConfigurations.IsOracleMigrationEnabled)
            {
                long Number = Convert.ToInt64(textValue);
                switch (filterType)
                {
                    case FilterType.Contains:
                    case FilterType.EndsWidth:
                    case FilterType.StartsWith:
                    case FilterType.Equals:
                        return source.Where(p => p.Transaction.Number.Equals(Number));
                }
            }
            else
            {
                switch (filterType)
                {
                    case FilterType.Contains:
                        return source.Where(p => p.Transaction.Number.ToString().ToLower().Contains(textValue.ToLower()));
                    case FilterType.EndsWidth:
                        return source.Where(p => p.Transaction.Number.ToString().ToLower().EndsWith(textValue.ToLower()));
                    case FilterType.StartsWith:
                        return source.Where(p => p.Transaction.Number.ToString().ToLower().StartsWith(textValue.ToLower()));
                    case FilterType.Equals:
                        return source.Where(p => p.Transaction.Number.ToString().ToLower().Equals(textValue.ToLower()));
                }
            }
            return source;
        }

        private IQueryable<TransactionCopy> SortTextByDocumentNumber(IQueryable<TransactionCopy> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.Transaction.DocumentNumber.ToString().ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.Transaction.DocumentNumber.ToString().ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.Transaction.DocumentNumber.ToString().ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.Transaction.DocumentNumber.ToString().ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }

        private IQueryable<TransactionCopy> SortTextByReminderDate(IQueryable<TransactionCopy> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.Transaction.RemindDate.ToString().ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.Transaction.RemindDate.ToString().ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.Transaction.RemindDate.ToString().ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.Transaction.RemindDate.ToString().ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }

        private IQueryable<TransactionCopy> SortTextByToDateTime(IQueryable<TransactionCopy> source, string textValue, FilterType filterType, string culureName)
        {
            var list = textValue.Split('/').ToList().Select(f => int.Parse(f)).ToList();
            DateTime dt = new DateTime(list[2], list[1], list[0]);
            return source.Where(p => p.Transaction.Date <= dt);
        }
        private IQueryable<TransactionCopy> SortTextByFromDateTime(IQueryable<TransactionCopy> source, string textValue, FilterType filterType, string culureName)
        {
            var list = textValue.Split('/').ToList().Select(f => int.Parse(f)).ToList();
            DateTime dt = new DateTime(list[2], list[1], list[0]);
            return source.Where(p => p.Transaction.Date >= dt);
        }

        private IQueryable<TransactionCopy> SortTextBySubject(IQueryable<TransactionCopy> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(p => p.Transaction.Subject.ToString().ToLower().Contains(textValue.ToLower()));
                case FilterType.EndsWidth:
                    return source.Where(p => p.Transaction.Subject.ToString().ToLower().EndsWith(textValue.ToLower()));
                case FilterType.StartsWith:
                    return source.Where(p => p.Transaction.Subject.ToString().ToLower().StartsWith(textValue.ToLower()));
                case FilterType.Equals:
                    return source.Where(p => p.Transaction.Subject.ToString().ToLower().Equals(textValue.ToLower()));
            }

            return source;
        }


        private IQueryable<TransactionCopy> OrderByToEntity(IQueryable<TransactionCopy> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.Entity.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.Entity.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }
        private IQueryable<TransactionCopy> OrderByFromEntity(IQueryable<TransactionCopy> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (searchCriteria.SearchData != 0)
            {
                return source.SmartOrderBy(p => p.Transaction.OrgUnitId == searchCriteria.SearchData);
            }
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.OrgUnitId);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.OrgUnitId);
            }

            return source;
        }

        private IQueryable<TransactionCopy> OrderByToUser(IQueryable<TransactionCopy> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.ToUser.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.ToUser.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<TransactionCopy> OrderByStatus(IQueryable<TransactionCopy> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.Status.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.Status.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<TransactionCopy> OrderByConfidentialityLevel(IQueryable<TransactionCopy> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.Confidentiality.Name.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Id);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.Confidentiality.Name.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Id);
            }

            return source;
        }

        private IQueryable<TransactionCopy> OrderByPriorityLevel(IQueryable<TransactionCopy> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.Priority.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Id);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.Priority.LocalizationIdentifier.Localizations
                               .Where(l => l.Culture.ShortName == searchCriteria.CultureName).FirstOrDefault().Id);
            }

            return source;
        }

        private IQueryable<TransactionCopy> OrderByNumber(IQueryable<TransactionCopy> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.Number);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.Number);
            }

            return source;
        }

        private IQueryable<TransactionCopy> OrderByDate(IQueryable<TransactionCopy> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.Date);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.Date);
            }

            return source;
        }

        private IQueryable<TransactionCopy> OrderById(IQueryable<TransactionCopy> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Transaction.Id);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Transaction.Id);
            }

            return source;
        }

        public void UpdateTransactionStatusByTransNo(long transactionNumber, int statusId, string rejectionReason = null)
        {
            try
            {
                Transaction transaction = GetTransactionByTransactionNumber(transactionNumber);
                transaction.StatusId = statusId;
                if (rejectionReason != null)
                {
                    transaction.RejectionReason = rejectionReason;
                }
                _oMCSDbContext.Entry(transaction).State = EntityState.Modified;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateTransactionDeleteByTransId(long transactionId, bool isDeleted)
        {
            try
            {
                Transaction transaction = GetTransactionByTransactionId(transactionId);
                transaction.IsDeleted = isDeleted;

                _oMCSDbContext.Entry(transaction).State = EntityState.Modified;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteDraftTransaction(long transactionId, bool isDeleted)
        {
            try
            {
                var deletedStatus = TransactionStatus.Deleted.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                var inProcessStatus = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                Transaction transaction = GetTransactionByTransactionId(transactionId);



                transaction.StatusId = isDeleted ? deletedStatus : inProcessStatus;



                _oMCSDbContext.Entry(transaction).State = EntityState.Modified;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateTransactionStatus(int transId, int statusId)
        {
            try
            {
                Transaction transaction = GetTransactionById(transId);
                transaction.StatusId = statusId;

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void UpdateTransactionDelivary(int transId, int DelivaryId)
        {
            try
            {
                Transaction transaction = GetTransactionById(transId);
                transaction.DeliveryMethodId = DelivaryId;

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public void UpdateTransactionSavedReason(int transId, string reason)
        {
            try
            {
                Transaction transaction = GetTransactionById(transId);
                transaction.SavedReason = reason;

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SetTransactionCopiesSent(int transactionId)
        {
            try
            {
                List<TransactionCopy> transactionCopies = _oMCSDbContext.TransactionCopies.Where(tc => tc.TransactionId == transactionId).ToList();
                transactionCopies.Select(tc => tc.IsSent == 1);
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void UpdateTransactionEntityAndToUser(int transactionId, int entityId, int? userId)
        {
            try
            {
                Transaction transaction = GetTransactionById(transactionId);
                transaction.EntityId = entityId;
                transaction.ToUserId = userId;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion

        public void SaveTransactionDeliveryNumber(Transaction transaction)
        {
            try
            {
                Transaction oldTransaction = GetTransactionById(transaction.Id);
                oldTransaction.DeliveryNumber = transaction.DeliveryNumber;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void UpdateTransactionStatusAndEntityByTransId(long transactionId, int statusId, int Entityid, int? Userid, string rejectionReason = null)
        {
            try
            {
                Transaction transaction = GetTransactionByTransactionId(transactionId);
                transaction.StatusId = statusId;
                transaction.EntityId = Entityid;
                transaction.ToUserId = Userid;
                if (rejectionReason != null)
                {
                    transaction.RejectionReason = rejectionReason;
                }
                _oMCSDbContext.Entry(transaction).State = EntityState.Modified;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool IsMatchNumberOrBarcode(int transId, string number, string barcode)
        {
            bool isMatch = true;

            if (!string.IsNullOrEmpty(number))
            {
                var transactionDeliveryReportResult = (from trans in _oMCSDbContext.Transactions
                                                       join transDeliveryReport in _oMCSDbContext.TransactionDeliveryReports on trans.Id equals transDeliveryReport.TransactionId
                                                       where trans.IsDeleted == false && trans.Id == transId
                                                       orderby transDeliveryReport.Id descending
                                                       select transDeliveryReport).Take(1);
                isMatch &= transactionDeliveryReportResult.FirstOrDefault(a => a.Number == number) != null ? true : false;
            }
            if (!string.IsNullOrEmpty(barcode))
            {
                var barcodeResult = (from trans in _oMCSDbContext.Transactions
                                     join bar in _oMCSDbContext.Barcodes on trans.Id equals bar.ReferenceId
                                     where trans.IsDeleted == false && trans.Id == transId
                                     orderby bar.Id descending
                                     select bar).Take(1);
                isMatch &= barcodeResult.FirstOrDefault(a => a.Value == barcode) != null ? true : false;
            }
            return isMatch;
        }
        public void UpdatePhysicalTransactionAssignment(int transactionId, int userId, int entityId)
        {
            TransactionAssignment transactionAssignment = GetTransactionById(transactionId).Assignments.FirstOrDefault();
            transactionAssignment.PhysicalEntityId = entityId;
            transactionAssignment.PhysicalUserId = userId;
            transactionAssignment.PhysicalDate = DateTime.Now;
            transactionAssignment.PhysicalDateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
            _oMCSDbContext.TransactionAssignments.Attach(transactionAssignment);
            _oMCSDbContext.Entry(transactionAssignment).State = EntityState.Modified;
            _oMCSDbContext.SaveChanges();
        }
        public void UpdateMainDcument(DocumentInfo mainDocument, int transactionId)
        {
            try
            {
                DocumentInfo documentInfo = GetMainDocumentByTransactionId(transactionId);

                if (documentInfo != null)
                {
                    documentInfo.Id = mainDocument.Id;
                    documentInfo.IsDeleted = mainDocument.IsDeleted;
                    documentInfo.MimeType = mainDocument.MimeType;
                    documentInfo.Size = mainDocument.Size;
                    documentInfo.Name = mainDocument.Name;
                    documentInfo.Document = mainDocument.Document;

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void CleanAttachment(int transactionId)
        {
            var attachments = _oMCSDbContext.Attachments.Where(x => x.TransactionId == transactionId).ToList();
            if (attachments != null && attachments.Count > 0)
            {

                foreach (var attachment in attachments)
                {
                    _oMCSDbContext.Attachments.Remove(attachment);
                }
                _oMCSDbContext.SaveChanges();
            }

        }
        public void UpdateTransactionExternalCopyStatus(int transactionId, int value, int status)
        {
            try
            {
                FindBy(tr => tr.Id == transactionId).ExternalCopies.First(ec => ec.EntityId == value).Status = status;

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateTransactionExternalCopyStatusById(long transactionNumber, int transactionsCopyId, int unableToDeliver)
        {
            try
            {
                FindBy(tr => tr.Number == transactionNumber).ExternalCopies.First(ec => ec.Id == transactionsCopyId).Status = unableToDeliver;

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Transaction GetTransactionBasicInfo(int transactionId, int year, string cultureName)
        {
            try
            {
                Transaction transaction =
                    _oMCSDbContext.Transactions.Where(t => t.Id == transactionId & t.YearH == year & !t.IsDeleted).FirstOrDefault();

                if (transaction == null)
                {
                    return null;
                }

                Transaction result = new Transaction
                {
                    Attachments = transaction.Attachments,
                    Id = transaction.Id,
                    Date = transaction.Date,
                    DateH = transaction.DateH,
                    OutboundDraftId = transaction.OutboundDraftId,
                    Number = transaction.Number,
                    DocumentNumber = transaction.DocumentNumber,
                    Remarks = transaction.Remarks,
                    Subject = transaction.Subject,
                    RemindDate = transaction.RemindDate,
                    RemindDateH = transaction.RemindDateH,
                    IsSigned = transaction.IsSigned,
                    OutboundDraftEditorType = transaction.OutboundDraftEditorType,
                    POBox = transaction.POBox,
                    PostCode = transaction.PostCode,
                    IsForIndividual = transaction.IsForIndividual,
                    StatusId = transaction.StatusId,
                    InboundDateH = transaction.InboundDateH,
                    DeliveryMethodId = transaction.DeliveryMethodId,
                    Status = (transaction.Status != null) ? new Lookup
                    {
                        Id = transaction.StatusId,
                        Text = transaction.Status.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    DeliveryMethod = (transaction.DeliveryMethod != null) ? new Lookup
                    {
                        Id = transaction.DeliveryMethodId,
                        Text = transaction.DeliveryMethod.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    TransactionCategory = (transaction.TransactionCategory != null) ? new Lookup
                    {
                        Id = transaction.TransactionCategory.Id,
                        Text = transaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    SuggestedTopic = (transaction.SuggestedTopic != null) ? new SuggestedTopic
                    {
                        Id = transaction.SuggestedTopic.Id,
                        Text = transaction.SuggestedTopic.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    SubjectClassifications = transaction.SubjectClassifications.Select(ts => new TransactionSubjectClassification
                    {
                        Id = ts.Id,
                        TransactionId = ts.TransactionId,

                        SubjectClassification = (ts.SubjectClassification != null) ? new SubjectClassification
                        {
                            Id = ts.SubjectClassification.Id,
                            Text = ts.SubjectClassification.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                    }
                        ).ToList(),
                    SignedByUser = (transaction.SignedByUser != null) ? new UserProfile
                    {
                        Id = transaction.SignedByUser.Id,
                        LocalName = transaction.SignedByUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Entity = (transaction.Entity != null) ? new OrgUnit
                    {
                        Id = transaction.Entity.Id,
                        LocalName = transaction.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    ToUser = (transaction.ToUser != null) ? new UserProfile
                    {
                        Id = transaction.ToUser.Id,
                        LocalName = transaction.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Priority = (transaction.Priority != null) ? new Priority
                    {
                        Id = transaction.Priority.Id,
                        Text = transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Confidentiality = (transaction.Confidentiality != null) ? new Permission
                    {
                        Id = transaction.Confidentiality.Id,
                        LocalName = transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    TransactionType = (transaction.TransactionType != null) ? new TransactionType
                    {
                        Id = transaction.TransactionType.Id,
                        Text = transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    LetterType = (transaction.LetterType != null) ? new LetterType
                    {
                        Id = transaction.LetterType.Id,
                        Text = transaction.LetterType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    ExternalParty = (transaction.ExternalParty != null) ? new ExternalParty
                    {
                        Id = transaction.ExternalParty.Id,
                        Number = transaction.ExternalParty.Number,
                        LocalName = (transaction.ExternalParty.Name != null) ? transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : string.Empty,
                        LocalAddress = (transaction.ExternalParty.Address != null) ? transaction.ExternalParty.Address.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : string.Empty
                    } : null,

                    ExternalPartyManager = (transaction.ExternalPartyManager != null) ? new ExternalPartyManager
                    {
                        Id = transaction.ExternalPartyManager.Id,
                        LocalName = transaction.ExternalPartyManager.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    ProcessPeriodTransaction = transaction.ProcessPeriodTransaction.ToString() == null ? 0 : transaction.ProcessPeriodTransaction,
                };

                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<int> GetUserTasksTransactionsIds(int userId, int OrgUnitId)
        {
            DateTime currDateTime = DateTime.Now.Date.AddDays(-1).AddMinutes(1);
            int receivedId = (TaskStatus.Received.LookupIdentity(LookupCategory.TaskStatus, string.Empty));
            int sentId = (TaskStatus.Sent.LookupIdentity(LookupCategory.TaskStatus, string.Empty));

            return _oMCSDbContext.Tasks
                .Where(f => f.ToUserId == userId
                        & !f.IsDeleted
                        & f.ToOrgUnitId == OrgUnitId
                        & f.ParentId == null
                        & (f.StatusId == receivedId | f.StatusId == sentId)).OrderByDescending(f => f.Date)
               // & (currDateTime <= f.DeliveryDate))
               .Select(s => s.TransactionId).ToList();
        }
        public IList<int> GetELcOutBoundIds(int userId, int OrgUnitId)
        {

            return _oMCSDbContext.TransactionElcOutBounds
                       .Where(f => f.EntityId == OrgUnitId &
                        (f.UserId == userId || f.UserId == null) &
                        !f.Ishidden)
                        .Select(s => s.TransactionId).ToList();
        }
        public IList<int> GetSentTransactionsIds(int userId, int OrgUnitId)
        {
            int InProcess = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
            return _oMCSDbContext.TransactionAssignmentHistories
                       .Where(f =>
                           (f.FromEntityId == f.ToEntityId & f.FromUserId != f.ToUserId) | (f.FromEntityId != f.ToEntityId) &
                            f.FromEntityId == OrgUnitId &
                            f.FromUserId == userId).Select(s => s.TransactionId.Value).ToList();
        }
        public IList<int> GetSavedCopiesIds(int userId, int OrgUnitId)
        {
            int Viewed = TransCopyStatus.Viewed.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);
            return _oMCSDbContext.TransactionCopies
                       .Where(f => f.EntityId == OrgUnitId &
                        (f.UserId == userId || f.UserId == null) &
                        f.Status == Viewed)
                        .Select(s => s.TransactionId).ToList();
        }

        public IList<int> GetOutboundExternalIds(int userId, int OrgUnitId)
        {

            return _oMCSDbContext.TransactionElcOutBounds
                       .Where(f => f.EntityId == OrgUnitId &
                        (f.UserId == userId || f.UserId == null) &&
                        !f.Ishidden)
                        .Select(s => s.TransactionId).ToList();
        }
        public IList<int> GetOrgUnitIds(int userId, int OrgUnitId)
        {
            //if (ReceiveElcOutBoundWithAcknowled(OrgUnitId))
            //{

            //    return _oMCSDbContext.TransactionElcOutBounds
            //         .Where(f => f.EntityId == OrgUnitId &
            //          (f.UserId == userId | f.UserId == null) &
            //          f.Ishidden)
            //          .Select(s => s.TransactionId).ToList();

            //}
            //else
            //{

            return _oMCSDbContext.TransactionElcOutBounds
                 .Where(f => f.EntityId == OrgUnitId &
                  (f.UserId == userId | f.UserId == null) &
                  !f.Ishidden)
                  .Select(s => s.TransactionId).ToList();

            //}

        }

        #region FollowUp 
        public int TransactionFollowUpAdd(TransactionFollowUp oTransactionFollowUp)
        {
            var follow = _oMCSDbContext.TransactionFollowUps.
            FirstOrDefault(f => f.TransactionId == oTransactionFollowUp.TransactionId
            & f.Active == true
            & f.FollowUpEntityId == oTransactionFollowUp.FollowUpEntityId & f.FollowUpUserId == oTransactionFollowUp.FollowUpUserId);
            if (follow == null)
            {
                oTransactionFollowUp.CreatedOn = oTransactionFollowUp.CreationDate;
                oTransactionFollowUp.DateTo = oTransactionFollowUp.FollowUpExpireDate;
                oTransactionFollowUp.DateToH = oTransactionFollowUp.FollowUpExpireDateHj = DateTimeUtility.ConvertToUmAlQuraCalendar(oTransactionFollowUp.FollowUpExpireDate);
                _oMCSDbContext.TransactionFollowUps.Add(oTransactionFollowUp);
                _oMCSDbContext.SaveChanges();
                return oTransactionFollowUp.Id;
            }
            return 0;
        }



        public void TransactionFollowUpUpdate(TransactionFollowUp oTransactionFollowUp)
        {
            TransactionFollowUp followdb = GetFollowUpById(oTransactionFollowUp.Id);
            followdb.FollowUpExpireDate = oTransactionFollowUp.FollowUpExpireDate;
            followdb.FollowUpExpireDateHj = oTransactionFollowUp.FollowUpExpireDateHj;
            followdb.FollowUpMethodId = oTransactionFollowUp.FollowUpMethodId;
            followdb.FollowUpPriortyId = oTransactionFollowUp.FollowUpPriortyId;
            followdb.FollowUpProccessId = oTransactionFollowUp.FollowUpProccessId;
            followdb.FollowUpSourceId = oTransactionFollowUp.FollowUpSourceId;
            followdb.FollowUpReceiveDate = followdb.FollowUpReceiveDate;
            followdb.FollowUpExpireDateHj = oTransactionFollowUp.FollowUpExpireDateHj;
            followdb.FollowUpProgressId = oTransactionFollowUp.FollowUpProgressId;
            followdb.FollowUpProccessNote = oTransactionFollowUp.FollowUpProccessNote;
            followdb.FollowUpCompletionDate = oTransactionFollowUp.FollowUpCompletionDate;
            followdb.FollowUpCompletionDateHj = oTransactionFollowUp.FollowUpCompletionDateHj;
            followdb.FollowUpStatusId = oTransactionFollowUp.FollowUpStatusId;
            followdb.FollowUpReason = oTransactionFollowUp.FollowUpReason;
            followdb.HasChild = oTransactionFollowUp.HasChild;
            followdb.IsReminder = followdb.IsReminder;
            followdb.IsEscalated = followdb.IsEscalated;
            followdb.ParentId = oTransactionFollowUp.ParentId;
            _oMCSDbContext.SaveChanges();
        }
        public void ChangeStatusTransactionFollowUp(TransactionFollowUp oTransactionFollowUp)
        {
            TransactionFollowUp followdb = GetFollowUpById(oTransactionFollowUp.Id);

            followdb.FollowUpReason = oTransactionFollowUp.FollowUpReason;
            followdb.Active = oTransactionFollowUp.Active;
            _oMCSDbContext.SaveChanges();
        }

        public void UpdateTransactionFollowUps(int transactionId, IList<TransactionFollowUp> oTransactionFollowUp)
        {
            try
            {
                List<TransactionFollowUp> dbFollows = _oMCSDbContext.TransactionFollowUps.Where(t => t.TransactionId == transactionId).ToList();

                if (oTransactionFollowUp.Count == 0 && dbFollows.Count > 0)//delete all
                {
                    foreach (var item in dbFollows)
                    {
                        item.IsDeleted = true;
                        _oMCSDbContext.Entry(item).Property(x => x.IsDeleted).IsModified = true;
                    }
                    _oMCSDbContext.SaveChanges();
                    return;
                }

                foreach (var dbItem in dbFollows)
                {
                    int dbItemId = dbItem.Id;
                    if (!oTransactionFollowUp.Select(f => f.Id).ToList().Contains(dbItemId))
                    {
                        dbItem.IsDeleted = true;
                        _oMCSDbContext.Entry(dbItem).Property(x => x.IsDeleted).IsModified = true;
                        continue;
                    }
                }

                foreach (var uiItem in oTransactionFollowUp)
                {
                    if (uiItem.Id < 1)
                    {
                        uiItem.IsDeleted = false;
                        uiItem.TransactionId = transactionId;

                        if (uiItem.DateTo.HasValue)
                        {
                            uiItem.DateToH = DateTimeUtility.ConvertToUmAlQuraCalendar(uiItem.DateTo.Value);
                        }

                        _oMCSDbContext.Entry(uiItem).State = EntityState.Added;
                    }
                }
                _oMCSDbContext.SaveChanges();
                return;


            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public void FollowUpDetailsAdd(int transactionId, int orgUnitId, int userId, string note)
        {
            var follow = _oMCSDbContext.TransactionFollowUps.FirstOrDefault(f => f.FollowUpUserId == userId && f.TransactionId == transactionId);
            if (follow != null)
            {
                var details = new FollowUpDetails();
                details.TransactionFollowUpId = follow.Id;
                details.Notes = note;
                details.CreatedOn = DateTime.Now;
                details.CreatedBy = userId;
                _oMCSDbContext.FollowUpDetails.Add(details);
                _oMCSDbContext.SaveChanges();
            }
        }



        public void AddFollowupUditTrial(FollowUpAuditTrail followUpAuditTrail)
        {
            _oMCSDbContext.FollowUpAuditTrails.Add(followUpAuditTrail);
            _oMCSDbContext.SaveChanges();

        }
        public void FollowUpUpdateIsDeleted(int transactionId, int userId)
        {
            var followup = _oMCSDbContext.TransactionFollowUps.FirstOrDefault(t => (t.FollowUpUserId == userId || userId == -1) && t.TransactionId == transactionId);
            if (followup != null)
            {
                followup.IsDeleted = true;
                _oMCSDbContext.SaveChanges();
            }
        }
        public void FollowUpUpdateReceive(int Id, int userid)

        {
            TransactionFollowUp followdb = GetFollowUpById(Id);
            var followup = _oMCSDbContext.TransactionFollowUps.FirstOrDefault(t => t.Id == Id);
            if (followup != null)
            {
                followup.FollowUpReceiveDate = DateTime.Now;
                followup.FollowUpUserId = userid;
                _oMCSDbContext.SaveChanges();
            }
        }
        public void FollowUpChangeStatus(int Id, int FollowupStatus, bool IsActive)

        {
            var followup = _oMCSDbContext.TransactionFollowUps.FirstOrDefault(t => t.Id == Id);
            if (followup != null)
            {
                followup.FollowUpStatusId = FollowupStatus;
                followup.Active = IsActive;
                _oMCSDbContext.SaveChanges();
            }
        }
        public void FollowUpUpdateReminderStatus(int Id, bool IsReminder)

        {
            var followup = _oMCSDbContext.TransactionFollowUps.FirstOrDefault(t => t.Id == Id);
            if (followup != null)
            {
                followup.IsReminder = IsReminder;
                _oMCSDbContext.SaveChanges();
            }
        }
        public void FollowUpUpdateEscalatedStatus(int Id, bool IsEscalated)

        {
            var followup = _oMCSDbContext.TransactionFollowUps.FirstOrDefault(t => t.Id == Id);
            if (followup != null)
            {
                followup.IsEscalated = IsEscalated;
                _oMCSDbContext.SaveChanges();
            }
        }
        public void ReminderTransactionFollowUp(int FollowUpId)
        {
            var existFollow = _oMCSDbContext.TransactionFollowUps.Where(t => t.ParentId == FollowUpId).SingleOrDefault();
            existFollow.IsReminder = true;
            existFollow.IsImportant = true;
            _oMCSDbContext.SaveChanges();
        }
        public void EscalateTransactionFollowUp(int FollowUpId)
        {
            var existFollow = _oMCSDbContext.TransactionFollowUps.Where(t => t.ParentId == FollowUpId).SingleOrDefault();
            existFollow.IsEscalated = true;
            existFollow.IsImportant = true;
            _oMCSDbContext.SaveChanges();
        }
        public void FollowUpUpdateIsDeleted(int id)
        {
            var existFollow = _oMCSDbContext.TransactionFollowUps.Find(id);
            existFollow.Active = false;
            _oMCSDbContext.SaveChanges();
        }
        public TransactionFollowUp FollowUpDetailsByTransId(int transId, int FollowUpStatusId, int UserId, int OrgUnitId, string cultureName)
        {

            TransactionFollowUp follows = new TransactionFollowUp();
            if (FollowUpStatusId == (int)FollowupStatus.All)
            {
                follows = _oMCSDbContext.TransactionFollowUps.Where(t => t.TransactionId == transId & t.FollowUpEntityId == OrgUnitId & (t.FollowUpUserId == UserId || t.FollowUpUserId == null) & t.FollowUpStatusId != (int)FollowupStatus.Cancled & t.FollowUpStatusId != (int)FollowupStatus.Completed).SingleOrDefault();

            }
            else if (FollowUpStatusId == (int)FollowupStatus.Completed)
            {
                follows = _oMCSDbContext.TransactionFollowUps.Where(t => t.TransactionId == transId & t.FollowUpEntityId == OrgUnitId & (t.FollowUpUserId == UserId || t.FollowUpUserId == null) & (t.FollowUpStatusId == (int)FollowupStatus.Completed ||
                t.FollowUpStatusId == (int)FollowupStatus.EnsureComplition)).FirstOrDefault();

            }
            else
            {
                follows = _oMCSDbContext.TransactionFollowUps.Where(t => t.TransactionId == transId & t.FollowUpEntityId == OrgUnitId & (t.FollowUpUserId == UserId || t.FollowUpUserId == null) & (t.FollowUpStatusId == FollowUpStatusId)).FirstOrDefault();

            }
            return follows;
        }
        public TransactionFollowUp FollowUpDetailsByFollowUpId(int FollowUpId, string cultureName)
        {
            var follows = _oMCSDbContext.TransactionFollowUps.Where(t => t.Id == FollowUpId).SingleOrDefault();
            return follows;
        }
        public IList<FollowUpDetails> FollowUpDetailsById(int id, string cultureName)
        {
            var follows = _oMCSDbContext.FollowUpDetails
                                        .Where(t => t.TransactionFollowUpId == id)
                                        .Select(f => new
                                        {
                                            f.Id,
                                            f.Notes,
                                            f.CreatedOn,
                                            f.TransactionFollowUpId,
                                            f.TransactionFollowUp.FollowUpUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                        }).ToList().Select(f => new FollowUpDetails()
                                        {
                                            Id = f.Id,
                                            Notes = f.Notes,
                                            CreatedOn = f.CreatedOn,
                                            TransactionFollowUpId = f.TransactionFollowUpId,
                                            TransactionFollowUp = new TransactionFollowUp
                                            {
                                                Id = f.TransactionFollowUpId,
                                                FollowUpUser = new UserProfile { LocalName = f.Text },
                                            }
                                        }).OrderByDescending(t => t.CreatedOn).ToList();


            return follows;
        }
        public IList<FollowUpAuditTrail> GetListFollowupUditTrial(int id, string cultureName)
        {
            var follows = _oMCSDbContext.FollowUpAuditTrails.Where(t => t.FollowupId == id).Select(t => new
            {

                t.Id,
                t.FollowupId,
                t.ProccessDate,
                t.ProccessDescription,
                t.ProccessId,
                t.EntityId,
                t.Entity,
                t.UserId,
                t.User,
                EntityLocalName = t.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                UserLocalName = t.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
            }).ToList().Select(t => new FollowUpAuditTrail()
            {
                Id = t.Id,
                FollowupId = t.FollowupId,
                ProccessDate = t.ProccessDate,
                ProccessDescription = t.ProccessDescription,
                ProccessId = t.ProccessId,
                EntityId = t.EntityId,
                Entity = new OrgUnit
                {
                    Id = t.EntityId,
                    LocalName = t.EntityLocalName
                },
                UserId = t.UserId,
                User = new UserProfile
                {
                    Id = t.UserId,
                    LocalName = t.UserLocalName
                }

            }).ToList();


            return follows;
        }
        public IList<TransactionFollowUp> TransactionFollowUpSelect(int transId, string cultureName)
        {

            var follows = _oMCSDbContext.TransactionFollowUps
                                     .Where(t => t.TransactionId == transId)
                                     .Select(f => new
                                     {
                                         f.Id,
                                         f.FollowUpEntityId,
                                         EntityLocalName = f.FollowUpEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                         f.FollowUpUserId,
                                         UserLocalName = f.FollowUpUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                         f.DateTo,
                                         f.DateToH
                                     }).ToList().Select(f => new TransactionFollowUp()
                                     {
                                         Id = f.Id,
                                         FollowUpEntityId = f.FollowUpEntityId,
                                         FollowUpEntity = new OrgUnit
                                         {
                                             Id = f.FollowUpEntityId,
                                             LocalName = f.EntityLocalName
                                         },
                                         FollowUpUserId = f.FollowUpUserId,
                                         FollowUpUser = new UserProfile
                                         {
                                             Id = f.FollowUpUserId.HasValue ? f.FollowUpUserId.Value : 0,
                                             LocalName = f.UserLocalName
                                         },
                                         DateTo = f.DateTo,
                                         DateToH = f.DateToH,
                                     }).OrderByDescending(t => t.CreatedOn).ToList();

            return follows;
        }

        public IList<int> GetUserFollowUpTransactionsIds(int userId, int OrgUnitId)
        {
            return _oMCSDbContext.TransactionFollowUps
                .Where(f => f.FollowUpEntityId == OrgUnitId &
                (f.FollowUpStatusId == (int)FollowupStatus.New ||
                f.FollowUpStatusId == (int)FollowupStatus.UnLockFollowup) &
                !f.IsEscalated) // 
               .Select(s => s.TransactionId).ToList();
        }

        public IList<int> GetUserFollowProcessIds(int userId, int OrgUnitId)
        {

            return _oMCSDbContext.TransactionFollowUps
                .Where(f => f.FollowUpEntityId == OrgUnitId &
                f.FollowUpUserId == userId &
                (f.FollowUpStatusId == (int)FollowupStatus.UnderFollowup ||
                f.FollowUpStatusId == (int)FollowupStatus.UnderFollowupSecondLevel)
                & (DbFunctions.TruncateTime(f.FollowUpExpireDate) >= DbFunctions.TruncateTime(DateTime.Now)) & !f.IsEscalated
               )
               .Select(s => s.TransactionId).ToList();

            //&(f.FollowUpExpireDate - DateTime.Now).Days >= 0
        }
        public IList<int> GetUserFollowCompleteIds(int userId, int OrgUnitId)
        {

            return _oMCSDbContext.TransactionFollowUps
                .Where(f => f.FollowUpEntityId == OrgUnitId & f.FollowUpUserId == userId & (f.FollowUpStatusId == (int)FollowupStatus.Completed || f.FollowUpStatusId == (int)FollowupStatus.EnsureComplition))
               .Select(s => s.TransactionId).ToList();
        }
        public IList<int> GetUserFollowLateIds(int userId, int OrgUnitId)
        {

            return _oMCSDbContext.TransactionFollowUps
                .Where(f =>
                f.FollowUpEntityId == OrgUnitId &
                f.FollowUpUserId == userId &
                (f.FollowUpStatusId == (int)FollowupStatus.Delayed ||
                (DbFunctions.TruncateTime(f.FollowUpExpireDate) < DbFunctions.TruncateTime(DateTime.Now)))
                & f.FollowUpStatusId != (int)FollowupStatus.Cancled & f.FollowUpStatusId != (int)FollowupStatus.Completed & f.FollowUpStatusId != (int)FollowupStatus.EnsureComplition &
                !f.IsEscalated)
               .Select(s => s.TransactionId).ToList();


            //|| (f.FollowUpExpireDate - DateTime.Now).Days < 0
        }
        public IList<int> GetUserFollowDeleteIds(int userId, int OrgUnitId)
        {

            return _oMCSDbContext.TransactionFollowUps
                .Where(f => f.FollowUpEntityId == OrgUnitId & (f.FollowUpUserId == userId || f.FollowUpUserId == null) & f.FollowUpStatusId == (int)FollowupStatus.Cancled)
               .Select(s => s.TransactionId).ToList();
        }
        public IList<int> GetUserFollowUpEscalationIds(int userId, int OrgUnitId)
        {

            return _oMCSDbContext.TransactionFollowUps
                .Where(f => f.FollowUpEntityId == OrgUnitId & f.FollowUpUserId == userId & f.FollowUpStatusId != (int)FollowupStatus.EnsureComplition
                & f.FollowUpStatusId != (int)FollowupStatus.Completed
                & f.FollowUpStatusId != (int)FollowupStatus.Cancled & f.IsEscalated == true)
               .Select(s => s.TransactionId).ToList();
        }
        public IList<int> GetUserFollowReminderIds(int userId, int OrgUnitId)
        {
            DateTime currDateTime = DateTime.Now.Date.AddDays(-1).AddMinutes(1);

            return _oMCSDbContext.TransactionFollowUps
                .Where(f => f.FollowUpEntityId == OrgUnitId & f.FollowUpUserId == userId & f.FollowUpStatusId != (int)FollowupStatus.EnsureComplition
                & f.FollowUpStatusId != (int)FollowupStatus.Completed
                & f.FollowUpStatusId != (int)FollowupStatus.Cancled & f.IsReminder == true)
               .Select(s => s.TransactionId).ToList();
        }

        public IList<TransactionFollowUp> TransactionFollowUpSelectByFollowUpId(int followupid, string cultureName)
        {
            return _oMCSDbContext.TransactionFollowUps
                                     .Where(t => t.Id == followupid)
                                     .Select(f => new
                                     {
                                         f.Id,
                                         f.TransactionId,
                                         f.CreatingUser,
                                         f.CreatingEntityId,
                                         f.CreatingEntity,
                                         f.CreationDate,
                                         f.FollowUpProgressId,
                                         f.FollowUpExpireDate,
                                         f.FollowUpExpireDateHj,
                                         f.FollowUpCompletionDate,
                                         f.FollowUpCompletionDateHj,
                                         f.FollowUpStatusId,
                                         f.IsCopy,
                                         f.IsImportant,
                                         f.IsReminder,
                                         f.IsEscalated,
                                         f.HasChild,
                                         f.ParentId,
                                         f.FollowUpUserId,
                                         FollowUpUser = f.FollowUpUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                         f.FollowUpEntityId,
                                         FollowUpEntity = f.FollowUpEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                         EntityLocalName = f.CreatingEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                         f.CreatingUserId,
                                         UserLocalName = f.CreatingUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                         f.DateTo,
                                         f.DateToH
                                     }).ToList().Select(f => new TransactionFollowUp()
                                     {
                                         Id = f.Id,
                                         TransactionId = f.TransactionId,
                                         CreatingUserId = f.CreatingUserId,
                                         CreatingUser = new UserProfile
                                         {
                                             Id = f.CreatingUserId,
                                             LocalName = f.UserLocalName
                                         },
                                         CreatingEntityId = f.CreatingEntityId,
                                         CreatingEntity = new OrgUnit
                                         {
                                             Id = f.CreatingEntityId,
                                             LocalName = f.EntityLocalName
                                         },
                                         CreationDate = f.CreationDate,
                                         FollowUpProgressId = f.FollowUpProgressId,
                                         FollowUpExpireDate = f.FollowUpExpireDate,
                                         FollowUpExpireDateHj = f.FollowUpExpireDateHj,
                                         FollowUpCompletionDate = f.FollowUpCompletionDate,
                                         FollowUpCompletionDateHj = f.FollowUpCompletionDateHj,
                                         FollowUpStatusId = f.FollowUpStatusId,
                                         FollowUpUserId = f.FollowUpUserId,
                                         FollowUpUser = new UserProfile
                                         {
                                             Id = f.FollowUpUserId.HasValue ? f.FollowUpUserId.Value : 0,
                                             LocalName = f.FollowUpUser
                                         },
                                         FollowUpEntityId = f.FollowUpEntityId,
                                         FollowUpEntity = new OrgUnit
                                         {
                                             Id = f.FollowUpEntityId,
                                             LocalName = f.FollowUpEntity
                                         },
                                         DateTo = f.DateTo,
                                         DateToH = f.DateToH,
                                     }).OrderByDescending(t => t.CreatedOn).ToList();





        }



        public IList<TransactionFollowUp> TransactionFollowUpSelectByTransId(int transId, string cultureName)
        {
            return _oMCSDbContext.TransactionFollowUps
                                     .Where(t => t.TransactionId == transId)
                                     .Select(f => new
                                     {
                                         f.Id,
                                         f.TransactionId,

                                         f.CreatingUser,

                                         f.CreatingEntity,
                                         f.FollowUpTypeId,
                                         f.CreationDate,
                                         f.FollowUpExpireDate,
                                         f.FollowUpExpireDateHj,
                                         f.FollowUpProgressId,
                                         f.FollowUpCompletionDate,
                                         f.FollowUpCompletionDateHj,
                                         f.FollowUpStatusId,
                                         f.IsCopy,
                                         f.IsImportant,
                                         f.IsReminder,
                                         f.IsEscalated,
                                         f.HasChild,
                                         f.ParentId,
                                         f.FollowUpUserId,
                                         f.FollowUpReceiveDate,
                                         FollowUpUser = f.FollowUpUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                         f.FollowUpEntityId,
                                         FollowUpEntity = f.FollowUpEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                         f.CreatingEntityId,
                                         EntityLocalName = f.CreatingEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                         f.CreatingUserId,
                                         UserLocalName = f.CreatingUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                         f.DateTo,
                                         f.DateToH
                                     }).ToList().Select(f => new TransactionFollowUp()
                                     {
                                         Id = f.Id,
                                         TransactionId = f.TransactionId,
                                         CreatingUserId = f.CreatingUserId,
                                         CreatingUser = new UserProfile
                                         {
                                             Id = f.CreatingUserId,
                                             LocalName = f.UserLocalName
                                         },
                                         CreatingEntityId = f.CreatingEntityId,
                                         CreatingEntity = new OrgUnit
                                         {
                                             Id = f.CreatingEntityId,
                                             LocalName = f.EntityLocalName
                                         },
                                         FollowUpTypeId = f.FollowUpTypeId,
                                         CreationDate = f.CreationDate,
                                         FollowUpExpireDate = f.FollowUpExpireDate,
                                         FollowUpExpireDateHj = f.FollowUpExpireDateHj,
                                         FollowUpProgressId = f.FollowUpProgressId,
                                         FollowUpCompletionDate = f.FollowUpCompletionDate,
                                         FollowUpReceiveDate = f.FollowUpReceiveDate,
                                         FollowUpCompletionDateHj = f.FollowUpCompletionDateHj,
                                         FollowUpStatusId = f.FollowUpStatusId,
                                         IsCopy = f.IsCopy,
                                         IsImportant = f.IsImportant,
                                         IsReminder = f.IsReminder,
                                         IsEscalated = f.IsEscalated,
                                         HasChild = f.HasChild,
                                         ParentId = f.ParentId,
                                         FollowUpUserId = f.FollowUpUserId,
                                         FollowUpUser = new UserProfile
                                         {
                                             Id = f.FollowUpUserId.HasValue ? f.FollowUpUserId.Value : 0,
                                             LocalName = f.FollowUpUser
                                         },
                                         FollowUpEntityId = f.FollowUpEntityId,
                                         FollowUpEntity = new OrgUnit
                                         {
                                             Id = f.FollowUpEntityId,
                                             LocalName = f.FollowUpEntity
                                         },
                                         DateTo = f.DateTo,
                                         DateToH = f.DateToH,
                                     }).OrderByDescending(t => t.CreatedOn).ToList();





        }


        public IList<FollowUpAuditTrail> GetFollowUpAuditTrail(int followUpId, string cultureName)
        {
            return _oMCSDbContext.FollowUpAuditTrails
                                   .Where(t => t.FollowupId == followUpId)
                                   .Select(f => new
                                   {
                                       f.Id,
                                       f.FollowupId,
                                       f.ProccessDescription,
                                       f.ProccessDate,
                                       f.EntityId,
                                       EntityLocalName = f.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                       f.UserId,
                                       UserLocalName = f.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,

                                   }).ToList().Select(f => new FollowUpAuditTrail()
                                   {
                                       Id = f.Id,
                                       FollowupId = f.FollowupId,
                                       ProccessDescription = f.ProccessDescription,
                                       ProccessDate = f.ProccessDate,
                                       EntityId = f.EntityId,
                                       Entity = new OrgUnit
                                       {
                                           Id = f.EntityId,
                                           LocalName = f.EntityLocalName
                                       },
                                       UserId = f.UserId,
                                       User = new UserProfile
                                       {
                                           Id = f.UserId,
                                           LocalName = f.UserLocalName
                                       },

                                   }).OrderByDescending(t => t.ProccessDate).ToList();


        }

        public TransactionFollowUp GetFollowUpById(int id)
        {
            var followUp = _oMCSDbContext.TransactionFollowUps.Find(id);
            return followUp;
        }

        public bool CheckIfFollowUpAdd(int TransactionId)
        {
            try
            {
                int? followupid = _oMCSDbContext.TransactionFollowUps.Where(e => e.TransactionId == TransactionId && e.FollowUpStatusId != (int)FollowupStatus.Cancled && e.FollowUpStatusId != (int)FollowupStatus.Completed)?.FirstOrDefault()?.Id;

                if (followupid.HasValue)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public int? GetChildFollowUpUserId(int FollowUpId)
        {
            try
            {
                return _oMCSDbContext.TransactionFollowUps.Where(e => e.ParentId == FollowUpId).FirstOrDefault().FollowUpUserId;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        #endregion


        #region Auditing & Log
        public List<MainAudit> GetAuditByEntityName(int userId, int orgUnitId, int transactionId, string EntityName, string culture, AuditFor auditFor, bool IsForPrint, out int itemsCount, SearchCriteriaCustom searchCriteria = null)
        {
            string SpName = string.Empty;
            if (IsForPrint)
            {
                switch (auditFor)
                {
                    case AuditFor.MainDataAuditDetails:
                        SpName = "GET_MAIN_AUDIT_FOR_PRINT";
                        break;
                    default:
                        SpName = "GET_MA_FOR_PRINT_BY_TRANS_ID";
                        break;
                }
            }
            else
            {
                switch (auditFor)
                {
                    case AuditFor.MainDataAuditDetails:
                        SpName = "GET_MAIN_AUDIT";
                        break;
                    case AuditFor.AssignmentAuditDetails:
                        SpName = "";
                        break;
                    case AuditFor.AttachmentsAuditDetails:
                    case AuditFor.NamesAuditDetails:
                    case AuditFor.ExplanationsAuditDetails:
                    case AuditFor.Copies:
                    case AuditFor.ExternalCopies:
                    case AuditFor.Tasks:
                    case AuditFor.FollowUp:
                    case AuditFor.Links:
                        SpName = "GET_MAIN_AUDIT_BY_TRANS_ID";
                        break;
                    case AuditFor.DocumentInfoAuditDetails:
                        SpName = ""; // doesn't has TransactionId Column 
                        break;
                    case AuditFor.Print:
                        SpName = "GET_MAIN_AUDIT_FOR_PRINT";
                        break;
                    default:
                        break;
                }
            }
            return GetAuditTrail(transactionId, EntityName, SpName, culture, out itemsCount, searchCriteria).ToList();
        }
        public List<AuditDetails> GetEntityAuditing(AuditFor auditFor, int auditId, string PropName, string culture)
        {
            try
            {
                string SpName = string.Empty;
                switch (auditFor)
                {
                    case AuditFor.MainDataAuditDetails:
                        SpName = "GET_MAINDATA_AUDITDETAILS";
                        break;
                    case AuditFor.AssignmentAuditDetails:
                        SpName = "";
                        break;
                    case AuditFor.AttachmentsAuditDetails:
                        SpName = "GET_Attachment_AUDITDETAILS";
                        break;
                    case AuditFor.NamesAuditDetails:
                        SpName = "GET_Name_AUDITDETAILS";
                        break;
                    case AuditFor.ExplanationsAuditDetails:
                        SpName = "GET_Explanation_AUDITDETAILS";
                        break;
                    case AuditFor.DocumentInfoAuditDetails:
                        SpName = "GET_DocumentInfo_AUDITDETAILS";
                        break;
                    case AuditFor.Copies:
                        SpName = "GET_IC_AUDITDETAILS";
                        break;
                    case AuditFor.ExternalCopies:
                        SpName = "GET_EC_AUDITDETAILS";
                        break;
                    case AuditFor.Links:
                        SpName = "";
                        break;
                    case AuditFor.Tasks:
                        SpName = "GET_TASK_AUDITDETAILS";
                        break;
                    case AuditFor.FollowUp:
                        SpName = "GET_FOLLOWUP_AUDITDETAILS";
                        break;
                    default:
                        break;
                }

                return GetMainTransactionAuditDetails(auditId, SpName, PropName, culture); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<AuditDetails> GetMainTransactionAuditDetails(int auditId, string SpName, string PropName, string culture)
        {
            List<AuditDetails> auditList = null;
            if (SystemConfigurations.IsOracleMigrationEnabled)
            {
                OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, SystemDataForSPs.ParameterDirection.Output);
                auditList = _oMCSDbContext.Database.SqlQuery<AuditDetails>(
                    "BEGIN " + SpName + "(:p_AuditId,:p_PropName, :p_CultureName, :p_Cur); END;",
                    new OracleParameter(":p_AuditId", OracleDbType.Int32, auditId, SystemDataForSPs.ParameterDirection.Input),
                    new OracleParameter(":p_PropName", OracleDbType.NVarchar2, PropName, SystemDataForSPs.ParameterDirection.Input),
                    new OracleParameter(":p_CultureName", OracleDbType.NVarchar2, culture, SystemDataForSPs.ParameterDirection.Input),
                    orcOutParam
                    ).ToList();
            }
            else
            {
                _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                auditList = _oMCSDbContext.Database.SqlQuery<AuditDetails>(
                    SpName + " @AuditId, @CultureName",
                new SqlParameter("AuditId", auditId),
                new SqlParameter("CultureName", culture)
                ).ToList();
            }
            return auditList;
        }
        private IList<MainAudit> GetAuditTrail(int transactionId, string EntityName, string SpName, string culture, out int itemsCount, SearchCriteriaCustom searchCriteria)
        {
            try
            {
                int page = 0;
                int pageSize = 10;
                string OrderBy = "Id";
                int Ascending = 0;
                IList<MainAudit> auditList = null;
                string PropName = "none";
                int AuditType = -1;
                DateTime p_AuditDateFrom = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime p_AuditDateTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                int UserId = -1;
                if (searchCriteria != null)
                {
                    if (searchCriteria.Filters != null)
                    {

                        foreach (var item in searchCriteria.Filters)
                        {
                            if (item.ColumnName == "PropName")
                            {
                                PropName = item.Value;
                            }
                            else if (item.ColumnName == "AuditType")
                            {
                                AuditType = Convert.ToInt32(item.Value);
                            }
                            else if (item.ColumnName == "AuditDateFrom")
                            {
                                p_AuditDateFrom = DateTime.ParseExact(item.Value, "dd/MM/yyyy", null);
                            }
                            else if (item.ColumnName == "AuditDateTo")
                            {
                                p_AuditDateTo = DateTime.ParseExact(item.Value, "dd/MM/yyyy", null);
                            }
                            else if (item.ColumnName == "EditedByUserName")
                            {
                                UserId = Convert.ToInt32(item.Value);
                            }
                        }
                    }
                    page = searchCriteria.PageIndex - 1;
                    pageSize = searchCriteria.PageSize;
                    OrderBy = searchCriteria.OrderBy;
                    Ascending = Convert.ToInt32(searchCriteria.Ascending);
                }

                if (SystemConfigurations.IsOracleMigrationEnabled)
                {
                    OracleParameter orcOutParam = new OracleParameter(":p_cur", OracleDbType.RefCursor, SystemDataForSPs.ParameterDirection.Output);
                    OracleParameter orcItemsCount = new OracleParameter(":p_TotalCount", OracleDbType.Int32, SystemDataForSPs.ParameterDirection.Output);
                    auditList = _oMCSDbContext.Database.SqlQuery<MainAudit>(
                        "BEGIN " + SpName + " (:p_PrimaryKey, :p_EntityName, :p_CultureName, :p_PropName, :p_AuditType, :p_AuditDateFrom,:p_AuditDateTo,:p_UserName,:p_PageIndex ,:p_PageSize,:p_OrderBy,:p_Ascending, :p_Cur, :p_TotalCount); END;",
                        new OracleParameter(":p_PrimaryKey", OracleDbType.Int32, transactionId, SystemDataForSPs.ParameterDirection.Input),
                        new OracleParameter(":p_EntityName", OracleDbType.NVarchar2, EntityName, SystemDataForSPs.ParameterDirection.Input),
                        new OracleParameter(":p_CultureName", OracleDbType.NVarchar2, culture, SystemDataForSPs.ParameterDirection.Input),
                        new OracleParameter(":p_PropName", OracleDbType.NVarchar2, PropName, SystemDataForSPs.ParameterDirection.Input),
                        new OracleParameter(":p_AuditType", OracleDbType.Int32, AuditType, SystemDataForSPs.ParameterDirection.Input),
                        new OracleParameter(":p_AuditDateFrom", OracleDbType.Date, p_AuditDateFrom, SystemDataForSPs.ParameterDirection.Input),
                        new OracleParameter(":p_AuditDateTo", OracleDbType.Date, p_AuditDateTo, SystemDataForSPs.ParameterDirection.Input),
                        new OracleParameter(":p_UserId", OracleDbType.Int32, UserId, SystemDataForSPs.ParameterDirection.Input),
                        new OracleParameter(":p_PageIndex", OracleDbType.Int32, page, SystemDataForSPs.ParameterDirection.Input),
                        new OracleParameter(":p_PageSize", OracleDbType.Int32, pageSize, SystemDataForSPs.ParameterDirection.Input),
                        new OracleParameter(":p_OrderBy", OracleDbType.NVarchar2, OrderBy, SystemDataForSPs.ParameterDirection.Input),
                        new OracleParameter(":p_Ascending", OracleDbType.Int32, Ascending, SystemDataForSPs.ParameterDirection.Input),
                        orcOutParam,
                        orcItemsCount
                        ).ToList();
                    itemsCount = Convert.ToInt32(orcItemsCount.Value.ToString());
                }
                else
                {
                    if (SpName == "GET_MA_FOR_PRINT_BY_TRANS_ID")
                    {
                        SqlParameter sqlPTotalCount = new SqlParameter("TotalCount", 0);
                        sqlPTotalCount.Direction = SystemDataForSPs.ParameterDirection.Output;

                        _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                        auditList = _oMCSDbContext.Database.SqlQuery<MainAudit>(
                            SpName + " @PrimaryKey, @EntityName, @CultureName, @PropName, @AuditType, @AuditDateFrom, @AuditDateTo, " +
                            "@UserId, @PageIndex, @PageSize, @OrderBy, @Ascending, @TotalCount",
                        new SqlParameter("PrimaryKey", transactionId),
                        new SqlParameter("EntityName", EntityName),
                        new SqlParameter("PropName", PropName),
                        new SqlParameter("AuditType", AuditType),
                        new SqlParameter("AuditDateFrom", p_AuditDateFrom),
                        new SqlParameter("AuditDateTo", p_AuditDateTo),
                        new SqlParameter("UserId", UserId),
                        new SqlParameter("PageIndex", page),
                        new SqlParameter("PageSize", pageSize),
                        new SqlParameter("OrderBy", OrderBy),
                        new SqlParameter("Ascending", Ascending),
                        new SqlParameter("CultureName", culture),
                        sqlPTotalCount
                        ).ToList();
                        itemsCount = 0;
                    }
                    else
                    {
                        _oMCSDbContext.Database.ExecuteSqlCommand("SET ARITHABORT ON;");
                        auditList = _oMCSDbContext.Database.SqlQuery<MainAudit>(
                            SpName + " @PrimaryKey, @EntityName, @CultureName",
                        new SqlParameter("PrimaryKey", transactionId),
                        new SqlParameter("EntityName", EntityName),
                        new SqlParameter("CultureName", culture)
                        ).ToList();
                        itemsCount = 0;
                    }

                }

                return auditList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public int AddTransactionReservation(TransactionReservation transactionReservation)
        {
            try
            {

                _oMCSDbContext.TransactionReservations.Add(transactionReservation);
                _oMCSDbContext.SaveChanges();

                return transactionReservation.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<TransactionReservation> GetTransactionReservations(int? orgUnitId, int? userId, SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<TransactionReservation> transactionReservations =
                    _oMCSDbContext.TransactionReservations
                                  .Where(p => (userId == null || p.UserId == userId) &&
                                            (orgUnitId == null || p.EntityId == orgUnitId))
                                  .AsQueryable();

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        if (filter.ColumnName == "TransactionType")
                        {
                            transactionReservations = FilterReservationByTransactionCategory(transactionReservations, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "EntityId")
                        {
                            transactionReservations = FilterReservationByEntityId(transactionReservations, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else if (filter.ColumnName == "UserId")
                        {
                            transactionReservations = FilterReservationByUserId(transactionReservations, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                    }
                }

                rowsCount = transactionReservations.Count();

                transactionReservations = transactionReservations.OrderByDescending(d => d.Id).Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                   .Take(searchCriteria.PageSize);

                return transactionReservations.ToList().Select(u => new TransactionReservation
                {
                    Id = u.Id,
                    Count = u.Count,
                    EntityId = u.EntityId,
                    UserId = u.UserId,
                    Reason = u.Reason,
                    TransactionCategoryId = u.TransactionCategoryId,
                    CreatedOn = u.CreatedOn,
                    User = new UserProfile
                    {
                        Id = u.User.Id,
                        LocalName = u.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    },
                    Entity = new OrgUnit
                    {
                        Id = u.Entity.Id,
                        LocalName = u.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    },
                    TransactionCategory = new Lookup
                    {
                        Id = u.TransactionCategory.Id,
                        Text = u.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == searchCriteria.CultureName).LocalText()
                    },
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<Transaction> GetReservedTransaction(int reservationId)
        {
            try
            {
                var transactionReservations =
                    _oMCSDbContext.Transactions
                                  .Where(p => (p.ReservationId == reservationId))
                                  .Select(r => new
                                  {
                                      r.Id,
                                      r.Number,
                                      r.YearH,
                                      r.TransactionCategory
                                  }).ToList()
                                  .Select(t => new Transaction
                                  {
                                      Id = t.Id,
                                      Number = t.Number,
                                      Year = t.YearH,
                                      TransactionCategory = t.TransactionCategory
                                  }
                                  ).ToList();

                return transactionReservations;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Transaction> GetTransactionsByNumber(string Number, int inquiryType, int yearH, int? DestinationId, string subject, int userId, int entityId)
        {
            try
            {
                _oMCSDbContext.Configuration.UseDatabaseNullSemantics = true;

                IQueryable<TransactionAssignmentHistory> transactionAssignment = _oMCSDbContext.TransactionAssignmentHistories.Where(x => (x.FromEntityId == entityId && x.FromUserId == userId) || (x.ToEntityId == entityId && x.ToUserId == userId));
                switch (inquiryType)
                {
                    case (int)InquiryType.TransactionNumber:
                        {
                            long number = Convert.ToInt64(Number);
                            transactionAssignment = transactionAssignment.Where(x => x.Transaction.Number == number);
                            break;
                        }

                    case (int)InquiryType.InboundDocumentNumber:
                        {
                            transactionAssignment = transactionAssignment.Where(x => x.Transaction.DocumentNumber.Equals(Number));

                            if (!transactionAssignment.Any())
                            {
                                transactionAssignment = transactionAssignment.Where(x => x.Transaction.DocumentNumber.Contains(Number));
                            }

                            break;
                        }

                    case (int)InquiryType.Name:
                        {
                            transactionAssignment = transactionAssignment.Where(x => x.Transaction.Name.FirstName.Contains(Number));
                            break;
                        }

                    case (int)InquiryType.SubjectSearch:
                        {
                            transactionAssignment = transactionAssignment.Where(x => x.Transaction.Subject.Contains(Number) && x.Transaction.YearH == yearH);
                            break;
                        }

                    default:
                        {
                            transactionAssignment = transactionAssignment.Where(x => x.Transaction.Subject.Contains(Number) && x.Transaction.YearH == yearH);
                            break;
                        }
                }

                List<int> ids = transactionAssignment.Where(x => x.TransactionId.HasValue && x.TransactionId.Value > 0).OrderByDescending(x => x.Id).Take(100).Select(x => x.TransactionId.Value).Distinct().ToList();

                //return _oMCSDbContext.Transactions.Where(t => ids.Any(trId => trId == t.Id)).Select(t => new Transaction
                //{
                //    Id = t.Id,
                //    Number = t.Number,
                //    Status = t.Status,
                //    Subject = t.Subject,
                //    Assignments = t.Assignments,
                //    TransactionType = t.TransactionType,
                //    TransactionTypeId = t.TransactionTypeId,
                //    User = t.User,
                //    Date = t.Date,
                //    ConfidentialityId = t.ConfidentialityId,
                //    Confidentiality = new Permission
                //    {
                //        Weight = t.Confidentiality.Weight
                //    }
                //}).ToList();
                return _oMCSDbContext.Transactions.Where(t => ids.Any(trId => trId == t.Id)).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
            finally
            {
                _oMCSDbContext.Configuration.UseDatabaseNullSemantics = false;
            }
        }

        public TransactionFollowUp GetFollowUpByTransactionIdAndUserId(int transactionId, int userId)
        {
            //var followup = _oMCSDbContext.TransactionFollowUps.FirstOrDefault(t => (t.UserId == userId || userId == -1) && t.TransactionId == transactionId);
            //return followup;
            return null;
        }

        public void UpdateTransactionSubject(int transactionId, string newSubject)
        {
            try
            {
                Transaction transaction = _oMCSDbContext.Transactions.Where(t => t.Id == transactionId).FirstOrDefault();
                transaction.Subject = newSubject;
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }


        }
        private IQueryable<TransactionReservation> FilterReservationByTransactionCategory(IQueryable<TransactionReservation> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != string.Empty)
            {
                var TransactionCategoryId = Convert.ToInt32(textValue);
                return source.Where(p => p.Transactions.Any(t => t.TransactionCategoryId == TransactionCategoryId));
            }

            return source;
        }
        private IQueryable<TransactionReservation> FilterReservationByUserId(IQueryable<TransactionReservation> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != string.Empty)
            {
                var UserId = Convert.ToInt32(textValue);
                return source.Where(p => p.UserId == UserId);
            }

            return source;
        }
        private IQueryable<TransactionReservation> FilterReservationByEntityId(IQueryable<TransactionReservation> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != string.Empty)
            {
                var EntityId = Convert.ToInt32(textValue);
                return source.Where(p => p.EntityId == EntityId);
            }

            return source;
        }

        public List<ReleaseNote> ReleaaseNotesUsersSelect(int userId)
        {
            var userReleaseNotes = _oMCSDbContext.ReleaseNotesUsers.Where(r => r.UserId == userId).Select(r => r.ReleaseNoteId);

            return _oMCSDbContext.ReleaseNotes.Where(r => r.IsActive && !userReleaseNotes.Any(ur => ur == r.Id)).OrderBy(r => r.ReleaseDate).ToList();
        }

        public void ReleaaseNotesUsersAdd(int userId)
        {
            var userReleaseNotes = _oMCSDbContext.ReleaseNotesUsers.Where(r => r.UserId == userId).Select(r => r.ReleaseNoteId);
            foreach (var item in _oMCSDbContext.ReleaseNotes.Where(r => r.IsActive && !userReleaseNotes.Contains(r.Id)))
            {
                _oMCSDbContext.ReleaseNotesUsers.Add(
              new ReleaseNotesUser
              {
                  UserId = userId,
                  ReleaseNoteId = item.Id
              });
            }

            _oMCSDbContext.SaveChanges();
        }
        public void UpdateOldWordTransaction(int transactionId, string oldWordConent)
        {
            var transaction = _oMCSDbContext.Transactions.Where(x => x.Id == transactionId).FirstOrDefault();

        }

        public void SetViewedTransactionCopy(int transactionCopyId, int userId)
        {
            var transactionCopy = _oMCSDbContext.TransactionCopies.Where(x => x.Id == transactionCopyId).AsNoTracking().FirstOrDefault();
            if (transactionCopy.ViewedOnDate == null && string.IsNullOrWhiteSpace(transactionCopy.ViewedOnDateH) && !transactionCopy.ViewedById.HasValue)
            {
                var datetime = DateTime.Now;
                transactionCopy.ViewedOnDate = datetime;
                transactionCopy.ViewedOnDateH = DateTimeUtility.ConvertToUmAlQuraCalendarViewedFormat(datetime);
                transactionCopy.ViewedById = userId;
                _oMCSDbContext.Entry(transactionCopy).State = EntityState.Modified;
                _oMCSDbContext.SaveChanges();
            }

        }
        #region TransactionEncryption
        public void AddTransactionEncryptionCode(TransactionEncryptionCode transactionEncryptionCode)
        {
            var transactionCodes = _oMCSDbContext.TransactionEncryptionCodes.FirstOrDefault(f => f.TransactionId == transactionEncryptionCode.TransactionId);
            if (transactionCodes == null)
            {
                _oMCSDbContext.TransactionEncryptionCodes.Add(transactionEncryptionCode);
                _oMCSDbContext.SaveChanges();
            }
            else
            {

                transactionCodes.UserId = transactionEncryptionCode.UserId;
                transactionCodes.OrgUnitId = transactionEncryptionCode.OrgUnitId;
                transactionCodes.EncryptionChannel = transactionEncryptionCode.EncryptionChannel;
                transactionCodes.Code = transactionEncryptionCode.Code;
                transactionCodes.ModefiedBy = transactionEncryptionCode.ModefiedBy;
                transactionCodes.ModefiedOn = transactionEncryptionCode.ModefiedOn;

            }
        }

        #endregion
        #region MobileApi
        public void DeletedTransaction(int transId)
        {
            try
            {
                Transaction transaction = GetTransactionById(transId);
                transaction.MainDocumentId = null;
                transaction.IsDeleted = true;

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void UserMobileUpdateTransactionStatus(int transId, int statusId, string reason)
        {
            Transaction transaction = GetTransactionById(transId);
            transaction.StatusId = statusId;
            transaction.SavedReason = reason;

            _oMCSDbContext.SaveChanges();
        }

        public Transaction GetUserMobileTransaction(int transId, string cultureName)
        {
            try
            {
                Transaction transaction = _oMCSDbContext.Transactions
                                                        .Where(t => t.Id == transId)
                                                        .FirstOrDefault();

                if (transaction == null || transaction.IsDeleted)
                {
                    return null;
                }

                Transaction resultTransaction = new Transaction
                {
                    Id = transaction.Id,
                    CreatedBy = transaction.CreatedBy,
                    Date = transaction.Date,
                    DateH = transaction.DateH,
                    Status = new Lookup
                    {
                        Id = transaction.Status.Id,
                        Text = transaction.Status.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()

                    },
                    TransactionCategory = transaction.TransactionCategory,
                    TransactionCategoryId = transaction.TransactionCategoryId,
                    OrgUnitId = transaction.OrgUnitId,
                    ToUserId = transaction.ToUserId,
                    EntityId = transaction.EntityId,
                    ExternalPartyId = transaction.ExternalPartyId,
                    Names = transaction.Names,
                    Number = transaction.Number,
                    DocumentNumber = transaction.DocumentNumber,
                    Remarks = transaction.Remarks,
                    Subject = transaction.Subject,
                    PrintedDeliveryReport = transaction.PrintedDeliveryReport,
                    DeliveryReportNumber = transaction.DeliveryReportNumber,
                    MainDocument = transaction.MainDocument,
                    RemindDate = transaction.RemindDate,
                    RemindDateH = transaction.RemindDateH,
                    OutboundDraftEditorType = transaction.OutboundDraftEditorType,
                    IsSigned = transaction.IsSigned,
                    OutboundDraftId = transaction.OutboundDraftId,
                    DeliveryMethodId = transaction.DeliveryMethodId,
                    InboundDateH = transaction.InboundDateH,
                    IsDraft = transaction.IsDraft,
                    ExternalPartyManagerId = transaction.ExternalPartyManagerId,
                    LetterTypeId = transaction.LetterTypeId,
                    RejectionReason = transaction.RejectionReason,
                    Year = transaction.Year,
                    YearH = transaction.YearH,
                    TransactionTypeId = transaction.TransactionTypeId,
                    SuggestedTopicId = transaction.SuggestedTopicId,
                    UserId = transaction.UserId,
                    SignedByUserId = transaction.SignedByUserId,
                    PostCode = transaction.PostCode,
                    POBox = transaction.POBox,
                    PriorityId = transaction.PriorityId,
                    StatusId = transaction.StatusId,
                    MainDocumentId = transaction.MainDocumentId,
                    ConfidentialityId = transaction.ConfidentialityId,
                    IsForIndividual = transaction.IsForIndividual,
                    ReporterId = transaction.ReporterId,
                    DeliveryNumber = transaction.DeliveryNumber,
                    InboundIntendedPerson = transaction.InboundIntendedPerson,
                    IsPresentationDraft = transaction.IsPresentationDraft,
                    PresentationDraftNumber = transaction.PresentationDraftNumber,
                    OutBoundDraftNumber = transaction.OutBoundDraftNumber,
                    IsElcOutBound = transaction.IsElcOutBound,
                    NeedAcknowled = transaction.NeedAcknowled,
                    OldWordDocumntId = transaction.OldWordDocumntId,
                    DeliveryMethod = (transaction.DeliveryMethod != null) ? new Lookup
                    {
                        Id = transaction.DeliveryMethodId,
                        Text = transaction.DeliveryMethod.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    SuggestedTopic = (transaction.SuggestedTopic != null) ? new SuggestedTopic
                    {
                        Id = transaction.SuggestedTopic.Id,
                        Text = transaction.SuggestedTopic.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()

                    } : null,

                    SignedByUser = (transaction.SignedByUser != null) ? new UserProfile
                    {
                        Id = transaction.SignedByUser.Id,
                        LocalName = transaction.SignedByUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()

                    } : null,

                    User = (transaction.User != null) ? new UserProfile
                    {
                        Id = transaction.User.Id,
                        LocalName = transaction.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    OrgUnit = (transaction.OrgUnit != null) ? new OrgUnit
                    {
                        Id = transaction.OrgUnit.Id,
                        LocalName = transaction.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Assignments = transaction.Assignments.Select(a => new TransactionAssignment
                    {
                        Description = a.Description,
                        Date = a.Date,
                        DateH = a.DateH,
                        Id = a.Id,
                        TransactionPathId = a.TransactionPathId,
                        CurrentPathStep = a.CurrentPathStep,
                        Tray = (a.Tray != null) ? new Tray
                        {
                            Id = a.Tray.Id,
                            LocalName = a.Tray.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        ToUser = (a.ToUser != null) ? new UserProfile
                        {
                            Id = a.ToUser.Id,
                            LocalName = a.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        Transaction = (a.Transaction != null) ? new Transaction
                        {
                            Id = a.Transaction.Id
                        } : null,

                        Action = (a.Action != null) ? new Action
                        {
                            Id = a.Action.Id,
                            LocalName = a.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                            Type = a.Action.Type
                        } : null,

                        FromEntity = (a.FromEntity != null) ? new OrgUnit
                        {
                            Id = a.FromEntity.Id,
                            LocalName = a.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        ToEntity = (a.ToEntity != null) ? new OrgUnit
                        {
                            Id = a.ToEntity.Id,
                            LocalName = a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        PhysicalEntity = (a.PhysicalEntity != null) ? new OrgUnit
                        {
                            Id = a.PhysicalEntity.Id,
                            LocalName = a.PhysicalEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        PhysicalUser = (a.PhysicalUser != null) ? new UserProfile
                        {
                            Id = a.PhysicalUser.Id,
                            LocalName = a.PhysicalUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null
                    }
                    ).ToList(),

                    Copies = transaction.Copies.Select(c => new TransactionCopy
                    {
                        Id = c.Id,
                        Date = c.Date,
                        DateH = c.DateH,
                        ActionId = c.ActionId,
                        TransactionId = c.TransactionId,
                        UserId = c.UserId,
                        Status = c.Status,
                        IsSent = c.IsSent,
                        Action = (c.Action != null) ? new Action
                        {
                            Id = c.Action.Id,
                            LocalName = c.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                            Type = c.Action.Type
                        } : null,

                        User = (c.User != null) ? new UserProfile
                        {
                            Id = c.User.Id,
                            LocalName = c.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        Entity = (c.Entity != null) ? new OrgUnit
                        {
                            Id = c.Entity.Id,
                            LocalName = c.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null
                    }
                    ).ToList(),

                    ExternalCopies = transaction.ExternalCopies.Select(c => new TransactionExternalCopy
                    {
                        Id = c.Id,
                        Date = c.Date,
                        DateH = c.DateH,
                        ActionId = c.ActionId,
                        TransactionId = c.TransactionId,
                        UserId = c.UserId,
                        Viewed = c.Viewed,
                        Action = (c.Action != null) ? new Action
                        {
                            Id = c.Action.Id,
                            LocalName = c.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        User = (c.User != null) ? new ExternalPartyManager
                        {
                            Id = c.User.Id,
                            LocalName = c.User.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        Entity = (c.Entity != null) ? new ExternalParty
                        {
                            Id = c.Entity.Id,
                            LocalName = c.Entity.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                            YasserRegistered = c.Entity.YasserRegistered
                        } : null,
                        EntityId = c.EntityId,

                        Status = c.Status,
                        SendEmail = c.SendEmail,
                        ExternalPartyAttachment = c.ExternalPartyAttachment.ToList().Select(o => new ExternalPartyAttachment
                        {
                            Id = o.Id,
                            PartyId = o.PartyId,
                            Name = o.Name,
                            DocumentInfo = new DocumentInfo
                            {
                                Document = new Document
                                {
                                    Id = o.DocumentInfo.Document.Id,
                                    Content = o.DocumentInfo.Document.Content
                                },

                                Id = o.DocumentInfo.Id,
                                MimeType = o.DocumentInfo.MimeType,
                                Name = o.DocumentInfo.Name,
                                Size = o.DocumentInfo.Size,
                                IsDeleted = o.DocumentInfo.IsDeleted,
                                ECMId = o.DocumentInfo.ECMId
                            },

                        }).ToList(),

                    }
                    ).ToList(),

                    Attachments = transaction.Attachments.Select(a => new Attachment
                    {
                        DocumentInfo = (a.DocumentInfo != null) ? new DocumentInfo
                        {
                            Document = (a.DocumentInfo.Document != null) ? new Document
                            {
                                Id = a.DocumentInfo.Document.Id
                            } : null,

                            Id = a.DocumentInfo.Id,
                            MimeType = a.DocumentInfo.MimeType,
                            Name = a.DocumentInfo.Name,
                            Size = a.DocumentInfo.Size,
                            CreatedBy = a.DocumentInfo.CreatedBy,
                            CreatedOn = a.DocumentInfo.CreatedOn,
                            ModefiedBy = a.DocumentInfo.ModefiedBy,
                            ModefiedOn = a.DocumentInfo.ModefiedOn
                        } : null,

                        Type = (a.Type != null) ? new AttachmentType
                        {
                            Archivable = a.Type.Archivable,
                            Id = a.Type.Id,
                            Text = a.Type.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        Description = a.Description,
                        Count = a.Count,
                        Id = a.Id,
                        TypeId = a.TypeId,
                        AttachmentSource = a.AttachmentSource,
                        CreatedBy = a.CreatedBy,
                        CreatedOn = a.CreatedOn,
                        ModefiedBy = a.ModefiedBy,
                        ModefiedOn = a.ModefiedOn
                    }).ToList(),
                    Explanations = transaction.Explanations.Select(e => new Explanation
                    {
                        Document = (e.Document != null) ? new DocumentInfo
                        {
                            Document = (e.Document.Document != null) ? new Document
                            {
                                Id = e.Document.Document.Id,
                            } : null,

                            Id = e.Document.Id,
                            MimeType = e.Document.MimeType,
                            Name = e.Document.Name,
                            Size = e.Document.Size,
                            CreatedBy = e.Document.CreatedBy,
                            CreatedOn = e.Document.CreatedOn,
                            ModefiedBy = e.Document.ModefiedBy,
                            ModefiedOn = e.Document.ModefiedOn

                        } : null,


                        Permission = (e.Permission != null) ? new Permission
                        {
                            Id = e.Permission.Id,
                            LocalName = e.Permission.Name.Localizations.LocalText(),
                            Code = e.Permission.Code
                        } : null,
                        Id = e.Id,
                        FromUser = (e.FromUser != null) ? new UserProfile
                        {
                            Id = e.FromUser.Id,
                            LocalName = e.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        CanBeDeleted = e.CanBeDeleted,
                        isCopies = e.isCopies,
                        ExplanationEditorType = e.ExplanationEditorType,
                        Date = e.Date,
                        DateH = e.DateH,
                        TransactionId = e.TransactionId,
                        CreatedBy = e.CreatedBy,
                        CreatedOn = e.CreatedOn,
                        ModefiedBy = e.ModefiedBy,
                        ModefiedOn = e.ModefiedOn
                    }).ToList(),
                    Entity = (transaction.Entity != null) ? new OrgUnit
                    {
                        Id = transaction.Entity.Id,
                        LocalName = transaction.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    ToUser = (transaction.ToUser != null) ? new UserProfile
                    {
                        Id = transaction.ToUser.Id,
                        LocalName = transaction.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Priority = (transaction.Priority != null) ? new Priority
                    {
                        Id = transaction.Priority.Id,
                        Text = transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    Confidentiality = (transaction.Confidentiality != null) ? new Permission
                    {
                        Id = transaction.Confidentiality.Id,
                        LocalName = transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                        Code = transaction.Confidentiality.Code
                    } : null,

                    TransactionType = (transaction.TransactionType != null) ? new TransactionType
                    {
                        Id = transaction.TransactionType.Id,
                        Text = transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    LetterType = (transaction.LetterType != null) ? new LetterType
                    {
                        Id = transaction.LetterType.Id,
                        Text = transaction.LetterType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    ExternalParty = (transaction.ExternalParty != null) ? new ExternalParty
                    {
                        Id = transaction.ExternalParty.Id,
                        Number = transaction.ExternalParty.Number,
                        LocalName = transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                        LocalAddress = transaction.ExternalParty.LocalAddress
                    } : null,

                    ExternalPartyManager = (transaction.ExternalPartyManager != null) ? new ExternalPartyManager
                    {
                        Id = transaction.ExternalPartyManager.Id,
                        LocalName = transaction.ExternalPartyManager.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    LetterNumber = transaction.LetterNumber
                };
                return resultTransaction;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<Transaction> GetTransactionsByExternalPartyId(int externalPartyId, int orgUnitId)
        {
            try
            {


                IList<Transaction> transactions = _oMCSDbContext.Transactions.Where(c => c.ExternalPartyId == externalPartyId && c.EntityId == orgUnitId).ToList();

                return transactions.Where(t => !t.IsDeleted).ToList();
            }

            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public List<Transaction> LateTransactionWithNotifyLetterTypes()
        {
            try
            {
                IList<Transaction> transactions = (from transaction in _oMCSDbContext.Transactions
                                                   where (DbFunctions.DiffDays(transaction.CreatedOn, DateTime.Now)) > 3 &&
                                                   transaction.LetterType.Notify && transaction.StatusId == 390
                                                   select new
                                                   {
                                                       transaction.Assignments,
                                                       transaction.Id,
                                                       transaction.Date,
                                                       transaction.Number
                                                   }).ToList().Select(t => new Transaction
                                                   {
                                                       Assignments = t.Assignments,
                                                       Id = t.Id,
                                                       Date = t.Date,
                                                       Number = t.Number
                                                   }).ToList();

                return transactions.Where(t => !t.IsDeleted).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<Transaction> SendNearlyLateTransaction()
        {
            try
            {
                IList<Transaction> transactions = (from transaction in _oMCSDbContext.Transactions
                                                   where
                                                   transaction.Assignments.FirstOrDefault().TransactionAssignmentProcessPeriod.HasValue &&
                                                   DbFunctions.DiffDays(transaction.Assignments.FirstOrDefault().TransactionAssignmentProcessPeriod, DateTime.Now) == 1
                                                   //|| DbFunctions.DiffDays(transaction.Assignments.FirstOrDefault().TransactionAssignmentProcessPeriod, transaction.Date) > 1

                                                   select new
                                                   {
                                                       transaction.Assignments,
                                                       transaction.Id,
                                                       transaction.Date,
                                                       transaction.Number
                                                   }).ToList().Select(t => new Transaction
                                                   {
                                                       Assignments = t.Assignments,
                                                       Id = t.Id,
                                                       Date = t.Date,
                                                       Number = t.Number
                                                   }).ToList();

                return transactions.Where(t => !t.IsDeleted).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        #endregion

        #region VIP

        public Transaction GetTransaction_VIP(Expression<Func<Transaction, bool>> @where, string cultureName, bool isNotification = false)
        {
            try
            {
                Transaction transaction = _oMCSDbContext.Transactions.Include(x => x.TransactionCategory).Include(x => x.MainDocument)
                    .Include(x => x.DeliveryMethod.Localizations).Include("Links.ToTransaction").Include(x => x.Confidentiality).Include(x => x.TransactionType)
                    .Include(x => x.Attachments).Include("Attachments.DocumentInfo").Include(x => x.Entity)
                    .Include(x => x.Assignments).Where(@where).FirstOrDefault();

                if (transaction == null || transaction.IsDeleted)
                {
                    return null;
                }

                Transaction result = new Transaction
                {
                    Id = transaction.Id,
                    CreatedBy = transaction.CreatedBy,
                    Date = transaction.Date,
                    DateH = transaction.DateH,
                    Status = transaction.Status,
                    TransactionCategory = transaction.TransactionCategory,
                    TransactionCategoryId = transaction.TransactionCategoryId,
                    OrgUnitId = transaction.OrgUnitId,
                    ToUserId = transaction.ToUserId,
                    EntityId = transaction.EntityId,
                    ExternalPartyId = transaction.ExternalPartyId,
                    Number = transaction.Number,
                    DocumentNumber = transaction.DocumentNumber,
                    Remarks = transaction.Remarks,
                    Subject = transaction.Subject,
                    MainDocument = transaction.MainDocument,
                    OldWordDocumnt = transaction.OldWordDocumnt,
                    OutboundDraftEditorType = transaction.OutboundDraftEditorType,
                    IsSigned = transaction.IsSigned,
                    OutboundDraftId = transaction.OutboundDraftId,
                    DeliveryMethodId = transaction.DeliveryMethodId,
                    InboundDateH = transaction.InboundDateH,
                    IsDraft = transaction.IsDraft,
                    ExternalPartyManagerId = transaction.ExternalPartyManagerId,
                    LetterTypeId = transaction.LetterTypeId,
                    RejectionReason = transaction.RejectionReason,
                    TransactionTypeId = transaction.TransactionTypeId,
                    SuggestedTopicId = transaction.SuggestedTopicId,
                    UserId = transaction.UserId,
                    SignedByUserId = transaction.SignedByUserId,
                    PriorityId = transaction.PriorityId,
                    StatusId = transaction.StatusId,
                    MainDocumentId = transaction.MainDocumentId,
                    ConfidentialityId = transaction.ConfidentialityId,
                    IsForIndividual = transaction.IsForIndividual,
                    ReporterId = transaction.ReporterId,
                    DeliveryNumber = transaction.DeliveryNumber,
                    SubjectClassificationsId = transaction.SubjectClassificationsId,
                    RecordNumber = transaction.RecordNumber,
                    SideContactExternalEntityID = transaction.SideContactExternalEntityID,
                    NumberContact = transaction.NumberContact,
                    ContactDateH = transaction.ContactDateH,
                    IsPresentationDraft = transaction.IsPresentationDraft,
                    PresentationDraftNumber = transaction.PresentationDraftNumber,
                    OutBoundDraftNumber = transaction.OutBoundDraftNumber,
                    IsElcOutBound = transaction.IsElcOutBound,
                    NeedAcknowled = transaction.NeedAcknowled,
                    OldWordDocumntId = transaction.OldWordDocumntId,
                    ProcessPeriodTransaction = transaction.ProcessPeriodTransaction,
                    InboundIntendedPerson = transaction.InboundIntendedPerson,
                    ComplaintNumber = transaction.ComplaintNumber,
                    IsDecisionDraft = transaction.IsDecisionDraft,
                    Encrypted = transaction.Encrypted,
                    RemindDateH = transaction.RemindDateH,
                    RemindDate = transaction.RemindDate,
                    SavedTransactionAssignments = transaction.SavedTransactionAssignments != null && transaction.SavedTransactionAssignments.Count() > 0 ?
                   transaction.SavedTransactionAssignments.Select(sta => new SavedTransactionAssignment
                   {
                       AssignmentList = sta.AssignmentList,
                       TransactionId = sta.TransactionId
                   }).ToList() : null,
                    Assignments = transaction.Assignments.Select(a => new TransactionAssignment
                    {
                        Description = a.Description,
                        Date = a.Date,
                        DateH = a.DateH,
                        Id = a.Id,
                        TransactionPathId = a.TransactionPathId,
                        CurrentPathStep = a.CurrentPathStep,
                        Tray = (a.Tray != null) ? new Tray
                        {
                            Id = a.Tray.Id,
                            LocalName = a.Tray.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        ToUser = (a.ToUser != null) ? new UserProfile
                        {
                            Id = a.ToUser.Id,
                            LocalName = a.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,


                        Action = (a.Action != null) ? new Action
                        {
                            Id = a.Action.Id,
                            LocalName = a.Action.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                            Type = a.Action.Type
                        } : null,

                        FromEntity = (a.FromEntity != null) ? new OrgUnit
                        {
                            Id = a.FromEntity.Id,
                            LocalName = a.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        ToEntity = (a.ToEntity != null) ? new OrgUnit
                        {
                            Id = a.ToEntity.Id,
                            LocalName = a.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        FromUser = (a.FromUser != null) ? new UserProfile
                        {
                            Id = a.FromUser.Id,
                            LocalName = a.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null
                    }
                    ).ToList(),

                    Links = transaction.Links.Select(tl => new TransactionLink
                    {
                        Id = tl.Id,
                        TransactionId = tl.TransactionId,
                        ToTransactionId = tl.ToTransactionId,
                        ToTransaction = tl.ToTransaction
                    }
                    ).ToList(),


                    Attachments = transaction.Attachments.Select(a => new Attachment
                    {
                        DocumentInfo = (a.DocumentInfo != null) ? new DocumentInfo
                        {
                            Document = (a.DocumentInfo.Document != null) ? new Document
                            {
                                Id = a.DocumentInfo.Document.Id
                            } : null,

                            Id = a.DocumentInfo.Id,
                            MimeType = a.DocumentInfo.MimeType,
                            Name = a.DocumentInfo.Name,
                            Size = a.DocumentInfo.Size,
                            FromEntityId = a.DocumentInfo.FromEntityId,
                            FromUserId = a.DocumentInfo.FromUserId,
                            FromEntity = (a.DocumentInfo.FromEntity != null) ? new OrgUnit
                            {
                                Id = a.DocumentInfo.FromEntity.Id,
                                LocalName = a.DocumentInfo.FromEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            } : null,
                            FromUser = (a.DocumentInfo.FromUser != null) ? new UserProfile
                            {
                                Id = a.DocumentInfo.FromUser.Id,
                                LocalName = a.DocumentInfo.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                            } : null

                        } : null,

                        Type = (a.Type != null) ? new AttachmentType
                        {
                            Archivable = a.Type.Archivable,
                            Id = a.Type.Id,
                            Text = a.Type.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        Description = a.Description,
                        Count = a.Count,
                        Id = a.Id,
                        TypeId = a.TypeId,
                        AttachmentSource = a.AttachmentSource,
                        CreatedBy = a.CreatedBy,
                    }).ToList(),


                    Confidentiality = (transaction.Confidentiality != null) ? new Permission
                    {
                        Id = transaction.Confidentiality.Id,
                        LocalName = transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                        Code = transaction.Confidentiality.Code
                    } : null,

                    Priority = (transaction.Priority != null) ? new Priority
                    {
                        Id = transaction.Priority.Id,
                        Text = transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                    FollowUp = transaction.FollowUp != null ? transaction.FollowUp.Where(f => !f.IsDeleted).Select(f => new TransactionFollowUp
                    {
                        Id = f.Id,
                        TransactionId = f.TransactionId,
                        DateTo = f.DateTo,
                        DateToH = f.DateToH,
                        IsDeleted = f.IsDeleted,
                        CreatedOn = f.CreatedOn,
                        CreatedBy = f.CreatedBy,
                        ModefiedOn = f.ModefiedOn,
                        ModefiedBy = f.ModefiedBy,
                        CreatingUserId = f.CreatingUserId,
                        CreatingUser = (f.CreatingUser != null) ? new UserProfile
                        {
                            Id = f.CreatingUserId,
                            LocalName = f.CreatingUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        CreatingEntityId = f.CreatingEntityId,
                        CreatingEntity = (f.CreatingEntity != null) ? new OrgUnit
                        {
                            Id = f.CreatingEntity.Id,
                            LocalName = f.CreatingEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,

                        FollowUpEntityId = f.FollowUpEntityId,
                        FollowUpEntity = (f.FollowUpEntity != null) ? new OrgUnit
                        {
                            Id = f.FollowUpEntity.Id,
                            LocalName = f.FollowUpEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        FollowUpUserId = f.FollowUpUserId,
                        FollowUpUser = (f.FollowUpUser != null) ? new UserProfile
                        {
                            Id = f.FollowUpUserId.Value,
                            LocalName = f.FollowUpUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                        } : null,
                        CreationDate = f.CreationDate,
                        FollowUpExpireDate = f.FollowUpExpireDate,
                        Notes = f.Notes,
                        Active = f.Active,
                        FollowUpProccessId = f.FollowUpProccessId,
                        ProccessPeriod = f.ProccessPeriod,
                        ProccessPeriodDate = f.ProccessPeriodDate,
                        FollowUpProccessNote = f.FollowUpProccessNote,
                        FollowUpCompletionDate = f.FollowUpCompletionDate,
                        FollowUpCompletionDateHj = f.FollowUpCompletionDateHj,
                        FollowUpExpireDateHj = f.FollowUpExpireDateHj,
                        FollowUpReceiveDate = f.FollowUpReceiveDate,
                        FollowUpReason = f.FollowUpReason,
                        FollowUpTypeId = f.FollowUpTypeId,
                        FollowUpStatusId = f.FollowUpStatusId,
                        FollowUpMethodId = f.FollowUpMethodId,
                        FollowUpPriortyId = f.FollowUpPriortyId,
                        FollowUpSourceId = f.FollowUpSourceId,
                        FollowUpProgressId = f.FollowUpProgressId,
                        IsCopy = f.IsCopy,
                        IsReminder = f.IsReminder,
                        IsEscalated = f.IsEscalated,
                        IsImportant = f.IsImportant,
                        HasChild = f.HasChild,
                        ParentId = f.ParentId,


                    }).ToList() : null,
                    LetterNumber = transaction.LetterNumber,
                    Entity = (transaction.Entity != null) ? new OrgUnit
                    {
                        Id = transaction.Entity.Id,
                        LocalName = transaction.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,
                };
                if (!isNotification && result.MainDocument != null && result.MainDocument.Document != null)
                    result.MainDocument.Document.Content = null;

                if (result?.OldWordDocumnt?.Document != null)
                    result.OldWordDocumnt.Document.Content = null;

                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        #endregion

        #region
        public bool HasSpecialAuthorize(int transactionId, int userId)
        {
            try
            {

                var isHasSpecialAuthorize = _oMCSDbContext.TransactionSpecialAuthorizes.Where(t => t.TransactionId == transactionId && t.UserProfileId == userId
                && (!t.ExpiredDate.HasValue || t.ExpiredDate.Value > DateTime.Now)).Count();

                return isHasSpecialAuthorize > 0;


            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        #endregion
        #region Save Assignment Option
        public void UpdateAssignmentSelectedoption(int transactionId, string assignmentList)
        {
            var savedTransactionAssignment = _oMCSDbContext.SavedTransactionAssignments.Where(x => x.TransactionId == transactionId).FirstOrDefault();
            if (savedTransactionAssignment != null)
            {
                savedTransactionAssignment.AssignmentList = assignmentList;
                _oMCSDbContext.Entry(savedTransactionAssignment).State = EntityState.Modified;
                _oMCSDbContext.SaveChanges();
            }
            else
            {
                savedTransactionAssignment = new SavedTransactionAssignment
                {
                    AssignmentList = assignmentList,
                    TransactionId = transactionId,
                    CreatedOn = DateTime.Now,

                };
                _oMCSDbContext.SavedTransactionAssignments.Add(savedTransactionAssignment);
                _oMCSDbContext.SaveChanges();
            }

        }
        #endregion
    }
}
