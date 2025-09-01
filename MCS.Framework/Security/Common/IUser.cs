using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Security
{
    public interface IUser
    {
        int Id { get; }
        string IPAddress { get; }
        string UserName { get; }
        bool IsActive { get; }
        DateTime? LastLogin { get; }
        bool HasClaim(string claimName);
        IList<UserClaim> Claims { get; set; }
        string RequestId { get; set; }
    }
}
