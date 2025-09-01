using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.MultiTenants
{
    public interface ITenant
    {
        int Id { get; }
        string HostName { get; }
        string DatabaseName { get; }
    }
}
