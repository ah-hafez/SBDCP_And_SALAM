using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class AssignmentPaperGroupDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        // public List<LocalizationDTO> Names { get; set; }
        public string Name { get; set; }
        public int OrderNo { get; set; }
        public int DefaultActionId { get; set; }
        public string DefaultActionName { get; set; }


    }
}
