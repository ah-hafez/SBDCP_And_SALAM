using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Hub
{
    public class DocumentVM
    {
        public int Id { get; set; }
        public string MimeType { get; set; }
        public byte[] Content { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public bool IsDeleted { get; set; }
        public string ECMID { get; set; }
    }
}