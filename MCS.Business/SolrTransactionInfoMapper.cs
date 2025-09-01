using System.Collections.Generic;
using System.Linq;
using MCS.Framework.Localization.SupportClasses;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public class SolrTransactionInfoMapper
    {
        public static TransactionInfo Map(Transaction transaction, string barcode)
        {
            TransactionInfo transactionInfo = MapTransactionInfo(transaction, barcode);

            return transactionInfo;
        }

        public static TransactionIndexLog Map(TransactionInfo transactionInfo)
        {
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
                TransactionTypeNameAr = transactionInfo.TransactionTypeNameAr,
                TransactionTypeNameEn = transactionInfo.TransactionTypeNameEn,
                Subject = transactionInfo.Subject,
                Assignments = (transactionInfo.Assignments != null) ? string.Join(",", transactionInfo.Assignments) : null,
                SubjectClassifications = (transactionInfo.SubjectClassifications != null) ? string.Join(",", transactionInfo.SubjectClassifications) : null,
                WithArchiving = transactionInfo.WithArchiving,
                Color = transactionInfo.Color
            };

            return transactionIndexLog;
        }

        public static TransactionInfo Map(TransactionIndexLog transactionIndexLog)
        {
            TransactionInfo transactionInfo = new TransactionInfo()
            {
                DocId = transactionIndexLog.TransId,
                TransactionCategoryId = transactionIndexLog.TransactionCategoryId,
                TransactionTypeId = transactionIndexLog.TransactionTypeId,
                Number = transactionIndexLog.Number,
                Barcode = transactionIndexLog.Barcode,
                Date = transactionIndexLog.Date,
                DateH = transactionIndexLog.DateH,
                Year = transactionIndexLog.Year,
                YearH = transactionIndexLog.YearH,
                PermissionCode = transactionIndexLog.PermissionCode,
                PriorityId = transactionIndexLog.PriorityId,
                PartyId = transactionIndexLog.PartyId,
                OrgUnitId = transactionIndexLog.OrgUnitId,
                DirectedToUserId = transactionIndexLog.DirectedToUserId,
                StatusId = transactionIndexLog.StatusId,
                LetterTypeId = transactionIndexLog.LetterTypeId,
                SignedByUserId = transactionIndexLog.SignedByUserId,
                OrgUnitNameAr = transactionIndexLog.OrgUnitNameAr,
                OrgUnitNameEn = transactionIndexLog.OrgUnitNameEn,
                TypeNameAr = transactionIndexLog.TypeNameAr,
                TypeNameEn = transactionIndexLog.TypeNameEn,
                PartyNameAr = transactionIndexLog.PartyNameAr,
                PartyNameEn = transactionIndexLog.PartyNameEn,
                SignedByNameAr = transactionIndexLog.SignedByNameAr,
                SignedByNameEn = transactionIndexLog.SignedByNameEn,
                ConfidentialityNameAr = transactionIndexLog.ConfidentialityNameAr,
                ConfidentialityNameEn = transactionIndexLog.ConfidentialityNameEn,
                PriorityNameAr = transactionIndexLog.PriorityNameAr,
                PriorityNameEn = transactionIndexLog.PriorityNameEn,
                StatusNameAr = transactionIndexLog.StatusNameAr,
                StatusNameEn = transactionIndexLog.StatusNameEn,
                TransactionTypeNameAr = transactionIndexLog.TransactionTypeNameAr,
                TransactionTypeNameEn = transactionIndexLog.TransactionTypeNameEn,
                Subject = transactionIndexLog.Subject,
                Assignments = transactionIndexLog.Assignments != null ? transactionIndexLog.Assignments.Split(',').ToList() : null,
                SubjectClassifications = transactionIndexLog.SubjectClassifications != null ? transactionIndexLog.SubjectClassifications.Split(',').ToList() : null,
                WithArchiving = transactionIndexLog.WithArchiving,
                Color = transactionIndexLog.Color
            };

            return transactionInfo;
        }

        private static TransactionInfo MapTransactionInfo(Transaction transaction, string barcode)
        {
            IPermissionBL permissionBL = new PermissionBL();
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
                WithArchiving = (transaction.MainDocumentId != null),
                TransactionTypeNameAr = (transaction.TransactionCategory != null) ? transaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.Arabic).LocalText() : string.Empty,
                TransactionTypeNameEn = (transaction.TransactionCategory != null) ? transaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.English).LocalText() : string.Empty,
                SubjectClassifications = (transaction.SubjectClassifications != null) ? transaction.SubjectClassifications.Select(s => s.SubjectClassificationId.ToString()).ToList() : null,
                LetterNumber = transaction.LetterNumber
            };

            if (transaction.TransactionType != null && transaction.TransactionType.Color != null)
            {
                transactionInfo.Color = transaction.TransactionType.Color.Id.ToString();
            }

            if (transaction.ToUserId.HasValue)
            {
                transactionInfo.DirectedToUserId = transaction.ToUserId;
            }
            else if (transaction.ExternalPartyManagerId.HasValue)
            {
                transactionInfo.DirectedToUserId = transaction.ExternalPartyManagerId;
            }

            if (transaction.SignedByUser != null)
            {
                transactionInfo.SignedByUserId = transaction.SignedByUser.Id;

                Localization localization =
                    transaction.SignedByUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.Arabic).FirstOrDefault();

                transactionInfo.SignedByNameAr = (localization != null) ? localization.Text : string.Empty;

                localization =
                    transaction.SignedByUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Constants.Languages.English).FirstOrDefault();

                transactionInfo.SignedByNameEn = (localization != null) ? localization.Text : string.Empty;
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
            TransactionInfo transactionInfo = MapTransactionInfo(transaction, barcode);

            transactionInfo.Assignments = (transactionAssignments != null) ?
                transactionAssignments.Select(ta => ta.ToEntityId.ToString()).ToList() : null;

            return transactionInfo;
        }
    }
}
