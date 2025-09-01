using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Models
{
    public class TaskAttachmentsVM
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int DocumentId { get; set; }
        public ReceivedTaskVM ReceivedTaskDTO { get; set; }
        public DocumentVM Attachment { get; set; }
    }
}