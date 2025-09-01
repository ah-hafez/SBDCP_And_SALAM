using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Hub
{
    public class DocumentInfoVM
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public string MimeType { get; set; }
        public bool IsDeleted { get; set; }
        public string ECMId { get; set; }

        public DocumentVM Document { get; set; }
        public int Id { get; internal set; }
    }
}