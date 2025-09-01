using Audit.EntityFramework;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
    [AuditIgnore]
    public class WordAddInTemp : EntityBase, IAuditable
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public byte[] ContentPDF { get; set; }
        public bool IsRead { get; set; }



    }
}
