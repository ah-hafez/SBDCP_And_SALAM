using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Logging;

namespace MCS.Framework.Exceptions
{
    public static class ExceptionHelper
    {
        public static bool HandleException(Exception exceptionToHandle, string policyName, out Exception exceptionToThrow)
        {
            Logger.WriteError(exceptionToHandle.Message);

            return Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicy.HandleException(exceptionToHandle, policyName, out exceptionToThrow);
        }

        public static bool HandleException(Exception exceptionToHandle, string policyName)
        {
            Logger.WriteError(exceptionToHandle.Message);

            return Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicy.HandleException(exceptionToHandle, policyName);
        }

        public static void HandleException(Exception exceptionToHandle)
        {
            Logger.WriteException(exceptionToHandle);
        }

        public static void HandleException(string message)
        {
            Logger.WriteError(message);
        }
    }
}
