using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Search
{
    public class InquirySearchResultVM
    {
        public int Id { get; set; }
        public long Number { get; set; }
        public string Subject { get; set; }
        public string StatusName { get; set; }
        public string ToEntity { get; set; }
        public string ToUser { get; set; }
        public int ToUserId { get; set; }
        public bool HasPermission { get; set; }
        public int TransactionTypeId { get; set; }
    }
}