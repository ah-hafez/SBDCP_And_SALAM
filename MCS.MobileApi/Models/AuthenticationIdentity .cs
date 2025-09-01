using System.Security.Principal;

namespace MobileApi.Models
{
    public class AuthenticationIdentity : GenericIdentity
    {
        public string UserName { get; set; }
        public int UserId { get; set; }

        public AuthenticationIdentity(string userName) : base(userName)
        {
            UserName = userName;
        }
    }
}