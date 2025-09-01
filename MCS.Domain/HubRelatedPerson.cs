using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class HubRelatedPerson : EntityBase
    {
        public string Address { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string NationalId { get; set; }
    }
}
