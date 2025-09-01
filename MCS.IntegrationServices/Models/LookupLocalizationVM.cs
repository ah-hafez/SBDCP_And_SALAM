using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class LookupLocalizationVM
    {
        public int Id { get; set; }
        public int LookupId { get; set; }

        // [CustomRequired("Admin.UnitInfo.Names")]
        public string Text { get; set; }

        public int CultureId { get; set; }
        public string CultureName { get; set; }
    }
}