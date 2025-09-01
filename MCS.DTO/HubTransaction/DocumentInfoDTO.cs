using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO.HubTransaction
{
    public class DocumentInfoDTO
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public string MimeType { get; set; }
        public bool IsDeleted { get; set; }
        public string ECMId { get; set; }

        public DocumentDTO Document { get; set; }
        public int Id { get; set; }
    }
}
