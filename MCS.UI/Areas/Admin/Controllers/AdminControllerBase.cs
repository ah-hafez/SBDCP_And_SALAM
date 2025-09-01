using MCS.Common;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Common;
using MCS.UI.Helpers;
using System.Net.Http;
using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using MCS.UI.Areas.Admin.Models.OrgUnit;
using MCS.Common.ApiControllerResults;

namespace MCS.UI.Areas.Admin.Controllers
{
    [CustomAuthorizationAttribute(UserClaims.Admin.Administrator)]
    public class AdminControllerBase : BaseController
    {
        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.GenerateKeyAPI)]
        public ActionResult GenerateHashKey()
        {

            return Json(new { Key = SecurityHelper.GenerateHashKey() }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult UpdateOrgUnitHierarchy()
        {
            try
            {
                OrgHelper.UpdateOrgUnitService();
                return Json(new { MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;

            }
        }
    }



}