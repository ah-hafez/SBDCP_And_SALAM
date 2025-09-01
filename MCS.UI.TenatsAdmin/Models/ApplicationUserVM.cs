using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.TenantsAdmin.Models
{
    public class ApplicationUserVM
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public string SessionId { get; set; }
        public List<string> Claims { get; set; }
        public bool HasClaim(string claimName)
        {
            return Claims != null && Claims.Any(c => c == claimName);
        }
    }
}