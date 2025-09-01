using System;

namespace MCS.DTO
{
    public class SearchCriteriaBySubjectLetterDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaBySubjectLetterDTO()
        {
            AdvancedSearch = new InboundAdvancedDTO(); 
        }
        public string FirstLetter { get; set; }
        public string SecondLetter { get; set; }
        public string ThirdLetter { get; set; }
        public string FourthLetter { get; set; }
        public int TransactionTypeId { get; set; }
        public int SearchTypeForFiltersId { get; set; }
        public bool HasFullPrivilege { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public InboundAdvancedDTO AdvancedSearch { get; set; }
         
    }
}