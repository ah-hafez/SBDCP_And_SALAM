using System;
using System.ComponentModel.DataAnnotations;

namespace MCS.DTO
{
    public class DocumentAttributeDTO
    {
        [Required]
        public int DocumentAttributeId { get; set; }
        [Required]
        public int DocumentNumber { get; set; }

        public int DocumentSysNumber { get; set; }

        public int DocumentTypeId { get; set; }

        public DateTime Date { get; set; }

        public string HijriDate { get; set; }

        public int SubjectId { get; set; }

        public int ConfidentialityId { get; set; }

        public int PriorityId { get; set; }

        public string Remarks { get; set; }

        [Required]
        public int DocumentId { get; set; }

        public int DestinationId { get; set; }

        public int SourceId { get; set; }




    }
}
