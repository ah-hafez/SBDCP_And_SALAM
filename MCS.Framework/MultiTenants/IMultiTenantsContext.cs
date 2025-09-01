using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.MultiTenants
{
    public interface IMultiTenantsContext
    {
        TTenant GetLoggedInTenant<TTenant>() where TTenant : ITenant;
    }
}
