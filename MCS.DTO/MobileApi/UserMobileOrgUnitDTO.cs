using System.Collections.Generic;

namespace MobileApi.Domain
{
    public class UserMobileOrgUnitDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public bool IsVirtual { get; set; }
        public string UserDefinedId { get; set; }
        public bool HasChilds { get; set; }
        public bool Active { get; set; }

        public List<UserMobileOrgUnitUsersDTO> Persons { get; set; }
    }
}
