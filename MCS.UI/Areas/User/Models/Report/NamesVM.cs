using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Report
{
    public class NamesVM
    {
        [CustomDisplayName("User.Transaction.Name.CivilID")]
        public string CivilID { get; set; } 

        [CustomDisplayName("User.Transaction.Name.FullName")]
        public string FullName { get; set; } 

        [CustomDisplayName("User.Transaction.Name.MobileNumber")]
        public string MobileNumber { get; set; } 
    }
}