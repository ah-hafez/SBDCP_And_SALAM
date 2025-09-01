using System.Collections.Generic;
using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction.Inbound
{
    public class AddInboundVM : TransactionVM
    {
        public override TransactionCategory Type
        {
            get { return TransactionCategory.Inbound; }
        }
        public AddInboundBasicInfoVM InboundBasicInfo { get; set; }= new AddInboundBasicInfoVM();
        public int SubjectClassificationsId { get; set; }

    }
}