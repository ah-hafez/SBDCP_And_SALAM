using System.Collections.Generic;
using MCS.Common;

namespace MCS.DTO
{

    public class AddOutboundInternalDTO : TransactionDTO
    {
        public AddOutboundInternalDTO()
        {
            OutboundInternalBasicInfoAdd = new AddOutboundInternalBasicInfoDTO();
            Names = new List<TransactionNameDTO>();
            Links = new List<TransactionLinkDTO>();
            Attachments = new List<TransactionAttachmentDTO>();
            Copies = new List<TransactionCopyDTO>();
        }

        public List<TransactionCopyDTO> Copies { get; set; }

        public override TransactionCategory TransactionCategory
        {
            get { return TransactionCategory.InternalOutbound; }
        }

        
        public AddOutboundInternalBasicInfoDTO OutboundInternalBasicInfoAdd { get; set; }

    }
}
