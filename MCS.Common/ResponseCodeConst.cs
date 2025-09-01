using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Common
{
    public class ResponseCodeConst
    {

        public const string Success = "000";
        public const string ValidationError = "400";
        public const string UnAuthorizedRequest = "401";
        public const string SignatureMismatch = "403";
        public const string UserNotFound = "404";
        public const string InternalServerError = "500";

    }
}
