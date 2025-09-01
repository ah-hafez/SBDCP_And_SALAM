using System.Security.Claims;

namespace MCS.UI.Areas.User.Models.Shared
{
    public class ExternalLoginInfoVM
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public ClaimsIdentity ExternalIdentity { get; set; }
        public string ProviderName { get; set; }
        public string ProviderKey { get; set; }
    }
}