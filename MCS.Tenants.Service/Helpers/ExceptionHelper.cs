using System.Linq;
using System.Web.Http;

namespace MCS.Tenants.Service.Helpers
{
    public static class ExceptionHelper
    {
        public static string GetDeepestInnerException(this HttpError ex)
        {
            return $"{ex.RecurseDeepestInnerExceptions()}";
        }
        private static string RecurseDeepestInnerExceptions(this HttpError ex)
        {
            if (ex.InnerException != null && ex.InnerException.Any())
                return $"{ex.InnerException.RecurseDeepestInnerExceptions()}";
            return $"{ex.Message}:{ex.ExceptionMessage}";
        }
        //
        public static string GetAllExceptions(this HttpError ex)
        {
            return $"{ex.Message}:{ex.ExceptionMessage}. \n\r {ex.RecurseInnerExceptions()}";
        }

        private static string RecurseInnerExceptions(this HttpError ex)
        {
            var message = $"   Inner Exception: {ex.Message}:{ex.ExceptionMessage}.";
            if (ex.InnerException != null && ex.InnerException.Any())
                message += $"  \n\r {ex.InnerException.RecurseInnerExceptions()}";
            return message;
        }

    }
}