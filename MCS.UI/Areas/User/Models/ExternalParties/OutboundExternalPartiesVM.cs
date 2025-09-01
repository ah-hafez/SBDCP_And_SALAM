using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.ExternalParties
{
    public class OutboundExternalPartiesVM
    {
        [CustomDisplayName("User.OutboundExternal.BasicInfo.Destination")]
        [CustomRequired("User.OutboundExternal.BasicInfo.DestinationRequired")]
        public int? DestinationId { get; set; }
    }
}