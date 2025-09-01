using System;
using System.Collections.Generic;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class DraftOutboundTrayBL : TrayBaseBL, IDraftOutboundTrayBL
    {
        public override TrayType TrayType
        {
            get { return TrayType.DraftOutbound; }
        }

        public override string TrayPermission
        {
            get
            {
                return User.HasClaim(UserClaims.Files.DraftOutbound) ? UserClaims.Files.DraftOutbound : UserClaims.Files.CopiesOutbound;
            }
        }

        public override void DeleteDraft(int draftTransactionId)
        {
            try
            {
                Transaction transaction = TransactionBL.GetTransactionById(draftTransactionId);

                ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));

                Transaction transactionLinkedToDraft = transactionBL.GetTransaction(t => t.OutboundDraftId == transaction.Id);

                if (transactionLinkedToDraft != null)
                {
                    transactionLinkedToDraft.OutboundDraftId = null;

                    transactionBL.Update(transactionLinkedToDraft);
                }

                TransactionBL.Delete(transaction);
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
        public override Transaction PrepareOutboundCreation(int transactionId, int OrgUnitId, string cultureName)
        {
            try
            {
                Transaction transaction = TransactionBL.GetTransaction(transactionId, User.Id, OrgUnitId, cultureName);
                if (transaction == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }
                ILookupBL lookupBL = new LookupBL();

                transaction.TransactionCategory = lookupBL.GetLookupItem(TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty));
                transaction.TransactionCategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                transaction.EntityId = OrgUnitId;
                transaction.Id = 0;

                return transaction;
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
        public override TransactionDetails CreateOutboundExternal(int transactionId, Transaction transactionExternal)
        {
            try
            {
                Transaction transactionDraft = TransactionBL.GetTransactionById(transactionId);

                if (transactionDraft == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }

                TransactionDetails transactionDetails = null;

                ITransactionBL transactionBL;

                Transaction transaction = TransactionBL.GetTransactionByDraftNumber(transactionDraft.Id);

                if (transaction != null)
                {
                    transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));

                    transaction.StatusId = Common.TransactionStatus.Outbound.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);

                    transactionBL.Update(transaction);
                }

                transactionBL = TransactionBL.Create(TransactionCategory.ExternalOutbound);

                if (transactionExternal.Copies == null)
                {
                    transactionExternal.Copies = new List<TransactionCopy>();
                }

                transactionExternal.Copies.Add(new TransactionCopy()
                {
                    EntityId = transactionDraft.OrgUnitId,
                    UserId = transactionDraft.UserId,
                    Date = DateTime.Now,
                    DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now)
                });

                transactionDetails = transactionBL.Save(transactionExternal);

                TransactionBL.Delete(transactionDraft);

                return transactionDetails;
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
