using System.Collections.Generic;
using MCS.Common;

namespace MCS.DTO
{

    public class EditOutboundExternalDTO : TransactionDTO
    {
        public EditOutboundExternalDTO()
        {
            OutboundExternalBasicInfo = new EditOutboundExternalBasicInfoDTO();
            Names = new List<TransactionNameDTO>();
            Links = new List<TransactionLinkDTO>();
            Copies = new List<TransactionCopyDTO>();
            Attachments = new List<TransactionAttachmentDTO>();
        }

        
        public override TransactionCategory TransactionCategory
        {
            get { return TransactionCategory.ExternalOutbound; }
        }

        
        public EditOutboundExternalBasicInfoDTO OutboundExternalBasicInfo { get; set; }

        
        public List<TransactionCopyDTO> Copies { get; set; }

        
        public int ModifiedByUserId { get; set; }
        public EditorType EditorType { get; set; }
        public IList<TransactionFollowUpDTO> FollowUp { get; set; }

    }
}
