using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Search
{
    public class AdvancedSearchVM
    {
        public int ID { get; set; }

        [CustomDisplayName("User.Search.Unit")]
        public int? OrgUnitId { get; set; }

        [CustomDisplayName("User.Search.SearchType")]
        [CustomRequired("User.Search.SearchTypeRequired")]
        public int SearchTypeId { get; set; }

        public SearchCriteriaByInboundVM InboundSearch { get; set; }
        public SearchCriteriaByEntityNameVM EntitySearch { get; set; }
        public SearchCriteriaByCreatorVM CreatorSearch { get; set; }
        public SearchCriteriaByAssignTransactionVM AssignTransactionSearch { get; set; }
        public SearchCriteriaByOutboundInternalVM OutboundInternalSearch { get; set; }
        public SearchCriteriaByOutboundVM OutboundSearch { get; set; }
        public SearchCriteriaByOutboundDraftVM OutboundDraftSearch { get; set; }
        public SearchCriteriaBySubjectVM SubjectSearch { get; set; }
        public SearchCriteriaByBarcodeVM BarcodeSearch { get; set; }
        public SearchCriteriaGeneralVM GeneralSearch { get; set; }
        public SearchCriteriaByDocumentNumberVM DocumentNumberSearch { get; set; } 


        public SearchCriteriaByRecordNumberVM RecordNumberSearch { get; set; }

        public SearchCriteriaByAssignmentNoteVM AssignmentNoteSearch { get; set; }
        public SearchCriteriaByCopyAssignemntVM CopyAssignemntSearch { get; set; }
        public SearchCriteriaByDailyVM DailySearch { get; set; }
        public SearchCriteriaByElcEmployeeVM ElcEmployeeSearch { get; set; }
        public SearchCriteriaByExternalOutBoundOrManifestNumberVM ExternalOutBoundOrManifestNumberSearch { get; set; }
        public SearchCriteriaByManifestNumberVM ManifestNumberSearch { get; set; }
        public SearchCriteriaByMilitaryNumberOrIdentityVM IdentificationNumber { get; set; }
        public SearchCriteriaByNamesVM NamesSearch { get; set; }
        public SearchCriteriaBySubjectLetterVM SubjectLetterSearch { get; set; }
        public SearchCriteriaByTransactionNotsVM TransactionNotsSearch { get; set; }
        public SearchCriteriaByTransactionNumberVM TransactionNumber { get; set; }
        public SearchCriteriaByExternalPartyCopiesVM ExternalPartyCopies { get; set; }
    }
}