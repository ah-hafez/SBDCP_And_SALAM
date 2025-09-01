using MCS.Framework.Security;
using MCS.Framework.Web;
using MCS.Common.TransactionContext;
using MCS.DataAccess;

namespace MCS.Business
{
    public class BaseBL
    {
        protected User User { get; set; }

        public BaseBL()
        {
            User = (User)UserContext.LoggedInUser;
        }
    }
}
