using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.Hub
{
    public class HubAttachmentVM
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public AttachmentTypeVM Type { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public DocumentInfoVM DocumentInfo { get; set; }
        public string ExternalAttachementId { get; set; }
    }
}