using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.UserPreferences
{
    public class VerificationInfo
    {
        public int UserId { get; set; }
        [CustomDisplayName("Admin.User.Email")]
        [CustomEmailAddress("Admin.User.EmailSyntax")]
        [CustomRequired("Admin.User.EmailRequired")]
        [CustomStringLength("Admin.User.EmailLength", 50, 0)]
        public string Email { get; set; }

        [CustomDisplayName("User.VarificationCode")]
        [CustomRequired("User.VarificationCodeRequired")]
        public int? Code { get; set; }
        public string Title { get; set; }
        public VerificationType VerificationType { get; set; }
    }
}