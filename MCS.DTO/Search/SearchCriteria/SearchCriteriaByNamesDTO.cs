using System;

namespace MCS.DTO
{
    public class SearchCriteriaByNamesDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaByNamesDTO()
        {
            AdvancedSearch = new InboundAdvancedDTO(); 
        }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string FamilyName { get; set; }
        public int SearchNamesType { get; set; }
        public bool HasFullPrivilege { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int TransactionTypeId { get; set; }
        public int SearchTypeForFiltersId { get; set; }
        public InboundAdvancedDTO AdvancedSearch { get; set; }
         
    }
}