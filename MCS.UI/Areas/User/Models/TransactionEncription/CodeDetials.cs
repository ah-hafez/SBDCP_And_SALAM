using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models
{
    public class CodeDetials
    {
        public int TransactionID { get; set; }
        public string HashedCode { get; set; }
        public DateTime CodeExpireDate { get; set; }

    }
}