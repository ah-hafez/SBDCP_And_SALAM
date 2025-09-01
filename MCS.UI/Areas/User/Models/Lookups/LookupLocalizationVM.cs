using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Lookups
{
    public class LookupLocalizationVM
    {
        public int Id { get; set; }
        public int LookupId { get; set; }

        [CustomRequired("Admin.UnitInfo.Names")]
        public string Text { get; set; }

        public int CultureId { get; set; }
        public string CultureName { get; set; }
    }
}