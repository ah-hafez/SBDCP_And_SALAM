using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class ReporterDTO
    {
        public int Id { get; set; }
        public int ToEntityId { get; set; }
        public string ToEntityName { get; set; }
        public string LocalName { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
        public List<LocalizationDTO> Names { get; set; }
    }
}
