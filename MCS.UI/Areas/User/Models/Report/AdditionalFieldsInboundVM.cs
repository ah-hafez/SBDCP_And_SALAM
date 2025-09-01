using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Report
{
    public class AdditionalFieldsInboundVM
    {
        [CustomDisplayName("User.Inbound.BasicInfo.InboundDestination")]
        public int EntitiesTypeId { get; set; }

        [CustomDisplayName("User.Inbound.BasicInfo.Destination")]
        public int ExternalPartiesId { get; set; }   //جهة الوارد//

        [CustomDisplayName("User.Inbound.BasicInfo.InboundDocumentNumber")]
        public string InboundDocumentNumber { get; set; }   //رقم المعاملة الواردة//

        [CustomDisplayName("User.Inbound.BasicInfo.InboundDateH")]
        public string InboundDateH { get; set; }
    }
}