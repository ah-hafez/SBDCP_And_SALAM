using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.ExternalParties
{
    public class ManagerAddVM
    {
        public List<LocalizationVM> Name { get; set; }
        public int PartyId { get; set; }
        //[Required]
        [CustomRequired("الحقل مطلوب")]
        public string EmailAddress { get; set; }
    }
}