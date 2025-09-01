
using DocumentFormat.OpenXml.Wordprocessing;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.DTO.Shared;
using MCS.Framework.Encryption;
using MCS.Framework.Localization;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Mappers;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Models.Hub;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.Survey;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Common;
using MCS.UI.Controls;
using MCS.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MCS.UI.Areas.User.Controllers
{
    public class OnlineUserController : BaseController
    {
        // GET: User/Survey
        public ActionResult Index()
        {
            try
            {
                GetResult<List<OnlineUserDTO>> onlineUserDTos =
                      HttpClientWrapper<GetResult<List<OnlineUserDTO>>>.GetItemRequest(string.Format("api/Common/GetOnlineUser?cultureName={0}", SessionInfo.CultureShortName)).Result;

                return View("~/Areas/User/Views/OnlineUsers/_OnlineUsersPartial.cshtml", onlineUserDTos.Result);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}