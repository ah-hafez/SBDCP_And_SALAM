using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Models.File
{
    public class WithdrawalVM
    {
        [CustomDisplayName("User.InboundSearch.InboundNumber")]
        [CustomStringLength("User.InboundSearch.InboundNumber", 20, 0)]
        public int? Number { get; set; }//رقم القيد

        [CustomDisplayName("User.InboundSearch.Year")]
        public int? Year { get; set; }//السنة

        [CustomRequired("User.Inbound.BasicInfo.InboundTypeRequired")]
        [CustomDisplayName("User.Inbound.BasicInfo.InboundType")]
        public int TransactionTypeId { get; set; }
    }
}