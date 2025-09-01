using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Report
{
    public class AdditionalFieldsOutboundVM
    {
        [CustomDisplayName("User.Inbound.BasicInfo.Destination")]
        public int ExternalPartiesId { get; set; } //جهة الصادر

        [CustomDisplayName("User.OutboundExternal.BasicInfo.Date")]
        public string OutboundDateH { get; set; }//تاريخ الصادر
    }
}