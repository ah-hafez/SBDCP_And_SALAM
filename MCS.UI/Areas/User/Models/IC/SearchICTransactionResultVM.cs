using System;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Search
{
    public class SearchICTransactionResultVM : BaseSearchResultVM
    {
        public SearchICTransactionResultVM()
        {
           
        }

        public int MainDocId { get; set; }

        public int IsMain { get; set; }

        public string GUID { get; set; }

        public int IsInIc { get; set; }

        public string  IcName { get; set; }
        public int? OrderFileNumber { get; set; }
        public string Description { get; set; }

    }
}