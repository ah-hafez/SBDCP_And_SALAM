using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MCS.Framework.Controls.Mvc;
using MCS.Framework.Localization;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Areas.Admin.Models.Groups;
using MCS.UI.Areas.Admin.Models.Permission;
using MCS.UI.Areas.User.Mappers.Lookups;
using CustomGrid = MCS.GridMvc.Ajax.GridExtensions;


namespace MCS.UI.Areas.Admin.Controllers
{
    public class PermissionsController : AdminControllerBase
    {
        // GET: Admin/Permissions
        public ActionResult Index()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                //GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.GetItemRequest(String.Format("api/Admin/GetAllPermissionsGroups?cultureName={0}", SessionInfo.CultureShortName)).Result;

                //TreeViewModel tree = BuildTree(PermissionMapper.Map(permissionGroupDTOs.Result), null);
                //PermissionsViewModel permissionsViewModel = new PermissionsViewModel();

                //permissionsViewModel.AddGroup.Permissions = new List<int>();

                //ViewData["PermissionsGroups"] = tree;

                GetResult<List<GroupDTO>> groups = HttpClientWrapper<GetResult<List<GroupDTO>>>
                    .GetItemRequest(string.Format("api/Admin/GetAllUserDefinedGroups?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                CustomGrid.IAjaxGrid grid = (CustomGrid.AjaxGrid<GroupVM>)new CustomGrid.AjaxGridFactory().CreateAjaxGrid(GroupMapper.Map(groups.Result).ToList(), 1, groups.RowsCount.Value, false, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult AddGroup()
        {
            GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
            ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

            GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.GetItemRequest(String.Format("api/Admin/GetAllPermissionsGroups?cultureName={0}&includeUserDefinedGroups={1}", SessionInfo.CultureShortName, false)).Result;

            TreeViewModel tree = BuildTree(PermissionMapper.Map(permissionGroupDTOs.Result), null);

            ViewData["PermissionsGroups"] = tree;

            return View("_GroupAddPartial");
        }

        [HttpPost]
        public ActionResult RolesSearch(int? page)
        {
            string parameters = GetListTransactionParameters(page);
            GetResult<List<GroupDTO>> groups = HttpClientWrapper<GetResult<List<GroupDTO>>>
                   .GetItemRequest(string.Format("api/Admin/GetAllUserDefinedGroups?{0}", parameters)).Result;

            CustomGrid.IAjaxGrid grid = (CustomGrid.AjaxGrid<GroupVM>)new CustomGrid.AjaxGridFactory().CreateAjaxGrid(GroupMapper.Map(groups.Result).ToList(), page ?? 1, groups.RowsCount.Value, page.HasValue, GridHelper.PageSize);

            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/Admin/Views/Roles/RolesGridPartial.cshtml", grid), JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult AddGroup(AddGroupVM addGroupVMs)
        {
            try
            {
                string message = string.Empty;

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);
                addGroupVMs.Name.IsActive = true;
                addGroupVMs.Name.CategoryId = (int)LookupCategory.CustomPermissionGroup;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostGroup", GroupMapper.Map(addGroupVMs)).Result;
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }


                GetResult<List<GroupDTO>> groupsDTOs = HttpClientWrapper<GetResult<List<GroupDTO>>>.GetItemRequest(string.Format("api/Admin/GetAllUserDefinedGroups?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (groupsDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, groupsDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<GroupVM>)new AjaxGridFactory().CreateAjaxGrid(GroupMapper.Map(groupsDTOs.Result).AsQueryable(), 1, false, groupsDTOs.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Permissions.AddSuccess");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_GroupGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditGroup(EditGroupVM editGroupVM)
        {
            try
            {
                string message = string.Empty;

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutGroup", GroupMapper.Map(editGroupVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<GroupDTO>> groupsDTOs = HttpClientWrapper<GetResult<List<GroupDTO>>>.GetItemRequest(string.Format("api/Admin/GetAllUserDefinedGroups?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (groupsDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, groupsDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<GroupVM>)new AjaxGridFactory().CreateAjaxGrid(GroupMapper.Map(groupsDTOs.Result).AsQueryable(), 1, false, groupsDTOs.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Permissions.UpdateSuccess");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_GroupGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetGroup(string id)
        {
            try
            {
                string message = string.Empty;

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                GetResult<EditGroupDTO> groupEditDTO = HttpClientWrapper<GetResult<EditGroupDTO>>.GetItemRequest(String.Format("api/Admin/GetGroupByID?groupId={0}", id)).Result;

                if (groupEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, groupEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                EditGroupVM editGroupVM = GroupMapper.Map(groupEditDTO.Result);

                GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.GetItemRequest(String.Format("api/Admin/GetAllPermissionsGroups?cultureName={0}&includeUserDefinedGroups={1}", SessionInfo.CultureShortName, false)).Result;

                List<PermissionGroupVM> permissionGroupVMs = PermissionMapper.Map(permissionGroupDTOs.Result);
                
                foreach (var item in editGroupVM.Permissions)
                {
                    if(permissionGroupVMs != null && permissionGroupVMs.Count > 0)
                    {
                        foreach (var group in permissionGroupVMs)
                        {
                            if (group.Permissions.Where(p => p.Id == item).SingleOrDefault() != null)
                            {
                                group.Permissions.Where(p => p.Id == item).SingleOrDefault().IsSelected = true;
                            }
                        }

                    }
                }

                TreeViewModel tree = BuildTree(permissionGroupVMs, null);

                ViewData["PermissionsGroups"] = tree;
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Permissions.UpdateSuccess");

                //return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_GroupEditPartial", editGroupVM), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
                return View("_GroupEditPartial", editGroupVM);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult DeleteGroup(string ids)
        {
            try
            {
                string message = string.Empty;

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                DeleteResult deleteResult = HttpClientWrapper<DeleteResult>.DeleteRequest(String.Format("api/Admin/DeleteGroups?ids={0}", ids)).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<GroupDTO>> groupsDTOs = HttpClientWrapper<GetResult<List<GroupDTO>>>.GetItemRequest(string.Format("api/Admin/GetAllUserDefinedGroups?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (groupsDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, groupsDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<GroupVM>)new AjaxGridFactory().CreateAjaxGrid(GroupMapper.Map(groupsDTOs.Result).AsQueryable(), 1, false, groupsDTOs.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Permissions.DeleteSuccess");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_GroupGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateGroupsGrid(int? page)
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                string parameters = GridHelper.GetGridParameters();

                GetResult<List<GroupDTO>> groupDTOs = HttpClientWrapper<GetResult<List<GroupDTO>>>.GetItemRequest(String.Format("api/Admin/GetAllUserDefinedGroups?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(GroupMapper.Map(groupDTOs.Result).AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, groupDTOs.RowsCount.Value);

                return Json(new { Html = grid.ToJson("_GroupGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult EditPermissionsName()
        {
            try
            {
                GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.GetItemRequest(string.Format("api/Admin/GetAllPermissionsGroups?cultureName={0}&includeUserDefinedGroups=false", SessionInfo.CultureShortName)).Result;

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_EditPermissionsNamePartial", PermissionMapper.Map(permissionGroupDTOs.Result)) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdatePermission(List<PermissionGroupVM> permissionGroupVMs)
        {
            try
            {
                string message = string.Empty;
                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Admin/UpdatePermissionsName?cultureName={0}", SessionInfo.CultureShortName), PermissionMapper.Map(permissionGroupVMs)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                TreeViewModel tree = BuildTree(permissionGroupVMs, null);

                AddGroupDTO addDTO = new AddGroupDTO();
                addDTO.Permissions = new List<int>();

                ViewData["PermissionsGroups"] = tree;

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Permissions.UpdateSuccess");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_PermissionTreePartial", GroupMapper.Map(addDTO)), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult RenderAddPermissionDialog(int groupId)
        {
            try
            {
                PermissionDTO permissionDTO = new PermissionDTO();

                permissionDTO.groupId = groupId;

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AddPermissionDialog", PermissionMapper.Map(permissionDTO)) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult RenderEditPermissionDialog(int PermissionId)
        {
            try
            {
                string message = string.Empty;

                GetResult<PermissionEditDTO> permissionEditDTO = HttpClientWrapper<GetResult<PermissionEditDTO>>.GetItemRequest(String.Format("api/Admin/GetPermissionByID?permissionId={0}", PermissionId)).Result;

                if (permissionEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, permissionEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_EditPermissionDialog", PermissionMapper.Map(permissionEditDTO.Result)), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddPermission(PermissionVM permissionVMs)
        {
            try
            {
                string message = string.Empty;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostPermission", PermissionMapper.Map(permissionVMs)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.GetItemRequest(String.Format("api/Admin/GetAllPermissionsGroups?cultureName={0}", SessionInfo.CultureShortName)).Result;

                ViewData["PermissionsGroups"] = BuildTree(PermissionMapper.Map(permissionGroupDTOs.Result), null);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Permissions.AddSuccess");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_PermissionTreePartial", new AddGroupVM()), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditPermission(PermissionEditVM permissionEditVMs)
        {
            try
            {
                string message = string.Empty;

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutPermission", PermissionMapper.Map(permissionEditVMs)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.GetItemRequest(String.Format("api/Admin/GetAllPermissionsGroups?cultureName={0}", SessionInfo.CultureShortName)).Result;

                ViewData["PermissionsGroups"] = BuildTree(PermissionMapper.Map(permissionGroupDTOs.Result), null);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Permissions.EditSuccess");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_PermissionTreePartial", new AddGroupVM()), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeletePermission(int permissionId)
        {
            try
            {
                string message = string.Empty;

                DeleteResult deleteResult = HttpClientWrapper<DeleteResult>.DeleteRequest(String.Format("api/Admin/DeletePermission?permissionId={0}", permissionId)).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.GetItemRequest(String.Format("api/Admin/GetAllPermissionsGroups?cultureName={0}", SessionInfo.CultureShortName)).Result;

                ViewData["PermissionsGroups"] = BuildTree(PermissionMapper.Map(permissionGroupDTOs.Result), null);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Permissions.DeleteSuccess");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_PermissionTreePartial", new AddGroupVM()), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ActivateDeactivateRole(int id)
        {
            try
            {
                string message = string.Empty;

                GetResult<GroupDTO> getResult = HttpClientWrapper<GetResult<GroupDTO>>
                    .GetItemRequest(string.Format("api/Admin/ActivateDeactivateRole?RoleId={0}&CultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GroupVM groupVM = GroupMapper.Map(getResult.Result);
                message = groupVM.IsActive ? "تم تفعيل الدور بنجاح" : "تم إلغاء تفعيل الدور بنجاح";

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/Admin/Views/Roles/RoleBoxPartial.cshtml", groupVM), MessageType = MessageType.Information, MessageText = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private TreeViewModel BuildTree(List<PermissionGroupVM> permissionGroupVMs, string rootName)
        {
            TreeViewModel tree = new TreeViewModel();
            List<TreeNode> nodes = new List<TreeNode>();

            tree.RootNode = new TreeNode { Id = 0, Name = rootName };
            if (permissionGroupVMs != null && permissionGroupVMs.Count > 0)
            {
                for (int i = 0; i < permissionGroupVMs.Count; i++)
                {
                    TreeNode groupNode = new TreeNode()
                    {
                        Id = permissionGroupVMs[i].Id,
                        ParentId = 0,
                        Name = permissionGroupVMs[i].Text,
                        IsUserDefined = permissionGroupVMs[i].IsUserDefined
                    };

                    foreach (PermissionVM permissionVM in permissionGroupVMs[i].Permissions)
                    {
                        TreeNode permissionNode = new TreeNode()
                        {
                            Id = permissionVM.Id,
                            ParentId = permissionGroupVMs[i].Id,
                            Name = permissionVM.Text,
                            IsSelected = permissionVM.IsSelected,
                            IsUserDefined = permissionVM.IsUserDefined
                        };

                        groupNode.Childs.Add(permissionNode);
                    }

                    tree.RootNode.Childs.Add(groupNode);
                }

            }

            nodes.Add(tree.RootNode);

            return tree;
        }

        public string GetListTransactionParameters(int? pageValue)
        {
            StringBuilder result = new StringBuilder();
            string filter = Request.Form["filter"];
            string sortColumnName = Request.Form["gridColumn"];
            string dir = Request.Form["dir"];
            string pageIndex = pageValue.HasValue ? pageValue.Value.ToString() : Request.Form["page"];
            string searchColumn = Request.Form["searchColumn"];
            string fromDate = Request.Form["fromDate"];
            string toDate = Request.Form["toDate"];
            string pageSize = Request.Form["pageSize"];
            result.Append("CultureName=").Append(SessionInfo.CultureShortName);
            FilterType filterType;
            if (!string.IsNullOrEmpty(filter))
            {
                string[] filterData = filter.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < filterData.Length; i++)
                {
                    string[] data = filterData[i].Split(new[] { "__" },
                    StringSplitOptions.RemoveEmptyEntries);
                    string filterValue = data.Count() == 3 ? data[2] : string.Empty;
                    string[] columnName = data[0].Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                    if (!Enum.TryParse(data[1], true, out filterType))
                    {
                        filterType = FilterType.Equals;
                    }
                    if (Convert.ToInt32(data[1]) == 2)
                    {
                        filterType = FilterType.Contains;
                    }
                    result.Append("&Filters[").Append(i).Append("].ColumnName=")
                          .Append(columnName[0]).Append("&Filters[").Append(i)
                          .Append("].Type=").Append(filterType).Append("&Filters[")
                          .Append(i).Append("].Value=").Append(filterValue);
                }
            }
            if (!string.IsNullOrEmpty(searchColumn))
            {
                string[] searchData = searchColumn.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < searchData.Length; i++)
                {
                    string[] data = searchData[i].Split(new[] { "__" },
                    StringSplitOptions.RemoveEmptyEntries);
                    result.Append("&SearchColunms[").Append(i).Append("].ColunmName=")
                          .Append(data[0]).Append("&SearchColunms[").Append(i)
                          .Append("].ColunmValue=").Append(data[1]);
                }
            }
            if (!string.IsNullOrEmpty(fromDate))
            {
                result.Append("&FromDate=").Append(fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                result.Append("&ToDate=").Append(toDate);
            }
            if (!string.IsNullOrEmpty(sortColumnName))
            {
                string[] sortData = sortColumnName.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                if (sortData.Length > 1)
                {
                    result.Append("&OrderBy=").Append(sortData[0]);
                }
                else
                {
                    result.Append("&OrderBy=").Append(sortData[0]);
                }
            }
            if (!string.IsNullOrEmpty(pageSize))
            {
                result.Append("&PageSize=").Append(pageSize);
            }
            else
            {
                result.Append("&PageSize=").Append(UIHelper.PageSize);
            }
            if (dir == "1")
            {
                result.Append("&Ascending=").Append(true);
            }
            else
            {
                result.Append("&Ascending=").Append(false);
            }
            if (!string.IsNullOrEmpty(pageIndex))
            {
                int page = Convert.ToInt32(pageIndex);
                result.Append("&PageIndex=").Append(page);
            }
            else
            {
                result.Append("&PageIndex=").Append(1);
            }
            return result.ToString();
        }
    }

}