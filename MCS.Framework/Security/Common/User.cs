using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Security
{
    [Serializable]
    public class User : IUser
    {
        public int Id { get; set; }
        public string IPAddress { get; set; }
        public virtual string UserName { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime? LastLogin { get; set; }
        public virtual IList<UserClaim> Claims { get; set; }
        public string Email { get; set; }

        public bool? PendingRegestration { get; set; }

        public string RequestId { get; set; }
        //public string TenantId { get; set; }
        public virtual bool HasClaim(string claimName)
        {
            return Claims.Any(c => c.Name == claimName);
        }
    }
}
