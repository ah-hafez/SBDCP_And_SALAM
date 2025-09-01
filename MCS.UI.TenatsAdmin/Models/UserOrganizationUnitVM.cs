using System.Collections.Generic;
using MCS.UI.TenantsAdmin.Models.LookupsVM;

namespace MCS.UI.TenantsAdmin.Models
{
    public class UserOrgUnitVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LocalizationVM> LoclizationName { get; set; }
        public bool IsSelected { get; set; }
    }
}