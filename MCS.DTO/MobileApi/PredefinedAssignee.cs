using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApi.Domain
{
    public class PredefinedAssignee
    {
        public int PartyID { get; set; }

        public int PersonID { get; set; }

        public string EntityName { get; set; }

        public string PersonName { get; set; }        
    }
}