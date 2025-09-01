using System.Collections.Generic;
using MCS.Common;

namespace MCS.DTO
{

    public class AddOutboundDraftDTO : TransactionDTO
    {
        public AddOutboundDraftDTO()
        {
            OutboundDraftBasicInfo = new AddOutboundDraftBasicInfoDTO();
            Names = new List<TransactionNameDTO>();
            Links = new List<TransactionLinkDTO>();
            Copies = new List<TransactionCopyDTO>();
            Attachments = new List<TransactionAttachmentDTO>();
            DocumentDTO = new DocumentDTO();
        }

        
        public override TransactionCategory TransactionCategory
        {
            get { return TransactionCategory.DraftOutbound; }
        }

        
        public AddOutboundDraftBasicInfoDTO OutboundDraftBasicInfo { get; set; }

        
        public List<TransactionCopyDTO> Copies { get; set; }

        
        public EditorType EditorType { get; set; }

    }
}
