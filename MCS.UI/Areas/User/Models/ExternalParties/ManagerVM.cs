using System;
using System.Collections.Generic;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.ExternalParties
{
    public class ManagerVM
    {
        public int Id { get; set; }
        public List<LocalizationVM> Name { get; set; }
        public string LocalName { get; set; }
        public DateTime AddedDate { get; set; }
        public int PartyId { get; set; }
        public string EmailAddress { get; set; }
    }
}