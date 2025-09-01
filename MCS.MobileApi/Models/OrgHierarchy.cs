using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApi.Models
{
    public class OrgHierarchy
    {
        public List<Entity> Entities { get; set; }
        public List<Entity> externalEntities { get; set; }
    }
}