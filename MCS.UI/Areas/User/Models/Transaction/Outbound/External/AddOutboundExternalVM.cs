using System.Collections.Generic;
using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.Shared;

namespace MCS.UI.Areas.User.Models.Transaction.Outbound.External
{
    public class AddOutboundExternalVM : TransactionVM
    {
        public override TransactionCategory Type
        {
            get { return TransactionCategory.ExternalOutbound; }
        }
        public AddOutboundExternalBasicInfoVM OutboundExternalBasicInfo { get; set; } = new AddOutboundExternalBasicInfoVM();
        public int? EditorTypeId { get; set; }
        public DocumentVM OldDocumentVM { get; set; }

        public MultiExternalOutboundVM MultiExternalOutbound { get; set; }

    }
}