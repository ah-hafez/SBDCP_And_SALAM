using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models
{
    public class InboundExternalPartiesVM
    {
        [CustomDisplayName("User.Inbound.BasicInfo.DestinationFrom")]
        [CustomRequired("User.Inbound.BasicInfo.DestinationRequired")]
        public int? DestinationId { get; set; }
    }
}