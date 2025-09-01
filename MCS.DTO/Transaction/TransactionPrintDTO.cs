using System;
using System.Collections.Generic;
using MCS.Common;

namespace MCS.DTO
{
    
    public class TransactionPrintDTO
    {
        public DocumentDTO DocumentDTO { get; set; }
        public List<TransactionAttachmentDTO> Attachments { get; set; }
        public List<ExplanationDTO> Explanations { get; set; }        
     
    }
}
