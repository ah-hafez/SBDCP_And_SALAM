using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class FollowUpAuditTrailVM
    {
        public int FollowupId { get; set; }
        public int ProccessId { get; set; }
        public string ProccessDescription { get; set; }
        public DateTime ProccessDate { get; set; }
        public String ProccessDateHj { get; set; }
        public int UserId { get; set; }
        public int EntityId { get; set; }
        public string UserName { get; set; }
        public string EntityName { get; set; }
        public string UserEntityName { get; set; }

         
    }
}