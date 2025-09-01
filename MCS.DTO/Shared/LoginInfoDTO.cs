using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class LoginInfoDTO
    {
        [CustomRequired("Global.Login.UserNameRequired")]
        [CustomStringLength("Global.Login.UserNameRequired", 20, 0)]
        public string UserName { get; set; }

        [CustomRequired("Global.Login.PasswordRequired")]
        [CustomStringLength("Global.Login.PasswordRequired", 20, 0)]
        public string Password { get; set; }

        public bool IsWindowsLogin { get; set; }

        public bool RememberMe { get; set; }

        public string grant_type
        {
            get { return "password"; }
        }
    }
}
