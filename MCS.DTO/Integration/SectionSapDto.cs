using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class SectionSapDto
    {

        public string ExternalCode { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public DateTime? MdfSystemEffectiveEndDate { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string ExternalNameDefaultValue { get; set; }
        public string ExternalNameArSA { get; set; }
        public string MdfSystemStatus { get; set; }
        public string LastModifiedBy { get; set; }
        public string ExternalNameLocalized { get; set; }
        public string MdfSystemRecordStatus { get; set; }
        public string CustNameInArabic { get; set; }
        public string CreatedBy { get; set; }
        public string CustHeadOfUnit { get; set; }
        public string ExternalNameEnUS { get; set; }
        public IList<SectionSapDto> CustToDepartments { get; set; }
        public DateTime? StartDate { get; set; }
        public string NameLocalized { get; set; }
        public DateTime? EndDate { get; set; }
        public string EntityUUID { get; set; }
        public string Description { get; set; }
        public string DescriptionArSA { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string HeadOfUnit { get; set; }
        public string NameEnUS { get; set; }
        public string NameDefaultValue { get; set; }
        public string CostCenter { get; set; }
        public string DescriptionDefaultValue { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string NameArSA { get; set; }
        public string Name { get; set; }
        public string DescriptionEnUS { get; set; }
        public string MdfSystemRecordId { get; set; }
        public string DescriptionLocalized { get; set; }
        public string Status { get; set; }


    }


}
