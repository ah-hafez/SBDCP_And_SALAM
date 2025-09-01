using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.TenantsAdmin.Models.Account
{
    public class LoginInfoVM
    {

        public string UserName { get; set; }


        public string Password { get; set; }

        public bool IsWindowsLogin { get; set; }

        public bool RememberMe { get; set; }

        public string grant_type
        {
            get { return "password"; }
        }
    }
}