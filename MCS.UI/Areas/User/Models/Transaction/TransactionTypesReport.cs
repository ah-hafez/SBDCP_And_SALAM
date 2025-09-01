using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionTypesReport
    {
        public string year { set; get; }
        public string inbound { set; get; }
        public string internalv { set; get; }
        public string external { set; get; }
    }
}