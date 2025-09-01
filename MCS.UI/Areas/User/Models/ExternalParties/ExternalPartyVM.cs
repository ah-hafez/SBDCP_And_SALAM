using System.Collections.Generic;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.ExternalParties
{
    public class ExternalPartyVM
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int? ParentId { get; set; }
        public List<LocalizationVM> Name { get; set; }
        public string LocalName { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsSelected { get; set; }
        public bool HasChilds { get; set; }
        public bool YasserRegistered { get; set; }
        public string Email { get; set; }
    }
}