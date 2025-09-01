using System.Collections.Generic;

namespace MCS.DTO
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public LookupDTO Name { get; set; }
        public string LocalName { get; set; }
        public bool IsActive { get; set; }
        public IList<BasicUserDto> Users { get; set; }


    }
}
