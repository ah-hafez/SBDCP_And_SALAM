using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace MCS.Business.ASPNETIdentity
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return Task.FromResult(0);
        }
    }
}
