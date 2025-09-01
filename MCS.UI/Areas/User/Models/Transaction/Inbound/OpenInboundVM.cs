using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Transaction.Inbound
{
    public class OpenInboundVM
    {
        [CustomDisplayName("User.Inbound.Open.InboundNumber")]
        [CustomRequired("User.Inbound.Open.InboundNumberRequired")]
        public int InboundNumber { set; get; }  //رقم القيد//

        [CustomDisplayName("User.Inbound.Open.Year")]
        public int? Year { set; get; } //السنة//

        [CustomDisplayName("User.Inbound.Open.Source")]
        public int? SourceId { set; get; }   //نوع الوارد//
    }
}