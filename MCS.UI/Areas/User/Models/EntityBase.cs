using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models
{
    public class EntityBase
    {
        public int Key { get; set; }
        public bool IsEnableAction { get; set; } = true;
    }
}