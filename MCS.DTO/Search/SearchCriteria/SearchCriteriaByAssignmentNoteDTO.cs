using System;

namespace MCS.DTO
{
    public class SearchCriteriaByAssignmentNoteDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaByAssignmentNoteDTO()
        {
            AdvancedSearch = new InboundAdvancedDTO();
        }
        public string AssignmentNote { get; set; }
        public bool HasFullPrivilege { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public InboundAdvancedDTO AdvancedSearch { get; set; }
    }
}