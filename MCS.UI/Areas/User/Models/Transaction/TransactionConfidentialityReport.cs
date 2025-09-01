using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionConfidentialityReport
    {
        public string year { set; get; }
        public string normal { set; get; }
        public string secret { set; get; }
        public string verysecret { set; get; }
        public string byHand { set; get; }
    }
}