using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class City : EntityBase 
    {
        public int CityId { get; set; }
        public virtual LocalizationIdentifier LocalizationIdentifier { get; set; }
        
        [NotMapped]
        public string Text { get; set; }
    }
}
