using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class ConfidentialityLevelVM
    {
        public int Id { get; set; }
        public virtual IList<LocalizationVM> LocalizationVMs { get; set; }
    }
}