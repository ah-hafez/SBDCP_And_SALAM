using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.MultiTenants
{
    public class TenantInfo : ITenant
    {
        public int Id { get; set; }
        public string HostName { get; set; }
        public string DatabaseName { get; set; }
        public string LocalName { get; set; }
        public byte[] Logo { get; set; }
        public string ECMProfileId { get; set; }
        public string ECMCategoryId { get; set; }
    }
}
