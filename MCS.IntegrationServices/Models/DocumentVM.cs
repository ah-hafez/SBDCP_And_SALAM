using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class DocumentVM
    {
        public int Id { get; set; }
        public string EncryptedId { get; set; }
        public string MimeType { get; set; }
        public byte[] Content { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public bool IsDeleted { get; set; }
        public int FromUserId { get; set; }
        public string FromUserName { get; set; }
        public int FromEntityId { get; set; }
        public string FromEntityName { get; set; }

        public bool Mode { get; set; }
    }
}