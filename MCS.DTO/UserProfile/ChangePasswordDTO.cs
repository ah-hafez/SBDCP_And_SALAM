using System.ComponentModel.DataAnnotations;
using MCS.Common.CustomAttributes;


namespace MCS.DTO
{
    public class ChangePasswordDTO
    {       
        //[CustomDisplayName("User.UserProfile.OldPassword")]
        [CustomRequired("User.UserProfile.OldPasswordRequierd")]
        public string OldPassword { get; set; }

        //[CustomDisplayName("User.UserProfile.NewPassword")]
        [CustomRequired("User.UserProfile.NewPasswordRequierd")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        //[CustomDisplayName("User.UserProfile.ReWritePassword")]
        [DataType(DataType.Password)]
        [CustomRequired("User.UserProfile.ReNewPasswordRequierd")]
        [CustomCompare("NewPassword", "User.UserProfile.ReNewPasswordCompare")]
        public string ReNewPassword { get; set; }
    }
}
