using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models
{
    public class ReportsExternalPartiesVM
    {
        [CustomDisplayName("User.Report.OutboundTransactions.Destination")]
        [CustomRequired("User.Inbound.BasicInfo.DestinationRequired")]
        public int ToEntity { get; set; }
    }
}