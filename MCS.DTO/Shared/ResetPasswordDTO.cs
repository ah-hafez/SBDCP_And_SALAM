using System.ComponentModel.DataAnnotations;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class ResetPasswordDTO
    {
        [CustomRequired("Global.ResetPassword.UsernameRequired")]
        public string UserName { get; set; }

        [CustomRequired("Global.ResetPassword.EmailRequired")]
        public string Email { get; set; }

        [CustomRequired("Global.ResetPassword.NewPasswordRequierd")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [CustomRequired("Global.ResetPassword.ReNewPasswordRequierd")]
        [CustomCompare("NewPassword", "Global.ResetPassword.ReNewPasswordCompare")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
        public string IdentityId { get; set; }
        public string PhoneNumber { get; set; }

        [CustomRequired("Global.ResetPassword.CodeRequierd")]
        public string Code { get; set; }
    }
}
