using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.ExternalParties
{
    public class ExternalPartyAddVM
    {
        public List<LocalizationVM> Name { get; set; }

        public List<AddressVM> Address { get; set; }

        //[CustomRequired("Global.ExternalParty.EmailRequired")]
        [CustomEmailAddress("Global.ExternalParty.InvalidEmail")]
        [CustomRegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", "Global.ExternalParty.InvalidEmail")]
        [CustomDisplayName("User.ExternalParty.Email")]
        public string Email { get; set; }

       // [CustomRequired("Global.ExternalParty.NumberRequired")]
        [CustomStringLength("Global.ExternalParty.PartyNumberLength", 10, 1)]
        [CustomDisplayName("User.ExternalParty.Number")]
        public string PartyNumber { get; set; }

        //[CustomRequired("Global.ExternalParty.PhoneRequired")]
        [CustomStringLength("Global.ExternalParty.PhoneNumberLength", 15, 10)]
        [CustomDisplayName("User.ExternalParty.Phone")]
        public string PhoneNumber { get; set; }

        //[CustomRequired("Global.ExternalParty.FaxRequired")]
        [CustomStringLength("Global.ExternalParty.FaxNumberLength", 10, 10)]
        [CustomDisplayName("User.ExternalParty.Fax")]
        public string FaxNumber { get; set; }

        [CustomDisplayName("User.ExternalParty.PartyType")]
        public List<ExternalPartyListTypeVM> Types { get; set; }

        [CustomDisplayName("User.ExternalParty.VirtualUnit")]
        public bool IsVirtual { get; set; }
        public int? ParentId { get; set; }
        public bool YasserRegistered { get; set; }

        [CustomRequired("ادخل الاسم بالعربي")]
        //[CustomStringLength("Global.Localization.Text", 100, 0)]
        [CustomRegularExpression("^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z ]+[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z-_ ]*$", "Global.Localization.TextExpression")]
        public string NameAr { get; set; }

        
        //[CustomStringLength("Global.Localization.Text", 100, 0)]
        [CustomRegularExpression("^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z ]+[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z-_ ]*$", "Global.Localization.TextExpression")]
        public string NameEn { get; set; }

    }
}