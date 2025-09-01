using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models
{
    public class VerificationCodeVM
    {
        public int TransactionId { get; set; }
        public int TransactionCategoryId { get; set; }
        public string VerifyCode { get; set; }
        public int UserId { get; set; }
        public int OrgUnitId { get; set; }
        public string Mode { get; set; }
        public int CodeExpirationDuration { get; set; }
    }
}