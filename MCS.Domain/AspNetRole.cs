using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class AspNetRole 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Discriminator { get; set; }
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
