using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class TaskActionDTO
    {
        public int TaskId { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public List<DocumentDTO> Document { get; set; }       
    }
}
