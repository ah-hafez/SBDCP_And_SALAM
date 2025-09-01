using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class AssignmentPaperGroup : EntityBase
    {
        public int UserId { get; set; }
        // public virtual LocalizationIdentifier LocalizationIdentifier { get; set; }
        public string Name { get; set; }
        public int OrderNo { get; set; }
        public int DefaultActionId { get; set; }
        public virtual Action DefaultAction { get; set; }



    }
}
