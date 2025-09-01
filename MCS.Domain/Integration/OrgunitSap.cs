using MCS.DTO;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
    public class OrgunitSap : EntityBase
    {
        public string Code { get; set; }
        public string SystemStatus { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ParentCode { get; set; }
    }
}
