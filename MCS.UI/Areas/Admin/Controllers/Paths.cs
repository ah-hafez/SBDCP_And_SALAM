using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MCS.Framework.Controls;
using MCS.Framework.Controls.Mvc;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Areas.Admin.Models;
using MCS.UI.Areas.Admin.Models.OrgUnit;
using MCS.UI.Areas.Admin.Models.Permission;
using MCS.UI.Areas.User;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.UserPreferences;

using CustomAjaxGrid = MCS.GridMvc.Ajax.GridExtensions;
using UserLookups = MCS.UI.Areas.User.Mappers.Lookups;

namespace MCS.UI.Areas.Admin.Controllers
{
    public class PathsController : AdminControllerBase
    {
        

        [HttpGet]
        public ActionResult Index()
        {
             TransactionPathVM transactionPathVM = new TransactionPathVM();
            transactionPathVM.TransactionPathDetailsVM = new TransactionPathDetailsVM();

            GetResult<List<TransactionPathDTO>> transactionPathsResult =
             HttpClientWrapper<GetResult<List<TransactionPathDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetAllPaths?pageIndex={0}&pageSize={1}", 1, GridHelper.PageSize)).Result;
            
            List<TransactionPathVM> transactionPathVMs = TransactionPathMapper.Map(transactionPathsResult.Result);

            int keyCount = 1;
            foreach (var item in transactionPathVMs)
            {
                item.Key = keyCount++;
            }

            transactionPathVM.TransactionPathsGrid = (CustomAjaxGrid.AjaxGrid<TransactionPathVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(transactionPathVMs, 1, transactionPathsResult.RowsCount.Value, false, GridHelper.PageSize);

            //GetResult<List<OrgUnitDTO>> orgUnitDTOs =
            //       HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            //List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);
            ViewData["Cultures"] = GetCultures();
            //ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs, SessionInfo.OrgUnitId);
            //ViewData["PathDepartmentData"] = UIHelper.BulidTree(organizationUnitVMs, SessionInfo.OrgUnitId);
            ViewData["OrgUnitUsers"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
            ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
            ViewData["ConfidentialityData"] = GetConfidentialityLevel();
            ViewData["PrioritiesData"] = GetPriorities();
            ViewData["TransactionCategoryData"] = GetTransactionCategoryLookups(TransactionCategory.DraftOutbound);
            ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
            ViewData["DeliveryMethod"] = GetDelivery(true);
            return View("~/Areas/Admin/Views/Paths/Index.cshtml", transactionPathVM);
        }
        [HttpPost]
        public string GetUsersByOrgUnitId(int? id)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                if (!id.HasValue || id == 0)
                {
                    return JsonConvert.SerializeObject(dataSource);
                }

                GetResult<List<UserProfileDTO>> userProfileDTOs =
                HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetUsersByOrgUnitId?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, id)).Result;

                List<UserProfileVM> userProfileVMS = UserProfileMapper.Map(userProfileDTOs.Result);

