namespace MCS.UI.TenantsAdmin.Models.Account
{
    public class ResetPasswordVM
    {

        public string UserName { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
        public string IdentityId { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
    }
}