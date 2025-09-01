using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class ConfidentialityLevelDTO 
    {
        public int Id { get; set; }
        public virtual IList<LocalizationDTO> LocalizationDTOs { get; set; }
    }
}
