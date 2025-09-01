using System.Collections.Generic;

namespace MCS.DTO
{
    public class IC_SUBJECTDTO
    {
        public string ITEM_CODE { get; set; }
        public string ITEM_DISPLAY { get; set; }
        public string ITEM_DESCRIPTION_AR { get; set; }
        public int? PARENT_ID { get; set; }
        public bool ACTIVE { get; set; }
        public int Id { get; set; }
        public bool HasChilds { get; set; }
        public string DirectoryNum { get; set; }
        //public int ClassificationId { get; set; }
        public bool IS_USED { get; set; }
        public string FULL_CODE { get; set; }
        public int? IcIndexId { get; set; }
        public int CONFID_ID { get; set; }
        public bool? Closed { get; set; }
    }
}
