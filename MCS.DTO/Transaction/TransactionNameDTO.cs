using MCS.Common;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class TransactionNameDTO
    {
        public int? Id { get; set; }     
        public string CivilID { get; set; }    //السجل المدني//
        public int? NationalityId { get; set; }  //الجنسية//
        public string FirstName { get; set; }   //الاسم الاول//
        //public string LastName { get; set; }    //الاسم الأخير//
        //public string SecondName { get; set; }  //الاسم الثاني//
        //public string ThirdName { get; set; }   //الاسم الثالث//
        public string OtherInformation { get; set; } // معلومات أخرى //
        public string MobileNumber { get; set; }   //رقم الجوال//
        public string Phone { get; set; }  //رقم الهاتف//
        public string Email { get; set; }   //البريد الإلكتروني//
        public string Address { get; set; } //العنوان//
        //public decimal? DueAmount { get; set; }   //المبلغ المستحق//
        //public string POBox { get; set; }  //صندوق البريد//
        //public string Fax { get; set; }    //الفاكس//
        //public string FourthName { get; set; }   //الاسم الرابع//
        public int? TitleId { get; set; }  //اللقب//
        public string RelativeRelation { get; set; }
        //public string PostCode { get; set; }
        public string City { get; set; }
        public int Gender { get; set; }
        public bool SendSMS { get; set; }
    }
}