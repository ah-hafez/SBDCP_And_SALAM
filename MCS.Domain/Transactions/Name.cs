using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using MCS.Common;

namespace MCS.Domain
{
    public class Name : EntityBase, IAuditable
    {
        public string CivilID { get; set; }
        public int? NationalityId { get; set; }
        public virtual Lookup Nationality { get; set; }
        public string FirstName { get; set; }
        //public string SecondName { get; set; }
        //public string ThirdName { get; set; }
        //public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        //public decimal? DueAmount { get; set; }
        //public string POBox { get; set; }
        //public string Fax { get; set; }
        public string OtherInformation { get; set; } // معلومات أخرى //
        //public string FourthName { get; set; }   //الاسم الرابع//
         public int? TitleId { get; set; }//اللقب//
        public virtual Lookup Title { get; set; }
        public string RelativeRelation { get; set; }
        //public string PostCode { get; set; }
        public string City { get; set; }
        public int Gender { get; set; }
    }
}
