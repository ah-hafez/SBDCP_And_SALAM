using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class OpenInboundDTO
    {
        //[CustomDisplayName("User.Inbound.Open.InboundNumber")]
        [CustomRequired("User.Inbound.Open.InboundNumberRequired")]
        public int InboundNumber { set; get; }  //رقم القيد//

        //[CustomDisplayName("User.Inbound.Open.Year")]
        public int? Year { set; get; } //السنة//

        //[CustomDisplayName("User.Inbound.Open.Source")]
        public int? SourceId { set; get; }   //نوع الوارد//
    }
}
