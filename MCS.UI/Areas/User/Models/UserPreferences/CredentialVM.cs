using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MCS.Common;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.UserPreferences
{
    public class CredentialVM
    {
        [DataType(DataType.Password)]
        [CustomDisplayName("UserPreferences.CurrentPassword")]
        [CustomRequired("User.CurrentPasswordRequired")]
        public string CurrentPassword { get; set; }

        [CustomDisplayName("UserPreferences.NewPassword")]
        [CustomRequired("User.NewPasswordRequired")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [CustomDisplayName("UserPreferences.ConfirmPassword")]
        [DataType(DataType.Password)]
        [CustomRequired("Global.ResetPassword.ReNewPasswordRequierd")]
        [CustomCompare("NewPassword", "Global.ResetPassword.ReNewPasswordCompare")]
        public string ConfirmPassword { get; set; }
        public string SignatureConfirmPasswordTxt { get; set; }
        public string SignatureCurrentPasswordTxt { get; set; }
        public string SignatureNewPasswordTxt { get; set; }
        public PasswordType PasswordType { get; set; } = PasswordType.None;
        public SigntureType SigntureType { get; set; } = SigntureType.Electronic;
    }
}