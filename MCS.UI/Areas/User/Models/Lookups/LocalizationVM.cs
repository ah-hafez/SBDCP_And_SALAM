using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Lookups
{
    public class LocalizationVM
    {
        public int Id { get; set; }

        [CustomRequired("Admin.UnitInfo.Names")]
        [CustomStringLength("Global.Localization.Text", 100, 0)]
        [CustomRegularExpression("^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z ]+[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z-_ ]*$", "Global.Localization.TextExpression")]
        public string Text { get; set; }

        public int CultureId { get; set; }

        public string CultureName { get; set; }
    }
}