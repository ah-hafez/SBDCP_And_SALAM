using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApi.Models
{
    public class UpdateInfo
    {
        public DataResult Result { get; set; }
        public bool IsUpdated { get; set; }
        public bool SettingsUpdated { get; set; }
        public bool RevocationNeeded { get; set; }
        public bool OrgChartUpdated { get; set; }
        public bool ResourcesUpdated { get; set; }
    }
}