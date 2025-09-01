using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MCS.Domain.IC;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class IC_SUBJECT : EntityBase
    {
        public string ITEM_CODE { get; set; }
        public string ITEM_DISPLAY { get; set; }
        public string ITEM_DESCRIPTION_AR { get; set; }
        public int? PARENT_ID { get; set; }

        [ForeignKey("PARENT_ID")]
        public virtual IC_SUBJECT Parent { get; set; }
        public bool ACTIVE { get; set; }
        public bool HasChilds { get; set; }
        public string Number { get; set; }
        //public virtual IC_CLASSIFICATION Classification { get; set; }
        //public int ClassificationId { get; set; }
        public bool IS_USED { get; set; }
        public string FULL_CODE { get; set; }
        public virtual IC_INDEX IcIndex { get; set; }
        public int? IcIndexId { get; set; }
        public int CONFID_ID { get; set; }
        public bool? Closed { get; set; }
        public virtual IList<IC_SUBJECTS_TRANSACTION> IC_SUBJECTS_TRANSACTIONS { get; set; }


    }
}
