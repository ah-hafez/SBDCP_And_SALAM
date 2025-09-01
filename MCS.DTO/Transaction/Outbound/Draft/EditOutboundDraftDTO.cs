using System.Collections.Generic;
using MCS.Common;

namespace MCS.DTO
{

    public class EditOutboundDraftDTO : TransactionDTO
    {
        public EditOutboundDraftDTO()
        {
            OutboundDraftBasicInfo = new EditOutboundDraftBasicInfoDTO();
            Names = new List<TransactionNameDTO>();
            Links = new List<TransactionLinkDTO>();
            Copies = new List<TransactionCopyDTO>();
            Attachments = new List<TransactionAttachmentDTO>();
            DocumentDTO = new DocumentDTO();
        }
        
        public EditOutboundDraftBasicInfoDTO OutboundDraftBasicInfo { get; set; }
        
        public override TransactionCategory TransactionCategory
        {
            get { return TransactionCategory.DraftOutbound; }
        }
        
        public List<TransactionCopyDTO> Copies { get; set; }

        public int ModifiedByUserId { get; set; }
        
        public EditorType? EditorType { get; set; }

        public IList<TransactionFollowUpDTO> FollowUps { get; set; }

    }
}
