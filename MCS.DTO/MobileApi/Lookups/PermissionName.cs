using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileApi.Domain
{
    public class PermissionName
    {
        public string Manager { get; set; } = "UserCategory.Management";
        public string CreateInternalOutbound { get; set; } = "Outbound.CreateInternalOutbound";
        public string EditInternalOutbound { get; set; } = "Outbound.EditOutbound";
        public string CreateOutboundDraft { get; set; } = "Outbound.CreateOutboundDraft";
        public string EditOutboundDraft { get; set; } = "Outbound.EditOutbound";
        public string EditInbound { get; set; } = "Inbound.EditInbound";
        public string EntityAccompleshmentsReport { get; set; } = "Reports.EntityAccompleshmentsReport";
        public string UserAccompleshmentsReport { get; set; } = "Reports.UserAccompleshmentsReport";
        public string Sign { get; set; } = "ES.MCS.GENERAL.CAN_SIGN";
        public string Explanations { get; set; } = "ES.MCS.GENERAL.MOBILE_EXPLANATIONS";
    }
}