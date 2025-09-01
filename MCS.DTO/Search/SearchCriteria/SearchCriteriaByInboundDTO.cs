using MCS.Common.CustomAttributes;
using System;
using System.Configuration;

namespace MCS.DTO
{
    public class SearchCriteriaByInboundDTO : BaseSearchCriteriaDTO
    {
        public SearchCriteriaByInboundDTO()
        {
            AdvancedSearch = new InboundAdvancedDTO();
        }
        public int? Number { get; set; }//رقم القيد
        public bool HasFullPrivilege { get; set; }
        public int? Year { get; set; }//السنة
        public int TransactionTypeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public InboundAdvancedDTO AdvancedSearch { get; set; }
        public int DeliveryMethodId { get; set; }
        public string DocumentNumber { get; set; }

    }
}
