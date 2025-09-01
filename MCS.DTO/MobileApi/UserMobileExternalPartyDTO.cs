using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApi.Domain
{
    public class UserMobileExternalPartyDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public bool IsVirtual { get; set; }
        public string UserDefinedId { get; set; }
        public bool? Active { get; set; }
        public List<UserMobileOrgUnitUsersDTO> Persons { get; set; }

    }
}
