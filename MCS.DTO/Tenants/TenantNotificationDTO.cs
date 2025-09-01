using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO.Tenants
{
    public class TenantNotificationDTO : BaseDTO
    {
        public List<TenantNotificationDetailDTO> Details { set; get; }
        public virtual string DelegatedEmail { set; get; }
        public int SourceId { set; get; }
        public TenantLookupDTO Source { set; get; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
    }
}
