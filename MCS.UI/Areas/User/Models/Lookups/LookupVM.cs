using System.Collections.Generic;

namespace MCS.UI.Areas.User.Models.Lookups
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