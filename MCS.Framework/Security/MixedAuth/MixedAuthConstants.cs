using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Security
{
    public class MixedAuthConstants
    {
        public const string DefaultAuthenticationType = "Windows";
        public const string TempCookieName = ".MixedAuth";
        public const int FakeStatusCode = 418;
    }
}
