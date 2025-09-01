using System.Collections.Generic;
using MCS.UI.TenantsAdmin.Models.LookupsVM;

namespace MCS.UI.TenantsAdmin.Models
{
    public class UserVM
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public string SessionId { get; set; }
        public List<UserOrgUnitVM> UserOrgUnits { get; set; }
        public string Name { get; set; }
        public List<LocalizationVM> LoclizationName { get; set; }
        public string UserName { get; set; }
        public string UserCategoryName { get; set; }
        public List<LocalizationVM> LoclizationUserCategory { get; set; }
        public List<string> Claims { get; set; }
        public string BaseOrgUnitName { get; set; }
        public byte[] Signature { get; set; }
        public byte[] Marking { get; set; }
    }
} 