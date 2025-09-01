using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models
{
    public class EditorOutboundDraftExternalPartiesVM
    {
        [CustomDisplayName("User.OutboundDraft.BasicInfo.Destination")]
        [CustomRequired("User.OutboundDraft.BasicInfo.DestinationRequired")]
        public int ExternalPartyId { get; set; }
    }
}