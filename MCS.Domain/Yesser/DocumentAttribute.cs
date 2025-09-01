using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class DocumentAttribute : EntityBase
    {
        public int DocumentAttributeId { get; set; }

        public int DocumentNumber { get; set; }

        public int? DocumentSysNumber { get; set; }

        public int? DocumentTypeId { get; set; }

        public DateTime Date { get; set; }

        public string HijriDate { get; set; }

        public int? SubjectId { get; set; }

        public int? ConfidentialityId { get; set; }

        public int? PriorityId { get; set; }

        public string Remarks { get; set; }

        public int DocumentId { get; set; }

        public int? DestinationId { get; set; }

        public int? SourceId { get; set; }




    }
}
