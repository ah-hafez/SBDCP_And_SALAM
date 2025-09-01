using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionAddressVM
    {
        [CustomDisplayName("User.OutboundExternal.BasicInfo.DirectedTo")]

        public string DirectedTo { get; set; }
        [CustomDisplayName("User.OutboundExternal.BasicInfo.Type")]

        public string DocumentType { get; set; }
        [CustomDisplayName("User.OutboundExternal.BasicInfo.Date")]
        public string TransactionDate { get; set; }
        [CustomDisplayName("User.OutboundExternal.BasicInfo.Destination")]

        public string DirectedToOrgUnit { get; set; }
        [CustomDisplayName("User.OutboundExternal.BasicInfo.OutboundNumber")]
        public string Transactionnumber { get; set; }
        [CustomDisplayName("User.Transaction.SaudiPostDeliveryNumber")]
        public string ShipmentNumber { get; set; }
    }
}