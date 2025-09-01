using System.Collections.Generic;
using MCS.Common;

namespace MCS.DTO
{

    public class AddOutboundExternalDTO : TransactionDTO
    {
        public AddOutboundExternalDTO()
        {
            OutboundExternalBasicInfo = new AddOutboundExternalBasicInfoDTO();
            Names = new List<TransactionNameDTO>();
            Links = new List<TransactionLinkDTO>();
            Copies = new List<TransactionCopyDTO>();
            Attachments = new List<TransactionAttachmentDTO>();
        }

        
        public override TransactionCategory TransactionCategory
        {
            get { return TransactionCategory.ExternalOutbound; }
        }

        
        public AddOutboundExternalBasicInfoDTO OutboundExternalBasicInfo { get; set; }

        
        public List<TransactionCopyDTO> Copies { get; set; }

        
        public int? EditorTypeId { get; set; }
        public string DeliveryNumber { get; set; }
    }
}
