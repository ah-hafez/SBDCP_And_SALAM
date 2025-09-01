using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain.IC
{
    public class IC_DOCS : EntityBase, IAuditable
    { 
        public virtual IC_FILE File { get; set; }
        public int? FileId { get; set; }
        public virtual Transaction Transaction { get; set; }
        public int TransactionId { get; set; }
        public int? DOC_ORIGIN_SOURCE { get; set; }
        public virtual IC_DOC_STATUS Status { get; set; }
        public int? StatusId { get; set; }
        public int? USER_ID { get; set; }
        public string SYSTEM_ID { get; set; }
        public string IP { get; set; }
        public string REMARKS_AR { get; set; }
        public int OFFICE_ID { get; set; }
        public virtual IC_INDEX IcIndex { get; set; }
        public int? IcIndexId { get; set; }
        public DateTime? ARCHIVE_DATE { get; set; }
        public string ARCHIVE_DATE_HJ { get; set; }
        public short? ARCHIVE_TYPE { get; set; }
        public int? SITE_ID { get; set; }
        public bool? ACTIVE { get; set; }
        public virtual IC_FILE_PARTS FilePart { get; set; }
        public int? FilePartId { get; set; }
        public string CLASSIFICATION_DATE_HJ { get; set; }
        public int? PARENT_TRANS_ID { get; set; }
        public int? STATUS_FLAG { get; set; }
        public int? PARENT_TRANS_CATEGORY { get; set; }
        public int? TRANS_COPY_ID { get; set; }
        public int? IC_DOC_NO { get; set; }
        public bool? IS_PHYSICAL_TRANS { get; set; }
        public int? TransSerial { get; set; }
        public int? IncludedItemSerial { get; set; }

    }

}
