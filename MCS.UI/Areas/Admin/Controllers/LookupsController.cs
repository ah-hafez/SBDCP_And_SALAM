using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MCS.Framework.Controls;
using MCS.Framework.Controls.Mvc;
using MCS.Framework.Localization;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Areas.Admin.Models;
using MCS.UI.Areas.Admin.Models.Actions;
using MCS.UI.Areas.Admin.Models.Lookups;
using MCS.UI.Areas.Admin.Models.OrgUnit;
using MCS.UI.Areas.Admin.Models.Permission;
using CustomAjaxGrid = MCS.GridMvc.Ajax.GridExtensions;
using UserDocuments = MCS.UI.Areas.User.Mappers.Shared;
using UserLookups = MCS.UI.Areas.User.Mappers.Lookups;
using System.Web;
using System.IO;
using MCS.Framework.MultiTenants;
using MCS.DTO;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace MCS.UI.Areas.Admin.Controllers
{
    public class LookupsController : AdminControllerBase
    {
        #region Shared

        private TreeViewModel BuildTree(List<PermissionGroupVM> permissionGroupVMs, string rootName = null)
        {
            TreeViewModel tree = new TreeViewModel();
            List<TreeNode> nodes = new List<TreeNode>();

            tree.RootNode = new TreeNode { Id = 0, Name = rootName, Mode = tree.Mode };

            for (int i = 0; i < permissionGroupVMs.Count; i++)
            {
                TreeNode groupNode = new TreeNode()
                {
                    Id = permissionGroupVMs[i].Id,
                    ParentId = 0,
                    Name = permissionGroupVMs[i].Text,
                    IsUserDefined = permissionGroupVMs[i].IsUserDefined
                };

                foreach (PermissionVM permission in permissionGroupVMs[i].Permissions)
                {
                    TreeNode permissionNode = new TreeNode()
                    {
                        Id = permission.Id,
                        ParentId = permissionGroupVMs[i].Id,
                        Name = permission.Text,
                        IsSelected = permission.IsSelected,
                        IsUserDefined = permission.IsUserDefined
                    };

                    groupNode.Childs.Add(permissionNode);
                }

                tree.RootNode.Childs.Add(groupNode);
            }

            nodes.Add(tree.RootNode);

            return tree;
        }

        private List<TransactionCategoryVM> GetTransactionCategoryLookups()
        {
            GetResult<IList<LookupVM>> lookupVMs = LookupsHelper.GetAdminLookupItems(LookupCategory.TransactionCategories, SessionInfo.CultureShortName);
            List<TransactionCategoryVM> transactionCategoryVMs = new List<TransactionCategoryVM>();

            if (lookupVMs != null)
            {
                foreach (LookupVM lookupVM in lookupVMs.Result)
                {
                    transactionCategoryVMs.Add(new TransactionCategoryVM()
                    {
                        Id = (lookupVM.EnumReference != null) ? lookupVM.EnumReference.Value : -1,
                        Text = lookupVM.Text,
                    });
                }
            }

            return transactionCategoryVMs;
        }

        private List<TransactionCategoryVM> MergeTransactionCategoryLookups(List<TransactionCategoryVM> transactionCategoryVMs)
        {
            List<TransactionCategoryVM> localizeTransactionCategoryVMs = GetTransactionCategoryLookups();

            foreach (TransactionCategoryVM transactionCategoryVM in transactionCategoryVMs)
            {
                if (localizeTransactionCategoryVMs.Where(l => l.Id == transactionCategoryVM.Id &&
                    transactionCategoryVM.IsSelected == true).SingleOrDefault() != null)
                {
                    localizeTransactionCategoryVMs.Where(l => l.Id == transactionCategoryVM.Id &&
                        transactionCategoryVM.IsSelected == true).SingleOrDefault().IsSelected = true;
                }
            }

            return localizeTransactionCategoryVMs;
        }

        private List<LetterListTypeVM> GetLetterListTypeLookups()
        {
            GetResult<IList<LookupVM>> lookupVMs = LookupsHelper.GetAdminLookupItems(LookupCategory.LetterListType, SessionInfo.CultureShortName);
            List<LetterListTypeVM> letterTypeListVMs = new List<LetterListTypeVM>();

            foreach (LookupVM lookupVM in lookupVMs.Result)
            {
                letterTypeListVMs.Add(new LetterListTypeVM()
                {
                    Id = (lookupVM.EnumReference != null) ? lookupVM.EnumReference.Value : -1,
                    Text = lookupVM.Text
                });
            }

            return letterTypeListVMs;
        }

        private List<LetterListTypeVM> MergeLetterListTypeLookups(List<LetterListTypeVM> letterTypeListVMs)
        {
            List<LetterListTypeVM> localizeLetterTypeListVMs = GetLetterListTypeLookups();

            foreach (LetterListTypeVM letterTypeListVM in letterTypeListVMs)
            {
                if (localizeLetterTypeListVMs.Where(l => l.Id == letterTypeListVM.Id &&
                    letterTypeListVM.IsSelected == true).SingleOrDefault() != null)
                {
                    localizeLetterTypeListVMs.Where(l => l.Id == letterTypeListVM.Id &&
                        letterTypeListVM.IsSelected == true).SingleOrDefault().IsSelected = true;
                }
            }

            return localizeLetterTypeListVMs;
        }

        private List<SpecificListLevelVM> GetSpecificLevelListLookups()
        {
            GetResult<IList<LookupVM>> lookupVMs = LookupsHelper.GetAdminLookupItems(LookupCategory.LetterListType, SessionInfo.CultureShortName);
            List<SpecificListLevelVM> specificListLevelVMs = new List<SpecificListLevelVM>();

            foreach (LookupVM lookupVM in lookupVMs.Result)
            {
                specificListLevelVMs.Add(new SpecificListLevelVM()
                {
                    Id = (lookupVM.EnumReference != null) ? lookupVM.EnumReference.Value : -1,
                    Text = lookupVM.Text
                });
            }

            return specificListLevelVMs;
        }

        private List<SpecificListLevelVM> MergeSpecificLevelListLookups(List<SpecificListLevelVM> SpecificListLevelVMs)
        {
            List<SpecificListLevelVM> localizeSpecificLevelListVMs = GetSpecificLevelListLookups();

            foreach (SpecificListLevelVM specificListLevelVM in SpecificListLevelVMs)
            {
                if (localizeSpecificLevelListVMs.Where(l => l.Id == specificListLevelVM.Id &&
                    specificListLevelVM.IsSelected == true).SingleOrDefault() != null)
                {
                    localizeSpecificLevelListVMs.Where(l => l.Id == specificListLevelVM.Id &&
                        specificListLevelVM.IsSelected == true).SingleOrDefault().IsSelected = true;
                }
            }

            return localizeSpecificLevelListVMs;
        }

        #endregion Shared



        #region FollowUpProccess
        public ActionResult FollowUpProccess()
        {

            GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

            ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);
            FollowUpLookUpsViewModel followUpProccessViewModel = new FollowUpLookUpsViewModel();
            List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups();

            followUpProccessViewModel.AddFollowUpLookUps.TransactionCategories = transactionCategoryVMs;
            followUpProccessViewModel.EditFollowUpLookUps.TransactionCategories = transactionCategoryVMs;

            GetResult<List<FollowUpLookUpDTO>> followUpProccessDTOs =
                HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpProccess?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

            List<FollowUpLookUpsVM> followpProccessVMs = FollowUpLookUpsMapper.Map(followUpProccessDTOs.Result);
            if (followpProccessVMs == null)
            {
                followpProccessVMs = new List<FollowUpLookUpsVM>();
                followUpProccessDTOs.RowsCount = 0;
            }

            //IAjaxGrid grid = (AjaxGrid<LinkVM>)new AjaxGridFactory().CreateAjaxGrid(linkVMs.AsQueryable(), 1, false, linkDTOs.RowsCount.Value);
            CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(followpProccessVMs, 1, followUpProccessDTOs.RowsCount.Value, false, GridHelper.PageSize);

            ViewData["GridData"] = grid;



            return View(followUpProccessViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddFollowUpProccess(FollowUpLookUpsAddVM followUpProccessVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostFollowUpProccess", FollowUpLookUpsMapper.Map(followUpProccessVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<FollowUpLookUpDTO>> FollowUpProccessDTOs =
                HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpProccess?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (FollowUpProccessDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, FollowUpProccessDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<AttachmentTypeVM>)new AjaxGridFactory().CreateAjaxGrid(AttachmentTypeMapper.Map(attachmentTypeDTOs.Result).AsQueryable(), 1, false, attachmentTypeDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FollowUpLookUpsMapper.Map(FollowUpProccessDTOs.Result), 1, FollowUpProccessDTOs.RowsCount.Value, false);


                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.FollowUpProccess.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FollowUpProccessGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditFollowUpProccess(FollowUpLookUpsEditVM followUpProccessEditVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PutResult PutResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutFollowUpProccess", FollowUpLookUpsMapper.Map(followUpProccessEditVM)).Result;

                if (PutResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, PutResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<FollowUpLookUpDTO>> followUpProccessDTOs =
                     HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpProccess?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (followUpProccessDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, followUpProccessDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<LinkVM>)new AjaxGridFactory().CreateAjaxGrid(LinkMapper.Map(linkDTOs.Result).AsQueryable(), 1, false, linkDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FollowUpLookUpsMapper.Map(followUpProccessDTOs.Result), 1, followUpProccessDTOs.RowsCount.Value, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.FollowUpProccess.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FollowUpProccessGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public ActionResult DeleteFollowUpProccess(string ids)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/DeleteFollowUpProccess?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<FollowUpLookUpDTO>> followUpProccessDTOs =
                   HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpProccess?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (followUpProccessDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, followUpProccessDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<LinkVM>)new AjaxGridFactory().CreateAjaxGrid(LinkMapper.Map(linkDTOs.Result).AsQueryable(), 1, false, linkDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FollowUpLookUpsMapper.Map(followUpProccessDTOs.Result), 1, followUpProccessDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.FollowUpProccess.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FollowUpProccessGridPartial", grid), LinksUsedList = deleteResult.Result, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult GetFollowUpProccess(string id)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                GetResult<FollowUpLookUpEditDTO> followUpProccessEditDTO =
                    HttpClientWrapper<GetResult<FollowUpLookUpEditDTO>>.GetItemRequest(String.Format("api/Admin/GetFollowUpProccessById?followUpSourceId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (followUpProccessEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, followUpProccessEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.TransactionLink.UpdateSucceeded");

                FollowUpLookUpsEditVM followUpSourcesEditVM = FollowUpLookUpsMapper.Map(followUpProccessEditDTO.Result);
                followUpSourcesEditVM.TransactionCategories = MergeTransactionCategoryLookups(followUpSourcesEditVM.TransactionCategories);

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FollowUpProccessEditPartial", followUpSourcesEditVM
                    ),
                    MessageText = message,
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult UpdateFollowUpProccessGrid(int? page)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                string parameters = GridHelper.GetGridParameters();

                GetResult<List<FollowUpLookUpDTO>> followUpProccessDTOs = HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(String.Format("api/Admin/GetFollowUpProccess?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                if (followUpProccessDTOs.StatusCode != StatusCode.Ok)
                {
                    //this.ShowMessage(MessageType.Error, priorityEditDTO.Item2.ToString());
                }

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FollowUpLookUpsMapper.Map(followUpProccessDTOs.Result), page ?? 1, followUpProccessDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                return Json(new { Html = grid.ToJson("_FollowUpProccessGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion FollowUpSource 
        #region FollowUpSource
        public ActionResult FollowUpSource()
        {

            GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

            ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);
            FollowUpLookUpsViewModel followUpSourceViewModel = new FollowUpLookUpsViewModel();
            List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups();

            followUpSourceViewModel.AddFollowUpLookUps.TransactionCategories = transactionCategoryVMs;
            followUpSourceViewModel.EditFollowUpLookUps.TransactionCategories = transactionCategoryVMs;

            GetResult<List<FollowUpLookUpDTO>> followUpSourceDTOs =
                HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpSource?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

            List<FollowUpLookUpsVM> followpSourceVMs = FollowUpLookUpsMapper.Map(followUpSourceDTOs.Result);
            if (followpSourceVMs == null)
            {
                followpSourceVMs = new List<FollowUpLookUpsVM>();
                followUpSourceDTOs.RowsCount = 0;
            }

            //IAjaxGrid grid = (AjaxGrid<LinkVM>)new AjaxGridFactory().CreateAjaxGrid(linkVMs.AsQueryable(), 1, false, linkDTOs.RowsCount.Value);
            CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(followpSourceVMs, 1, followUpSourceDTOs.RowsCount.Value, false, GridHelper.PageSize);

            ViewData["GridData"] = grid;



            return View(followUpSourceViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddFollowUpSource(FollowUpLookUpsAddVM followUpSourcesVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostFollowUpSource", FollowUpLookUpsMapper.Map(followUpSourcesVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<FollowUpLookUpDTO>> FollowUpSourceDTOs =
                HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpSource?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (FollowUpSourceDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, FollowUpSourceDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<AttachmentTypeVM>)new AjaxGridFactory().CreateAjaxGrid(AttachmentTypeMapper.Map(attachmentTypeDTOs.Result).AsQueryable(), 1, false, attachmentTypeDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FollowUpLookUpsMapper.Map(FollowUpSourceDTOs.Result), 1, FollowUpSourceDTOs.RowsCount.Value, false);


                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.FollowUpSource.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FollowUpSourceGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditFollowUpSource(FollowUpLookUpsEditVM followUpSourcesEditVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PutResult PutResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutFollowUpSource", FollowUpLookUpsMapper.Map(followUpSourcesEditVM)).Result;

                if (PutResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, PutResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<FollowUpLookUpDTO>> followUpSourceDTOs =
                     HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpSource?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (followUpSourceDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, followUpSourceDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<LinkVM>)new AjaxGridFactory().CreateAjaxGrid(LinkMapper.Map(linkDTOs.Result).AsQueryable(), 1, false, linkDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FollowUpLookUpsMapper.Map(followUpSourceDTOs.Result), 1, followUpSourceDTOs.RowsCount.Value, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.FollowUpSource.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FollowUpSourceGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public ActionResult DeleteFollowUpSource(string ids)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/DeleteFollowUpSource?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<FollowUpLookUpDTO>> followUpSourceDTOs =
                   HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpSource?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (followUpSourceDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, followUpSourceDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<LinkVM>)new AjaxGridFactory().CreateAjaxGrid(LinkMapper.Map(linkDTOs.Result).AsQueryable(), 1, false, linkDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FollowUpLookUpsMapper.Map(followUpSourceDTOs.Result), 1, followUpSourceDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.FollowUpSource.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FollowUpSourceGridPartial", grid), LinksUsedList = deleteResult.Result, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult GetFollowUpSource(string id)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                GetResult<FollowUpLookUpEditDTO> followUpSourceEditDTO =
                    HttpClientWrapper<GetResult<FollowUpLookUpEditDTO>>.GetItemRequest(String.Format("api/Admin/GetFollowUpSourceById?followUpSourceId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (followUpSourceEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, followUpSourceEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.TransactionLink.UpdateSucceeded");

                FollowUpLookUpsEditVM followUpSourcesEditVM = FollowUpLookUpsMapper.Map(followUpSourceEditDTO.Result);
                followUpSourcesEditVM.TransactionCategories = MergeTransactionCategoryLookups(followUpSourcesEditVM.TransactionCategories);

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FollowUpSourceEditPartial", followUpSourcesEditVM
                    ),
                    MessageText = message,
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult UpdateFollowUpSourceGrid(int? page)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                string parameters = GridHelper.GetGridParameters();

                GetResult<List<FollowUpLookUpDTO>> followUpSourceDTOs = HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(String.Format("api/Admin/GetFollowUpSource?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                if (followUpSourceDTOs.StatusCode != StatusCode.Ok)
                {
                    //this.ShowMessage(MessageType.Error, priorityEditDTO.Item2.ToString());
                }

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FollowUpLookUpsMapper.Map(followUpSourceDTOs.Result), page ?? 1, followUpSourceDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                return Json(new { Html = grid.ToJson("_FollowUpSourceGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion FollowUpSource
        #region FollowUpMethod
        public ActionResult FollowUpMethod()
        {

            GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

            ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);
            FollowUpLookUpsViewModel followUpMethodViewModel = new FollowUpLookUpsViewModel();
            List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups();

            followUpMethodViewModel.AddFollowUpLookUps.TransactionCategories = transactionCategoryVMs;
            followUpMethodViewModel.EditFollowUpLookUps.TransactionCategories = transactionCategoryVMs;

            GetResult<List<FollowUpLookUpDTO>> followUpMethodDTOs =
                HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpMethod?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

            List<FollowUpLookUpsVM> followpMethodVMs = FollowUpLookUpsMapper.Map(followUpMethodDTOs.Result);
            if (followpMethodVMs == null)
            {
                followpMethodVMs = new List<FollowUpLookUpsVM>();
                followUpMethodDTOs.RowsCount = 0;
            }
            CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(followpMethodVMs, 1, followUpMethodDTOs.RowsCount.Value, false, GridHelper.PageSize);

            ViewData["GridData"] = grid;

            return View(followUpMethodViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddFollowUpMethod(FollowUpLookUpsAddVM followUpMethodsVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostFollowUpMethod", FollowUpLookUpsMapper.Map(followUpMethodsVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<FollowUpLookUpDTO>> FollowUpMethodDTOs =
                HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpMethod?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (FollowUpMethodDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, FollowUpMethodDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FollowUpLookUpsMapper.Map(FollowUpMethodDTOs.Result), 1, FollowUpMethodDTOs.RowsCount.Value, false);


                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.FollowUpMethod.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FollowUpMethodGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditFollowUpMethod(FollowUpLookUpsEditVM followUpMethodsEditVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PutResult PutResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutFollowUpMethod", FollowUpLookUpsMapper.Map(followUpMethodsEditVM)).Result;

                if (PutResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, PutResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<FollowUpLookUpDTO>> followUpMethodDTOs =
                     HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpMethod?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (followUpMethodDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, followUpMethodDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<LinkVM>)new AjaxGridFactory().CreateAjaxGrid(LinkMapper.Map(linkDTOs.Result).AsQueryable(), 1, false, linkDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FollowUpLookUpsMapper.Map(followUpMethodDTOs.Result), 1, followUpMethodDTOs.RowsCount.Value, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.FollowUpMethod.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FollowUpMethodGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public ActionResult DeleteFollowUpMethod(string ids)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/DeleteFollowUpMethod?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<FollowUpLookUpDTO>> followUpMethodDTOs =
                   HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpMethod?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (followUpMethodDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, followUpMethodDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<LinkVM>)new AjaxGridFactory().CreateAjaxGrid(LinkMapper.Map(linkDTOs.Result).AsQueryable(), 1, false, linkDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FollowUpLookUpsMapper.Map(followUpMethodDTOs.Result), 1, followUpMethodDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.FollowUpMethod.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FollowUpMethodGridPartial", grid), LinksUsedList = deleteResult.Result, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult GetFollowUpMethod(string id)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                GetResult<FollowUpLookUpEditDTO> followUpMethodEditDTO =
                    HttpClientWrapper<GetResult<FollowUpLookUpEditDTO>>.GetItemRequest(String.Format("api/Admin/GetFollowUpMethodById?followUpMethodId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (followUpMethodEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, followUpMethodEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.TransactionLink.UpdateSucceeded");

                FollowUpLookUpsEditVM followUpMethodsEditVM = FollowUpLookUpsMapper.Map(followUpMethodEditDTO.Result);
                followUpMethodsEditVM.TransactionCategories = MergeTransactionCategoryLookups(followUpMethodsEditVM.TransactionCategories);

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FollowUpMethodEditPartial", followUpMethodsEditVM
                    ),
                    MessageText = message,
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult UpdateFollowUpMethodGrid(int? page)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                string parameters = GridHelper.GetGridParameters();

                GetResult<List<FollowUpLookUpDTO>> followUpMethodDTOs = HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(String.Format("api/Admin/GetFollowUpMethod?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                if (followUpMethodDTOs.StatusCode != StatusCode.Ok)
                {
                    //this.ShowMessage(MessageType.Error, priorityEditDTO.Item2.ToString());
                }

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FollowUpLookUpsMapper.Map(followUpMethodDTOs.Result), page ?? 1, followUpMethodDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                return Json(new { Html = grid.ToJson("_FollowUpMethodGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion FollowUpMethod
        #region FollowUpPriority
        //todo
        public ActionResult FollowUpPriorityType()
        {

            GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

            ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);
            FollowUpLookUpsViewModel followUpPriorityTypeViewModel = new FollowUpLookUpsViewModel();
            List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups();

            followUpPriorityTypeViewModel.AddFollowUpLookUps.TransactionCategories = transactionCategoryVMs;
            followUpPriorityTypeViewModel.EditFollowUpLookUps.TransactionCategories = transactionCategoryVMs;

            GetResult<List<FollowUpLookUpDTO>> followUpPriorityDTOs =
                HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpPrioritytype?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

            List<FollowUpLookUpsVM> followpPriorityTypeVMs = FollowUpLookUpsMapper.Map(followUpPriorityDTOs.Result);
            if (followpPriorityTypeVMs == null)
            {
                followpPriorityTypeVMs = new List<FollowUpLookUpsVM>();
                followUpPriorityDTOs.RowsCount = 0;
            }

            CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(followpPriorityTypeVMs, 1, followUpPriorityDTOs.RowsCount.Value, false, GridHelper.PageSize);

            ViewData["GridData"] = grid;



            return View(followUpPriorityTypeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddFollowUpPriorityType(FollowUpLookUpsAddVM followUpPriorityTypesVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostFollowUpPrioritytype", FollowUpLookUpsMapper.Map(followUpPriorityTypesVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<FollowUpLookUpDTO>> FollowUpPriorityTypeDTOs =
                HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpPrioritytype?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (FollowUpPriorityTypeDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, FollowUpPriorityTypeDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<AttachmentTypeVM>)new AjaxGridFactory().CreateAjaxGrid(AttachmentTypeMapper.Map(attachmentTypeDTOs.Result).AsQueryable(), 1, false, attachmentTypeDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FollowUpLookUpsMapper.Map(FollowUpPriorityTypeDTOs.Result), 1, FollowUpPriorityTypeDTOs.RowsCount.Value, false);


                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.FollowUpPriorityType.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FollowUpPriorityTypeGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditFollowUpPriorityType(FollowUpLookUpsEditVM followUpPriorityTypesEditVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PutResult PutResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutFollowUpPrioritytype", FollowUpLookUpsMapper.Map(followUpPriorityTypesEditVM)).Result;

                if (PutResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, PutResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<FollowUpLookUpDTO>> followUpPriorityTypeDTOs =
                     HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpPrioritytype?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (followUpPriorityTypeDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, followUpPriorityTypeDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<LinkVM>)new AjaxGridFactory().CreateAjaxGrid(LinkMapper.Map(linkDTOs.Result).AsQueryable(), 1, false, linkDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FollowUpLookUpsMapper.Map(followUpPriorityTypeDTOs.Result), 1, followUpPriorityTypeDTOs.RowsCount.Value, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.FollowUpPriorityType.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FollowUpPriorityTypeGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public ActionResult DeleteFollowUpPriorityType(string ids)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/DeleteFollowUpPrioritytype?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<FollowUpLookUpDTO>> followUpPriorityTypeDTOs =
                   HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpPrioritytype?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (followUpPriorityTypeDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, followUpPriorityTypeDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<LinkVM>)new AjaxGridFactory().CreateAjaxGrid(LinkMapper.Map(linkDTOs.Result).AsQueryable(), 1, false, linkDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FollowUpLookUpsMapper.Map(followUpPriorityTypeDTOs.Result), 1, followUpPriorityTypeDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.FollowUpPriorityType.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FollowUpPriorityTypeGridPartial", grid), LinksUsedList = deleteResult.Result, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult GetFollowUpPriorityType(string id)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                GetResult<FollowUpLookUpEditDTO> followUpPriorityTypeEditDTO =
                    HttpClientWrapper<GetResult<FollowUpLookUpEditDTO>>.GetItemRequest(String.Format("api/Admin/GetFollowUpPrioritytypeById?followUpPriorityTypeId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (followUpPriorityTypeEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, followUpPriorityTypeEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.TransactionLink.UpdateSucceeded");

                FollowUpLookUpsEditVM followUpPriorityTypesEditVM = FollowUpLookUpsMapper.Map(followUpPriorityTypeEditDTO.Result);
                followUpPriorityTypesEditVM.TransactionCategories = MergeTransactionCategoryLookups(followUpPriorityTypesEditVM.TransactionCategories);

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FollowUpPriorityTypeEditPartial", followUpPriorityTypesEditVM
                    ),
                    MessageText = message,
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult UpdateFollowUpPriorityTypeGrid(int? page)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                string parameters = GridHelper.GetGridParameters();

                GetResult<List<FollowUpLookUpDTO>> followUpPriorityTypeDTOs = HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(String.Format("api/Admin/GetFollowUpPrioritytype?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                if (followUpPriorityTypeDTOs.StatusCode != StatusCode.Ok)
                {
                    //this.ShowMessage(MessageType.Error, priorityEditDTO.Item2.ToString());
                }

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FollowUpLookUpsMapper.Map(followUpPriorityTypeDTOs.Result), page ?? 1, followUpPriorityTypeDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                return Json(new { Html = grid.ToJson("_FollowUpPriorityTypeGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }



        #endregion FollowUpPriority

        #region Attachments

        [HttpGet]
        public ActionResult AttachmentType()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                AttachmentTypeViewModel attachmentTypeViewModel = new AttachmentTypeViewModel();
                List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups();

                attachmentTypeViewModel.AddAttachmentType.TransactionCategories = transactionCategoryVMs;
                attachmentTypeViewModel.EditAttachmentType.TransactionCategories = transactionCategoryVMs;

                GetResult<List<AttachmentTypeDTO>> attachmentTypeDTOs =
                    HttpClientWrapper<GetResult<List<AttachmentTypeDTO>>>.GetItemRequest(string.Format("api/Admin/GetAttachmentTypes?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                List<AttachmentTypeVM> attachmentTypeVMs = AttachmentTypeMapper.Map(attachmentTypeDTOs.Result);
                if (attachmentTypeVMs == null)
                {
                    attachmentTypeVMs = new List<AttachmentTypeVM>();
                    attachmentTypeDTOs.RowsCount = 0;
                }

                //IAjaxGrid grid = (AjaxGrid<AttachmentTypeVM>)new AjaxGridFactory().CreateAjaxGrid(attachmentTypeVMs.AsQueryable(), 1, false, attachmentTypeDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<AttachmentTypeVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(attachmentTypeVMs, 1, attachmentTypeDTOs.RowsCount.Value, false, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                return View(attachmentTypeViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ConfidentialityAcknowledgments()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                ConfidentialityAcknowledgmentsViewModel ConfidentialityAcknowledgmentsViewModel = new ConfidentialityAcknowledgmentsViewModel();
                List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups();

                ConfidentialityAcknowledgmentsViewModel.AddConfidentialityAcknowledgments.TransactionCategories = transactionCategoryVMs;
                ConfidentialityAcknowledgmentsViewModel.EditConfidentialityAcknowledgments.TransactionCategories = transactionCategoryVMs;

                GetResult<List<ConfidentialityAcknowledgmentsDTO>> ConfidentialityAcknowledgmentsDTOs =
                    HttpClientWrapper<GetResult<List<ConfidentialityAcknowledgmentsDTO>>>.GetItemRequest(string.Format("api/Admin/GetConfidentialityAcknowledgments?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                List<ConfidentialityAcknowledgmentsVM> ConfidentialityAcknowledgmentsVMs = ConfidentialityAcknowledgmentsMapper.Map(ConfidentialityAcknowledgmentsDTOs.Result);
                if (ConfidentialityAcknowledgmentsVMs == null)
                {
                    ConfidentialityAcknowledgmentsVMs = new List<ConfidentialityAcknowledgmentsVM>();
                    ConfidentialityAcknowledgmentsDTOs.RowsCount = 0;
                }

                //IAjaxGrid grid = (AjaxGrid<ConfidentialityAcknowledgmentsVM>)new AjaxGridFactory().CreateAjaxGrid(ConfidentialityAcknowledgmentsVMs.AsQueryable(), 1, false, ConfidentialityAcknowledgmentsDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<ConfidentialityAcknowledgmentsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(ConfidentialityAcknowledgmentsVMs, 1, ConfidentialityAcknowledgmentsDTOs.RowsCount.Value, false, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                return View(ConfidentialityAcknowledgmentsViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetAttachmentExtentions(int? page)
        {
            try
            {
                GetResult<List<AttachmentExtensionDTO>> attachmentTypeDTOs =
                    HttpClientWrapper<GetResult<List<AttachmentExtensionDTO>>>.GetItemRequest(string.Format("api/Admin/GetAttachmentExtentions?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                List<AttachmentExtensionVM> attachmentExtensionVMs = AttachmentExtensionMapper.Map(attachmentTypeDTOs.Result);
                if (attachmentExtensionVMs == null)
                {
                    attachmentExtensionVMs = new List<AttachmentExtensionVM>();
                    attachmentTypeDTOs.RowsCount = 0;
                }

                // IAjaxGrid grid = (AjaxGrid<AttachmentTypeVM>)new AjaxGridFactory().CreateAjaxGrid(attachmentExtensionVMs.AsQueryable(), 1, false, attachmentTypeDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<AttachmentExtensionVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(attachmentExtensionVMs, page ?? 1, attachmentTypeDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                return View("_AttachmentExtensionsPartial", grid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult UpdateAttachmentExtentionsGrid(int? page)
        {
            try
            {
                GetResult<List<AttachmentExtensionDTO>> attachmentTypeDTOs =
                    HttpClientWrapper<GetResult<List<AttachmentExtensionDTO>>>.GetItemRequest(string.Format("api/Admin/GetAttachmentExtentions?PageIndex={0}&PageSize={1}&CultureName={2}", page, GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                List<AttachmentExtensionVM> attachmentExtensionVMs = AttachmentExtensionMapper.Map(attachmentTypeDTOs.Result);
                if (attachmentExtensionVMs == null)
                {
                    attachmentExtensionVMs = new List<AttachmentExtensionVM>();
                    attachmentTypeDTOs.RowsCount = 0;
                }

                // IAjaxGrid grid = (AjaxGrid<AttachmentTypeVM>)new AjaxGridFactory().CreateAjaxGrid(attachmentExtensionVMs.AsQueryable(), 1, false, attachmentTypeDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<AttachmentExtensionVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(attachmentExtensionVMs, page ?? 1, attachmentTypeDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                return Json(new { Html = grid.ToJson("_AttachmentExtensionsGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult GetCities(int? page)
        {
            try
            {
                GetResult<List<CityDTO>> getResult =
                    HttpClientWrapper<GetResult<List<CityDTO>>>.GetItemRequest(string.Format("api/Admin/GetCities?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                List<CityVM> cityVMs = CityMapper.Map(getResult.Result);
                if (cityVMs == null)
                {
                    cityVMs = new List<CityVM>();
                    getResult.RowsCount = 0;
                }

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<CityVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(cityVMs, 1, getResult.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                return View("_CitiesPartial", grid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult UpdateCities(int? page)
        {
            try
            {
                GetResult<List<CityDTO>> getResult =
                    HttpClientWrapper<GetResult<List<CityDTO>>>.GetItemRequest(string.Format("api/Admin/GetCities?PageIndex={0}&PageSize={1}&CultureName={2}", page, GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                List<CityVM> cityVMs = CityMapper.Map(getResult.Result);
                if (cityVMs == null)
                {
                    cityVMs = new List<CityVM>();
                    getResult.RowsCount = 0;
                }

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<CityVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(cityVMs, page ?? 1, getResult.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                return View("_CitiesGridPartial", grid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void MapTransactionSources<T>(T lookupType)
        {
            lookupType.GetType().GetProperty("TransactionSource").SetValue(lookupType, "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddAttachmentType(AttachmentTypeAddVM attachmentTypeAddVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostAttachmentType", AttachmentTypeMapper.Map(attachmentTypeAddVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<AttachmentTypeDTO>> attachmentTypeDTOs =
                   HttpClientWrapper<GetResult<List<AttachmentTypeDTO>>>.GetItemRequest(string.Format("api/Admin/GetAttachmentTypes?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (attachmentTypeDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, attachmentTypeDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<AttachmentTypeVM>)new AjaxGridFactory().CreateAjaxGrid(AttachmentTypeMapper.Map(attachmentTypeDTOs.Result).AsQueryable(), 1, false, attachmentTypeDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<AttachmentTypeVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(AttachmentTypeMapper.Map(attachmentTypeDTOs.Result), 1, attachmentTypeDTOs.RowsCount.Value, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentType.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AttachmentTypeGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditAttachmentType(AttachmentTypeEditVM attachmentTypeEditVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutAttachmentType", AttachmentTypeMapper.Map(attachmentTypeEditVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<AttachmentTypeDTO>> attachmentTypeDTOs =
                          HttpClientWrapper<GetResult<List<AttachmentTypeDTO>>>.GetItemRequest(string.Format("api/Admin/GetAttachmentTypes?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (attachmentTypeDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, attachmentTypeDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                // IAjaxGrid grid = (AjaxGrid<AttachmentTypeVM>)new AjaxGridFactory().CreateAjaxGrid(AttachmentTypeMapper.Map(attachmentTypeDTOs.Result).AsQueryable(), 1, false, attachmentTypeDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<AttachmentTypeVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(AttachmentTypeMapper.Map(attachmentTypeDTOs.Result), 1, attachmentTypeDTOs.RowsCount.Value, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentType.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AttachmentTypeGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteAttachmentType(string ids)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/DeleteAttachmentTypes?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<AttachmentTypeDTO>> attachmentTypeDTOs =
                      HttpClientWrapper<GetResult<List<AttachmentTypeDTO>>>.GetItemRequest(string.Format("api/Admin/GetAttachmentTypes?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (attachmentTypeDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, attachmentTypeDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                // IAjaxGrid grid = (AjaxGrid<AttachmentTypeVM>)new AjaxGridFactory().CreateAjaxGrid(AttachmentTypeMapper.Map(attachmentTypeDTOs.Result).AsQueryable(), 1, false, attachmentTypeDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<AttachmentTypeVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(AttachmentTypeMapper.Map(attachmentTypeDTOs.Result), 1, attachmentTypeDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentType.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AttachmentTypeGridPartial", grid), AttachmentTypeUsedList = deleteResult.Result, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetAttachmentType(int id)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                GetResult<AttachmentTypeEditDTO> attachmentTypeEditDTO =
                    HttpClientWrapper<GetResult<AttachmentTypeEditDTO>>.GetItemRequest(String.Format("api/Admin/GetAttachmentTypeById?attachmentTypeId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (attachmentTypeEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, attachmentTypeEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentType.UpdateSucceeded");

                AttachmentTypeEditVM attachmentTypeEditVM = AttachmentTypeMapper.Map(attachmentTypeEditDTO.Result);
                attachmentTypeEditVM.TransactionCategories = MergeTransactionCategoryLookups(AttachmentTypeMapper.Map(attachmentTypeEditDTO.Result).TransactionCategories);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AttachmentTypeEditPartial", attachmentTypeEditVM), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult UpdateAttachmentTypeGrid(int? page)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                string parameters = GridHelper.GetGridParameters();

                GetResult<List<AttachmentTypeDTO>> attachmentTypeDTOs = HttpClientWrapper<GetResult<List<AttachmentTypeDTO>>>.GetItemRequest(String.Format("api/Admin/GetAttachmentTypes?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<AttachmentTypeVM>)new CustomAjaxGrid.AjaxGridFactory()
                    .CreateAjaxGrid(AttachmentTypeMapper.Map(attachmentTypeDTOs.Result), page ?? 1
                    , attachmentTypeDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                return Json(new { Html = grid.ToJson("_AttachmentTypeGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult UpdateSaveReasonGrid(int? page)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                string parameters = GridHelper.GetGridParameters();

                List<MCS.UI.Areas.User.Models.Lookups.LookupVM> LookupVMs = LookupsHelper.GetLookupItemswithoutCached(LookupCategory.SaveReason, SessionInfo.CultureShortName).Result.ToList();

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<MCS.UI.Areas.User.Models.Lookups.LookupVM>)new CustomAjaxGrid.AjaxGridFactory()
                    .CreateAjaxGrid(LookupVMs, page ?? 1
                    , LookupVMs.Count, page.HasValue, GridHelper.PageSize);

                return Json(new { Html = grid.ToJson("_SaveReasonGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion Attachments
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddConfidentialityAcknowledgments(ConfidentialityAcknowledgmentsAddVM confidentialityAcknowledgmentsAddVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                GetResult<List<ConfidentialityAcknowledgmentsDTO>> ConfidentialityAcknowledgmentsDTOsCount =
                HttpClientWrapper<GetResult<List<ConfidentialityAcknowledgmentsDTO>>>.GetItemRequest(string.Format("api/Admin/GetConfidentialityAcknowledgments?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (ConfidentialityAcknowledgmentsDTOsCount.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ConfidentialityAcknowledgmentsDTOsCount.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                if (ConfidentialityAcknowledgmentsDTOsCount.RowsCount.Value >= 8)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.ConotAddMoreThanEightConfidentialityAcknowledgments");

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);

                }


                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostConfidentialityAcknowledgments", ConfidentialityAcknowledgmentsMapper.Map(confidentialityAcknowledgmentsAddVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<ConfidentialityAcknowledgmentsDTO>> ConfidentialityAcknowledgmentsDTOs =
                   HttpClientWrapper<GetResult<List<ConfidentialityAcknowledgmentsDTO>>>.GetItemRequest(string.Format("api/Admin/GetConfidentialityAcknowledgments?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (ConfidentialityAcknowledgmentsDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ConfidentialityAcknowledgmentsDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<ConfidentialityAcknowledgmentsVM>)new AjaxGridFactory().CreateAjaxGrid(ConfidentialityAcknowledgmentsMapper.Map(ConfidentialityAcknowledgmentsDTOs.Result).AsQueryable(), 1, false, ConfidentialityAcknowledgmentsDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<ConfidentialityAcknowledgmentsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(ConfidentialityAcknowledgmentsMapper.Map(ConfidentialityAcknowledgmentsDTOs.Result), 1, ConfidentialityAcknowledgmentsDTOs.RowsCount.Value, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.ConfidentialityAcknowledgments.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ConfidentialityAcknowledgmentsGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditConfidentialityAcknowledgments(ConfidentialityAcknowledgmentsEditVM confidentialityAcknowledgmentsEditVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutConfidentialityAcknowledgments", ConfidentialityAcknowledgmentsMapper.Map(confidentialityAcknowledgmentsEditVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<ConfidentialityAcknowledgmentsDTO>> confidentialityAcknowledgmentsDTOs =
                          HttpClientWrapper<GetResult<List<ConfidentialityAcknowledgmentsDTO>>>.GetItemRequest(string.Format("api/Admin/GetConfidentialityAcknowledgments?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (confidentialityAcknowledgmentsDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, confidentialityAcknowledgmentsDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                // IAjaxGrid grid = (AjaxGrid<ConfidentialityAcknowledgmentsVM>)new AjaxGridFactory().CreateAjaxGrid(ConfidentialityAcknowledgmentsMapper.Map(ConfidentialityAcknowledgmentsDTOs.Result).AsQueryable(), 1, false, ConfidentialityAcknowledgmentsDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<ConfidentialityAcknowledgmentsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(ConfidentialityAcknowledgmentsMapper.Map(confidentialityAcknowledgmentsDTOs.Result), 1, confidentialityAcknowledgmentsDTOs.RowsCount.Value, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.ConfidentialityAcknowledgments.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ConfidentialityAcknowledgmentsGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteConfidentialityAcknowledgments(string ids)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/DeleteConfidentialityAcknowledgments?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<ConfidentialityAcknowledgmentsDTO>> ConfidentialityAcknowledgmentsDTOs =
                      HttpClientWrapper<GetResult<List<ConfidentialityAcknowledgmentsDTO>>>.GetItemRequest(string.Format("api/Admin/GetConfidentialityAcknowledgments?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (ConfidentialityAcknowledgmentsDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ConfidentialityAcknowledgmentsDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                // IAjaxGrid grid = (AjaxGrid<ConfidentialityAcknowledgmentsVM>)new AjaxGridFactory().CreateAjaxGrid(ConfidentialityAcknowledgmentsMapper.Map(ConfidentialityAcknowledgmentsDTOs.Result).AsQueryable(), 1, false, ConfidentialityAcknowledgmentsDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<ConfidentialityAcknowledgmentsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(ConfidentialityAcknowledgmentsMapper.Map(ConfidentialityAcknowledgmentsDTOs.Result), 1, ConfidentialityAcknowledgmentsDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.ConfidentialityAcknowledgments.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ConfidentialityAcknowledgmentsGridPartial", grid), ConfidentialityAcknowledgmentsUsedList = deleteResult.Result, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetConfidentialityAcknowledgments(int id)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                GetResult<ConfidentialityAcknowledgmentsEditDTO> ConfidentialityAcknowledgmentsEditDTO =
                    HttpClientWrapper<GetResult<ConfidentialityAcknowledgmentsEditDTO>>.GetItemRequest(String.Format("api/Admin/GetConfidentialityAcknowledgmentsById?ConfidentialityAcknowledgmentsId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (ConfidentialityAcknowledgmentsEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ConfidentialityAcknowledgmentsEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.ConfidentialityAcknowledgments.UpdateSucceeded");

                ConfidentialityAcknowledgmentsEditVM ConfidentialityAcknowledgmentsEditVM = ConfidentialityAcknowledgmentsMapper.Map(ConfidentialityAcknowledgmentsEditDTO.Result);
                ConfidentialityAcknowledgmentsEditVM.TransactionCategories = MergeTransactionCategoryLookups(ConfidentialityAcknowledgmentsMapper.Map(ConfidentialityAcknowledgmentsEditDTO.Result).TransactionCategories);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ConfidentialityAcknowledgmentsEditPartial", ConfidentialityAcknowledgmentsEditVM), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult UpdateConfidentialityAcknowledgmentsGrid(int? page)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                string parameters = GridHelper.GetGridParameters();

                GetResult<List<ConfidentialityAcknowledgmentsDTO>> ConfidentialityAcknowledgmentsDTOs = HttpClientWrapper<GetResult<List<ConfidentialityAcknowledgmentsDTO>>>.GetItemRequest(String.Format("api/Admin/GetConfidentialityAcknowledgments?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<ConfidentialityAcknowledgmentsVM>)new CustomAjaxGrid.AjaxGridFactory()
                    .CreateAjaxGrid(ConfidentialityAcknowledgmentsMapper.Map(ConfidentialityAcknowledgmentsDTOs.Result), page ?? 1
                    , ConfidentialityAcknowledgmentsDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                return Json(new { Html = grid.ToJson("_ConfidentialityAcknowledgmentsGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Forms

        public ActionResult Form()
        {
            try
            {
                Session["OfficeOnlineFileGuid"] = Guid.NewGuid();
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                FormViewModel formViewModel = new FormViewModel();
                List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups().Where(x => x.Id == (int)(TransactionCategories.DraftOutbound)).ToList();
                transactionCategoryVMs.ForEach(x => x.IsSelected = true);
                formViewModel.AddForm.TransactionCategories = transactionCategoryVMs;
                formViewModel.EditForm.TransactionCategories = transactionCategoryVMs;

                GetResult<List<FormDTO>> formDTOs =
                    HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(string.Format("api/Admin/GetForms?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
                List<FormVM> formVMs = FormMapper.Map(formDTOs.Result);
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
      HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                ViewData["orgUnitDataInfo"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                if (formVMs == null)
                {
                    formVMs = new List<FormVM>();
                    formDTOs.RowsCount = 0;
                }

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FormVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(formVMs, 1, formDTOs.RowsCount.Value, false, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest("api/Admin/GetOrgUnits?cultureName=" + SessionInfo.CultureShortName).Result;

                //ViewData["OrgUnitData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), -1, true);

                //Need to add int? page as parameter in Form method
                //var grid = (GridMvc.Ajax.GridExtensions.AjaxGrid<FormVM>)new GridMvc.Ajax.GridExtensions.AjaxGridFactory()
                //    .CreateAjaxGrid(formVMs, page ?? 1, formDTOs.RowsCount.Value, page.HasValue, UIHelper.PageSize);
                //formViewModel.FormVMs = grid;

                //if (page.HasValue)
                //{
                //    return Json(new
                //    {
                //        Html = grid.ToJson("~/Areas/Admin/Views/Lookups/_FormGridPartial.cshtml", this),
                //        grid.HasItems
                //    }, JsonRequestBehavior.AllowGet);
                //}

                return View(formViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken()]
        public ActionResult AddForm(FormAddVM formAddVM, HttpPostedFileBase files)
        {
            try
            {
                string message = string.Empty;
                bool noTransCatSelected = true;

                if (formAddVM.OrgUnitIds.Count > 1)
                {
                    formAddVM.OrgUnitIds.Remove(formAddVM.OrgUnitIds.FirstOrDefault(o => o == 0));
                }
                foreach (var item in formAddVM.TransactionCategories)
                {
                    if (item.IsSelected)
                    {
                        noTransCatSelected = false;
                        break;
                    }
                }

                if (noTransCatSelected)
                {
                    foreach (var item in formAddVM.TransactionCategories)
                    {
                        item.IsSelected = true;
                    }
                }
                // string officeOnlineFileGuid = Session["OfficeOnlineFileGuid"].ToString();
                //string officeOnlineFileGuid = Session["OfficeOnlineFileGuid"].ToString();
                string FilePrefix;
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    FilePrefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_" + Constants.ExplanationFile;
                }
                else
                {
                    FilePrefix = "_" + SessionInfo.CurrentUser.Id + "_" + Constants.ExplanationFile + "_";
                }
                string path = StringUtility.ValidateFileNames(SystemConfigurations.ExternalCopiesAttachmentPath) + FilePrefix;
                var fullPath = StringUtility.ValidateFileNames($"{path}{formAddVM.FileName}");
                byte[] fileContent = System.IO.File.ReadAllBytes(fullPath);
                string fileExtenstion = GetAttchementMimeType(formAddVM.FileName);


                if (fileContent != null && fileContent.Length > 0)
                {
                   
                    //DocumentViewerHelper.GetOfficeFile(officeOnlineFileGuid);
                    formAddVM.FormContentVM = new FormContentVM
                    {
                        Content = fileContent
                    };

                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.SelectTemplete");

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                //byte[] data = DocumentViewerHelper.GetOfficeFile(officeOnlineFileGuid);
                //formAddVM.FormContentVM.Content = data;
                // DocumentViewerHelper.DeleteOfficeFile(officeOnlineFileGuid);
                //string officeOnlineFileGuid = Session["OfficeOnlineFileGuid"].ToString();
                //formAddVM.FormContentVM.Content = DocumentViewerHelper.GetOfficeFile(officeOnlineFileGuid);

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostForm", FormMapper.Map(formAddVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                GetResult<List<FormDTO>> formDTOs =
                   HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(string.Format("api/Admin/GetForms?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (formDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, formDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<FormVM>)new AjaxGridFactory().CreateAjaxGrid(FormMapper.Map(formDTOs.Result).AsQueryable(), 1, false, formDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FormVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FormMapper.Map(formDTOs.Result), 1, formDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Form.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FormGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private byte[] GetFileAsByteArray(HttpPostedFileBase files)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                files.InputStream.CopyTo(ms);
                return ms.ToArray();
            }
        }
        [HttpPost]
        public ActionResult UploadMainDocAttachments(int AttachmentTypeId)
        {
            string addedFilesJson = string.Empty;
            bool isValid = true;
            List<object> list = new List<object>();
            if (Request.Files.Count <= 0)
            {
                return Json(new
                {
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            HttpPostedFileBase file;

            string FilePrefix;
            if (SystemConfigurations.MultiTenantEnabled)
            {
                FilePrefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_" + AttachmentTypeId + "_";
            }
            else
            {
                FilePrefix = "_" + SessionInfo.CurrentUser.Id + "_" + AttachmentTypeId + "_";
            }
            int totalCount = Directory.GetFiles(SystemConfigurations.ExternalCopiesAttachmentPath).Select(o => Path.GetFileName(o)).Where(o => o.StartsWith(FilePrefix)).Count();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                file = Request.Files[i];

                if (!IsValidMimeType(MimeMapping.GetMimeMapping(SystemConfigurations.ExternalCopiesAttachmentPath + file.FileName)))
                {
                    isValid = false;
                    break;
                }
                file.SaveAs(SystemConfigurations.ExternalCopiesAttachmentPath + FilePrefix + file.FileName);
                list.Add(new { Id = totalCount++, Name = file.FileName, IsDeleted = 0 });
                // addedFilesJson += JsonConvert.SerializeObject(new { Id = totalCount++, AttachmentName = file.FileName, IsDeleted = 0 });
            }
            addedFilesJson = JsonConvert.SerializeObject(list);

            if (isValid == false)
            {
                return Json(new
                {
                    MessageType = MessageType.Error,
                    MessageText = DbRes.TResource("Task.File.MimeType")
                });
            }

            return Json(new
            {
                AddedFilesJson = addedFilesJson,
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditForm(FormEditVM formEditVM, HttpPostedFileBase files)
        {
            try
            {

                string message = string.Empty;
                bool noTransCatSelected = true;

                if (formEditVM.AllOrgUnitsSelected == true)
                {
                    formEditVM.OrgUnitIds = new List<int>();
                    formEditVM.OrgUnitIds.Add(0);
                }

                if (formEditVM.OrgUnitIds.Count > 1)
                {
                    formEditVM.OrgUnitIds.Remove(formEditVM.OrgUnitIds.FirstOrDefault(o => o == 0));
                }
                foreach (var item in formEditVM.TransactionCategories)
                {
                    if (item.IsSelected)
                    {
                        noTransCatSelected = false;
                        break;
                    }
                }

                if (noTransCatSelected)
                {
                    foreach (var item in formEditVM.TransactionCategories)
                    {
                        item.IsSelected = true;
                    }
                }


                string FilePrefix;
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    FilePrefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_" + Constants.ExplanationFile;
                }
                else
                {
                    FilePrefix = "_" + SessionInfo.CurrentUser.Id + "_" + Constants.ExplanationFile + "_";
                }
                string path = StringUtility.ValidateFileNames(SystemConfigurations.ExternalCopiesAttachmentPath) + FilePrefix;
                var fullPath = StringUtility.ValidateFileNames($"{path}{formEditVM.FileName}");
                byte[] fileContent = System.IO.File.ReadAllBytes(fullPath);
                string fileExtenstion = GetAttchementMimeType(formEditVM.FileName);


                if (fileContent != null && fileContent.Length > 0)
                {

                    //DocumentViewerHelper.GetOfficeFile(officeOnlineFileGuid);
                    formEditVM.FormContentVM = new FormContentVM
                    {
                        Content = fileContent
                    };

                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.SelectTemplete");

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

              
                //if (files != null)
                //{
                //    byte[] data = GetFileAsByteArray(files);
                //    //DocumentViewerHelper.GetOfficeFile(officeOnlineFileGuid);

                //    if (files != null)
                //    {

                //        //DocumentViewerHelper.GetOfficeFile(officeOnlineFileGuid);


                //    }

                //}
                // DocumentViewerHelper.DeleteOfficeFile(officeOnlineFileGuid);
                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutForm", FormMapper.Map(formEditVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
     HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                ViewData["orgUnitDataInfo"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                GetResult<List<FormDTO>> formDTOs =
                    HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(string.Format("api/Admin/GetForms?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (formDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, formDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<FormVM>)new AjaxGridFactory().CreateAjaxGrid(FormMapper.Map(formDTOs.Result).AsQueryable(), 1, false, formDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FormVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FormMapper.Map(formDTOs.Result), 1, formDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Form.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FormGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteForm(string ids)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                DeleteResult deleteResult = HttpClientWrapper<DeleteResult>.DeleteRequest(String.Format("api/Admin/DeleteForms?ids={0}", ids)).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<FormDTO>> formDTOs =
                 HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(string.Format("api/Admin/GetForms?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (formDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, formDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<FormVM>)new AjaxGridFactory().CreateAjaxGrid(FormMapper.Map(formDTOs.Result).AsQueryable(), 1, false, formDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FormVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FormMapper.Map(formDTOs.Result), 1, formDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Form.DeleteSucceeded");

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FormGridPartial", grid),
                    MessageText = message,
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetForm(string id)
        {
            try
            {
                Session["OfficeOnlineFileGuid"] = Guid.NewGuid().ToString();
                string message = string.Empty;

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                GetResult<FormEditDTO> formEditDTO =
                    HttpClientWrapper<GetResult<FormEditDTO>>.GetItemRequest(String.Format("api/Admin/GetFormById?formId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (formEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, formEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
     HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                ViewData["orgUnitDataInfo"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Form.UpdateSucceeded");

                FormEditVM formEditVM = FormMapper.Map(formEditDTO.Result);

                GetResult<List<OrgUnitDTO>> orgUnitByIdsDTOs =
                      HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnitsByIds?orgUnitIds={0}&cultureName={1}", string.Join(",", formEditVM.OrgUnitIds), SessionInfo.CultureShortName)).Result;

                //GetOrgUnitsNodesByIds
                if (formEditVM.OrgUnitIds == null || formEditVM.OrgUnitIds.Count() == 0)
                {
                    formEditVM.AllOrgUnitsSelected = true;
                }
                else
                {
                    formEditVM.OrgUnitsKeyValue = new Dictionary<int, string>();
                    foreach (var item in orgUnitByIdsDTOs.Result)
                    {
                        formEditVM.OrgUnitsKeyValue.Add(item.Id, item.Name);
                    }
                }
                List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups().Where(x => x.Id == (int)(TransactionCategories.DraftOutbound)).ToList();
                transactionCategoryVMs.ForEach(x => x.IsSelected = true);
                formEditVM.TransactionCategories = transactionCategoryVMs;



                return Json(new { AllOrgUnitsSelected = formEditVM.AllOrgUnitsSelected, Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FormEditPartial", formEditVM), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult UpdateFormGrid(int? page)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                string parameters = GridHelper.GetGridParameters();

                GetResult<List<FormDTO>> formDTOs = HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(String.Format("api/Admin/GetForms?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                if (formDTOs.StatusCode != StatusCode.Ok)
                {

                }

                //var grid = new AjaxGridFactory().CreateAjaxGrid(FormMapper.Map(formDTOs.Result).AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, formDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FormVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(FormMapper.Map(formDTOs.Result), page ?? 1, formDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FormGridPartial", grid) /*grid.ToJson("_FormGridPartial", this), grid.HasItems*/ }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetDocument(int documentId)
        {
            try
            {
                GetResult<DocumentDTO> documentDTO =
                            HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(String.Format("api/Document/GetDocumentById?cultureName={0}&documentId={1}", SessionInfo.CultureShortName, documentId)).Result;
                var documentVM = UserDocuments.DocumentMapper.Map(documentDTO.Result);

                return File(documentVM.Content, documentVM.MimeType, documentVM.Name);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult DeleteFormDocument(int documentId, int formId)
        {
            try
            {
                string message = string.Empty;

                DeleteResult deleteResult =
                            HttpClientWrapper<DeleteResult>.DeleteRequest(String.Format("api/Admin/DeleteFormDocument?documentId={0}&formId={1}", documentId, formId)).Result;


                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                GetResult<FormEditDTO> formEditDTO =
                    HttpClientWrapper<GetResult<FormEditDTO>>.GetItemRequest(String.Format("api/Admin/GetFormById?formId={0}&cultureName={1}", formId, SessionInfo.CultureShortName)).Result;

                if (formEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, formEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                FormEditVM formEditVM = FormMapper.Map(formEditDTO.Result);

                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest("api/Admin/GetOrgUnits?cultureName=" + SessionInfo.CultureShortName).Result;

                //foreach (var item in formEditDTO.Result.DepartmentIds)
                //{
                //    List<OrgUnitDTO> orgUnitslist = orgUnitDTOs.Result;
                //    foreach (var orgunit in orgUnitslist)
                //    {
                //        if (orgunit.Id == item)
                //        {
                //            orgunit.IsSelected = true;
                //        }
                //    }
                //}

                //ViewData["OrgUnitData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), -1, true);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Form.DeleteDocumentSucceeded");

                formEditVM.TransactionCategories = MergeTransactionCategoryLookups(formEditVM.TransactionCategories);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FormEditPartial", FormMapper.Map(formEditDTO.Result)), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Forms

        #region Link
        //yousef
        public ActionResult Link()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                LinkViewModel linkViewModel = new LinkViewModel();
                List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups();

                linkViewModel.AddLink.TransactionCategories = transactionCategoryVMs;
                linkViewModel.EditLink.TransactionCategories = transactionCategoryVMs;

                GetResult<List<LinkDTO>> linkDTOs =
                    HttpClientWrapper<GetResult<List<LinkDTO>>>.GetItemRequest(string.Format("api/Admin/GetLinks?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                List<LinkVM> linkVMs = LinkMapper.Map(linkDTOs.Result);
                if (linkVMs == null)
                {
                    linkVMs = new List<LinkVM>();
                    linkDTOs.RowsCount = 0;
                }

                //IAjaxGrid grid = (AjaxGrid<LinkVM>)new AjaxGridFactory().CreateAjaxGrid(linkVMs.AsQueryable(), 1, false, linkDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<LinkVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(linkVMs, 1, linkDTOs.RowsCount.Value, false, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                return View(linkViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddLink(LinkAddVM linkAddVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostLink", LinkMapper.Map(linkAddVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<LinkDTO>> linkDTOs =
                   HttpClientWrapper<GetResult<List<LinkDTO>>>.GetItemRequest(string.Format("api/Admin/GetLinks?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (linkDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, linkDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<LinkVM>)new AjaxGridFactory().CreateAjaxGrid(LinkMapper.Map(linkDTOs.Result).AsQueryable(), 1, false, linkDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<LinkVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(LinkMapper.Map(linkDTOs.Result), 1, linkDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.TransactionLink.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_LinkGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditLink(LinkEditVM linkEditVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PutResult PutResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutLink", LinkMapper.Map(linkEditVM)).Result;

                if (PutResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, PutResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<LinkDTO>> linkDTOs =
                     HttpClientWrapper<GetResult<List<LinkDTO>>>.GetItemRequest(string.Format("api/Admin/GetLinks?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (linkDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, linkDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<LinkVM>)new AjaxGridFactory().CreateAjaxGrid(LinkMapper.Map(linkDTOs.Result).AsQueryable(), 1, false, linkDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<LinkVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(LinkMapper.Map(linkDTOs.Result), 1, linkDTOs.RowsCount.Value, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.TransactionLink.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_LinkGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteLink(string ids)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/DeleteLinks?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<LinkDTO>> linkDTOs =
                   HttpClientWrapper<GetResult<List<LinkDTO>>>.GetItemRequest(string.Format("api/Admin/GetLinks?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (linkDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, linkDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<LinkVM>)new AjaxGridFactory().CreateAjaxGrid(LinkMapper.Map(linkDTOs.Result).AsQueryable(), 1, false, linkDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<LinkVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(LinkMapper.Map(linkDTOs.Result), 1, linkDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.TransactionLink.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_LinkGridPartial", grid), LinksUsedList = deleteResult.Result, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetLink(string id)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                GetResult<LinkEditDTO> linkEditDTO =
                    HttpClientWrapper<GetResult<LinkEditDTO>>.GetItemRequest(String.Format("api/Admin/GetLinkById?linkId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (linkEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, linkEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //TODO: Change Source Key To Be "Admin.Link.UpdateSucceeded"
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.TransactionLink.UpdateSucceeded");

                LinkEditVM linkEditVM = LinkMapper.Map(linkEditDTO.Result);
                linkEditVM.TransactionCategories = MergeTransactionCategoryLookups(linkEditVM.TransactionCategories);

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_LinkEditPartial", linkEditVM
                    ),
                    MessageText = message,
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult UpdateLinkGrid(int? page)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                string parameters = GridHelper.GetGridParameters();

                GetResult<List<LinkDTO>> linkDTOs = HttpClientWrapper<GetResult<List<LinkDTO>>>.GetItemRequest(String.Format("api/Admin/GetLinks?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                if (linkDTOs.StatusCode != StatusCode.Ok)
                {
                    //this.ShowMessage(MessageType.Error, priorityEditDTO.Item2.ToString());
                }

                //var grid = new AjaxGridFactory().CreateAjaxGrid(LinkMapper.Map(linkDTOs.Result).AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, linkDTOs.RowsCount.Value);

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<LinkVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(LinkMapper.Map(linkDTOs.Result), page ?? 1, linkDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                return Json(new { Html = grid.ToJson("_LinkGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Link

        #region Priority

        public ActionResult Priority()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PriorityViewModel priorityViewModel = new PriorityViewModel();
                List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups();

                priorityViewModel.AddPriority.TransactionCategories = transactionCategoryVMs;
                priorityViewModel.EditPriority.TransactionCategories = transactionCategoryVMs;

                GetResult<List<PriorityDTO>> priorityDTOs =
                    HttpClientWrapper<GetResult<List<PriorityDTO>>>
                    .GetItemRequest(string.Format("api/Admin/GetPriorities?PageIndex=1&PageSize={0}&CultureName={1}",
                    GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (priorityDTOs.Result == null)
                {
                    priorityDTOs.Result = new List<PriorityDTO>();
                    priorityDTOs.RowsCount = 0;
                }

                //IAjaxGrid grid = (AjaxGrid<PriorityVM>)new AjaxGridFactory().CreateAjaxGrid(PriorityMapper.Map(priorityDTOs.Result).AsQueryable(), 1, false, priorityDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<PriorityVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(PriorityMapper.Map(priorityDTOs.Result), 1, priorityDTOs.RowsCount.Value, false, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                return View(priorityViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddPriority(PriorityAddVM priorityAddVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostPriority", PriorityMapper.Map(priorityAddVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<PriorityDTO>> priorityDTOs =
                   HttpClientWrapper<GetResult<List<PriorityDTO>>>.GetItemRequest(string.Format("api/Admin/GetPriorities?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (priorityDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, priorityDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<PriorityVM>)new AjaxGridFactory().CreateAjaxGrid(PriorityMapper.Map(priorityDTOs.Result).AsQueryable(), 1, false, priorityDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<PriorityVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(PriorityMapper.Map(priorityDTOs.Result), 1, priorityDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Priority.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_PriorityGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditPriority(PriorityEditVM priorityEditVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutPriority", PriorityMapper.Map(priorityEditVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<PriorityDTO>> priorityDTOs =
                    HttpClientWrapper<GetResult<List<PriorityDTO>>>.GetItemRequest(string.Format("api/Admin/GetPriorities?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;


                if (priorityDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, priorityDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<PriorityVM>)new AjaxGridFactory().CreateAjaxGrid(PriorityMapper.Map(priorityDTOs.Result).AsQueryable(), 1, false, priorityDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<PriorityVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(PriorityMapper.Map(priorityDTOs.Result), 1, priorityDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Priority.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_PriorityGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeletePriority(string ids)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/DeletePriorities?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<PriorityDTO>> priorityDTOs =
                   HttpClientWrapper<GetResult<List<PriorityDTO>>>.GetItemRequest(string.Format("api/Admin/GetPriorities?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (priorityDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, priorityDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<PriorityVM>)new AjaxGridFactory().CreateAjaxGrid(PriorityMapper.Map(priorityDTOs.Result).AsQueryable(), 1, false, priorityDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<PriorityVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(PriorityMapper.Map(priorityDTOs.Result), 1, priorityDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Priority.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_PriorityGridPartial", grid), PrioritiesUsedList = deleteResult.Result, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetPriority(string id)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                GetResult<PriorityEditDTO> priorityEditDTO =
                    HttpClientWrapper<GetResult<PriorityEditDTO>>.GetItemRequest(String.Format("api/Admin/GetPriorityById?priorityId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (priorityEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, priorityEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Priority.UpdateSucceeded");

                PriorityEditVM priorityEditVM = PriorityMapper.Map(priorityEditDTO.Result);
                priorityEditVM.TransactionCategories = MergeTransactionCategoryLookups(priorityEditVM.TransactionCategories);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_PriorityEditPartial", priorityEditVM), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult UpdatePriorityGrid(int? page)
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {

                }

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                string parameters = GridHelper.GetGridParameters();

                GetResult<List<PriorityDTO>> priorityDTOs = HttpClientWrapper<GetResult<List<PriorityDTO>>>
                    .GetItemRequest(String.Format("api/Admin/GetPriorities?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                if (priorityDTOs.StatusCode != StatusCode.Ok)
                {

                }

                // IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(PriorityMapper.Map(priorityDTOs.Result).AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, priorityDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<PriorityVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(PriorityMapper.Map(priorityDTOs.Result), page ?? 1, priorityDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                return Json(new { Html = grid.ToJson("_PriorityGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Priority

        #region PriorityException

        [HttpGet]
        public ActionResult EditPriorityDetails(int priorityId)
        {
            int key = 0;
            Tuple<PriorityEditVM, int> Priority = GetPriorityById(priorityId);
            Priority.Item1.PriorityExceptions.ForEach(pe => pe.Key = key++);

            CustomAjaxGrid.AjaxGrid<PriorityExceptionVM> grid = (CustomAjaxGrid.AjaxGrid<PriorityExceptionVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(Priority.Item1.PriorityExceptions, 1, Priority.Item2, false, GridHelper.PageSize);
            Priority.Item1.PriorityExceptions = grid;

            //ViewData["OrgUnitData"] = GetOrgUnitsTree();
            //ViewData["GridData"] = grid;
            //return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_PriorityDetailsPartial", null) }, JsonRequestBehavior.AllowGet);
            return View("_PriorityDetailsPartial", Priority.Item1);
        }
        [HttpGet]
        public ActionResult UpdatePriorityExceptionGrid(int priorityId, int? page)
        {
            SearchCriteria searchCriteria = new SearchCriteria
            {
                PageIndex = page ?? 1,
                PageSize = GridHelper.PageSize,
                CultureName = SessionInfo.CultureShortName,
                SearchColunms = new List<SearchColunm>() { new SearchColunm { ColunmName = "PriorityId", ColunmValue = priorityId.ToString() } }
            };

            GetResult<List<PriorityExceptionDTO>> priorityExceptionDTOs =
               HttpClientWrapper<GetResult<List<PriorityExceptionDTO>>>.GetItemRequest(string.Format("api/Admin/GetPriorityExceptions?PageIndex={0}&PageSize={1}&priorityId={2}&cultureName={3}", page, UIHelper.PageSize, priorityId, SessionInfo.CultureShortName)).Result;


            List<PriorityExceptionVM> priorityExceptionVMs = PriorityExceptionMapper.Map(priorityExceptionDTOs.Result);
            CustomAjaxGrid.AjaxGrid<PriorityExceptionVM> grid = (CustomAjaxGrid.AjaxGrid<PriorityExceptionVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(priorityExceptionVMs, page ?? 1, priorityExceptionDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

            return Json(new { Html = grid.ToJson("_PriorityExceptionGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditPriorityDetails(PriorityEditVM priorityEditVM)
        {
            string message = string.Empty;

            PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutPriority", PriorityMapper.Map(priorityEditVM)).Result;

            if (putResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Priority.UpdateSucceeded");

            return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddPriorityException(PriorityExceptionVM priorityExceptionVM)
        {
            string message = string.Empty;
            int key = 0;

            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostPriorityException", PriorityExceptionMapper.Map(priorityExceptionVM)).Result;

            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            GetResult<List<PriorityExceptionDTO>> priorityExceptionDTOs =
               HttpClientWrapper<GetResult<List<PriorityExceptionDTO>>>.GetItemRequest(string.Format("api/Admin/GetPriorityExceptions?PageIndex=1&PageSize={0}&priorityId={1}&CultureName={2}", GridHelper.PageSize, priorityExceptionVM.PriorityId, SessionInfo.CultureShortName)).Result;

            if (priorityExceptionDTOs.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, priorityExceptionDTOs.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            List<PriorityExceptionVM> priorityExceptionVMs = PriorityExceptionMapper.Map(priorityExceptionDTOs.Result);
            priorityExceptionVMs.ForEach(pe => pe.Key = key++);

            CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<PriorityExceptionVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(priorityExceptionVMs, 1, priorityExceptionDTOs.RowsCount.Value, false, GridHelper.PageSize);

            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Lookups.PriorityExceptions.AddSucceeded");

            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_PriorityExceptionGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public ActionResult EditPriorityException(PriorityExceptionVM priorityExceptionVM)
        {
            string message = string.Empty;
            int key = 0;

            PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutPriorityException", PriorityExceptionMapper.Map(priorityExceptionVM)).Result;

            if (putResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            GetResult<List<PriorityExceptionDTO>> priorityExceptionDTOs =
               HttpClientWrapper<GetResult<List<PriorityExceptionDTO>>>.GetItemRequest(string.Format("api/Admin/GetPriorityExceptions?PageIndex=1&PageSize={0}&priorityId={1}&CultureName={2}", GridHelper.PageSize, priorityExceptionVM.PriorityId, SessionInfo.CultureShortName)).Result;

            if (priorityExceptionDTOs.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, priorityExceptionDTOs.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            List<PriorityExceptionVM> priorityExceptionVMs = PriorityExceptionMapper.Map(priorityExceptionDTOs.Result);
            priorityExceptionVMs.ForEach(pe => pe.Key = key++);

            CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<PriorityExceptionVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(priorityExceptionVMs, 1, priorityExceptionDTOs.RowsCount.Value, false, GridHelper.PageSize);

            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Lookups.PriorityExceptions.EditSucceeded");

            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_PriorityExceptionGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeletePriorityException(int priorityExceptionId, int priorityId)
        {
            string message = string.Empty;
            int key = 0;
            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Admin/DeletePriorityException?priorityExceptionId={0}", priorityExceptionId), null).Result;

            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            GetResult<List<PriorityExceptionDTO>> priorityExceptionDTOs =
               HttpClientWrapper<GetResult<List<PriorityExceptionDTO>>>.GetItemRequest(string.Format("api/Admin/GetPriorityExceptions?PageIndex=1&PageSize={0}&priorityId={1}&CultureName={2}", GridHelper.PageSize, priorityId, SessionInfo.CultureShortName)).Result;

            if (priorityExceptionDTOs.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, priorityExceptionDTOs.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            List<PriorityExceptionVM> priorityExceptionVMs = PriorityExceptionMapper.Map(priorityExceptionDTOs.Result);
            priorityExceptionVMs.ForEach(pe => pe.Key = key++);

            CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<PriorityExceptionVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(PriorityExceptionMapper.Map(priorityExceptionDTOs.Result), 1, priorityExceptionDTOs.RowsCount.Value, false, GridHelper.PageSize);

            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Lookups.PriorityExceptions.DeleteSucceeded");

            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_PriorityExceptionGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }
        public Tuple<PriorityEditVM, int> GetPriorityById(int priorityId)
        {
            //GetResult<List<PriorityExceptionDTO>> getResult = HttpClientWrapper<GetResult<List<PriorityExceptionDTO>>>.GetItemRequest(string.Format("api/Admin/GetPriorityExceptionByPriorityId?priorityId={0}&cultureName={1}", priorityId, SessionInfo.CultureShortName)).Result;
            GetResult<PriorityEditDTO> priorityEditDTO =
                    HttpClientWrapper<GetResult<PriorityEditDTO>>.GetItemRequest(String.Format("api/Admin/GetPriorityById?PageIndex=1&PageSize={0}&priorityId={1}&cultureName={2}", UIHelper.PageSize, priorityId, SessionInfo.CultureShortName)).Result;

            if (priorityEditDTO.StatusCode != StatusCode.Ok)
            {
                throw new Exception(ResourceHelper.GetResourceValue(ResourceSet.StatusCode, priorityEditDTO.StatusCode.ToString()));
            }

            return new Tuple<PriorityEditVM, int>(PriorityMapper.Map(priorityEditDTO.Result), priorityEditDTO.RowsCount ?? 0);
        }

        //private TreeViewModel GetOrgUnitsTree()
        //{
        //    GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest("api/Admin/GetOrgUnits?cultureName=" + SessionInfo.CultureShortName).Result;

        //    return UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), -1, true);
        //}
        #endregion PriorityException

        #region Letter Type

        public ActionResult LetterType()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs =
                    HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.Result == null)
                {
                    cultureDTOs.Result = new List<CultureDTO>();
                    cultureDTOs.RowsCount = 0;
                }

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                LetterTypeViewModel letterTypeViewModel = new LetterTypeViewModel();
                List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups();
                List<LetterListTypeVM> letterListTypeVMs = GetLetterListTypeLookups();

                letterTypeViewModel.AddLetterType.TransactionCategories = transactionCategoryVMs;
                letterTypeViewModel.EditLetterType.TransactionCategories = transactionCategoryVMs;

                letterTypeViewModel.AddLetterType.List = letterListTypeVMs;
                letterTypeViewModel.EditLetterType.List = letterListTypeVMs;

                GetResult<List<LetterTypeDTO>> letterTypeDTOs =
                    HttpClientWrapper<GetResult<List<LetterTypeDTO>>>.GetItemRequest(string.Format("api/Admin/GetLetterTypes?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (letterTypeDTOs.Result == null)
                {
                    letterTypeDTOs.Result = new List<LetterTypeDTO>();
                    letterTypeDTOs.RowsCount = 0;
                }

                //IAjaxGrid grid = (AjaxGrid<LetterTypeVM>)new AjaxGridFactory().CreateAjaxGrid(LetterTypeMapper.Map(letterTypeDTOs.Result).AsQueryable(), 1, false, letterTypeDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<LetterTypeVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(LetterTypeMapper.Map(letterTypeDTOs.Result), 1, letterTypeDTOs.RowsCount.Value, false, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                return View(letterTypeViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddLetterType(LetterTypeAddVM letterTypeAddVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostLetterType", LetterTypeMapper.Map(letterTypeAddVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<LetterTypeDTO>> letterTypeDTOs =
                   HttpClientWrapper<GetResult<List<LetterTypeDTO>>>.GetItemRequest(string.Format("api/Admin/GetLetterTypes?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (letterTypeDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, letterTypeDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<LetterTypeVM>)new AjaxGridFactory().CreateAjaxGrid(LetterTypeMapper.Map(letterTypeDTOs.Result).AsQueryable(), 1, false, letterTypeDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<LetterTypeVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(LetterTypeMapper.Map(letterTypeDTOs.Result), 1, letterTypeDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.LetterType.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_LetterTypeGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditLetterType(LetterTypeEditVM letterTypeEditVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutLetterType", LetterTypeMapper.Map(letterTypeEditVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<LetterTypeDTO>> letterTypeDTOs =
                     HttpClientWrapper<GetResult<List<LetterTypeDTO>>>.GetItemRequest(string.Format("api/Admin/GetLetterTypes?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (letterTypeDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, letterTypeDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<LetterTypeVM>)new AjaxGridFactory().CreateAjaxGrid(LetterTypeMapper.Map(letterTypeDTOs.Result).AsQueryable(), 1, false, letterTypeDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<LetterTypeVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(LetterTypeMapper.Map(letterTypeDTOs.Result), 1, letterTypeDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.LetterType.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_LetterTypeGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteLetterType(string ids)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/DeleteLetterTypes?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<LetterTypeDTO>> letterTypeDTOs =
                       HttpClientWrapper<GetResult<List<LetterTypeDTO>>>.GetItemRequest(string.Format("api/Admin/GetLetterTypes?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (letterTypeDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, letterTypeDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<LetterTypeVM>)new AjaxGridFactory().CreateAjaxGrid(LetterTypeMapper.Map(letterTypeDTOs.Result).AsQueryable(), 1, false, letterTypeDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<LetterTypeVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(LetterTypeMapper.Map(letterTypeDTOs.Result), 1, letterTypeDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.LetterType.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_LetterTypeGridPartial", grid), LetterTypesUsedList = deleteResult.Result, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetLetterType(string id)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                GetResult<LetterTypeEditDTO> letterTypeEditDTO = HttpClientWrapper<GetResult<LetterTypeEditDTO>>.GetItemRequest(String.Format("api/Admin/GetLetterTypeById?letterTypeId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (letterTypeEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, letterTypeEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                LetterTypeEditVM letterTypeEditVM = LetterTypeMapper.Map(letterTypeEditDTO.Result);
                letterTypeEditVM.TransactionCategories = MergeTransactionCategoryLookups(letterTypeEditVM.TransactionCategories);
                letterTypeEditVM.List = MergeLetterListTypeLookups(letterTypeEditVM.List);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.LetterType.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_LetterTypeEditPartial", letterTypeEditVM), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult UpdateLetterTypeGrid(int? page)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                string parameters = GridHelper.GetGridParameters();

                GetResult<List<LetterTypeDTO>> letterTypeDTOs = HttpClientWrapper<GetResult<List<LetterTypeDTO>>>.GetItemRequest(String.Format("api/Admin/GetLetterTypes?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                //var grid = new AjaxGridFactory().CreateAjaxGrid(LetterTypeMapper.Map(letterTypeDTOs.Result).AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, letterTypeDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<LetterTypeVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(LetterTypeMapper.Map(letterTypeDTOs.Result), page ?? 1, letterTypeDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_LetterTypeGridPartial", grid), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Letter Type

        #region Transaction Type

        public ActionResult TransactionType()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                TransactionTypeViewModel transactionTypeViewModel = new TransactionTypeViewModel();
                List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups();

                transactionTypeViewModel.AddTransactionType.TransactionCategories = transactionCategoryVMs;
                transactionTypeViewModel.EditTransactionType.TransactionCategories = transactionCategoryVMs;

                GetResult<List<TransactionTypeDTO>> transactionTypeDTOs =
                    HttpClientWrapper<GetResult<List<TransactionTypeDTO>>>.GetItemRequest(string.Format("api/Admin/GetTransactionTypes?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (transactionTypeDTOs.Result == null)
                {
                    transactionTypeDTOs.Result = new List<TransactionTypeDTO>();
                    transactionTypeDTOs.RowsCount = 0;
                }

                //IAjaxGrid grid = (AjaxGrid<TransactionTypeVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionTypeMapper.Map(transactionTypeDTOs.Result).AsQueryable(), 1, false, transactionTypeDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<TransactionTypeVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(TransactionTypeMapper.Map(transactionTypeDTOs.Result), 1, transactionTypeDTOs.RowsCount.Value, false, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.GetItemRequest(string.Format("api/Admin/GetPermissionsGroups?permissionGroupNames[0]={0}&permissionGroupNames[1]={1}&permissionGroupNames[2]={2}&cultureName={3}", PermissionGroupName.InboundTransactionsTypes, PermissionGroupName.OutboundTransactionsTypes, PermissionGroupName.InternalOutboundTransactionsTypes, SessionInfo.CultureShortName)).Result;
                ViewData["PermissionsGroups"] = BuildTree(PermissionMapper.Map(permissionGroupDTOs.Result));
                return View(transactionTypeViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddTransactionType(TransactionTypeAddVM transactionTypeAddVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostTransactionType", TransactionTypeMapper.Map(transactionTypeAddVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<TransactionTypeDTO>> transactionTypeDTOs =
                   HttpClientWrapper<GetResult<List<TransactionTypeDTO>>>.GetItemRequest(string.Format("api/Admin/GetTransactionTypes?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (transactionTypeDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transactionTypeDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<TransactionTypeVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionTypeMapper.Map(transactionTypeDTOs.Result).AsQueryable(), 1, false, transactionTypeDTOs.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.TransactionType.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionTypeGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditTransactionType(TransactionTypeEditVM transactionTypeEditVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutTransactionType", TransactionTypeMapper.Map(transactionTypeEditVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<TransactionTypeDTO>> transactionTypeDTOs =
                  HttpClientWrapper<GetResult<List<TransactionTypeDTO>>>.GetItemRequest(string.Format("api/Admin/GetTransactionTypes?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (transactionTypeDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transactionTypeDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<TransactionTypeVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionTypeMapper.Map(transactionTypeDTOs.Result).AsQueryable(), 1, false, transactionTypeDTOs.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.TransactionType.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionTypeGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteTransactionType(string ids)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/DeleteTransactionTypes?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<TransactionTypeDTO>> transactionTypeDTOs =
                HttpClientWrapper<GetResult<List<TransactionTypeDTO>>>.GetItemRequest(string.Format("api/Admin/GetTransactionTypes?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (transactionTypeDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transactionTypeDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<TransactionTypeVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionTypeMapper.Map(transactionTypeDTOs.Result).AsQueryable(), 1, false, transactionTypeDTOs.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.TransactionType.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionTypeGridPartial", grid), TransactionTypesUsedList = deleteResult.Result, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetTransactionType(string id)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                GetResult<TransactionTypeEditDTO> transactionTypeEditDTO =
                    HttpClientWrapper<GetResult<TransactionTypeEditDTO>>.GetItemRequest(String.Format("api/Admin/GetTransactionTypeById?transactionTypeId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;


                if (transactionTypeEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transactionTypeEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.TransactionType.UpdateSucceeded");
                TransactionTypeEditVM transactionTypeEditVM = TransactionTypeMapper.Map(transactionTypeEditDTO.Result);
                transactionTypeEditVM.TransactionCategories = MergeTransactionCategoryLookups(transactionTypeEditVM.TransactionCategories);

                GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.GetItemRequest(string.Format("api/Admin/GetPermissionsGroups?permissionGroupNames[0]={0}&permissionGroupNames[1]={1}&permissionGroupNames[2]={2}&cultureName={3}", PermissionGroupName.InboundTransactionsTypes, PermissionGroupName.OutboundTransactionsTypes, PermissionGroupName.InternalOutboundTransactionsTypes, SessionInfo.CultureShortName)).Result;

                bool permissionFound = false;

                foreach (PermissionGroupDTO permissionGroupDTO in permissionGroupDTOs.Result)
                {
                    foreach (PermissionDTO permissionDTO in permissionGroupDTO.Permissions)
                    {
                        if (permissionDTO.Id == transactionTypeEditVM.PermissionId)
                        {
                            permissionDTO.IsSelected = true;
                            permissionFound = true;
                        }
                    }

                    if (permissionFound)
                    {
                        break;
                    }
                }

                ViewData["PermissionsGroups"] = BuildTree(PermissionMapper.Map(permissionGroupDTOs.Result));

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionTypeEditPartial", TransactionTypeMapper.Map(transactionTypeEditDTO.Result)), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult UpdateTransactionTypeGrid(int? page)
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {

                }

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                string parameters = GridHelper.GetGridParameters();

                GetResult<List<TransactionTypeDTO>> transactionTypeDTOs = HttpClientWrapper<GetResult<List<TransactionTypeDTO>>>.GetItemRequest(String.Format("api/Admin/GetTransactionTypes?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                if (transactionTypeDTOs.StatusCode != StatusCode.Ok)
                {

                }

                //var grid = new AjaxGridFactory().CreateAjaxGrid(TransactionTypeMapper.Map(transactionTypeDTOs.Result).AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, transactionTypeDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<TransactionTypeVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(TransactionTypeMapper.Map(transactionTypeDTOs.Result), page ?? 1, transactionTypeDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                return Json(new { Html = grid.ToJson("_TransactionTypeGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddPermission(PermissionVM permissionVM)
        {
            try
            {
                string message = string.Empty;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostPermission", PermissionMapper.Map(permissionVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.GetItemRequest(string.Format("api/Admin/GetPermissionsGroups?permissionGroupNames[0]={0}&permissionGroupNames[1]={1}&permissionGroupNames[2]={2}&cultureName={3}", PermissionGroupName.InboundTransactionsTypes, PermissionGroupName.OutboundTransactionsTypes, PermissionGroupName.InternalOutboundTransactionsTypes, SessionInfo.CultureShortName)).Result;

                ViewData["PermissionsGroups"] = BuildTree(PermissionMapper.Map(permissionGroupDTOs.Result), null);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Permissions.Success");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/Admin/Views/Lookups/_TransactionTypeTreePartial.cshtml", new TransactionTypeAddDTO()), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult EditPermission(PermissionEditVM permissionEditVM)
        {
            try
            {
                string message = string.Empty;

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutPermission", PermissionMapper.Map(permissionEditVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.GetItemRequest(string.Format("api/Admin/GetPermissionsGroups?permissionGroupNames[0]={0}&permissionGroupNames[1]={1}&permissionGroupNames[2]={2}&cultureName={3}", PermissionGroupName.InboundTransactionsTypes, PermissionGroupName.OutboundTransactionsTypes, PermissionGroupName.InternalOutboundTransactionsTypes, SessionInfo.CultureShortName)).Result;

                ViewData["PermissionsGroups"] = BuildTree(PermissionMapper.Map(permissionGroupDTOs.Result), null);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Permissions.Success");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/Admin/Views/Lookups/_TransactionTypeTreePartial.cshtml", new TransactionTypeAddDTO()), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
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
                GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.GetItemRequest(string.Format("api/Admin/GetPermissionsGroups?permissionGroupNames[0]={0}&permissionGroupNames[1]={1}&permissionGroupNames[2]={2}&cultureName={3}", PermissionGroupName.InboundTransactionsTypes, PermissionGroupName.OutboundTransactionsTypes, PermissionGroupName.InternalOutboundTransactionsTypes, SessionInfo.CultureShortName)).Result;

                ViewData["PermissionsGroups"] = BuildTree(PermissionMapper.Map(permissionGroupDTOs.Result), null);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Permissions.Success");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/Admin/Views/Lookups/_TransactionTypeTreePartial.cshtml", new TransactionTypeAddDTO()), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Transaction Type

        #region Suggested Topic

        public ActionResult SuggestedTopic()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest("api/Admin/GetOrgUnits?cultureName=" + SessionInfo.CultureShortName).Result;

                //if (orgUnitDTOs.Result == null)
                //{
                //    orgUnitDTOs.Result = new List<OrgUnitDTO>();
                //    orgUnitDTOs.RowsCount = 0;
                //}

                //ViewData["OrgUnits"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), -1, true);

                SuggestedTopicViewModel suggestedTopicViewModel = new SuggestedTopicViewModel();

                GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs = HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest("api/Admin/GetSuggestedTopics").Result;

                if (suggestedTopicDTOs.Result != null)
                {
                    suggestedTopicViewModel.SuggestedTopics = SuggestedTopicMapper.Map(suggestedTopicDTOs.Result);
                }

                ViewData["SuggestedTopics"] = JsonConvert.SerializeObject(suggestedTopicViewModel.SuggestedTopics);

                suggestedTopicViewModel.SuggestedTopics = SortSuggestedTopic(suggestedTopicViewModel.SuggestedTopics);

                return View(suggestedTopicViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddSuggestedTopic(SuggestedTopicVM suggestedTopicAddVM, string hdnAddSuggestedTopics)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<SuggestedTopicVM> suggestedTopicVMs = new List<SuggestedTopicVM>();
                List<SuggestedTopicVM> sortedSuggestedTopicVMs = new List<SuggestedTopicVM>();

                if (!string.IsNullOrEmpty(hdnAddSuggestedTopics))
                {
                    suggestedTopicVMs.AddRange(javaScriptSerializer.Deserialize(hdnAddSuggestedTopics, typeof(List<SuggestedTopicVM>)) as List<SuggestedTopicVM>);
                }

                suggestedTopicAddVM.Id = suggestedTopicVMs.Count > 0 ? suggestedTopicVMs.Max(s => s.Id) + 1 : 1;
                suggestedTopicAddVM.IsNew = true;

                suggestedTopicVMs.Add(suggestedTopicAddVM);

                string data = JsonConvert.SerializeObject(suggestedTopicVMs);

                sortedSuggestedTopicVMs = SortSuggestedTopic(suggestedTopicVMs);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SuggestedTopicTreePartial", sortedSuggestedTopicVMs), Data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditSuggestedTopic(SuggestedTopicVM suggestedTopicVM, string hdnEditSuggestedTopics)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<SuggestedTopicVM> suggestedTopicVMs = new List<SuggestedTopicVM>();
                List<SuggestedTopicVM> sortedSuggestedTopicVMs = new List<SuggestedTopicVM>();

                if (!string.IsNullOrEmpty(hdnEditSuggestedTopics))
                {
                    suggestedTopicVMs.AddRange(javaScriptSerializer.Deserialize(hdnEditSuggestedTopics, typeof(List<SuggestedTopicVM>)) as List<SuggestedTopicVM>);
                }

                for (int i = 0; i < suggestedTopicVMs.Count; i++)
                {
                    if (suggestedTopicVMs[i].Id == suggestedTopicVM.Id)
                    {
                        suggestedTopicVMs[i] = suggestedTopicVM;
                    }
                }

                string data = JsonConvert.SerializeObject(suggestedTopicVMs);

                sortedSuggestedTopicVMs = SortSuggestedTopic(suggestedTopicVMs);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SuggestedTopicTreePartial", sortedSuggestedTopicVMs), Data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SaveSuggestedTopic(string hdnSuggestedTopics)
        {
            try
            {
                string message = string.Empty;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<SuggestedTopicVM> suggestedTopicVMs = new List<SuggestedTopicVM>();
                List<SuggestedTopicVM> sortedSuggestedTopicVMs = new List<SuggestedTopicVM>();

                if (!string.IsNullOrEmpty(hdnSuggestedTopics))
                {
                    suggestedTopicVMs.AddRange(javaScriptSerializer.Deserialize(hdnSuggestedTopics, typeof(List<SuggestedTopicVM>)) as List<SuggestedTopicVM>);
                }

                suggestedTopicVMs = SetOrgUnitByParentSuggestedTopic(suggestedTopicVMs);


                PostObjectResult<List<int>> postResult = HttpClientWrapper<PostObjectResult<List<int>>>.PostRequest("api/Admin/PostSuggestedTopics", SuggestedTopicMapper.Map(suggestedTopicVMs)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                string data = JsonConvert.SerializeObject(suggestedTopicVMs);

                sortedSuggestedTopicVMs = SortSuggestedTopic(suggestedTopicVMs);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.SuggestedTopic.Success");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SuggestedTopicTreePartial", sortedSuggestedTopicVMs), Data = data, MessageType = MessageType.Information, MessageText = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetSuggestedTopic(int id, string suggestedTopics)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest("api/Admin/GetOrgUnits?cultureName=" + SessionInfo.CultureShortName).Result;

                //if (orgUnitDTOs.StatusCode != StatusCode.Ok)
                //{
                //    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, orgUnitDTOs.StatusCode.ToString());

                //    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                //}


                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<SuggestedTopicVM> suggestedTopicVMs = new List<SuggestedTopicVM>();
                SuggestedTopicVM suggestedTopicVM = new SuggestedTopicVM();

                if (!string.IsNullOrEmpty(suggestedTopics))
                {
                    suggestedTopicVMs.AddRange(javaScriptSerializer.Deserialize(suggestedTopics, typeof(List<SuggestedTopicVM>)) as List<SuggestedTopicVM>);
                }

                suggestedTopicVM = suggestedTopicVMs.Where(s => s.Id == id).FirstOrDefault();

                if (suggestedTopicVM.OrgUnits != null)
                {
                    foreach (int orgUnitId in suggestedTopicVM.OrgUnits)
                    {
                        //orgUnitDTOs.Result.Where(o => o.Id == orgUnitId).FirstOrDefault().IsSelected = true;
                    }
                }


                //ViewData["OrgUnits"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), -1, true);


                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SuggestedTopicEditPartial", suggestedTopicVM), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteSuggestedTopics(List<int> ids, string suggestedTopics)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<SuggestedTopicVM> suggestedTopicVMs = new List<SuggestedTopicVM>();
                List<SuggestedTopicVM> sortedSuggestedTopicVMs = new List<SuggestedTopicVM>();

                if (!string.IsNullOrEmpty(suggestedTopics))
                {
                    suggestedTopicVMs.AddRange(javaScriptSerializer.Deserialize(suggestedTopics, typeof(List<SuggestedTopicVM>)) as List<SuggestedTopicVM>);
                }

                foreach (var id in ids)
                {
                    SuggestedTopicVM item = suggestedTopicVMs.Where(s => s.Id == id).FirstOrDefault();

                    if (item != null)
                    {
                        if (item.IsNew)
                        {
                            suggestedTopicVMs.Remove(item);
                        }
                        else
                        {
                            suggestedTopicVMs.Where(s => s.Id == id).FirstOrDefault().IsDeleted = true;
                        }
                    }
                }

                suggestedTopicVMs = DeleteByParentsIdSuggestedTopic(suggestedTopicVMs, ids);

                string data = JsonConvert.SerializeObject(suggestedTopicVMs);

                sortedSuggestedTopicVMs = SortSuggestedTopic(suggestedTopicVMs);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SuggestedTopicTreePartial", sortedSuggestedTopicVMs), MessageType = MessageType.Information, Data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<SuggestedTopicVM> DeleteByParentsIdSuggestedTopic(List<SuggestedTopicVM> suggestedTopicVM, List<int> parentIds)
        {
            List<int> ids = new List<int>();

            foreach (var parentId in parentIds)
            {
                ids.AddRange(suggestedTopicVM.Where(s => s.ParentId == parentId).Select(t => t.Id).ToList());

                suggestedTopicVM.Where(s => s.ParentId == parentId).ToList().ForEach(s =>
                {
                    if (s.IsNew)
                    {
                        suggestedTopicVM.Remove(s);
                    }
                    else
                    {
                        s.IsDeleted = true;
                    }
                });
            }

            if (ids.Count > 0)
            {
                DeleteByParentsIdSuggestedTopic(suggestedTopicVM, ids);
            }

            return suggestedTopicVM;
        }

        private List<SuggestedTopicVM> SetOrgUnitByParentSuggestedTopic(List<SuggestedTopicVM> suggestedTopicVM)
        {
            suggestedTopicVM.Where(s => s.OrgUnits == null).ToList().ForEach(s =>
            {
                s.OrgUnits = GetParentOrgUnitsSuggestedTopic(suggestedTopicVM, s.Id);
            });

            return suggestedTopicVM;
        }

        private List<int> GetParentOrgUnitsSuggestedTopic(List<SuggestedTopicVM> suggestedTopicVMs, int elementId)
        {
            SuggestedTopicVM subjectClassificationVM = suggestedTopicVMs.Where(s => s.Id == elementId).FirstOrDefault();

            if (subjectClassificationVM.ParentId == null)
            {
                return subjectClassificationVM.OrgUnits;
            }

            if (subjectClassificationVM.OrgUnits == null)
            {
                SuggestedTopicVM parent =
                    suggestedTopicVMs.Where(s => s.Id == subjectClassificationVM.ParentId).FirstOrDefault();

                if (parent != null)
                {
                    if (parent.OrgUnits == null)
                    {
                        return GetParentOrgUnitsSuggestedTopic(suggestedTopicVMs, parent.Id);
                    }

                    return parent.OrgUnits;
                }
            }

            return subjectClassificationVM.OrgUnits;
        }

        private List<SuggestedTopicVM> SortSuggestedTopic(List<SuggestedTopicVM> suggestedTopicVMs)
        {
            List<SuggestedTopicVM> data = new List<SuggestedTopicVM>();

            suggestedTopicVMs.Where(o => o.ParentId == null).ToList().ForEach(d =>
            {
                data.Add(AddChildsSuggestedTopic(suggestedTopicVMs, d));
            });

            return suggestedTopicVMs;
        }

        private SuggestedTopicVM AddChildsSuggestedTopic(List<SuggestedTopicVM> suggestedTopicVMs, SuggestedTopicVM suggestedTopicVM)
        {
            suggestedTopicVMs.Where(o => o.ParentId == suggestedTopicVM.Id).ToList().ForEach(d =>
            {
                if (suggestedTopicVM.Childs == null)
                {
                    suggestedTopicVM.Childs = new List<SuggestedTopicVM>();
                }

                suggestedTopicVMs.Remove(d);
                suggestedTopicVM.Childs.Add(AddChildsSuggestedTopic(suggestedTopicVMs, d));
            });

            return suggestedTopicVM;
        }

        #endregion

        #region SubjectClassification

        public ActionResult SubjectClassification()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest("api/Admin/GetOrgUnits?cultureName=" + SessionInfo.CultureShortName).Result;

                //if (orgUnitDTOs.Result == null)
                //{
                //    orgUnitDTOs.Result = new List<OrgUnitDTO>();
                //    orgUnitDTOs.RowsCount = 0;
                //}

                //ViewData["OrgUnits"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), -1, true);

                SubjectClassificationViewModel subjectClassificationViewModel = new SubjectClassificationViewModel();

                //GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs = HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest("api/Admin/GetSubjectClassifications").Result;

                //if (subjectClassificationDTOs.Result != null)
                //{
                //     subjectClassificationViewModel.SubjectClassifications = SubjectClassificationMapper.Map(subjectClassificationDTOs.Result);
                //}

                // ViewData["SubjectClassifications"] = JsonConvert.SerializeObject(subjectClassificationViewModel.SubjectClassifications);

                //  subjectClassificationViewModel.SubjectClassifications = SortSubjectClassification(subjectClassificationViewModel.SubjectClassifications);

                return View(subjectClassificationViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddSubjectClassification(SubjectClassificationVM subjectClassificationAddVM, string hdnAddSubjectClassifications)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<SubjectClassificationVM> subjectClassificationVMs = new List<SubjectClassificationVM>();
                List<SubjectClassificationVM> sortedSubjectClassificationVMs = new List<SubjectClassificationVM>();

                if (!string.IsNullOrEmpty(hdnAddSubjectClassifications))
                {
                    subjectClassificationVMs.AddRange(javaScriptSerializer.Deserialize(hdnAddSubjectClassifications, typeof(List<SubjectClassificationVM>)) as List<SubjectClassificationVM>);
                }

                subjectClassificationAddVM.Id = subjectClassificationVMs.Count > 0 ? subjectClassificationVMs.Max(s => s.Id) + 1 : 1;
                subjectClassificationAddVM.IsNew = true;

                subjectClassificationVMs.Add(subjectClassificationAddVM);

                string data = JsonConvert.SerializeObject(subjectClassificationVMs);

                sortedSubjectClassificationVMs = SortSubjectClassification(subjectClassificationVMs);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectClassificationTreePartial", sortedSubjectClassificationVMs), Data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditSubjectClassification(SubjectClassificationVM subjectClassificationVM, string hdnEditSubjectClassifications)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PutResult PutResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutSubjectClassification", SubjectClassificationMapper.Map(subjectClassificationVM)).Result;

                if (PutResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, PutResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
                     HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Admin/GetSubjectClassifications?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (subjectClassificationDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, subjectClassificationDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<SubjectClassificationVM>)new AjaxGridFactory().CreateAjaxGrid(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result).AsQueryable(), 1, false, subjectClassificationDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<SubjectClassificationVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result), 1, subjectClassificationDTOs.RowsCount.Value, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "تم التعديل");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectClassificationGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SaveSubjectClassification(string hdnSubjectClassifications)
        {
            try
            {
                string message = string.Empty;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<SubjectClassificationVM> subjectClassificationVMs = new List<SubjectClassificationVM>();
                List<SubjectClassificationVM> sortedSubjectClassificationVMs = new List<SubjectClassificationVM>();

                if (!string.IsNullOrEmpty(hdnSubjectClassifications))
                {
                    subjectClassificationVMs.AddRange(javaScriptSerializer.Deserialize(hdnSubjectClassifications, typeof(List<SubjectClassificationVM>)) as List<SubjectClassificationVM>);
                }

                subjectClassificationVMs = SetOrgUnitByParent(subjectClassificationVMs);

                PostObjectResult<List<int>> postResult = HttpClientWrapper<PostObjectResult<List<int>>>.PostRequest("api/Admin/PostSubjectClassifications", SubjectClassificationMapper.Map(subjectClassificationVMs)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                string data = JsonConvert.SerializeObject(subjectClassificationVMs);

                sortedSubjectClassificationVMs = SortSubjectClassification(subjectClassificationVMs);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.SubjectClassification.Success");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectClassificationTreePartial", sortedSubjectClassificationVMs), Data = data, MessageType = MessageType.Information, MessageText = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetSubjectClassification(int id, string subjectClassifications)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest("api/Admin/GetOrgUnits?cultureName=" + SessionInfo.CultureShortName).Result;

                //if (orgUnitDTOs.StatusCode != StatusCode.Ok)
                //{
                //    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, orgUnitDTOs.StatusCode.ToString());

                //    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                //}

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<SubjectClassificationVM> subjectClassificationVMs = new List<SubjectClassificationVM>();
                SubjectClassificationVM subjectClassificationVM = new SubjectClassificationVM();

                if (!string.IsNullOrEmpty(subjectClassifications))
                {
                    subjectClassificationVMs.AddRange(javaScriptSerializer.Deserialize(subjectClassifications, typeof(List<SubjectClassificationVM>)) as List<SubjectClassificationVM>);
                }

                subjectClassificationVM = subjectClassificationVMs.Where(s => s.Id == id).FirstOrDefault();

                if (subjectClassificationVM.OrgUnits != null)
                {
                    foreach (int orgUnitId in subjectClassificationVM.OrgUnits)
                    {
                        //orgUnitDTOs.Result.Where(o => o.Id == orgUnitId).FirstOrDefault().IsSelected = true;
                    }
                }

                //ViewData["OrgUnits"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), -1, true);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectClassificationEditPartial", subjectClassificationVM), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteSubjectClassifications(List<int> ids, string subjectClassifications)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<SubjectClassificationVM> subjectClassificationVMs = new List<SubjectClassificationVM>();
                List<SubjectClassificationVM> sortedSubjectClassificationVMs = new List<SubjectClassificationVM>();

                if (!string.IsNullOrEmpty(subjectClassifications))
                {
                    subjectClassificationVMs.AddRange(javaScriptSerializer.Deserialize(subjectClassifications, typeof(List<SubjectClassificationVM>)) as List<SubjectClassificationVM>);
                }

                foreach (var id in ids)
                {
                    SubjectClassificationVM item = subjectClassificationVMs.Where(s => s.Id == id).FirstOrDefault();
                    if (item != null)
                    {
                        if (item.IsNew)
                        {
                            subjectClassificationVMs.Remove(item);
                        }
                        else
                        {
                            subjectClassificationVMs.Where(s => s.Id == id).FirstOrDefault().IsDeleted = true;
                        }
                    }
                }

                subjectClassificationVMs = DeleteByParentsId(subjectClassificationVMs, ids);

                string data = JsonConvert.SerializeObject(subjectClassificationVMs);

                sortedSubjectClassificationVMs = SortSubjectClassification(subjectClassificationVMs);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectClassificationTreePartial", sortedSubjectClassificationVMs), MessageType = MessageType.Information, Data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult LockUnlockLookup(LookupType lookupType, LookupOperationType lookupOperationType, int lookUpId)
        {
            try
            {
                if (lookupOperationType != LookupOperationType.Lock && lookupOperationType != LookupOperationType.UnLock)
                {
                    return Json(new { MessageText = DbRes.TValidation("Admin.Lookups.UnlockValidation"), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                if (lookupOperationType == LookupOperationType.UnLock && (!(CheckIfHasUnlockPermission(UserClaims.Admin.LookupsUnLock) || GetLockOwner(lookupType, lookUpId) == SessionInfo.CurrentUser.Id)))
                {
                    return Json(new { MessageText = DbRes.TValidation("Admin.Lookups.UnlockValidation"), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                PutResult putResult =
                          HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Admin/LockUnlockLookup?lookupType={0}&lookUpId={1}&UserId={2}", (int)lookupType, lookUpId, SessionInfo.CurrentUser.Id), null).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    return Json(new { MessageText = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString()), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                string GridPartialName = string.Empty;
                CustomAjaxGrid.IAjaxGrid ajaxGrid = null;
                switch (lookupType)
                {
                    case LookupType.Form:
                        ajaxGrid = RefreshGridFormItems();
                        GridPartialName = "_FormGridPartial";
                        break;
                    case LookupType.Link:
                        ajaxGrid = RefreshGridLinkItems();
                        GridPartialName = "_LinkGridPartial";
                        break;
                    case LookupType.AttachmentType:
                        ajaxGrid = RefreshGridAttachmentTypeItems();
                        GridPartialName = "_AttachmentTypeGridPartial";
                        break;
                    case LookupType.Actions:
                        ajaxGrid = RefreshGridActionsItems();
                        GridPartialName = "~/Areas/Admin/Views/Actions/_GridActionPartial.cshtml";
                        break;
                    case LookupType.Correspondent:
                        ajaxGrid = RefreshGridCorrespondentsItems();
                        GridPartialName = "_CorrespondentGridPartial";
                        break;
                    case LookupType.FollowUpPriorityType:
                        ajaxGrid = RefreshGridFollowUpPrioritytypeItems();
                        GridPartialName = "_FollowUpPriorityTypeGridParial";
                        break;
                    case LookupType.FollowUpMethod:
                        ajaxGrid = RefreshGridFollowUpMethodItems();
                        GridPartialName = "_FollowUpMethodGridParial";
                        break;
                    case LookupType.FollowUpProccess:
                        ajaxGrid = RefreshGridFollowUpProccessItems();
                        GridPartialName = "_FollowUpProccessGridParial";
                        break;
                    case LookupType.FollowUpSource:
                        ajaxGrid = RefreshGridFollowUpSourceItems();
                        GridPartialName = "_FollowUpSourceTypeGridParial";
                        break;
                    default:
                        break;
                }

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, GridPartialName, ajaxGrid) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult ActiveDeactiveLookup(LookupType lookupType, LookupOperationType lookupOperationType, int lookUpId)
        {
            try
            {
                string message = string.Empty;
                bool IsActive = lookupOperationType == LookupOperationType.Active ? true : false;
                PutResult putResult =
                      HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Admin/ActiveDeactiveLookup?lookupType={0}&lookUpId={1}", (int)lookupType, lookUpId), null).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                string GridPartialName = string.Empty;
                CustomAjaxGrid.IAjaxGrid ajaxGrid = null;
                switch (lookupType)
                {
                    case LookupType.Form:
                        ajaxGrid = RefreshGridFormItems();
                        GridPartialName = "_FormGridPartial";
                        break;
                    case LookupType.Link:
                        ajaxGrid = RefreshGridLinkItems();
                        GridPartialName = "_LinkGridPartial";
                        break;
                    case LookupType.AttachmentType:
                        ajaxGrid = RefreshGridAttachmentTypeItems();
                        GridPartialName = "_AttachmentTypeGridPartial";
                        break;
                    case LookupType.Actions:
                        ajaxGrid = RefreshGridActionsItems();
                        GridPartialName = "~/Areas/Admin/Views/Actions/_GridActionPartial.cshtml";
                        break;
                    case LookupType.Correspondent:
                        ajaxGrid = RefreshGridCorrespondentsItems();
                        GridPartialName = "_CorrespondentGridPartial";
                        break;
                    case LookupType.FollowUpPriorityType:
                        ajaxGrid = RefreshGridFollowUpPrioritytypeItems();
                        GridPartialName = "_FollowUpPriorityTypeGridParial";
                        break;
                    case LookupType.FollowUpMethod:
                        ajaxGrid = RefreshGridFollowUpMethodItems();
                        GridPartialName = "_FollowUpMethodGridParial";
                        break;
                    case LookupType.FollowUpProccess:
                        ajaxGrid = RefreshGridFollowUpProccessItems();
                        GridPartialName = "_FollowUpProccessGridParial";
                        break;
                    case LookupType.FollowUpSource:
                        ajaxGrid = RefreshGridFollowUpSourceItems();
                        GridPartialName = "_FollowUpSourceTypeGridParial";
                        break;
                    case LookupType.SaveReason:
                        ajaxGrid = RefreshGridSaveReasonItems();
                        GridPartialName = "_SaveReasonGridPartial";
                        break;
                    default:
                        break;
                }

                //GetResult<List<FormDTO>> formDTOs =
                //       HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(string.Format("api/Admin/GetForms?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
                //List<FormVM> formVMs = FormMapper.Map(formDTOs.Result);
                //if (formVMs == null)
                //{
                //    formVMs = new List<FormVM>();
                //    formDTOs.RowsCount = 0;
                //}

                //CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<FormVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(formVMs, 1, 0, false);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, GridPartialName, ajaxGrid) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private int GetLockOwner(LookupType lookupType, int lookUpId)
        {
            int lockedBy = 0;
            switch (lookupType)
            {
                case LookupType.Form:
                    GetResult<FormEditDTO> formEditDTO =
                    HttpClientWrapper<GetResult<FormEditDTO>>.GetItemRequest(String.Format("api/Admin/GetFormById?formId={0}&cultureName={1}", lookUpId, SessionInfo.CultureShortName)).Result;
                    FormEditVM formEditVM = FormMapper.Map(formEditDTO.Result);
                    lockedBy = formEditVM.LockedBy ?? 0;
                    break;
                case LookupType.Link:
                    GetResult<LinkEditDTO> linkEditDTO =
                 HttpClientWrapper<GetResult<LinkEditDTO>>.GetItemRequest(String.Format("api/Admin/GetLinkById?linkId={0}&cultureName={1}", lookUpId, SessionInfo.CultureShortName)).Result;
                    LinkEditVM linkEditVM = LinkMapper.Map(linkEditDTO.Result);
                    lockedBy = linkEditVM.LockedBy ?? 0;
                    break;
                default:
                    break;
            }
            return lockedBy;
        }

        private CustomAjaxGrid.AjaxGrid<FormVM> RefreshGridFormItems()
        {
            GetResult<List<FormDTO>> formDTOs =
                   HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(string.Format("api/Admin/GetForms?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
            List<FormVM> formVMs = FormMapper.Map(formDTOs.Result);
            return (CustomAjaxGrid.AjaxGrid<FormVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(formVMs, 1, formDTOs.RowsCount.Value, false);
        }
        private CustomAjaxGrid.AjaxGrid<LinkVM> RefreshGridLinkItems()
        {
            GetResult<List<LinkDTO>> getResult =
                   HttpClientWrapper<GetResult<List<LinkDTO>>>.GetItemRequest(string.Format("api/Admin/GetLinks?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
            List<LinkVM> linkVMs = LinkMapper.Map(getResult.Result);
            return (CustomAjaxGrid.AjaxGrid<LinkVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(linkVMs, 1, getResult.RowsCount.Value, false);
        }
        private CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM> RefreshGridFollowUpPrioritytypeItems()
        {
            GetResult<List<FollowUpLookUpDTO>> getResult =
                   HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpPrioritytype?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
            List<FollowUpLookUpsVM> followUpPriorityTypesVMs = FollowUpLookUpsMapper.Map(getResult.Result);
            return (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(followUpPriorityTypesVMs, 1, getResult.RowsCount.Value, false);
        }
        private CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM> RefreshGridFollowUpMethodItems()
        {
            GetResult<List<FollowUpLookUpDTO>> getResult =
                   HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpMethod?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
            List<FollowUpLookUpsVM> followUpMethodsVMs = FollowUpLookUpsMapper.Map(getResult.Result);
            return (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(followUpMethodsVMs, 1, getResult.RowsCount.Value, false);
        }
        private CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM> RefreshGridFollowUpSourceItems()
        {
            GetResult<List<FollowUpLookUpDTO>> getResult =
                   HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpSource?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
            List<FollowUpLookUpsVM> followUpSourcesVMs = FollowUpLookUpsMapper.Map(getResult.Result);
            return (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(followUpSourcesVMs, 1, getResult.RowsCount.Value, false);
        }
        private CustomAjaxGrid.AjaxGrid<MCS.UI.Areas.User.Models.Lookups.LookupVM> RefreshGridSaveReasonItems()
        {
            List<MCS.UI.Areas.User.Models.Lookups.LookupVM> LookupVMs = LookupsHelper.GetLookupItemswithoutCached(LookupCategory.SaveReason, SessionInfo.CultureShortName).Result.ToList();

            return (CustomAjaxGrid.AjaxGrid<MCS.UI.Areas.User.Models.Lookups.LookupVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(LookupVMs, 1, LookupVMs.Count, false);
        }
        private CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM> RefreshGridFollowUpProccessItems()
        {
            GetResult<List<FollowUpLookUpDTO>> getResult =
                   HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Admin/GetFollowUpProccess?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
            List<FollowUpLookUpsVM> followUpProccesssVMs = FollowUpLookUpsMapper.Map(getResult.Result);
            return (CustomAjaxGrid.AjaxGrid<FollowUpLookUpsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(followUpProccesssVMs, 1, getResult.RowsCount.Value, false);
        }
        private CustomAjaxGrid.AjaxGrid<AttachmentTypeVM> RefreshGridAttachmentTypeItems()
        {
            GetResult<List<AttachmentTypeDTO>> getResult =
                   HttpClientWrapper<GetResult<List<AttachmentTypeDTO>>>.GetItemRequest(string.Format("api/Admin/GetAttachmentTypes?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
            List<AttachmentTypeVM> attachmentTypeVMs = AttachmentTypeMapper.Map(getResult.Result);
            return (CustomAjaxGrid.AjaxGrid<AttachmentTypeVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(attachmentTypeVMs, 1, getResult.RowsCount.Value, false);
        }
        private CustomAjaxGrid.AjaxGrid<ActionVM> RefreshGridActionsItems()
        {
            GetResult<List<ActionDTO>> processDTOs = HttpClientWrapper<GetResult<List<ActionDTO>>>
                    .GetItemRequest(string.Format("api/Admin/GetActions?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
            List<ActionVM> actionVMs = ActionMapper.Map(processDTOs.Result);
            return (CustomAjaxGrid.AjaxGrid<ActionVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(actionVMs, 1, processDTOs.RowsCount.Value, false);
        }
        private CustomAjaxGrid.AjaxGrid<ReporterVM> RefreshGridCorrespondentsItems()
        {
            GetResult<List<ReporterDTO>> getResult =
                                  HttpClientWrapper<GetResult<List<ReporterDTO>>>.GetItemRequest(string.Format("api/Admin/GetReporters?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
            List<ReporterVM> reporterVMs = ReporterMapper.Map(getResult.Result);
            return (CustomAjaxGrid.AjaxGrid<ReporterVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(reporterVMs, 1, getResult.RowsCount.Value, false);
        }

        private bool CheckIfHasUnlockPermission(string permission)
        {
            return SessionInfo.CurrentUser.Claims.Contains(permission);
        }

        private List<SubjectClassificationVM> DeleteByParentsId(List<SubjectClassificationVM> subjectClassificationVMs, List<int> parentIds)
        {
            List<int> ids = new List<int>();

            foreach (var parentId in parentIds)
            {
                ids.AddRange(subjectClassificationVMs.Where(s => s.ParentId == parentId).Select(t => t.Id).ToList());

                subjectClassificationVMs.Where(s => s.ParentId == parentId).ToList().ForEach(s =>
                {
                    if (s.IsNew)
                    {
                        subjectClassificationVMs.Remove(s);
                    }
                    else
                    {
                        s.IsDeleted = true;
                    }
                });
            }

            if (ids.Count > 0)
            {
                DeleteByParentsId(subjectClassificationVMs, ids);
            }

            return subjectClassificationVMs;
        }

        private List<SubjectClassificationVM> SetOrgUnitByParent(List<SubjectClassificationVM> subjectClassificationVMs)
        {
            subjectClassificationVMs.Where(s => s.OrgUnits == null).ToList().ForEach(s =>
            {
                s.OrgUnits = GetParentOrgUnits(subjectClassificationVMs, s.Id);
            });

            return subjectClassificationVMs;
        }

        private List<int> GetParentOrgUnits(List<SubjectClassificationVM> subjectClassificationVMs, int elementId)
        {
            SubjectClassificationVM subjectClassificationVM = subjectClassificationVMs.Where(s => s.Id == elementId).FirstOrDefault();

            if (subjectClassificationVM.ParentId == null)
            {
                return subjectClassificationVM.OrgUnits;
            }

            if (subjectClassificationVM.OrgUnits == null)
            {
                SubjectClassificationVM parent = subjectClassificationVMs.Where(s => s.Id == subjectClassificationVM.ParentId).FirstOrDefault();

                if (parent != null)
                {
                    if (parent.OrgUnits == null)
                    {
                        return GetParentOrgUnits(subjectClassificationVMs, parent.Id);
                    }

                    return parent.OrgUnits;
                }
            }

            return subjectClassificationVM.OrgUnits;
        }

        private List<SubjectClassificationVM> AddParentSubjectClassification(List<SubjectClassificationVM> subjectClassificationVMs)
        {
            subjectClassificationVMs.ForEach(parent =>
            {
                bool hasChild = false;

                subjectClassificationVMs.ForEach(child =>
                {
                    if (parent.Id == child.ParentId)
                    {
                        hasChild = true;
                        child.Parent = parent;
                    }
                });

                if (hasChild)
                {
                    subjectClassificationVMs.Remove(parent);
                }
            });

            return subjectClassificationVMs;
        }

        private List<SubjectClassificationVM> SortSubjectClassification(List<SubjectClassificationVM> subjectClassificationVMs)
        {
            List<SubjectClassificationVM> data = new List<SubjectClassificationVM>();

            subjectClassificationVMs.Where(o => o.ParentId == null).ToList().ForEach(d =>
            {
                data.Add(AddChilds(subjectClassificationVMs, d));
            });

            return subjectClassificationVMs;
        }

        private SubjectClassificationVM AddChilds(List<SubjectClassificationVM> subjectClassificationVMs, SubjectClassificationVM subjectClassificationVM)
        {
            subjectClassificationVMs.Where(o => o.ParentId == subjectClassificationVM.Id).ToList().ForEach(d =>
            {
                if (subjectClassificationVM.Childs == null)
                {
                    subjectClassificationVM.Childs = new List<SubjectClassificationVM>();
                }

                subjectClassificationVMs.Remove(d);
                subjectClassificationVM.Childs.Add(AddChilds(subjectClassificationVMs, d));
            });

            return subjectClassificationVM;
        }

        #endregion SubjectClassification


        #region ConfidentialityLevel
        [HttpGet]
        public ActionResult ConfidentialityLevel()
        {
            try
            {
                var urlConfidentialityLevel = string.Format("api/Admin/GetConfidentialities?PageIndex=1&PageSize={0}&CultureName={1}&groupId={2}", GridHelper.PageSize, SessionInfo.CultureShortName, (int)PermissionGroupName.TransactiosConfidentiality);
                GetResult<List<ConfidentialityLevelDTO>> confidentialityLevelDTOs = HttpClientWrapper<GetResult<List<ConfidentialityLevelDTO>>>.GetItemRequest(urlConfidentialityLevel).Result;

                List<ConfidentialityLevelVM> confidentialityLevelVMs = ConfidentialityLevelMapper.Map(confidentialityLevelDTOs.Result);

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<ConfidentialityLevelVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(confidentialityLevelVMs, 1, confidentialityLevelDTOs.RowsCount.Value, false, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                return View("_ConfidentialityLevelPartial", grid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult UpdateConfidentialityLevel(int? page)
        {
            try
            {
                var urlConfidentialityLevel = string.Format("api/Admin/GetConfidentialities?PageIndex={0}&PageSize={1}&CultureName={2}&groupId={3}", page, GridHelper.PageSize, SessionInfo.CultureShortName, (int)PermissionGroupName.TransactiosConfidentiality);
                GetResult<List<ConfidentialityLevelDTO>> confidentialityLevelDTOs = HttpClientWrapper<GetResult<List<ConfidentialityLevelDTO>>>.GetItemRequest(urlConfidentialityLevel).Result;

                List<ConfidentialityLevelVM> confidentialityLevelVMs = ConfidentialityLevelMapper.Map(confidentialityLevelDTOs.Result);

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<ConfidentialityLevelVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(confidentialityLevelVMs, page ?? 1, confidentialityLevelDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ConfidentialityLevelGridPartial", grid) /*grid.ToJson("_FormGridPartial", this), grid.HasItems*/ }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region Correspondents
        public ActionResult Correspondent()
        {

            GetResult<List<ReporterDTO>> getResult =
                    HttpClientWrapper<GetResult<List<ReporterDTO>>>.GetItemRequest(string.Format("api/Admin/GetReporters?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
            List<ReporterVM> reporterVMs = ReporterMapper.Map(getResult.Result);
            if (reporterVMs == null)
            {
                reporterVMs = new List<ReporterVM>();
                getResult.RowsCount = 0;
            }

            CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<ReporterVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(reporterVMs, 1, getResult.RowsCount.Value, false, GridHelper.PageSize);

            ViewData["GridData"] = grid;
            GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                       HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
            ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
            //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest("api/Admin/GetOrgUnits?cultureName=" + SessionInfo.CultureShortName).Result;

            //ViewData["OrgUnitData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), -1, true);

            return View(new ReporterVM());
        }

        [HttpGet]
        public ActionResult UpdateCorrespondentGrid(int? page)
        {

            GetResult<List<ReporterDTO>> getResult =
                    HttpClientWrapper<GetResult<List<ReporterDTO>>>.GetItemRequest(string.Format("api/Admin/GetReporters?PageIndex={0}&PageSize={1}&CultureName={2}", page, GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
            List<ReporterVM> reporterVMs = ReporterMapper.Map(getResult.Result);
            if (reporterVMs == null)
            {
                reporterVMs = new List<ReporterVM>();
                getResult.RowsCount = 0;
            }

            CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<ReporterVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(reporterVMs, page ?? 1, getResult.RowsCount.Value, page.HasValue, GridHelper.PageSize);

            ViewData["GridData"] = grid;

            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CorrespondentGridPartial", grid) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddCorrespondent(ReporterVM reporterVM)
        {
            try
            {
                string message = string.Empty;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostReporter", ReporterMapper.Map(reporterVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                GetResult<List<ReporterDTO>> getResult =
                                   HttpClientWrapper<GetResult<List<ReporterDTO>>>.GetItemRequest(string.Format("api/Admin/GetReporters?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;


                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                List<ReporterVM> correspondentVMs = ReporterMapper.Map(getResult.Result);

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<ReporterVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(correspondentVMs, 1, getResult.RowsCount.Value, false, UIHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentType.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CorrespondentGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetCorrespondent(string id)
        {
            try
            {
                string message = string.Empty;

                GetResult<ReporterDTO> getResult =
                    HttpClientWrapper<GetResult<ReporterDTO>>.GetItemRequest(String.Format("api/Admin/GetReporterById?reporterId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                     HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
                //TODO: Change Source Key To Be "Admin.Link.UpdateSucceeded"
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.TransactionLink.UpdateSucceeded");

                ReporterVM reporterVM = ReporterMapper.Map(getResult.Result);

                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest("api/Admin/GetOrgUnits?cultureName=" + SessionInfo.CultureShortName).Result;

                //ViewData["OrgUnitData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), -1, true);

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CorrespondentEditPartial", reporterVM
                    ),
                    MessageText = message,
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditCorrespondent(ReporterVM correspondentVM)
        {
            try
            {
                string message = string.Empty;

                PutResult PutResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutReporter", ReporterMapper.Map(correspondentVM)).Result;

                if (PutResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, PutResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<ReporterDTO>> getResult =
                                  HttpClientWrapper<GetResult<List<ReporterDTO>>>.GetItemRequest(string.Format("api/Admin/GetReporters?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<ReporterVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(ReporterMapper.Map(getResult.Result), 1, getResult.RowsCount.Value, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.TransactionLink.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CorrespondentGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteCorrespondent(string ids)
        {
            try
            {
                string message = string.Empty;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Admin/DeleteReporter?reporterId={0}", ids), null).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<ReporterDTO>> getResult =
                                  HttpClientWrapper<GetResult<List<ReporterDTO>>>.GetItemRequest(string.Format("api/Admin/GetReporters?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<LinkVM>)new AjaxGridFactory().CreateAjaxGrid(LinkMapper.Map(linkDTOs.Result).AsQueryable(), 1, false, linkDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<ReporterVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(ReporterMapper.Map(getResult.Result), 1, getResult.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.TransactionLink.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CorrespondentGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult SubjectClassifications()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                SubjectClassificationViewModel subjectClassificationViewModel = new SubjectClassificationViewModel();
                List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups();

                subjectClassificationViewModel.SubjectClassificationAddVM.TransactionCategories = transactionCategoryVMs;
                subjectClassificationViewModel.SubjectClassificationEditVM.TransactionCategories = transactionCategoryVMs;

                GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOList =
                    HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Admin/GetSubjectClassifications")).Result;

                List<SubjectClassificationVM> subjectClassificationVMList = SubjectClassificationMapper.Map(subjectClassificationDTOList.Result);
                if (subjectClassificationVMList == null)
                {
                    subjectClassificationVMList = new List<SubjectClassificationVM>();
                    subjectClassificationDTOList.RowsCount = 0;
                }

                //IAjaxGrid grid = (AjaxGrid<LinkVM>)new AjaxGridFactory().CreateAjaxGrid(linkVMs.AsQueryable(), 1, false, linkDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<SubjectClassificationVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(subjectClassificationVMList, 1, subjectClassificationDTOList.RowsCount.Value, false, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                return View(subjectClassificationViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddSubjectClassifications(SubjectClassificationVM subjectClassificationVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                subjectClassificationVM.IsNew = true;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostSubjectClassifications", SubjectClassificationMapper.Map(new List<SubjectClassificationVM>() { subjectClassificationVM })).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOList =
                   HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Admin/GetSubjectClassifications?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (subjectClassificationDTOList.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, subjectClassificationDTOList.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<SubjectClassificationVM>)new AjaxGridFactory().CreateAjaxGrid(SubjectClassificationMapper.Map(SubjectClassificationDTOs.Result).AsQueryable(), 1, false, SubjectClassificationDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<SubjectClassificationVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(SubjectClassificationMapper.Map(subjectClassificationDTOList.Result), 1, subjectClassificationDTOList.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "تم الحفظ");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectClassificationGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetSubjectClassification(string id)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                GetResult<SubjectClassificationDTO> subjectClassificationEditDTO =
                    HttpClientWrapper<GetResult<SubjectClassificationDTO>>.GetItemRequest(String.Format("api/Admin/GetSubjectClassificationById?subjectClassificationId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (subjectClassificationEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, subjectClassificationEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //TODO: Change Source Key To Be "Admin.SubjectClassification.UpdateSucceeded"
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.TransactionSubjectClassification.UpdateSucceeded");

                SubjectClassificationEditVM subjectClassificationEditVM = SubjectClassificationMapper.Map(subjectClassificationEditDTO.Result);
                subjectClassificationEditVM.TransactionCategories = MergeTransactionCategoryLookups(new List<TransactionCategoryVM>());

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectClassificationEditPartial", subjectClassificationEditVM
                    ),
                    MessageText = message,
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult UpdateLetterTypeNotifyOption(int letterTypeId, bool operationType)
        {
            string message = "";
            PutResult putResult = HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Admin/UpdateLetterTypeNotifyOption?letterTypeId={0}&operationType={1}", letterTypeId, operationType), null).Result;

            if (putResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult UpdateLetterTypeWithExtraFieldOption(int letterTypeId, bool operationType)
        {
            string message = "";
            PutResult putResult = HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Admin/UpdateLetterTypeWithExtraFieldOption?letterTypeId={0}&operationType={1}", letterTypeId, operationType), null).Result;

            if (putResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region SpecificLevel

        public ActionResult SpecificLevel()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs =
                    HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.Result == null)
                {
                    cultureDTOs.Result = new List<CultureDTO>();
                    cultureDTOs.RowsCount = 0;
                }

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                SpecificLevelViewModel specificLevelViewModel = new SpecificLevelViewModel();
                List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups();
                List<SpecificListLevelVM> specificListLevelVMs = GetSpecificLevelListLookups();

                specificLevelViewModel.AddSpecificLevel.TransactionCategories = transactionCategoryVMs;
                specificLevelViewModel.EditSpecificLevel.TransactionCategories = transactionCategoryVMs;

                specificLevelViewModel.AddSpecificLevel.List = specificListLevelVMs;
                specificLevelViewModel.EditSpecificLevel.List = specificListLevelVMs;

                GetResult<List<SpecificLevelDTO>> specificLevelDTOs =
                    HttpClientWrapper<GetResult<List<SpecificLevelDTO>>>.GetItemRequest(string.Format("api/Admin/GetSpecificLevels?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (specificLevelDTOs.Result == null)
                {
                    specificLevelDTOs.Result = new List<SpecificLevelDTO>();
                    specificLevelDTOs.RowsCount = 0;
                }

                //IAjaxGrid grid = (AjaxGrid<SpecificLevelVM>)new AjaxGridFactory().CreateAjaxGrid(SpecificLevelMapper.Map(SpecificLevelDTOs.Result).AsQueryable(), 1, false, specificLevelDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<SpecificLevelVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(SpecificLevelMapper.Map(specificLevelDTOs.Result), 1, specificLevelDTOs.RowsCount.Value, false, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                return View(specificLevelViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddSpecificLevel(SpecificLevelAddVM specificLevelAddVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostSpecificLevel", SpecificLevelMapper.Map(specificLevelAddVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<SpecificLevelDTO>> specificLevelDTOs =
                   HttpClientWrapper<GetResult<List<SpecificLevelDTO>>>.GetItemRequest(string.Format("api/Admin/GetSpecificLevels?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (specificLevelDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, specificLevelDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<SpecificLevelVM>)new AjaxGridFactory().CreateAjaxGrid(SpecificLevelMapper.Map(specificLevelDTOs.Result).AsQueryable(), 1, false, specificLevelDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<SpecificLevelVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(SpecificLevelMapper.Map(specificLevelDTOs.Result), 1, specificLevelDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.SpecificLevel.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SpecificLevelGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditSpecificLevel(SpecificLevelEditVM specificLevelEditVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutSpecificLevel", SpecificLevelMapper.Map(specificLevelEditVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<SpecificLevelDTO>> specificLevelDTOs =
                     HttpClientWrapper<GetResult<List<SpecificLevelDTO>>>.GetItemRequest(string.Format("api/Admin/GetSpecificLevels?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (specificLevelDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, specificLevelDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<SpecificLevelVM>)new AjaxGridFactory().CreateAjaxGrid(SpecificLevelMapper.Map(specificLevelDTOs.Result).AsQueryable(), 1, false, specificLevelDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<SpecificLevelVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(SpecificLevelMapper.Map(specificLevelDTOs.Result), 1, specificLevelDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.SpecificLevel.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SpecificLevelGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteSpecificLevel(string ids)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/DeleteSpecificLevels?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<SpecificLevelDTO>> specificLevelDTOs =
                       HttpClientWrapper<GetResult<List<SpecificLevelDTO>>>.GetItemRequest(string.Format("api/Admin/GetSpecificLevels?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (specificLevelDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, specificLevelDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<SpecificLevelVM>)new AjaxGridFactory().CreateAjaxGrid(SpecificLevelMapper.Map(specificLevelDTOs.Result).AsQueryable(), 1, false, specificLevelDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<SpecificLevelVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(SpecificLevelMapper.Map(specificLevelDTOs.Result), 1, specificLevelDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.SpecificLevel.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SpecificLevelGridPartial", grid), SpecificLevelsUsedList = deleteResult.Result, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetSpecificLevel(string id)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                GetResult<SpecificLevelEditDTO> specificLevelEditDTO = HttpClientWrapper<GetResult<SpecificLevelEditDTO>>.GetItemRequest(String.Format("api/Admin/GetSpecificLevelById?specificLevelId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (specificLevelEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, specificLevelEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                SpecificLevelEditVM specificLevelEditVM = SpecificLevelMapper.Map(specificLevelEditDTO.Result);
                specificLevelEditVM.TransactionCategories = MergeTransactionCategoryLookups(specificLevelEditVM.TransactionCategories);
                specificLevelEditVM.List = MergeSpecificLevelListLookups(specificLevelEditVM.List);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.SpecificLevel.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SpecificLevelEditPartial", specificLevelEditVM), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult UpdateSpecificLevelGrid(int? page)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                string parameters = GridHelper.GetGridParameters();

                GetResult<List<SpecificLevelDTO>> specificLevelDTOs = HttpClientWrapper<GetResult<List<SpecificLevelDTO>>>.GetItemRequest(String.Format("api/Admin/GetSpecificLevels?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                //var grid = new AjaxGridFactory().CreateAjaxGrid(SpecificLevelMapper.Map(SpecificLevelDTOs.Result).AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, specificLevelDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<SpecificLevelVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(SpecificLevelMapper.Map(specificLevelDTOs.Result), page ?? 1, specificLevelDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SpecificLevelGridPartial", grid), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Letter Type


        #region Upload File
        [HttpPost]
        public ActionResult UploadAttachments()
        {
            string addedFilesJson = string.Empty;
            bool isValid = true;
            List<object> list = new List<object>();
            if (Request.Files.Count <= 0)
            {
                return Json(new
                {
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            HttpPostedFileBase file;

            string FilePrefix;
            if (SystemConfigurations.MultiTenantEnabled)
            {
                FilePrefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_" + Constants.ExplanationFile + "_";
            }
            else
            {
                FilePrefix = "_" + SessionInfo.CurrentUser.Id + "_" + Constants.ExplanationFile + "_";
            }
            int totalCount = Directory.GetFiles(SystemConfigurations.ExternalCopiesAttachmentPath).Select(o => Path.GetFileName(o)).Where(o => o.StartsWith(FilePrefix)).Count();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                file = Request.Files[i];

                if (!IsValidMimeType(MimeMapping.GetMimeMapping(SystemConfigurations.ExternalCopiesAttachmentPath + file.FileName)))
                {
                    isValid = false;
                    break;
                }
                file.SaveAs(SystemConfigurations.ExternalCopiesAttachmentPath + FilePrefix + file.FileName);
                list.Add(new { Id = totalCount++, Name = file.FileName, IsDeleted = 0, ExtensionFile = MimeMapping.GetMimeMapping(SystemConfigurations.ExternalCopiesAttachmentPath + file.FileName) });
                // addedFilesJson += JsonConvert.SerializeObject(new { Id = totalCount++, AttachmentName = file.FileName, IsDeleted = 0 });
            }
            addedFilesJson = JsonConvert.SerializeObject(list);

            if (isValid == false)
            {
                return Json(new
                {
                    MessageType = MessageType.Error,
                    MessageText = DbRes.TResource("Task.File.MimeType")
                });
            }

            return Json(new
            {
                AddedFilesJson = addedFilesJson,
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpGet]
        public ActionResult SaveReason()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                MCS.UI.Areas.User.Models.Lookups.LookupVM model = new MCS.UI.Areas.User.Models.Lookups.LookupVM();

                List<MCS.UI.Areas.User.Models.Lookups.LookupVM> LookupVMs =  LookupsHelper.GetLookupItemswithoutCached(LookupCategory.SaveReason, SessionInfo.CultureShortName).Result.ToList();


                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<MCS.UI.Areas.User.Models.Lookups.LookupVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(LookupVMs, 1, LookupVMs.Count, false, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                return View(model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddSaveReason(MCS.UI.Areas.User.Models.Lookups.LookupVM lookupVM)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);
                lookupVM.CategoryId = (int)LookupCategory.SaveReason;
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest($"api/Lookups/PostLookupItem?cultureName={SessionInfo.CultureShortName}",
                  UserLookups.LookupMapper.Map(lookupVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                List<MCS.UI.Areas.User.Models.Lookups.LookupVM> LookupVMs = LookupsHelper.GetLookupItemswithoutCached(LookupCategory.SaveReason, SessionInfo.CultureShortName).Result.ToList();


                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<MCS.UI.Areas.User.Models.Lookups.LookupVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(LookupVMs, 1, LookupVMs.Count, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentType.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SaveReasonGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}