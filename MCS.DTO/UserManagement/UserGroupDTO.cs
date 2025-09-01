using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class UserGroupDTO
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string GroupName { get; set; }
        public string Name { get; set; }

        public string CreatedBy { get; set; }
        public string AdminUserName { get; set; }
        public List<string> OrgUnitNames { get; set; }
    }
}
