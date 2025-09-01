using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO.AdminAudit
{
    public class AdminAuditDetailsDTO
    { 
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string Description { get; set; }
            public DateTime Date { get; set; }
       
    }
}
