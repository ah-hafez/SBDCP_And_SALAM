using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Security
{
    public interface IMemeberShipProvider
    {
        ICustomSignInManager GetMemeberShipInstance();
        IApplicationUser GetMemeberShipApplicationUser();
    }
}
