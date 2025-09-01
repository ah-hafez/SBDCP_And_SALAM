using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.ExternalParties
{
    public class ManagerEditVM
    {
        public int Id { get; set; }
        public List<LocalizationVM> Name { get; set; }
        public int PartyId { get; set; }
        [CustomRequired("الحقل مطلوب")]
        public string EmailAddress { get; set; }
    }
}