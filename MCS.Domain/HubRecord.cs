using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class HubRecord : EntityBase
    {
        public string OuterText { get; set; }
        public string MethodName { get; set; }
    }
}
