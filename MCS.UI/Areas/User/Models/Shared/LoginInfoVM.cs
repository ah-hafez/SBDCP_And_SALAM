using System.ComponentModel.DataAnnotations;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Shared
{
    public class LoginInfoVM
    {
        [CustomRequired("Global.Login.UserNameRequired")]
        //[CustomStringLength("Global.Login.UserNameRequired", 20, 0)]
        public string UserName { get; set; }

        [CustomRequired("Global.Login.PasswordRequired")] 
        //[CustomStringLength("Global.Login.PasswordRequired", 20, 0)]
        public string Password { get; set; }

        public bool IsWindowsLogin { get; set; }

        public bool RememberMe { get; set; }

        public string grant_type
        {
            get { return "password"; }
        }
    }
}