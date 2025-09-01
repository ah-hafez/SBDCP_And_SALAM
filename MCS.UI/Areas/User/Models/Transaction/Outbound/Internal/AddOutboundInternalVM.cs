using System.Collections.Generic;
using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction.Outbound.Internal
{
    public class AddOutboundInternalVM : TransactionVM
    {
        public override TransactionCategory Type
        {
            get { return TransactionCategory.InternalOutbound; }
        }
        public List<TransactionFollowUpVM> FollowUps { get; set; } = (AjaxGrid<TransactionFollowUpVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionFollowUpVM>(), 1, 0, false);

        public AddOutboundInternalBasicInfoVM OutboundInternalBasicInfoAdd { get; set; }= new AddOutboundInternalBasicInfoVM();
        public int? EditorTypeId { get; set; }
        public MultiInternalOutboundVM MultiInternalOutbound { get; set; }
    }
}