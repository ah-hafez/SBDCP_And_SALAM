using Audit.EntityFramework;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
  
    public class Theme
    {
        public int Id { get; set; }
        public virtual LocalizationIdentifier LocalizationIdentifier { get; set; }
       public string Path { get; set; }
    }
}
