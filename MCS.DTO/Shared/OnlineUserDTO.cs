using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO.Shared
{
    public class OnlineUserDTO
    {
        public int UserId { set; get; }
        public string OrgUnitName { set; get; }
        public int? OrgUnitId { set; get; }
        public string UserFullName { set; get; }
    }
}
