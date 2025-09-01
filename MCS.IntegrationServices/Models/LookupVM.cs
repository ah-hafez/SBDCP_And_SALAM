using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class LookupVM
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public int Sort { get; set; }
        public int? EnumReference { get; set; }
        public string Text { get; set; }
        public List<LookupLocalizationVM> Localizations { get; set; }
    }
}