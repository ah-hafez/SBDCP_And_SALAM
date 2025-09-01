using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Security
{
    public interface IUserContext
    {
        TUser GetLoggedInUser<TUser>() where TUser : IUser;
    }
}
