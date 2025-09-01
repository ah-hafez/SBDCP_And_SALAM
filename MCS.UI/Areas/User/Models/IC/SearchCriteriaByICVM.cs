using System;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Search
{
    public class SearchCriteriaByICVM
    {
        public SearchCriteriaByICVM()
        {
           
        }

        [CustomDisplayName("User.Search.Unit")]
        public int? OrgUnitId { get; set; }

        public int? Number { get; set; }//رقم القيد

        [CustomDisplayName("User.InboundSearch.Year")]
        public int? Year { get; set; }//السنة
        [CustomRequired("User.Inbound.BasicInfo.InboundTypeRequired")]
        [CustomDisplayName("User.Inbound.BasicInfo.InboundType")]
        public int TransactionTypeId { get; set; }

    }
}