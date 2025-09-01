using System.Collections.Generic;
using MCS.Common;
using MCS.UI.Areas.User.Models.Shared;

namespace MCS.UI.Areas.User.Models.Transaction.Outbound.Draft
{
    public class AddOutboundDraftVM : TransactionVM
    {
        public override TransactionCategory Type
        {
            get { return TransactionCategory.DraftOutbound; }
        }
        public AddOutboundDraftBasicInfoVM OutboundDraftBasicInfo { get; set; }= new AddOutboundDraftBasicInfoVM();
        public EditorType EditorType { get; set; }

    }
}