using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.ExternalParties
{
    public class OutboundDraftExternalPartiesVM
    {
        [CustomDisplayName("User.OutboundDraft.BasicInfo.Destination")]
        [CustomRequired("User.OutboundDraft.BasicInfo.DestinationRequired")]
        public int DestinationId { get; set; }
    }
}