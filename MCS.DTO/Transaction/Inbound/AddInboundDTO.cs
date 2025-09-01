using System.Collections.Generic;
using MCS.Common;

namespace MCS.DTO
{

    public class AddInboundDTO : TransactionDTO
    {
        public AddInboundDTO()
        {
            InboundBasicInfo = new AddInboundBasicInfoDTO();
            Names = new List<TransactionNameDTO>();
            Links = new List<TransactionLinkDTO>();
            Attachments = new List<TransactionAttachmentDTO>();
            Copies = new List<TransactionCopyDTO>();
        }
        public List<TransactionCopyDTO> Copies { get; set; }
        public override TransactionCategory TransactionCategory
        {
            get { return TransactionCategory.Inbound; }
        }

        
        public AddInboundBasicInfoDTO InboundBasicInfo { get; set; }
        public object SubjectClassificationsId { get; set; }

    }
}
