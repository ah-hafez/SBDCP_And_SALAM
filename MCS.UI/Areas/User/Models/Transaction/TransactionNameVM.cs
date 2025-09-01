using System.Collections.Generic;
using MCS.Common;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionNameVM: EntityBase
    {
        public int? Id { get; set; }

        [CustomDisplayName("User.Transaction.Name.CivilID")]
        [CustomRequired("User.Transaction.Name.CivilIDRequired")]
        [CustomStringLength("User.Transaction.Name.CivilIDLength", 10, 10)]
        public string CivilID { get; set; }    //السجل المدني//

        [CustomDisplayName("User.Transaction.Name.Nationality")]
        public int? NationalityId { get; set; }  //الجنسية//

        [CustomDisplayName("User.Transaction.Name.FullName")]
        [CustomRequired("User.Transaction.Name.FullNameRequired")]
        [CustomStringLength("User.Transaction.Name.FullNameLength", 120, 0)]
        public string FirstName { get; set; }   //الاسم الاول//

        [CustomDisplayName("User.Transaction.Name.OtherInformation")]
        [CustomStringLength("User.Transaction.Name.OtherInformation", 200, 0)]
        public string OtherInformation { get; set; } // معلومات أخرى //

        [CustomDisplayName("User.Transaction.Name.MobileNumber")]
        //[CustomRequired("User.Transaction.Name.MobileNumberRequired")]
        [CustomStringLength("User.Transaction.Name.MobileNumberLength", 15, 10)]
        public string MobileNumber { get; set; }   //رقم الجوال//

        [CustomDisplayName("User.Transaction.Name.Phone")]
        [CustomStringLength("User.Transaction.Name.PhoneLength", 15, 10)]
        public string Phone { get; set; }  //رقم الهاتف//

        [CustomDisplayName("User.Transaction.Name.Email")]
        [CustomEmailAddress("User.Transaction.Name.EmailSyntax")]
        [CustomStringLength("User.Transaction.Name.ThirdNameLength", 150, 0)]
        public string Email { get; set; }   //البريد الإلكتروني//

        [CustomDisplayName("User.Transaction.Name.Address")]
        [CustomStringLength("User.Transaction.Name.AddressLength", 255, 0)]
        public string Address { get; set; } //العنوان//

        [CustomDisplayName("User.Transaction.Name.Title")]
        public int? TitleId { get; set; }//اللقب//

        [CustomDisplayName("User.Transaction.Name.RelativeRelation")]
        public string RelativeRelation { get; set; }

        [CustomDisplayName("User.Transaction.Name.City")]
        public string City { get; set; }

        [CustomRequired("User.Transaction.Name.GenderRequired")]
        [CustomDisplayName("User.Transaction.Name.Gender")]
        public int Gender { get; set; }
        public List<TransactionNameVM> Names { get; set; } = (AjaxGrid<TransactionNameVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionNameVM>(), 1, 0, false);

        public bool SendSMS { get; set; }

    }
}