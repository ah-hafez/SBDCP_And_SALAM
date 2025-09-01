using System.Collections.Generic;
using MCS.Common;

namespace MCS.DTO
{
    public class EditOutboundInternalDTO : TransactionDTO
    {
        
        public override TransactionCategory TransactionCategory
        {
            get { return TransactionCategory.InternalOutbound; }
        }

        public List<TransactionCopyDTO> Copies { get; set; }
        public IList<TransactionFollowUpDTO> FollowUps { get; set; }

        public EditOutboundInternalBasicInfoDTO OutboundInternalBasicInfoEdit { get; set; }

    }
}