                if (userProfileVMS != null)
                {
                    foreach (UserProfileVM userProfileVM in userProfileVMS)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = userProfileVM.Id.ToString(),
                            Label = userProfileVM.LocalName
                        });
                    }
                }

                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult GetInternalPartyChildren(OrgHierarchyTreeViewModel treeVM)
        {
            try
            {
                List<OrgUnitDTO> orgUnitsVM = new List<OrgUnitDTO>();

                var orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>
                    .GetItemRequest(string.Format("api/Common/GetOrgUnits?cultureName={0}&parentId={1}&UserId={2}", SessionInfo.CultureShortName, treeVM.SelectedNode, treeVM.UserId)).Result;



                OrgHierarchyTreeViewModel treeViewModel = new OrgHierarchyTreeViewModel()
                {
                    GetChildrenActionURL = treeVM.GetChildrenActionURL,
                    GetChildrenActionParameters = treeVM.GetChildrenActionParameters,
                    CallBackFunction = treeVM.CallBackFunction,
                    TreeId = treeVM.TreeId,
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


        [HttpPost]
        public ActionResult UpdateGridTransactionPath(int? page, string param)
        {
            try
            {
                GetResult<List<TransactionPathDTO>> transactionPathsResult =
                 HttpClientWrapper<GetResult<List<TransactionPathDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetTransactionPath?userId={0}&orgUnitId=null&pageIndex={1}&pageSize={2}", SessionInfo.CurrentUser.Id, page.HasValue ? page.Value : 1, GridHelper.PageSize)).Result;

                List<TransactionPathVM> transactionPathVMs = TransactionPathMapper.Map(transactionPathsResult.Result);

                int keyCount = 1;
                foreach (var item in transactionPathVMs)
                {
                    item.Key = keyCount++;
                }

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<TransactionPathVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(transactionPathVMs, page.HasValue ? page.Value : 1, transactionPathsResult.RowsCount.Value, page.HasValue);

                return Json(new { Html = grid.ToJson("~/Areas/Admin/Views/Paths/_PathsGridPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddPathsDetails(TransactionPathDetailsVM transactionPathDetailsVM, List<TransactionPathDetailsVM> TransactionPathDetailsGrid)
        {
            try
            {
                List<TransactionPathDetailsVM> transactionPaths = new List<TransactionPathDetailsVM>();
                if (TransactionPathDetailsGrid == null)
                {
                    TransactionPathDetailsGrid = new List<TransactionPathDetailsVM>();
                }

                if (!TransactionPathDetailsGrid.Any(d =>
                       d.EntityId == transactionPathDetailsVM.EntityId && d.UserId == transactionPathDetailsVM.UserId))
                {
                    transactionPathDetailsVM.Key = TransactionPathDetailsGrid.Count + 1;
                    transactionPathDetailsVM.Sort = TransactionPathDetailsGrid.Count + 1;
                    if (transactionPathDetailsVM.EntityId > 0)
                    {
                        OrgUnitDTO orgUnitDTO = OrgHelper.GetOrgUnit(transactionPathDetailsVM.EntityId, SessionInfo.CultureShortName);
                        transactionPathDetailsVM.EntityName = orgUnitDTO.Name;
                    }
                    transactionPaths.Add(transactionPathDetailsVM);
                }
                else
                {
                    return Json(new { MessageText = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Path.DuplicatePathDetails"), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<TransactionPathDetailsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(transactionPaths, 1, transactionPaths.Count, true);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/Admin/Views/Paths/_PathsDetailsGridPartial.cshtml", grid) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult SaveTransactionPath(TransactionPathVM transactionPathVM)
        {
            TransactionPathDTO transactionPathDTOs = new TransactionPathDTO();
            //transactionPathVM.OrgUnitId = SessionInfo.OrgUnitId;
            transactionPathVM.UserId = transactionPathVM.CreatedBy;

            transactionPathDTOs = TransactionPathMapper.Map(transactionPathVM);
            string message = string.Empty;

            if (transactionPathVM.TransactionPathDetailsGrid.Count < 2)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Path.DetailsCount");
                return Json(new { MessageText = message, MessageType = MessageType.Error });
            }

            GetResult<List<TransactionPathDTO>> PathsNameResult =
         HttpClientWrapper<GetResult<List<TransactionPathDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetPathsName?OrgUnitId={0}", transactionPathVM.OrgUnitId)).Result;

            
            foreach (var item in PathsNameResult.Result)
            {
                if ((transactionPathVM.Name.Trim() == item.Name.Trim() && transactionPathVM.Id !=0 && transactionPathVM.Id !=item.Id) || (transactionPathVM.Name.Trim() == item.Name.Trim() && transactionPathVM.Id == 0))
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Path.NameDuplicate" );
                    return Json(new { MessageText = message, MessageType = MessageType.Error });
                }
            }
            
            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/UserProfile/PostTransactionPath", transactionPathDTOs).Result;
            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error });
            }

            return Json(new { MessageText = message, MessageType = MessageType.Information });

        }
     
        [HttpGet]
        public ActionResult GetTransactionPathById(int pathId)
        {
            try
            {
                string message = string.Empty;

                GetResult<TransactionPathDTO> transactionPathDTO =
                   HttpClientWrapper<GetResult<TransactionPathDTO>>.GetItemRequest(String.Format("api/UserProfile/GetTransactionPathById?pathId={0}", pathId)).Result;

                if (transactionPathDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transactionPathDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                TransactionPathVM transactionPathVM = TransactionPathMapper.Map(transactionPathDTO.Result);
                transactionPathVM.CreatedBy = transactionPathDTO.Result.UserId;
                transactionPathVM.CreatedByName = transactionPathDTO.Result.UserName;
                int keyCount = 1;
                foreach (var item in transactionPathVM.TransactionPathDetails)
                {
                    item.Key = keyCount++;
                }

                transactionPathVM.TransactionPathDetailsGrid = (CustomAjaxGrid.AjaxGrid<TransactionPathDetailsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(transactionPathVM.TransactionPathDetails, 1, transactionPathVM.TransactionPathDetails.Count, false);

                //GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                //            HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                //List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);
                ViewData["Cultures"] = GetCultures();
                //ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs, SessionInfo.OrgUnitId);
                //ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                //ViewData["PathDepartmentData"] = UIHelper.BulidTree(organizationUnitVMs, SessionInfo.OrgUnitId);
                ViewData["OrgUnitUsers"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["PrioritiesData"] = GetPriorities();
                ViewData["TransactionCategoryData"] = GetTransactionCategoryLookups(TransactionCategory.DraftOutbound);
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                ViewData["DeliveryMethod"] = GetDelivery(true);
                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/Admin/Views/Paths/_PathsAddPartial.cshtml", transactionPathVM),
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
        public ActionResult DeleteTransactionPath(int pathId)
        {
            try
            {
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/UserProfile/DeleteTransactionPath?pathId=" + pathId, null).Result;

                if (!postResult.Id.HasValue || (postResult.Id.HasValue && postResult.Id < 0))
                {
                    return Json(new { MessageType = MessageType.Error, MessageText = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Path.UnableToRemove") });
                }

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    return Json(new { MessageType = MessageType.Error, MessageText = "Fail" });
                }

                return Json(new { MessageType = MessageType.Information, MessageText = "Success" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AdminEditPathsInfo(TransactionPathDetailsVM transactionPathDetailsVM, List<TransactionPathDetailsVM> TransactionPathDetailsGrid)
        {
            try
            {
                string message = string.Empty;

                List<TransactionPathDetailsVM> transactionPaths = new List<TransactionPathDetailsVM>();
                if (TransactionPathDetailsGrid == null)
                {
                    TransactionPathDetailsGrid = new List<TransactionPathDetailsVM>();
                }

                if (!TransactionPathDetailsGrid.Any(d =>
                       d.EntityId == transactionPathDetailsVM.EntityId && d.UserId == transactionPathDetailsVM.UserId && d.Key != transactionPathDetailsVM.Key))
                {
                    if (transactionPathDetailsVM.EntityId > 0)
                    {
                        OrgUnitDTO orgUnitDTO = OrgHelper.GetOrgUnit(transactionPathDetailsVM.EntityId, SessionInfo.CultureShortName);
                        transactionPathDetailsVM.EntityName = orgUnitDTO.Name;
                    }
                    transactionPaths.Add(transactionPathDetailsVM);
                }
                else
                {
                    return Json(new { MessageText = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Path.DuplicatePathDetails"), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Key = transactionPathDetailsVM.Key,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/Admin/Views/Paths/_PathsDetailsGridPartial.cshtml",
                    (CustomAjaxGrid.AjaxGrid<TransactionPathDetailsVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(transactionPaths, 1, transactionPaths.Count, true))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AdminEditPathsInfoSort(int pathId, int sort, string order)
        {
            try
            {
                string message = string.Empty;
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/UserProfile/UpdateTransactionPathDetailsSort?pathId={0}&sort={1}&order={2}", pathId, sort, order), null).Result;

                return Json(new
                {
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string GetCultures()
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

            GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;


            List<CultureVM> cultureVMs = CultureMapper.Map(cultureDTOs.Result);
            if (cultureVMs != null)
            {
                foreach (CultureVM cultureDTOVM in cultureVMs)
                {
                    dataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = cultureDTOVM.Id.ToString(),
                        Label = cultureDTOVM.LocalName
                    });
                }
            }

            return JsonConvert.SerializeObject(dataSource);
        }
        protected string GetConfidentialityLevel()
        {
            try
            {
                var urlPermission = string.Format("api/Common/GetPermissionsByGroupId?permissionGroupName={0}&cultureName={1}", PermissionGroupName.TransactiosConfidentiality, SessionInfo.CultureShortName);

                GetResult<List<PermissionDTO>> permissionDTOs = HttpClientWrapper<GetResult<List<PermissionDTO>>>.GetItemRequest(urlPermission).Result;

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                List<PermissionVM> permissionVMs = PermissionMapper.Map(permissionDTOs.Result);


                if (permissionVMs != null)
                {
                    foreach (PermissionVM permissionVM in permissionVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = permissionVM.Id.ToString(),
                            Label = permissionVM.Text
                        });
                    }
                }
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected string GetPriorities()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                GetResult<List<PriorityDTO>> priorityDTOs = HttpClientWrapper<GetResult<List<PriorityDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetPriorities?cultureName={0}", SessionInfo.CultureShortName)).Result;

                List<Models.Lookups.PriorityVM> priorityVMs =Mappers.PriorityMapper.Map(priorityDTOs.Result);
                foreach (Models.Lookups.PriorityVM priorityVM in priorityVMs)
                {
                    dataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = priorityVM.Id.ToString(),
                        Label = priorityVM.LocalName
                    });
                }
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected string GetTransactionCategoryLookups(TransactionCategory transactionType = TransactionCategory.None)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> lookupVMs = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                if (lookupVMs != null)
                {
                    foreach (LookupVM lookupVM in lookupVMs.Result.Where(t => transactionType == TransactionCategory.None || t.Id == (int)transactionType.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName)))
                    {
                        if (lookupVM.Id != (int)TransactionCategory.None &&
                            lookupVM.Id != TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                        {
                            dataSource.Add(new AutoCompleteDataSource()
                            {
                                Value = lookupVM.Id.ToString(),
                                Label = lookupVM.Text
                            });
                        }
                    }
                }
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public string GetDelivery(bool isPaper)
        {
            try
            {
                int[] ContainPaper = { DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName), DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) };
                int[] elctronic = { DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) };

                var dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> lookups = LookupsHelper.GetLookupItems(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName);
                if (lookups.Result != null)
                {
                    lookups.Result = lookups.Result.OrderBy(a => a.Sort).ToList();
                    if (isPaper)
                    {
                        lookups.Result = lookups.Result.Where(a => ContainPaper.Contains(a.Id)).ToList();
                    }
                    else
                    {
                        lookups.Result = lookups.Result.Where(a => elctronic.Contains(a.Id)).ToList();
                    }
                    foreach (var item in lookups.Result)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = item.Id.ToString(),
                            Label = item.Text
                        });
                    }
                }
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}