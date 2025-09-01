using System;

namespace MCS.DTO
{
    public class UserDelegationDTO
    {
        public int Id { get; set; }
        public string FromDateH { get; set; }
        public string ToDateH { get; set; }
        public string OrgUnit { get; set; }
        public string DirectedTo { get; set; }
        public string Priority { get; set; }
        public string Confidentiality { get; set; }
        public string SourceType { get; set; }


        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int OrgUnitId { get; set; }
        public int? PriorityId { get; set; }
        public int? ConfidentialityId { get; set; }
        public int? SourceTypesId { get; set; }
        public int UserPreferenceId { get; set; }

        public int StatusId { get; set; }
        public string Status { get; set; }
        public string RejectionReason { get; set; }
        public int DirectedToId { get; set; }
        public bool ReceiveCopy { get; set; } 
        public bool ShowTransaction { get; set; }
        public string SelectedTransactionCategoriesIdList { get; set; }
        public string UserPreferenceName { get; set; }
        public string SelectedConfidentialityLevelsIdList { get; set; }
        public string TransacionCategoryIds { get; set; }
        public string TransacionConfidentialityIds { get; set; }
    }
}
