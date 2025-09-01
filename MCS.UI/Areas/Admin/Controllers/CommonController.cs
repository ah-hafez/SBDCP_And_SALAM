using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Areas.Admin.Models;
using MCS.UI.Areas.Admin.Models.OrgUnit;
using MCS.UI.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MCS.UI.Areas.Admin.Controllers
{
    public class CommonController : AdminControllerBase
    {
        // GET: Admin/Shared
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetInternalPartyChildren(OrgHierarchyTreeViewModel treeVM)
        {
            try
            {
                if (treeVM.SelectedNode == -1)
                {
                    treeVM.SelectedNode = null;
                }
                List<OrgUnitDTO> orgUnitsVM = new List<OrgUnitDTO>();

                var orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>
                    .GetItemRequest(string.Format("api/Common/GetOrgUnits?cultureName={0}&parentId={1}&UserId={2}&orgUnitTreeMode={3}", SessionInfo.CultureShortName, treeVM.SelectedNode, treeVM.UserId, treeVM.OrgUnitTreeMode)).Result;



                OrgHierarchyTreeViewModel treeViewModel = new OrgHierarchyTreeViewModel()
                {
                    GetChildrenActionURL = treeVM.GetChildrenActionURL,
                    GetChildrenActionParameters = treeVM.GetChildrenActionParameters,
                    CallBackFunction = treeVM.CallBackFunction,
                    TreeId = treeVM.TreeId,
                    OrgUnitTreeMode = treeVM.OrgUnitTreeMode,
                    Nodes = orgUnitDTOs.Result.Select(x => new OrgHierarchyTreeNodeViewModel()
                    {
                        DepartmentNumber = x.Number.ToString(),
                        IsSelected = x.IsSelected,
                        IsSelectable = x.IsVirtualUnit ? false : true,
                        Name = x.Name,
                        Id = x.Id,
                        HasChilds = x.HasChilds && !treeVM.UserId.HasValue,
                        IsYesserRegistered = false,
                        ParentId = treeVM.SelectedNode
                    }).ToList()
                };

                if (treeVM.SelectedNode.HasValue)
                {
                    return PartialView("~/Areas/Admin/Views/Shared/EditorTemplates/OrgHierarchyModalItem.cshtml", treeViewModel);
                }
                else
                {
                    return PartialView("~/Areas/Admin/Views/Shared/EditorTemplates/OrgHierarchyModal.cshtml", treeViewModel);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetInternalPartyInfoById(string partyId)
        {
            GetResult<OrgUnitDTO> orgUnitDTO =
                   HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, partyId)).Result;
            OrgUnitVM orgUnitVM = OrgUnitMapper.Map(orgUnitDTO.Result);

            return new JsonResult() { Data = new OrgHierarchyTreeNodeViewModel() { Id = orgUnitVM.Id, DepartmentNumber = orgUnitVM.Number.ToString(), Name = orgUnitVM.Name }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetInternalPartyInfoByNumber(string partyNumber)
        {
            GetResult<OrgUnitDTO> orgUnitDTO =
                   HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetInternalPartyInfoByNumber?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, partyNumber)).Result;
            OrgUnitVM orgUnitVM = OrgUnitMapper.Map(orgUnitDTO.Result);
            return new JsonResult() { Data = new OrgHierarchyTreeNodeViewModel() { Id = orgUnitVM.Id, DepartmentNumber = orgUnitVM.Number.ToString(), Name = orgUnitVM.Name }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        [HttpGet]
        public ActionResult GetUserImage(int UserImageId)
        {
            GetResult<DocumentDTO> documentDTO =
                           HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(String.Format("api/Document/GetDocumentById?cultureName={0}&documentId={1}", SessionInfo.CultureShortName, UserImageId)).Result;

            var documentVM = DocumentMapper.Map(documentDTO.Result);

            byte[] userImage = documentVM.Content;

            if (userImage != null)
                return new FileContentResult(userImage, "image/jpeg");

            return null;
        }


        [HttpGet]
        public ActionResult ConvertLanguage()
        {
            CultureInfo cultureInfo;
            HttpCookie cookieTemp;
            var arCulture = ConfigurationManager.AppSettings["DefaultArabicCulture"].ToString();
            var enCulture = ConfigurationManager.AppSettings["DefaultEnglishCulture"].ToString();
            if (SessionInfo.CultureShortName == "en")
            {
                cultureInfo = new CultureInfo(arCulture);
                cookieTemp = cultureInfo.SetCookieCulture(arCulture);
            }
            else
            {
                cultureInfo = new CultureInfo(enCulture);
                cookieTemp = cultureInfo.SetCookieCulture(enCulture);
            }



            Response.Cookies.Add(cookieTemp);
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

            return Redirect("~/Admin");
        }

    }
}