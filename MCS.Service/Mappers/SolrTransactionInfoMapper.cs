using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Framework.Localization.SupportClasses;
using MCS.Business;
using MCS.Common;
using MCS.Domain;

namespace MCS.Service.Mappers
{
    public class SolrTransactionInfoMapper
    {
        public static TransactionInfo Map(Transaction transaction, string barcode)
        {
            if (transaction == null)
                return null;

            TransactionInfo transactionInfo = MapTransactionInfo(transaction, barcode);

            return transactionInfo;
        }

        public static TransactionIndexLog Map(TransactionInfo transactionInfo)
        {
            if (transactionInfo == null)
                return null;

            TransactionIndexLog transactionIndexLog = new TransactionIndexLog()
            {
                TransId = transactionInfo.DocId,
                TransactionCategoryId = transactionInfo.TransactionCategoryId,
                TransactionTypeId = transactionInfo.TransactionTypeId,
                Number = transactionInfo.Number,
                Barcode = transactionInfo.Barcode,
                Date = transactionInfo.Date,
                DateH = transactionInfo.DateH,
                Year = transactionInfo.Year,
                YearH = transactionInfo.YearH,
                PermissionCode = transactionInfo.PermissionCode,
                PriorityId = transactionInfo.PriorityId,
                PartyId = transactionInfo.PartyId,
                OrgUnitId = transactionInfo.OrgUnitId,
                DirectedToUserId = transactionInfo.DirectedToUserId,
                StatusId = transactionInfo.StatusId,
                LetterTypeId = transactionInfo.LetterTypeId,
                SignedByUserId = transactionInfo.SignedByUserId,
                OrgUnitNameAr = transactionInfo.OrgUnitNameAr,
                OrgUnitNameEn = transactionInfo.OrgUnitNameEn,
                TypeNameAr = transactionInfo.TypeNameAr,
                TypeNameEn = transactionInfo.TypeNameEn,
                PartyNameAr = transactionInfo.PartyNameAr,
                PartyNameEn = transactionInfo.PartyNameEn,
                SignedByNameAr = transactionInfo.SignedByNameAr,
                SignedByNameEn = transactionInfo.SignedByNameEn,
                ConfidentialityNameAr = transactionInfo.ConfidentialityNameAr,
                ConfidentialityNameEn = transactionInfo.ConfidentialityNameEn,
                PriorityNameAr = transactionInfo.PriorityNameAr,
                PriorityNameEn = transactionInfo.PriorityNameEn,
                StatusNameAr = transactionInfo.StatusNameAr,
                StatusNameEn = transactionInfo.StatusNameEn,
                Subject = transactionInfo.Subject,
                Assignments = (transactionInfo.Assignments != null) ? string.Join(",", transactionInfo.Assignments) : null
            };

            return transactionIndexLog;
        }

        private static TransactionInfo MapTransactionInfo(Transaction transaction, string barcode)
        {
            if (transaction == null)
                return null;

            IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
            Permission permission = permissionBL.GetPermissionById(transaction.ConfidentialityId);

            TransactionInfo transactionInfo = new TransactionInfo()
            {
                DocId = transaction.Id,
                TransactionCategoryId = transaction.TransactionCategoryId,
                TransactionTypeId = (transaction.TransactionTypeId.HasValue) ? transaction.TransactionTypeId.Value : -1,
                Number = transaction.Number,
                Barcode = barcode,
                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(transaction.Date),
                Date = transaction.Date,
                Year = transaction.Date.Year,
                YearH = DateTimeUtility.GetHijriYear(transaction.Date),
                PermissionCode = (permission != null) ? permission.Code : string.Empty,
                PriorityId = transaction.PriorityId,
                PartyId = transaction.ExternalPartyId,
                OrgUnitId = transaction.OrgUnitId,
                StatusId = transaction.StatusId,
                DirectedToUserId = transaction.ToUserId,
                LetterTypeId = (transaction.LetterTypeId.HasValue) ? transaction.LetterTypeId.Value : -1,
                Subject = transaction.Subject,
                OrgUnitNameAr = (transaction.OrgUnit != null) ? transaction.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.Arabic).LocalText() : string.Empty,
                OrgUnitNameEn = (transaction.OrgUnit != null) ? transaction.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.English).LocalText() : string.Empty,
                TypeNameAr = (transaction.TransactionType != null) ? transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.Arabic).LocalText() : string.Empty,
                TypeNameEn = (transaction.TransactionType != null) ? transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.English).LocalText() : string.Empty,
                PartyNameAr = (transaction.ExternalParty != null) ? transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.Arabic).LocalText() : string.Empty,
                PartyNameEn = (transaction.ExternalParty != null) ? transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.English).LocalText() : string.Empty,
                ConfidentialityNameAr = transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.Arabic).LocalText(),
                ConfidentialityNameEn = transaction.Confidentiality.Name.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.English).LocalText(),
                PriorityNameAr = transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.Arabic).LocalText(),
                PriorityNameEn = transaction.Priority.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.English).LocalText(),
                StatusNameAr = (transaction.Status != null) ? transaction.Status.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.Arabic).LocalText() : string.Empty,
                StatusNameEn = (transaction.Status != null) ? transaction.Status.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.English).LocalText() : string.Empty,
                LetterNumber = transaction.LetterNumber
            };

            if (transaction.SignedByUser != null)
            {
                transactionInfo.SignedByUserId = transaction.SignedByUser.Id;

                Localization localization =
                    transaction.SignedByUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.Arabic).FirstOrDefault();

                transactionInfo.SignedByNameAr = (localization != null) ? localization.Text : string.Empty;

                localization =
                    transaction.SignedByUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.English).FirstOrDefault();

                transactionInfo.SignedByNameEn = localization.Text;
            }
            else if (transaction.ExternalPartyManager != null)
            {
                transactionInfo.SignedByUserId = transaction.ExternalPartyManager.Id;

                Localization localization =
                    transaction.ExternalPartyManager.Name.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.Arabic).FirstOrDefault();

                transactionInfo.SignedByNameAr = (localization != null) ? localization.Text : string.Empty;

                localization =
                    transaction.ExternalPartyManager.Name.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.English).FirstOrDefault();

                transactionInfo.SignedByNameEn = (localization != null) ? localization.Text : string.Empty;
            }

            return transactionInfo;
        }

        public static TransactionInfo MapAssignments(Transaction transaction, string barcode, IList<TransactionAssignment> transactionAssignments)
        {
            if (transaction == null || transactionAssignments == null || !transactionAssignments.Any())
            {
                return null;
            }
            TransactionInfo transactionInfo = MapTransactionInfo(transaction, barcode);

            transactionInfo.Assignments = (transactionAssignments != null) ?
                transactionAssignments.Select(ta => ta.ToEntity.Id.ToString()).ToList() : null;

            return transactionInfo;
        }
    }
}