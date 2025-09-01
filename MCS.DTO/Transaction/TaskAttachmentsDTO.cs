using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace MCS.DTO
{
    public class TaskAttachmentsDTO
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int DocumentId { get; set; }
        public ReceivedTaskDTO ReceivedTaskDTO { get; set; }
        public DocumentDTO Attachment { get; set; }
    }
}
