using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.ExternalParties
{
    public class AddressVM
    {
        public int Id { get; set; }

        //[CustomRequired("Admin.UnitInfo.Names")]
        [CustomStringLength("Global.Localization.Text", 100, 0)]
        public string Text { get; set; }

        public int CultureId { get; set; }

        public string CultureName { get; set; }
    }
}