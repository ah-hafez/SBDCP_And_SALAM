using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MCS.Framework.Controls.Mvc;
using MCS.Framework.Localization;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Areas.Admin.Models;
using MCS.UI.Areas.Admin.Models.BarcodeDesigner;
using MCS.UI.Areas.Admin.Models.Groups;
using MCS.UI.Areas.Admin.Models.Lookups;
using MCS.UI.Areas.Admin.Models.OrgUnit;
using MCS.UI.Areas.Admin.Models.Permission;
using MCS.UI.Areas.Admin.Models.UserCategories;
using MCS.UI.Controls;
using UserLookups = MCS.UI.Areas.User.Mappers.Lookups;
using CustomGrid = MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.Admin.Controllers
{
    #region Enums
    public enum OrgUnitMode
    {
        NoRoot = 1,
        NotRoot = 2,
        IsRoot = 3
    }

    public enum ViewMode
    {
        Add = 1,
        Edit = 2
    }

    public enum DialogMode
    {
        UnitInfo = 1,
        UnitCounter,
        UnitUsers,
        AssignmentPaper,
        UnitLinks,
        BarcodeDesigner,
    }
    #endregion

    [ValidateInput(false)]
    public class OrgUnitStructureController : AdminControllerBase
    {
        private bool isParentValid = true;

        #region Index
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                GetRoot();
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Refresh()
        {
            try
            {
                GetIndexView();

                return View("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Unit Users
        [HttpPost]
        public ActionResult OrgUnitUsers(int orgUnitKey)
        {
            try
            {
                string url = string.Format("api/Admin/GetAllUsers?cultureName={0}", SessionInfo.CultureShortName);

                var userProfileDTOs = HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(url).Result;
                ViewData["ListOfUsers"] = UserProfileMapper.Map(userProfileDTOs.Result).Select(u => new AutoCompleteDataSource()
                {
                    Label = u.LocalName,
                    Value = u.Id.ToString()
                }).ToList();

                ViewData["OrgUnitKey"] = orgUnitKey;
                ViewData["DialogType"] = DialogMode.UnitUsers;

                IAjaxGrid grid;
                int rowCount = 0;
                var userProfileVMs = GetOrgUnitUserVMs(orgUnitKey, out rowCount);
                grid = new AjaxGridFactory().CreateAjaxGrid(userProfileVMs != null ? userProfileVMs.AsQueryable() : new List<OrgUnitUserVM>().AsQueryable(),
                     1, false, rowCount);

                ViewData["GridData"] = grid;
                ViewData["orgUnitKey"] = orgUnitKey;
                return new JsonResult()
                {
                    Data = new
                    {
                        View = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OrgUnitUsersPartial", new OrgUnitUserVM()),
                        ContainerHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "DialogContainer", null)
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult OrgUnitAddUsers(OrgUnitUserVM orgUnitUsersVM, int orgUnitKey)
        {
            try
            {
                string message = string.Empty;

                var orgStructureInfoVM = GetOrgStructureInfoByOrgUnitKey(orgUnitKey, DialogMode.UnitUsers);

                if (orgStructureInfoVM == null)
                {
                    orgStructureInfoVM = new OrgStructureInfoVM();
                    orgStructureInfoVM.Key = orgUnitKey;
                    orgStructureInfoVM.IsActive = true;
                    orgStructureInfoVM.Users = new List<OrgUnitUserVM>();
                }
                else
                {
                    orgStructureInfoVM.Users = orgStructureInfoVM.Users ?? new List<OrgUnitUserVM>();
                }

                if (!orgStructureInfoVM.Users.Any(a => a.Id == orgUnitUsersVM.Id))
                {
                    orgStructureInfoVM.Users.Add(orgUnitUsersVM);
                }
                else
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.OrgUnitUsers.UserExist");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }

                var orgUnitDTO = new OrgUnitDTO { Id = orgUnitKey };

                orgStructureInfoVM.Users.ForEach(a =>
                {
                    var userProfile = new UserProfileDTO
                    {
                        Id = a.Id,
                        UserName = a.UserName
                    };
                    orgUnitDTO.Users.Add(userProfile);
                });

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/UpdateOrgUnitWithUsers?cultureName={SessionInfo.CultureShortName}", orgUnitDTO).Result;
                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid;
                int rowCount = 0;
                var userProfileVMs = GetOrgUnitUserVMs(orgUnitKey, out rowCount);
                grid = new AjaxGridFactory().CreateAjaxGrid(userProfileVMs != null ? userProfileVMs.AsQueryable() : new List<OrgUnitUserVM>().AsQueryable(), 1, false, rowCount);
                ViewData["orgUnitKey"] = orgUnitKey;

                return new JsonResult()
                {
                    Data = new
                    {
                        Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OrgUnitUsersGridPartial", grid),
                        MessageType = MessageType.Information,
                        MessageText = message
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteOrgUnitUsers(string ids, int orgUnitKeyGrid)
        {
            try
            {

                var orgStructureInfoVM = GetOrgStructureInfoByOrgUnitKey(orgUnitKeyGrid, DialogMode.UnitUsers);
                List<int> usersIds = ids.Split(',').Select(int.Parse).ToList();

                List<string> usersRelatedToAssignmentPaperArray = new List<string>();

                string managerName = string.Empty;
                string managerMsg = string.Empty;

                usersIds.ForEach(id =>
                {
                    bool allowRemove = true;
                    bool isManager = false;

                    OrgUnitUserVM user = orgStructureInfoVM.Users.Where(u => u.Id == id).FirstOrDefault();
                    if (user != null)
                    {

                        if (orgStructureInfoVM.ManagerId == user.Id)
                        {
                            allowRemove = false;
                            isManager = true;
                        }

                        if (orgStructureInfoVM.AssignmentPaper != null)
                        {
                            if (orgStructureInfoVM.AssignmentPaper != null)
                            {
                                if (orgStructureInfoVM.AssignmentPaper.Beneficiaries != null)
                                {
                                    orgStructureInfoVM.AssignmentPaper.Beneficiaries.ForEach(b =>
                                    {
                                        if (b.UserId != null)
                                        {
                                            if (b.UserId == id && b.BeneficiaryOrgUnitId == orgUnitKeyGrid)
                                            {
                                                allowRemove = false;
                                            }
                                        }
                                    });
                                }
                            }
                        }

                        if (allowRemove)
                        {
                            orgStructureInfoVM.Users.RemoveAll(a => a.Id == user.Id);
                        }

                        else if (!allowRemove && isManager)
                        {
                            managerName = user.UserName;
                            managerMsg = DbRes.TResource("Admin.OrgUnitUsers.DeleteManagerMsg").Replace("{0}", managerName);
                        }
                        else
                        {
                            usersRelatedToAssignmentPaperArray.Add(user.UserName);
                        }
                    }
                });

                if (!string.IsNullOrEmpty(managerMsg))
                {
                    return Json(new { MessageText = managerMsg, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                var orgUnitDTO = new OrgUnitDTO { Id = orgUnitKeyGrid };

                orgStructureInfoVM.Users.RemoveAll(a => usersIds.Contains(a.Id));

                orgStructureInfoVM.Users.ForEach(a =>
                {
                    UserProfileDTO userProfile = new UserProfileDTO
                    {
                        Id = a.Id,
                        UserName = a.UserName
                    };
                    orgUnitDTO.Users.Add(userProfile);
                });

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/UpdateOrgUnitWithUsers?cultureName={SessionInfo.CultureShortName}", orgUnitDTO).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    managerMsg = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = managerMsg, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid;
                int rowCount = 0;
                var userProfileVMs = GetOrgUnitUserVMs(orgUnitKeyGrid, out rowCount);
                grid = new AjaxGridFactory().CreateAjaxGrid(userProfileVMs != null ? userProfileVMs.AsQueryable() : new List<OrgUnitUserVM>().AsQueryable(), 1, false, rowCount);
                ViewData["orgUnitKey"] = orgUnitKeyGrid;
                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OrgUnitUsersGridPartial", grid),
                    ManagerName = managerName,
                    ManagerMsg = managerMsg,
                    UsersRelatedToAssignmentPaper = JsonConvert.SerializeObject(usersRelatedToAssignmentPaperArray),
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult UpdateGridOrgUnitUser(int? page, int orgUnitKey)
        {
            try
            {
                string data = GridHelper.GetGridParameters();
                GetResult<List<UserProfileDTO>> userProfileDTOs = HttpClientWrapper<GetResult<List<UserProfileDTO>>>
                    .GetItemRequest(string.Format("api/Admin/GetUsersByOrgUnitId?{0}&orgUnitId={1}", data, orgUnitKey)).Result;
                List<OrgUnitUserVM> userProfileVMs = OrgUnitMapper.MapToOrgUnitUser(userProfileDTOs.Result);

                var grid = new AjaxGridFactory().CreateAjaxGrid(userProfileVMs != null ? userProfileVMs.AsQueryable() : new List<OrgUnitUserVM>().AsQueryable(),
                    page.HasValue ? page.Value : 1, page.HasValue, userProfileDTOs.RowsCount.Value);
                ViewData["orgUnitKey"] = orgUnitKey;

                return Json(new
                {
                    Html = grid.ToJson("_OrgUnitUsersGridPartial", this),
                    grid.HasItems
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<OrgUnitUserVM> GetOrgUnitUserVMs(int orgUnitKey, out int rowCount)
        {
            GetResult<List<UserProfileDTO>> userProfileDTOs = HttpClientWrapper<GetResult<List<UserProfileDTO>>>
                .GetItemRequest(string.Format("api/Admin/GetUsersByOrgUnitId?PageIndex=1&PageSize={0}&CultureName={1}&orgUnitId={2}",
                GridHelper.PageSize, SessionInfo.CultureShortName, orgUnitKey)).Result;
            rowCount = userProfileDTOs.RowsCount.Value;
            List<OrgUnitUserVM> userProfileVMs = OrgUnitMapper.MapToOrgUnitUser(userProfileDTOs.Result);
            return userProfileVMs;
        }
        #endregion

        #region Counter
        [HttpGet]
        public ActionResult Counter(int orgUnitKey)
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                OrgStructureInfoVM orgStructureInfoVM = GetOrgStructureInfoByOrgUnitKey(orgUnitKey, DialogMode.UnitCounter);
                if (orgStructureInfoVM == null)
                {
                    orgStructureInfoVM = new OrgStructureInfoVM();
                }
                List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionSourceLookups();

                var counterVM = new CounterVM();
                counterVM.TransactionCategories = transactionCategoryVMs;
                counterVM.Year = DateTimeUtility.GetHijriYear(DateTime.Now);

                counterVM.IsRoot = true;
                if (orgStructureInfoVM.Counter != null)
                {
                    counterVM.CounterId = orgStructureInfoVM.Counter.CounterId;
                    counterVM.IsGeneral = orgStructureInfoVM.Counter.IsGeneral;
                    counterVM.Description = orgStructureInfoVM.Counter.Description;
                    counterVM.OwnerEntityId = orgUnitKey;
                    if (orgStructureInfoVM.Counter.Year != 0)
                    {
                        counterVM.Year = orgStructureInfoVM.Counter.Year;
                    }
                    if (orgStructureInfoVM.Counter.CounterId != 0)
                    {
                        counterVM.IsRoot = counterVM.IsGeneral && orgStructureInfoVM.Counter.OwnerEntityId == orgUnitKey;
                    }
                    else
                    {
                        counterVM.IsRoot = false;
                    }
                }

                ViewData["GridDataGeneralCounter"] = (AjaxGrid<CounterDetailVM>)new AjaxGridFactory().CreateAjaxGrid((new List<CounterDetailVM>()).AsQueryable(), 1, false, 0);
                OrgStructureInfoVM orgStructureInfoVMGeneralCounter = GetOrgUnitsGeneralCounter();
                if (orgStructureInfoVMGeneralCounter != null)
                {
                    if (orgStructureInfoVMGeneralCounter.Counter != null)
                    {
                        ViewData["IsRoot"] = counterVM.IsRoot;
                        orgStructureInfoVMGeneralCounter.Counter = orgStructureInfoVMGeneralCounter.Counter ?? new CounterVM();
                        IAjaxGrid grid = (AjaxGrid<CounterDetailVM>)new AjaxGridFactory()
                           .CreateAjaxGrid((orgStructureInfoVMGeneralCounter.Counter.CounterDetails ?? new List<CounterDetailVM>()).AsQueryable(), 1, false, 0);
                        ViewData["GridDataGeneralCounter"] = grid;
                    }
                }


                ViewData["GridData"] = (AjaxGrid<CounterDetailVM>)new AjaxGridFactory().CreateAjaxGrid((new List<CounterDetailVM>()).AsQueryable(), 1, false, 0);
                if (counterVM.IsRoot || !counterVM.IsGeneral)
                {
                    IAjaxGrid grid = (AjaxGrid<CounterDetailVM>)new AjaxGridFactory()
                        .CreateAjaxGrid((orgStructureInfoVM.Counter.CounterDetails ?? new List<CounterDetailVM>()).AsQueryable(), 1, false, 0);
                    ViewData["GridData"] = grid;
                }

                return Json(new
                {
                    View = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CounterPartial", counterVM),
                    ContainerHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "DialogContainer", null),
                    Mode = (int)ViewMode.Add
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult GetCounter(int id, int orgUnitId)
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                OrgStructureInfoVM orgStructureInfoVM = GetOrgStructureInfoByOrgUnitKey(orgUnitId, DialogMode.UnitCounter);
                if (orgStructureInfoVM == null)
                {
                    orgStructureInfoVM = new OrgStructureInfoVM();
                }
                List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionSourceLookups();
                var counterDetailVM = orgStructureInfoVM.Counter.CounterDetails.FirstOrDefault(a => a.Id == id);

                var counterVM = new CounterVM();
                counterVM.IsRoot = true;
                counterVM.CounterId = orgStructureInfoVM.Counter.CounterId;
                counterVM.IsGeneral = orgStructureInfoVM.Counter.IsGeneral;
                counterVM.OwnerEntityId = orgStructureInfoVM.Counter.OwnerEntityId;
                counterVM.Description = orgStructureInfoVM.Counter.Description;
                counterVM.Year = DateTimeUtility.GetHijriYear(DateTime.Now);
                if (orgStructureInfoVM.Counter.Year != 0)
                {
                    counterVM.Year = orgStructureInfoVM.Counter.Year;
                }
                counterVM.CounterDetailId = counterDetailVM.Id;
                counterVM.InitialValue = counterDetailVM.InitialValue;
                counterVM.Count = counterDetailVM.Count;
                counterVM.TransactionCategories = MergeTransactionCategoryLookups(counterDetailVM.TransactionCategories);
                counterVM.IsRoot = counterVM.IsGeneral && orgStructureInfoVM.Counter.OwnerEntityId == orgUnitId;

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CounterAddEditPartial", counterVM),
                    ContainerHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "DialogContainer", null),
                    Mode = (int)ViewMode.Edit
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddEditCounter(CounterVM counterVM)
        {
            string message = string.Empty;
            OrgUnitDTO orgUnitDTO = null;
            bool isUsedTransactionCategory = false;
            OrgStructureInfoVM orgStructureInfoVM = GetOrgStructureInfoByOrgUnitKey(counterVM.OwnerEntityId, DialogMode.UnitCounter);
            if (orgStructureInfoVM == null)
            {
                orgStructureInfoVM = new OrgStructureInfoVM();
                orgStructureInfoVM.Counter = new CounterVM();
            }
            //When switch between General and Ungeneral
            List<int> TransactionCategoryIds;
            if (counterVM.JoinToGeneralCounter)
            {
                TransactionCategoryIds = Enum.GetValues(typeof(TransactionCategories)).Cast<TransactionCategories>()
                                        .Select(v => (int)v).ToList();
                TransactionCategoryIds.RemoveAt(0);
            }
            else
            {
                TransactionCategoryIds = counterVM.TransactionCategories.Where(a => a.IsSelected).Select(a => a.Id).ToList();
            }
            var orgUnitUsed = HttpClientWrapper<PostResult>.PostRequest($"api/Admin/CheckOrgUnitUsedInTransaction?orgUnitId={counterVM.OwnerEntityId}",
                TransactionCategoryIds).Result;
            if (orgUnitUsed.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, orgUnitUsed.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if ((bool)orgUnitUsed.Result)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.UsedCounter.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error });
                }
                else
                {
                    if (counterVM.JoinToGeneralCounter == false && orgStructureInfoVM.Counter.OwnerEntityId != counterVM.OwnerEntityId)
                    {
                        counterVM.CounterId = 0;
                        counterVM.IsGeneral = false;
                        orgStructureInfoVM.Counter.OwnerEntityId = counterVM.OwnerEntityId;
                        orgStructureInfoVM.Counter.CounterDetails = new List<CounterDetailVM>();
                    }
                }
            }
            if (counterVM.CounterId != 0)
            {
                #region Check if Transaction Category already exists in old items
                if (orgStructureInfoVM.Counter.CounterDetails != null && orgStructureInfoVM.Counter.CounterDetails.Count > 0)
                {
                    var transactionCategoriesIdsToUpdate = counterVM.TransactionCategories.Where(a => a.IsSelected).Select(a => a.Id).ToList();
                    var oldCounterDetails = orgStructureInfoVM.Counter.CounterDetails.Where(a => a.Id != counterVM.CounterDetailId).ToList();
                    foreach (var item in oldCounterDetails)
                    {
                        var selectedItem = item.TransactionCategories.Where(a => a.IsSelected && transactionCategoriesIdsToUpdate.Any(b => a.Id == b)).FirstOrDefault();
                        if (selectedItem != null && selectedItem.IsSelected)
                        {
                            isUsedTransactionCategory = true;
                            break;
                        }
                    }
                    if (isUsedTransactionCategory)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.CounterExisted.ToString());
                        return Json(new { MessageText = message, MessageType = MessageType.Error });
                    }
                }
                #endregion
            }

            if (counterVM.JoinToGeneralCounter)
            {
                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest($"api/Admin/UpdateOrgUnitToJoinGeneralCounter?orgUnitId={counterVM.OwnerEntityId}", null).Result;
                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                orgStructureInfoVM.Counter.CounterId = counterVM.CounterId;
                orgStructureInfoVM.Counter.IsGeneral = counterVM.IsGeneral;
                orgStructureInfoVM.Counter.Year = counterVM.Year;
                orgStructureInfoVM.Counter.Description = counterVM.Description;

                //Add New Counter
                if (counterVM.CounterId == 0)
                {
                    orgStructureInfoVM.Counter.CounterDetails.Add(new CounterDetailVM
                    {
                        InitialValue = counterVM.InitialValue,
                        Count = counterVM.InitialValue,
                        TransactionCategories = counterVM.TransactionCategories
                    });
                }
                else //Edit Counter
                {
                    if (counterVM.CounterDetailId != 0)
                    {
                        //Edit Counter Details
                        #region Edit Counter Details
                        #region Check if InitialValue used in any transaction
                        //Check if InitialValue used in any transaction
                        var counterDetailToEdit = orgStructureInfoVM.Counter.CounterDetails.FirstOrDefault(a => a.Id == counterVM.CounterDetailId);
                        if (counterDetailToEdit.InitialValue != counterDetailToEdit.Count)
                        {
                            message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.UsedCounter.ToString());
                            return Json(new { MessageText = message, MessageType = MessageType.Error });
                        }
                        #endregion
                        foreach (var item in orgStructureInfoVM.Counter.CounterDetails.Where(a => a.Id == counterVM.CounterDetailId))
                        {
                            item.InitialValue = item.Count = counterVM.InitialValue;
                            item.TransactionCategories = counterVM.TransactionCategories;
                        }
                        #endregion
                    }
                    else
                    {
                        //New Counter Details
                        #region New Counter Details
                        if (orgStructureInfoVM.Counter.CounterDetails == null)
                        {
                            orgStructureInfoVM.Counter.CounterDetails = new List<CounterDetailVM>();
                        }
                        orgStructureInfoVM.Counter.CounterDetails.Add(new CounterDetailVM
                        {
                            InitialValue = counterVM.InitialValue,
                            Count = counterVM.InitialValue,
                            TransactionCategories = counterVM.TransactionCategories,
                        });
                        #endregion
                    }
                }

                orgStructureInfoVM.BarCode = orgStructureInfoVM.Number.ToString();
                orgUnitDTO = new OrgUnitDTO
                {
                    Id = counterVM.OwnerEntityId,
                    BarCode = orgStructureInfoVM.BarCode,
                    IsActive = true,
                    Counter = CounterMapper.Map(orgStructureInfoVM.Counter)
                };

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest($"api/Admin/UpdateOrgUnitWithCounter?cultureName={SessionInfo.CultureShortName}", orgUnitDTO).Result;
                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
            }

            #region Get Grid Data
            orgStructureInfoVM = GetOrgStructureInfoByOrgUnitKey(counterVM.OwnerEntityId, DialogMode.UnitCounter);
            IAjaxGrid grid;
            List<CounterDetailVM> counterDetailVMs;
            if (orgStructureInfoVM.Counter.CounterDetails == null)
            {
                counterDetailVMs = new List<CounterDetailVM>();
                grid = (AjaxGrid<CounterDetailVM>)new AjaxGridFactory().CreateAjaxGrid(counterDetailVMs.AsQueryable(), 1, false, 0);
            }
            else
            {
                counterDetailVMs = counterVM.JoinToGeneralCounter ? new List<CounterDetailVM>() : orgStructureInfoVM.Counter.CounterDetails;
                grid = (AjaxGrid<CounterDetailVM>)new AjaxGridFactory().CreateAjaxGrid(counterDetailVMs.AsQueryable(), 1, false, counterDetailVMs.Count);
            }
            ViewData["GridName"] = "GridCounter";
            return Json(new
            {
                MessageText = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Counter.AddSucceeded"),
                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CounterGridPartial", grid),
                MessageType = MessageType.Information
            });
            #endregion
        }
        [HttpPost]
        public ActionResult DeleteCounterDetail(int id, int orgUnitId)
        {
            try
            {
                string message = string.Empty;

                #region Check if InitialValue used in any transaction
                //Check if InitialValue used in any transaction
                OrgStructureInfoVM orgStructureInfoVM = GetOrgStructureInfoByOrgUnitKey(orgUnitId, DialogMode.UnitCounter);
                if (orgStructureInfoVM == null)
                {
                    orgStructureInfoVM = new OrgStructureInfoVM();
                }
                List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionSourceLookups();
                var counterDetailVM = orgStructureInfoVM.Counter.CounterDetails.FirstOrDefault(a => a.Id == id);
                if (counterDetailVM.InitialValue != counterDetailVM.Count)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.DeleteCounterDetail.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error });
                }
                #endregion

                DeleteResult deleteResult = HttpClientWrapper<DeleteResult>.DeleteRequest(string.Format("api/Admin/DeleteCounterDetail?id={0}", id)).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                orgStructureInfoVM = GetOrgStructureInfoByOrgUnitKey(orgUnitId, DialogMode.UnitCounter);
                IAjaxGrid grid;
                List<CounterDetailVM> counterDetailVMs;
                if (orgStructureInfoVM.Counter.CounterDetails == null)
                {
                    counterDetailVMs = new List<CounterDetailVM>();
                    grid = (AjaxGrid<CounterDetailVM>)new AjaxGridFactory().CreateAjaxGrid(counterDetailVMs.AsQueryable(), 1, false, 0);
                }
                else
                {
                    counterDetailVMs = orgStructureInfoVM.Counter.CounterDetails;
                    grid = (AjaxGrid<CounterDetailVM>)new AjaxGridFactory().CreateAjaxGrid(counterDetailVMs.AsQueryable(), 1, false, counterDetailVMs.Count);
                }
                ViewData["GridName"] = "GridCounter";
                return Json(new
                {
                    MessageText = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.User.DeleteSucceeded"),
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CounterGridPartial", grid),
                    MessageType = MessageType.Information
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Org Unit Info
        [HttpGet]
        public ActionResult OrgUnitInfo(int orgUnitKey)
        {
            try
            {



                ViewMode viewMode = ViewMode.Edit;
                string htmlView;

                ViewData["OrgUnitKey"] = orgUnitKey;
                ViewData["DialogType"] = (int)DialogMode.UnitInfo;

                string containerHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "DialogContainer", null);

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                List<LocalizationVM> localizationVMList = cultureDTOs.Result.Select(c => new LocalizationVM()
                {

                    CultureId = c.Id,
                    CultureName = c.ShortName
                }).ToList();

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
      HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                ViewData["orgUnitDataInfo"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));

                List<OrgStructureInfoVM> orgStructureInfoVMs = GetOrgStructuresNew();

                OrgStructureInfoVM orgStructureInfoVM = orgStructureInfoVMs.FirstOrDefault(o => o.Key == orgUnitKey);

                IList<AutoCompleteDataSource> autoCompleteDataSources = orgStructureInfoVMs
                    .Where(o => !o.IsDeleted && o.Key != orgUnitKey)
                    .Select(o => new AutoCompleteDataSource()
                    {
                        Label = o.Name,
                        Value = o.Key.ToString()
                    }).ToList();

                ViewData["ListOfUnits"] = autoCompleteDataSources;



                List<AutoCompleteDataSource> OrgUnitUsers = new List<AutoCompleteDataSource>();
                if (orgStructureInfoVM != null)
                {
                    orgStructureInfoVM.Users = GetOrgStructureInfoByOrgUnitKey(orgUnitKey, DialogMode.UnitInfo).Users;
                }

                if (orgStructureInfoVM.Users != null)
                {
                    //orgStructureInfoVM.Users.ForEach(u =>
                    //{
                    //    OrgUnitUsers.Add(new AutoCompleteDataSource() { Label = u.UserName, Value = u.Id.ToString() });
                    //});
                    CustomGrid.IAjaxGrid grid = (CustomGrid.AjaxGrid<OrgUnitUserVM>)new CustomGrid.AjaxGridFactory().CreateAjaxGrid(orgStructureInfoVM.Users.ToList(), 1, orgStructureInfoVM.Users.Count, false, UIHelper.PageSize);

                    ViewData["GridData"] = grid;
                }
                else
                {


                    ViewData["GridData"] = null;

                    // OrgUnitUsers.Add(new AutoCompleteDataSource() { Label = SessionInfo.CurrentUser.UserName, Value = SessionInfo.CurrentUser.Id.ToString() });
                }

                ViewData["OrgUnitUsers"] = OrgUnitUsers;

                OrgUnitMode orgUnitMode = OrgUnitMode.NoRoot;

                if (orgStructureInfoVMs.FirstOrDefault(o => !o.IsDeleted && o.ParentId == -1) != null)
                {
                    if (orgStructureInfoVM.ParentId == -1)
                    {
                        orgUnitMode = OrgUnitMode.IsRoot;
                    }
                    else
                    {
                        orgUnitMode = OrgUnitMode.NotRoot;
                    }
                }

                ViewData["OrgUnitMode"] = (int)orgUnitMode;

                if (orgStructureInfoVM.ParentId == 0)
                {
                    viewMode = ViewMode.Add;

                    OrgStructureInfoAddVM orgStructureInfoAddVM = new OrgStructureInfoAddVM();

                    orgStructureInfoAddVM.Names = localizationVMList;
                    orgStructureInfoAddVM.Key = orgUnitKey;

                    if (orgUnitMode == OrgUnitMode.IsRoot)
                    {
                        orgStructureInfoAddVM.IsRoot = true;
                        orgStructureInfoAddVM.ParentId = -1;
                    }

                    if (orgUnitMode == OrgUnitMode.NotRoot)
                    {
                        orgStructureInfoAddVM.IsRoot = false;
                    }

                    htmlView = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OrgUnitInfoAddPartial", orgStructureInfoAddVM);

                    return Json(new { View = htmlView, ContainerHtml = containerHtml, Mode = (int)viewMode }, JsonRequestBehavior.AllowGet);
                }

                OrgStructureInfoEditVM orgStructureInfoEditVM = new OrgStructureInfoEditVM();

                orgStructureInfoEditVM.Names = orgStructureInfoVM.Names;

                if (orgStructureInfoEditVM.Names.Count == 0)
                {
                    orgStructureInfoEditVM.Names = localizationVMList;
                }

                orgStructureInfoEditVM.ParentId = orgStructureInfoVM.ParentId;
                orgStructureInfoEditVM.ManagerId = orgStructureInfoVM.ManagerId;
                orgStructureInfoEditVM.Number = orgStructureInfoVM.Number;
                orgStructureInfoEditVM.TransactionsProcessingPeriod = orgStructureInfoVM.TransactionsProcessingPeriod;
                orgStructureInfoEditVM.BarCode = orgStructureInfoVM.BarCode;
                orgStructureInfoEditVM.IsVirtualUnit = orgStructureInfoVM.IsVirtualUnit;
                orgStructureInfoEditVM.Key = orgUnitKey;
                orgStructureInfoEditVM.ExternalId = orgStructureInfoVM.ExternalId;
                orgStructureInfoEditVM.IoDepartment = orgStructureInfoVM.IoDepartment;
                orgStructureInfoEditVM.FollowUpDepartment = orgStructureInfoVM.FollowUpDepartment;
                orgStructureInfoEditVM.IsExecutive = orgStructureInfoVM.IsExecutive;
                orgStructureInfoEditVM.ReceiveElcOutBoundWithAcknowled = orgStructureInfoVM.ReceiveElcOutBoundWithAcknowled;
                orgStructureInfoEditVM.SendSpecialCopy = orgStructureInfoVM.SendSpecialCopy;
                orgStructureInfoEditVM.IsGeneralIoDepartment = orgStructureInfoVM.IsGeneralIoDepartment;
                orgStructureInfoEditVM.Lineage = orgStructureInfoVM.Lineage;
                if (orgUnitMode == OrgUnitMode.IsRoot)
                {
                    orgStructureInfoEditVM.IsRoot = true;
                }

                if (orgUnitMode == OrgUnitMode.NotRoot)
                {
                    orgStructureInfoEditVM.IsRoot = false;
                }

                htmlView = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OrgUnitInfoEditPartial", orgStructureInfoEditVM);

                // return Json(new { View = htmlView, ContainerHtml = containerHtml, Mode = viewMode }, JsonRequestBehavior.AllowGet);





                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Admin/GetOrgUnits?cultureName={0}", SessionInfo.CultureShortName)).Result;

                //if (orgUnitDTOs.Result == null)
                //{
                //    orgUnitDTOs.Result = new List<OrgUnitDTO>();
                //    orgUnitDTOs.RowsCount = 0;
                //}

                //ViewData["OrgUnits"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), -1, true);
                BarcodeDesignerDTO barcodeDesignerDTO = new BarcodeDesignerDTO();
                barcodeDesignerDTO.TypeId = BarcodeDesignType.Inbound.LookupIdentity(LookupCategory.BarcodeDesignType, SessionInfo.CultureShortName);
                barcodeDesignerDTO.IsGeneral = true;
                GetResult<BarcodeDesignerDTO> barcodeDesigner = HttpClientWrapper<GetResult<BarcodeDesignerDTO>>.GetItemRequest(String.Format("api/Admin/GetBarcodeDesign?isGeneral={0}&typeId={1}", true, barcodeDesignerDTO.TypeId)).Result;

                if (barcodeDesigner.Result == null)
                {
                    barcodeDesignerDTO.Id = 0;
                    barcodeDesignerDTO.Html = "<div id=\"barCode\" style=\"position:relative;\" class=\"droppable ui-widget-header col-md-6 encryption_code size_c\"> </div>";
                    barcodeDesignerDTO.HtmlAttachment = "<div id=\"barCodeAttachment\" style=\"position:relative;\" class=\"droppable ui-widget-header col-md-6 encryption_code size_c\"> </div>";
                }
                else
                {
                    string imag2DStyle = string.Format("<img style='width: {0}px;  height: {1}px;' class='imag2D' src='{2}/Content/Admin/Lib/images/morasalat/code_2d_v.png' />", barcodeDesigner.Result.Width * 0.6, barcodeDesigner.Result.Height * 0.2, UrlHelper.GetBaseUri());
                    string imag3DStyle = string.Format("<img style='width: {1}px;  height: {1}px;' class='imag3D' src='{2}/Content/Admin/Lib/images/morasalat/code_3d_v.png' />", barcodeDesigner.Result.Width * 0.3, barcodeDesigner.Result.Height * 0.3, UrlHelper.GetBaseUri());

                    barcodeDesignerDTO.Id = barcodeDesigner.Result.Id;
                    //barcodeDesignerDTO.Html = string.Format(barcodeDesigner.Result.Html, "", imag2DStyle, "",
                    //                           imag3DStyle, "",
                    //                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.DepartmentPreparedInbound"), "",
                    //                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionDate"), "",
                    //                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionNumber"), "",
                    //                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Attachments"), "",
                    //                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Abbreviation"), "",
                    //                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TanentName"), ""
                    //                           );

                    if (barcodeDesigner.Result.HtmlAttachment != null)
                    {
                        barcodeDesignerDTO.HtmlAttachment = FillAttachmentDesgin(barcodeDesigner.Result.HtmlAttachment, barcodeDesigner.Result);
                    }
                    else
                    {
                        barcodeDesignerDTO.HtmlAttachment = "<div id=\"barCodeAttachment\" style=\"position:relative;\" class=\"droppable ui-widget-header col-md-6 encryption_code size_c\"> </div>";
                    }

                }
                orgStructureInfoEditVM.objBarcodeDesignerVM = BarcodeDesignerMapper.Map(barcodeDesignerDTO);

                List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups();

                ViewData["TransactionCategory"] = transactionCategoryVMs;
                ViewData["Users"] = GetUsersAutoCompleteDataSource(orgUnitKey);

                List<OrgStructureInfoVM> objOrgStructureInfoVMList = GetAllUnitByLineage(orgStructureInfoVM.Lineage);
                orgStructureInfoEditVM.objOrgStructureInfoVMList = objOrgStructureInfoVMList;

                return PartialView("_OrgUnitManagePartial", orgStructureInfoEditVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetUsersAutoCompleteDataSource(int orgUnitKey)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                List<UserProfileVM> UserProfileVMs = GetUserByorgParentId(orgUnitKey);

                if (UserProfileVMs != null && UserProfileVMs.Count() > 0)
                {
                    foreach (UserProfileVM userProfileVM in UserProfileVMs)
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<TransactionCategoryVM> GetTransactionCategoryLookups()
        {
            GetResult<IList<LookupVM>> lookupVMs = LookupsHelper.GetAdminLookupItems(LookupCategory.BarcodeDesignType, SessionInfo.CultureShortName);
            List<TransactionCategoryVM> transactionCategoryVMs = new List<TransactionCategoryVM>();

            if (lookupVMs.Result != null)
            {
                foreach (LookupVM lookupVM in lookupVMs.Result)
                {
                    if (lookupVM.Id == BarcodeDesignType.Attachment.LookupIdentity(LookupCategory.BarcodeDesignType, string.Empty))
                    {
                        continue;
                    }

                    transactionCategoryVMs.Add(new TransactionCategoryVM()
                    {
                        Id = lookupVM.Id,
                        Text = lookupVM.Text,
                    });
                }
            }

            return transactionCategoryVMs;
        }

        public string FillAttachmentDesgin(string htmlDesign, BarcodeDesignerDTO barcodeDesignerDTO)
        {
            string imag2DStyle = string.Format("<img style='width: {0}px;  height: {1}px;' class='imag2D' src='{2}/Content/Admin/Lib/images/morasalat/code_2d_v.png' />", barcodeDesignerDTO.Width * 0.6, barcodeDesignerDTO.Height * 0.2, UrlHelper.GetBaseUri());
            string imag3DStyle = string.Format("<img style='width: {1}px;  height: {1}px;' class='imag3D' src='{2}/Content/Admin/Lib/images/morasalat/code_3d_v.png' />", barcodeDesignerDTO.Width * 0.3, barcodeDesignerDTO.Height * 0.3, UrlHelper.GetBaseUri());

            htmlDesign = htmlDesign.Replace("{attachmentOrgunit}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentBarcodeDesigner.Orgunit"));
            htmlDesign = htmlDesign.Replace("{attachmentCount}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentBarcodeDesigner.Count"));
            htmlDesign = htmlDesign.Replace("{attachmentName}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentBarcodeDesigner.AttachmentName"));
            htmlDesign = htmlDesign.Replace("{attachmentDate}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentBarcodeDesigner.AttachmentDate"));
            htmlDesign = htmlDesign.Replace("{attachment2DImage}", "");
            htmlDesign = htmlDesign.Replace("{attachment2DImageValue}", imag2DStyle);
            htmlDesign = htmlDesign.Replace("{attachment3DImage}", "");
            htmlDesign = htmlDesign.Replace("{attachment3DImageValue}", imag3DStyle);
            htmlDesign = htmlDesign.Replace("{attachmentOrgunitValue}", "");
            htmlDesign = htmlDesign.Replace("{attachmentCountValue}", "");
            htmlDesign = htmlDesign.Replace("{attachmentNameValue}", "");
            htmlDesign = htmlDesign.Replace("{attachmentDateValue}", "");

            return htmlDesign;
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddOrgUnitInfo(OrgStructureInfoAddVM orgStructureInfoAddVM)
        {
            try
            {
                string message = string.Empty;
                List<OrgStructureInfoVM> orgStructureInfoVMs = GetOrgStructuresLight();

                //OrgStructureInfoVM orgUnit = orgStructureInfoVMs.FirstOrDefault(o => o.ManagerId == orgStructureInfoAddVM.ManagerId && !o.IsDeleted);

                //if (orgUnit != null)
                //{
                //    message = DbRes.TValidation("Admin.OrgUnitInfo.InvalidManager").Replace("{0}", orgUnit.Name);
                //    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                //}


                OrgStructureInfoVM orgStructureInfoVM = orgStructureInfoVMs.FirstOrDefault(o => o.Key == orgStructureInfoAddVM.Key);

                IsParentValid(orgStructureInfoVMs, orgStructureInfoAddVM.Key, orgStructureInfoAddVM.ParentId);
                if (!isParentValid)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.OrgUnitInfo.InValidParent");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }

                orgStructureInfoAddVM.Names.ForEach(n =>
                {
                    orgStructureInfoVMs.Where(o => !o.IsDeleted).ToList().ForEach(o =>
                    {
                        if (o.Key != orgStructureInfoAddVM.Key)
                        {
                            o.Names.ToList().ForEach(l =>
                            {
                                if (l.CultureId == n.CultureId && l.Text == n.Text)
                                {
                                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, string.Format("Admin.OrgUnitInfo.InValidName.{0}", l.CultureName));
                                }
                            });
                        }
                    });

                    int number;
                    if (int.TryParse(n.Text, out number))
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.OrgUnitInfo.InValidNameNumber");
                    }
                });

                if (message != string.Empty)
                {
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }

                //if (orgStructureInfoVMs.Where(o => o.Number == orgStructureInfoAddVM.Number && !o.IsDeleted).ToList().Count() != 0)
                //{
                //    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.OrgUnitInfo.InValidNumber");
                //    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                //}
                if (orgStructureInfoVM == null)
                {
                    orgStructureInfoVM = new OrgStructureInfoVM();
                    orgStructureInfoVM.IsActive = true;
                    orgStructureInfoVM.Key = orgStructureInfoAddVM.Key;
                }
                orgStructureInfoVM.Names = orgStructureInfoAddVM.Names;
                if (orgStructureInfoAddVM.Name != string.Empty)
                {
                    orgStructureInfoVM.Name = orgStructureInfoAddVM.Name;
                }

                orgStructureInfoVM.ManagerId = orgStructureInfoAddVM.ManagerId.HasValue ? orgStructureInfoAddVM.ManagerId.Value : 0;
                orgStructureInfoVM.Number = orgStructureInfoAddVM.Number;
                orgStructureInfoVM.TransactionsProcessingPeriod = orgStructureInfoAddVM.TransactionsProcessingPeriod;
                orgStructureInfoVM.IsVirtualUnit = orgStructureInfoAddVM.IsVirtualUnit;
                orgStructureInfoVM.BarCode = orgStructureInfoAddVM.BarCode ?? string.Empty;
                orgStructureInfoVM.IoDepartment = orgStructureInfoAddVM.IoDepartment;
                orgStructureInfoVM.FollowUpDepartment = orgStructureInfoAddVM.FollowUpDepartment;
                orgStructureInfoVM.IsExecutive = orgStructureInfoAddVM.IsExecutive;
                orgStructureInfoVM.ReceiveElcOutBoundWithAcknowled = orgStructureInfoAddVM.ReceiveElcOutBoundWithAcknowled;
                orgStructureInfoVM.SendSpecialCopy = orgStructureInfoAddVM.SendSpecialCopy;
                orgStructureInfoVM.IsGeneralIoDepartment = orgStructureInfoAddVM.IsGeneralIoDepartment;
                orgStructureInfoVM.Lineage = orgStructureInfoAddVM.Lineage;
                if (orgStructureInfoAddVM.IsRoot == true)
                {
                    orgStructureInfoVM.ParentId = -1;
                }
                else
                {
                    orgStructureInfoVM.ParentId = orgStructureInfoAddVM.ParentId;
                }

                if (orgStructureInfoAddVM.ParentId == -1 && orgStructureInfoVM.Counter != null)
                {
                    orgStructureInfoVM.Counter.IsGeneral = true;
                }

                string parentName;

                if (orgStructureInfoAddVM.ParentId != -1)
                {
                    parentName = orgStructureInfoVMs.FirstOrDefault(o => o.Key == orgStructureInfoAddVM.ParentId).Name;
                }
                else
                {
                    parentName = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.OrgUnitInfo.IsRoot");
                }

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest($"api/Admin/UpdateOrgUnitInfo", OrgStructureInfoMapper.Map(orgStructureInfoVM)).Result;
                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.OrgUnitInfo.AddSucceeded");
                return Json(new { MessageText = message, MessageType = MessageType.Information, Id = putResult.Id }, JsonRequestBehavior.AllowGet);
                //return Json(new
                //{
                //    parentId = orgStructureInfoAddVM.ParentId.ToString(),
                //    parentName,
                //    key = orgStructureInfoAddVM.Key.ToString(),
                //    name = orgStructureInfoAddVM.Name,
                //    MessageText = message,
                //    MessageType = MessageType.Information
                //});
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditOrgUnitInfo(OrgStructureInfoEditVM orgStructureInfoEditVM)
        {
            try
            {
                string message = string.Empty;
                List<OrgStructureInfoVM> orgStructureInfoVMs = GetOrgStructuresLight();

                OrgStructureInfoVM orgUnit = orgStructureInfoVMs.FirstOrDefault(o => o.ManagerId == orgStructureInfoEditVM.ManagerId && !o.IsDeleted);

                //if (orgUnit != null && orgUnit.Key != orgStructureInfoEditVM.Key)
                //{
                //    message = DbRes.TValidation("Admin.OrgUnitInfo.InvalidManager").Replace("{0}", orgUnit.Name);
                //    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                //}

                OrgStructureInfoVM orgStructureInfoVM = orgStructureInfoVMs.FirstOrDefault(o => o.Key == orgStructureInfoEditVM.Key);

                IsParentValid(orgStructureInfoVMs, orgStructureInfoEditVM.Key, orgStructureInfoEditVM.ParentId);
                if (!isParentValid)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.OrgUnitInfo.InValidParent");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }

                orgStructureInfoEditVM.Names.ForEach(n =>
                {
                    orgStructureInfoVMs.Where(o => !o.IsDeleted).ToList().ForEach(o =>
                    {
                        if (o.Key != orgStructureInfoEditVM.Key)
                        {
                            o.Names.ToList().ForEach(l =>
                            {
                                if (l.CultureId == n.CultureId && l.Text == n.Text)
                                {
                                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, string.Format("Admin.OrgUnitInfo.InValidName.{0}", l.CultureName));
                                }
                            });
                        }
                    });

                    int number;
                    if (int.TryParse(n.Text, out number))
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.OrgUnitInfo.InValidNameNumber");
                    }
                });

                if (!string.IsNullOrEmpty(message))
                {
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }

                if (orgStructureInfoVMs.ToList().Where(o => o.Number == orgStructureInfoEditVM.Number
                    && o.Key != orgStructureInfoEditVM.Key
                    && !o.IsDeleted).ToList().Count() != 0)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.OrgUnitInfo.InValidNumber");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }

                orgStructureInfoVM.Names = orgStructureInfoEditVM.Names;

                if (orgStructureInfoEditVM.Name != string.Empty)
                {
                    orgStructureInfoVM.Name = orgStructureInfoEditVM.Name;
                }

                int oldParentId = orgStructureInfoVM.ParentId;

                orgStructureInfoVM.ManagerId = orgStructureInfoEditVM.ManagerId.HasValue ? orgStructureInfoEditVM.ManagerId.Value : 0;
                orgStructureInfoVM.Number = orgStructureInfoEditVM.Number;
                orgStructureInfoVM.TransactionsProcessingPeriod = orgStructureInfoEditVM.TransactionsProcessingPeriod;
                orgStructureInfoVM.IsVirtualUnit = orgStructureInfoEditVM.IsVirtualUnit;
                orgStructureInfoVM.BarCode = orgStructureInfoEditVM.BarCode ?? string.Empty;
                orgStructureInfoVM.ExternalId = orgStructureInfoEditVM.ExternalId;

                orgStructureInfoVM.ParentId = -1;
                if (!orgStructureInfoEditVM.IsRoot)
                {
                    orgStructureInfoVM.ParentId = orgStructureInfoEditVM.ParentId;
                }

                if (orgStructureInfoEditVM.ParentId.ToString() == "-1" && orgStructureInfoVM.Counter != null)
                {
                    orgStructureInfoVM.Counter.IsGeneral = true;
                }

                if (oldParentId.ToString() == "-1" && orgStructureInfoEditVM.ParentId.ToString() != "-1")
                {
                    orgStructureInfoVM.Counter = null;
                }

                string parentName = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.OrgUnitInfo.IsRoot");
                if (orgStructureInfoEditVM.ParentId != -1)
                {
                    parentName = orgStructureInfoVMs.ToList().Where(o => o.Key == orgStructureInfoEditVM.ParentId).FirstOrDefault().Name;
                }

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest($"api/Admin/UpdateOrgUnitInfo", OrgStructureInfoMapper.Map(orgStructureInfoVM)).Result;
                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.OrgUnitInfo.AddSucceeded");
                return Json(new
                {
                    parentId = orgStructureInfoEditVM.ParentId.ToString(),
                    parentName,
                    oldParentId,
                    key = orgStructureInfoEditVM.Key.ToString(),
                    name = orgStructureInfoEditVM.Name,
                    MessageText = message,
                    MessageType = MessageType.Information
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Org Unit Links
        [HttpPost]
        public ActionResult OrgUnitLinks(int orgUnitKey)
        {
            try
            {
                ViewData["OrgUnitKey"] = orgUnitKey;
                ViewData["DialogType"] = (int)DialogMode.UnitLinks;

                string containerHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "DialogContainer", null);

                List<OrgStructureInfoVM> orgStructureInfoVMs = GetOrgUnitsWithLinks();
                OrgStructureInfoVM orgStructureInfoVM = orgStructureInfoVMs.FirstOrDefault(o => o.Key == orgUnitKey);

                orgStructureInfoVMs = orgStructureInfoVMs.Where(o => o.Key != orgUnitKey).ToList();
                List<AutoCompleteDataSource> autoCompleteDataSources = orgStructureInfoVMs.Select(o => new AutoCompleteDataSource()
                {
                    Label = o.Name,
                    Value = o.Key.ToString()
                }).ToList();

                ViewData["ListOfLinks"] = autoCompleteDataSources;

                List<OrgUnitLinkVM> orgUnitLinkVMs = new List<OrgUnitLinkVM>();

                if (orgStructureInfoVM != null)
                {
                    orgStructureInfoVM.LinkUnitsKeys.ForEach(k =>
                    {
                        if (orgStructureInfoVMs.Where(o => o.Key == k).FirstOrDefault() != null)
                        {
                            orgUnitLinkVMs.Add(new OrgUnitLinkVM()
                            {
                                Key = k,
                                OrgUnitName = orgStructureInfoVMs.Where(o => o.Key == k).FirstOrDefault().Name
                            });
                        }
                    });
                }
                else
                {
                    orgStructureInfoVM = new OrgStructureInfoVM();
                }

                ViewData["GridData"] = (AjaxGrid<OrgUnitLinkVM>)new AjaxGridFactory().CreateAjaxGrid(orgUnitLinkVMs.AsQueryable(), 1, false, orgUnitLinkVMs.Count());
                return Json(new
                {
                    View = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OrgUnitLinksPartial", new OrgUnitLinkVM()),
                    ContainerHtml = containerHtml
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult OrgUnitAddLinks(OrgUnitLinkVM orgUnitLinkVM, int orgUnitKey)
        {
            try
            {
                string message = string.Empty;

                List<OrgStructureInfoVM> orgStructureInfoVMs = GetOrgUnitsWithLinks();

                OrgStructureInfoVM orgStructureInfoVM = orgStructureInfoVMs.Where(o => o.Key == Convert.ToInt32(orgUnitKey)).FirstOrDefault();
                if (orgStructureInfoVM == null)
                {
                    orgStructureInfoVM = new OrgStructureInfoVM();
                    orgStructureInfoVM.LinkUnitsKeys = new List<int>();
                }
                orgStructureInfoVMs = orgStructureInfoVMs.Where(o => !o.IsDeleted && o.Key != Convert.ToInt32(orgUnitKey)).ToList();

                List<OrgUnitLinkVM> orgUnitLinkVMs = new List<OrgUnitLinkVM>();
                if (!orgStructureInfoVM.LinkUnitsKeys.Any(a => a == orgUnitLinkVM.Key) && orgUnitLinkVM.Key != 0)
                {
                    foreach (var item in orgStructureInfoVM.LinkUnitsKeys)
                    {
                        var oldOrgUnitLink = orgStructureInfoVMs.FirstOrDefault(a => a.Key == item);
                        if (oldOrgUnitLink != null)
                        {
                            orgUnitLinkVMs.Add(new OrgUnitLinkVM()
                            {
                                Key = oldOrgUnitLink.Key,
                                OrgUnitName = oldOrgUnitLink.Name
                            });
                        }
                    }
                    orgUnitLinkVMs.Add(new OrgUnitLinkVM()
                    {
                        Key = orgUnitLinkVM.Key,
                        OrgUnitName = orgUnitLinkVM.OrgUnitName
                    });
                }
                else
                {
                    message = DbRes.TResource("Admin.OrgUnitLinks.UnitExistMsg");
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                var orgUnitDTO = new OrgUnitDTO
                {
                    Id = orgUnitKey,
                    Key = orgUnitKey,
                    IsActive = true,
                    LinkUnitsKeys = orgUnitLinkVMs.Select(a => a.Key).ToList()
                };

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest($"api/Admin/UpdateOrgUnitWithLink?cultureName={SessionInfo.CultureShortName}", orgUnitDTO).Result;
                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<OrgUnitLinkVM>)new AjaxGridFactory().CreateAjaxGrid(orgUnitLinkVMs.AsQueryable(), 1, false, orgUnitLinkVMs.Count());
                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OrgUnitLinksGridPartial", grid),
                    MessageType = MessageType.Information,
                    MessageText = message
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteOrgUnitLinks(string ids, int orgUnitKeyGrid)
        {
            try
            {
                List<OrgUnitLinkVM> orgUnitLinkVMs = new List<OrgUnitLinkVM>();
                List<OrgStructureInfoVM> orgStructureInfoVMs = GetOrgUnitsWithLinks();

                List<int> linkUnitsKeys = orgStructureInfoVMs.FirstOrDefault(a => a.Key == orgUnitKeyGrid).LinkUnitsKeys;
                List<int> listOfIDs = new List<int>(ids.Split(',').Select(a => int.Parse(a)));

                List<int> toDelete = linkUnitsKeys.Where(a => !listOfIDs.Contains(a)).ToList();

                foreach (var item in toDelete)
                {
                    var oldOrgUnitLink = orgStructureInfoVMs.FirstOrDefault(a => a.Key == item);
                    if (oldOrgUnitLink != null)
                    {
                        orgUnitLinkVMs.Add(new OrgUnitLinkVM()
                        {
                            Key = oldOrgUnitLink.Key,
                            OrgUnitName = oldOrgUnitLink.Name
                        });
                    }
                }

                var orgUnitDTO = new OrgUnitDTO
                {
                    Id = orgUnitKeyGrid,
                    Key = orgUnitKeyGrid,
                    LinkUnitsKeys = orgUnitLinkVMs.Select(a => a.Key).ToList()
                };

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest($"api/Admin/UpdateOrgUnitWithLink?cultureName={SessionInfo.CultureShortName}", orgUnitDTO).Result;
                if (putResult.StatusCode != StatusCode.Ok)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<OrgUnitLinkVM>)new AjaxGridFactory().CreateAjaxGrid(orgUnitLinkVMs.AsQueryable(), 1, false, orgUnitLinkVMs.Count());
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OrgUnitLinksGridPartial", grid) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult UpdateGridOrgUnitLink(int? page, string param)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<OrgStructureInfoVM> orgStructureInfoVMs = javaScriptSerializer.Deserialize(param, typeof(List<OrgStructureInfoVM>)) as List<OrgStructureInfoVM>;

                OrgStructureInfoVM orgStructureInfoVM = orgStructureInfoVMs.Where(o => o.Key == Convert.ToInt32(TempData.Peek("OrgUnitKey"))).FirstOrDefault();

                orgStructureInfoVMs = orgStructureInfoVMs.Where(o => !o.IsDeleted && o.Key != Convert.ToInt32(TempData.Peek("OrgUnitKey"))).ToList();

                List<OrgUnitLinkVM> orgUnitLinkVMs = new List<OrgUnitLinkVM>();

                orgStructureInfoVM.LinkUnitsKeys.ForEach(k =>
                {

                    if (orgStructureInfoVMs.Where(o => o.Key == k).FirstOrDefault() != null)
                    {
                        orgUnitLinkVMs.Add(new OrgUnitLinkVM()
                        {
                            Key = k,
                            OrgUnitName = orgStructureInfoVMs.Where(o => o.Key == k).FirstOrDefault().Name
                        });
                    }
                });

                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(orgUnitLinkVMs.AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, orgUnitLinkVMs.Count(), true);

                return Json(new { Html = grid.ToJson("_OrgUnitLinksGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region CreateUser
        [HttpPost]
        public ActionResult CreateUser(string orgUnitKey)
        {
            try
            {
                string message = string.Empty;

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);
                GetResult<List<GroupDTO>> groupsDTOs = HttpClientWrapper<GetResult<List<GroupDTO>>>.GetItemRequest(string.Format("api/Admin/GetAllUserDefinedPermissionsGroups?CultureName={0}", SessionInfo.CultureShortName)).Result;

                if (groupsDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, groupsDTOs.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                ViewData["Groups"] = BuildGroupTree(GroupMapper.Map(groupsDTOs.Result), null);

                ViewData["UserCategory"] = GetUserCategories();
                ViewData["SelectedOrgUnitId"] = orgUnitKey;

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OrgUnitUsersAddPartial", new AddUserProfileVM()) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddUserProfile(AddUserProfileVM addUserProfileVM)
        {
            try
            {
                string message = string.Empty;

                if (addUserProfileVM.Email == null)
                {
                    addUserProfileVM.Email = addUserProfileVM.UserName + "@yasser.gov.sa";
                }

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Admin/PostUser?cultureName={0}&resetPasswordUrl={1}", SessionInfo.CultureShortName, UrlHelper.GetResetPasswordUrl(ControllerContext)), UserProfileMapper.Map(addUserProfileVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<UserProfileDTO>> userProfileDTOs =
                                 HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetAllUsers?cultureName={0}", SessionInfo.CultureShortName)).Result;

                if (userProfileDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userProfileDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IList<AutoCompleteDataSource> autoCompleteDataSources = new List<AutoCompleteDataSource>();

                var user = UserProfileMapper.Map(userProfileDTOs.Result);
                foreach (UserProfileVM userProfileVM in user)
                {
                    autoCompleteDataSources.Add(new AutoCompleteDataSource { Label = userProfileVM.LocalName, Value = userProfileVM.Id.ToString() });
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.User.AddSucceeded");

                return Json(new { DataSource = JsonConvert.SerializeObject(autoCompleteDataSources), MessageText = message, MessageType = MessageType.Information, UserId = postResult.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private TreeViewModel BulidTree(List<PermissionGroupVM> permissionGroupVMs, string rootName)
        {
            TreeViewModel tree = new TreeViewModel();
            List<TreeNode> nodes = new List<TreeNode>();

            tree.RootNode = new TreeNode { Id = 0, Name = rootName };

            for (int i = 0; i < permissionGroupVMs.Count; i++)
            {
                TreeNode groupNode = new TreeNode()
                {
                    Id = permissionGroupVMs[i].Id,
                    ParentId = 0,
                    Name = permissionGroupVMs[i].Text,
                };

                foreach (PermissionVM permission in permissionGroupVMs[i].Permissions)
                {
                    TreeNode permissionNode = new TreeNode()
                    {
                        Id = permission.Id,
                        ParentId = permissionGroupVMs[i].Id,
                        Name = permission.Text,
                        IsSelected = permission.IsSelected,
                    };

                    groupNode.Childs.Add(permissionNode);
                }

                tree.RootNode.Childs.Add(groupNode);
            }

            nodes.Add(tree.RootNode);

            return tree;
        }
        private string GetUserCategories()
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

            GetResult<List<UserCategoryDTO>> userCategoryDTOs =
              HttpClientWrapper<GetResult<List<UserCategoryDTO>>>.GetItemRequest(String.Format("api/Admin/GetAllUsersCategories?cultureName={0}", SessionInfo.CultureShortName)).Result;

            if (userCategoryDTOs.Result != null)
            {
                var user = UserCategoryMapper.Map(userCategoryDTOs.Result);
                foreach (UserCategoryVM userCategoryVM in user)
                {
                    dataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = userCategoryVM.Id.ToString(),
                        Label = userCategoryVM.CategoryText
                    });
                }
            }

            return JsonConvert.SerializeObject(dataSource);
        }
        private TreeViewModel BuildOrgUnitsTree(List<OrgUnitVM> orgUnitVMs)
        {
            TreeViewModel tree = new TreeViewModel();
            List<TreeNode> nodes = new List<TreeNode>();

            tree.RootNode = new TreeNode { Id = 0, Mode = tree.Mode };

            OrgUnitVM root = orgUnitVMs.Where(o => o.ParentId == -1).SingleOrDefault();

            if (root != null)
            {
                orgUnitVMs.Where(o => o.ParentId == root.Id).ToList().ForEach(d =>
                {
                    tree.RootNode.Childs.Add(AddChilds(orgUnitVMs, d));
                });
            }

            return tree;
        }
        private TreeNode AddChilds(List<OrgUnitVM> orgUnitVMs, OrgUnitVM orgUnitVM)
        {
            TreeNode treeNode = new TreeNode()
            {
                DepartmentNumber = orgUnitVM.Number.ToString(),
                IsSelected = orgUnitVM.IsSelected,
                Selectable = true,
                Name = orgUnitVM.Name,
                Id = orgUnitVM.Id
            };

            orgUnitVMs.Where(o => o.ParentId == orgUnitVM.Id).ToList().ForEach(d =>
            {
                treeNode.Childs.Add(AddChilds(orgUnitVMs, d));
            });

            return treeNode;
        }
        #endregion CreateUser

        #region Barcode
        [HttpPost]
        public ActionResult BarcodeDesigner(int orgUnitKey)
        {
            try
            {
                OrgStructureInfoVM orgStructureInfoVM = GetOrgStructureInfoByOrgUnitKey(orgUnitKey, DialogMode.BarcodeDesigner);

                string containerHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "DialogContainer", null);

                List<TransactionCategoryVM> transactionCategoryVMs = GetBarcodeDesignTypeLookups();

                transactionCategoryVMs = transactionCategoryVMs.Where(tc => tc.Id != BarcodeDesignType.VisitTicket.LookupIdentity(LookupCategory.BarcodeDesignType, string.Empty)).ToList();
                ViewData["TransactionCategory"] = transactionCategoryVMs;

                BarcodeDesignerVM barcodeDesignerVM = new BarcodeDesignerVM
                {
                    TypeId = BarcodeDesignType.Inbound.LookupIdentity(LookupCategory.BarcodeDesignType, string.Empty),
                    IsGeneral = false
                };

                if (orgStructureInfoVM == null)
                {
                    orgStructureInfoVM = new OrgStructureInfoVM();
                }
                if (orgStructureInfoVM.BarcodeDesigners == null
                    || orgStructureInfoVM.BarcodeDesigners.Where(b => b.TypeId == BarcodeDesignType.Inbound.LookupIdentity(LookupCategory.BarcodeDesignType, SessionInfo.CultureShortName)).FirstOrDefault() == null)// then it is Add
                {
                    barcodeDesignerVM.Id = 0;
                    barcodeDesignerVM.Html = "<div id=\"barCode\" style=\"position:relative;\" class=\"droppable ui-widget-header col-md-6 encryption_code size_c\"> </div>";
                }
                else
                {
                    BarcodeDesignerVM barcodeDesigner = orgStructureInfoVM.BarcodeDesigners.Where(b => b.TypeId == BarcodeDesignType.Inbound.LookupIdentity(LookupCategory.BarcodeDesignType, SessionInfo.CultureShortName)).FirstOrDefault();
                    string imag2DStyle = string.Format("<img style='width: {0}px;  height: {1}px;' class='imag2D' src='{2}/Content/Admin/Lib/images/morasalat/code_2d_v.png' />", barcodeDesigner.Width * 0.6, barcodeDesigner.Height * 0.2, UrlHelper.GetBaseUri());
                    string imag3DStyle = string.Format("<img style='width: {1}px;  height: {1}px;' class='imag3D' src='{2}/Content/Admin/Lib/images/morasalat/code_3d_v.png' />", barcodeDesigner.Width * 0.3, barcodeDesigner.Height * 0.3, UrlHelper.GetBaseUri());

                    barcodeDesignerVM.Id = barcodeDesigner.Id;
                    barcodeDesignerVM.Html = string.Format(barcodeDesigner.Html, "", imag2DStyle, "",
                                               imag3DStyle, "",
                                               ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.DepartmentPreparedInbound"), "",
                                               ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionDate"), "",
                                               ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionNumber"), "",
                                               ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Attachments"), "",
                                               ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Abbreviation"), ""
                                               );
                }
                string htmlView = UIHelper.RenderRazorViewToHtml(ControllerContext, "_BarcodeDesignerPartial", barcodeDesignerVM);
                return Json(new { View = htmlView, ContainerHtml = containerHtml }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddDesign(BarcodeDesignerVM designVM, int orgUnitKey)
        {
            try
            {
                string message = string.Empty;
                OrgStructureInfoVM orgStructureInfoVM = GetOrgStructureInfoByOrgUnitKey(designVM.orgUnitKey, DialogMode.BarcodeDesigner);

                if (orgStructureInfoVM == null)
                {
                    orgStructureInfoVM = new OrgStructureInfoVM();
                    orgStructureInfoVM.BarcodeDesigners = new List<BarcodeDesignerVM>();
                }

                orgStructureInfoVM.BarcodeDesigners = orgStructureInfoVM.BarcodeDesigners ?? new List<BarcodeDesignerVM>();

                var currentBarcodeDesigner = orgStructureInfoVM.BarcodeDesigners.FirstOrDefault(b => b.TypeId == designVM.TypeId);
                if (currentBarcodeDesigner != null)
                {
                    designVM.Id = currentBarcodeDesigner.Id;
                    orgStructureInfoVM.BarcodeDesigners.Remove(currentBarcodeDesigner);
                }
                orgStructureInfoVM.BarcodeDesigners.Add(designVM);

                var orgUnitDTO = new OrgUnitDTO
                {
                    Id = orgUnitKey,
                    Key = orgUnitKey,
                    IsActive = true,
                    BarcodeDesigns = BarcodeDesignerMapper.Map(orgStructureInfoVM.BarcodeDesigners)
                };
                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest($"api/Admin/UpdateOrgUnitWithBarcodeDesign?cultureName={SessionInfo.CultureShortName}", orgUnitDTO).Result;
                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    MessageText = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.ExternalParty.AddSucceeded"),
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetDesign(int orgUnitKey, int typeId)
        {
            try
            {
                string message = string.Empty;
                string html = string.Empty;
                OrgStructureInfoVM orgStructureInfoVM = GetOrgStructureInfoByOrgUnitKey(orgUnitKey, DialogMode.BarcodeDesigner);

                BarcodeDesignerVM barcodeDesignerVM = new BarcodeDesignerVM
                {
                    Id = 0,
                    TypeId = typeId,
                    IsGeneral = false,
                    Html = string.Empty,
                    orgUnitKey = orgUnitKey
                };

                if (orgStructureInfoVM != null && orgStructureInfoVM.BarcodeDesigners != null)// then it is Add
                {
                    BarcodeDesignerVM barcode = orgStructureInfoVM.BarcodeDesigners.FirstOrDefault(b => b.TypeId == typeId);
                    if (barcode != null)
                    {
                        barcodeDesignerVM.Id = barcode.Id;
                        barcodeDesignerVM.Html = barcode.Html;
                        barcodeDesignerVM.Width = barcode.Width;
                        barcodeDesignerVM.Height = barcode.Height;
                    }
                }
                html = GetViewHtml(barcodeDesignerVM);
                return Json(new { Html = html }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetViewHtml(BarcodeDesignerVM barcodeDesignerVM)
        {
            string barcode2D = $"<img style='width: {barcodeDesignerVM.Width * 0.6}px;  height: {barcodeDesignerVM.Height * 0.2}px;' class='imag2D' src='{UrlHelper.GetBaseUri()}/Content/Admin/Lib/images/morasalat/code_2d_v.png' />";
            string barcode3D = $"<img style='width: {barcodeDesignerVM.Width * 0.3}px;  height: {barcodeDesignerVM.Height * 0.3}px;' class='imag3D' src='{UrlHelper.GetBaseUri()}/Content/Admin/Lib/images/morasalat/code_3d_v.png' />";
            string result = string.Empty;
            switch ((BarcodeDesignType)barcodeDesignerVM.TypeId.LookupInternalID(LookupCategory.BarcodeDesignType, SessionInfo.CultureShortName))
            {
                case BarcodeDesignType.Inbound:
                    if (string.IsNullOrEmpty(barcodeDesignerVM.Html))
                    {
                        barcodeDesignerVM.Html = "<div id=\"barCode\" style=\"position:relative;\" class=\"droppable ui-widget-header col-md-6 encryption_code size_c\"> </div>";
                    }
                    else
                    {
                        barcodeDesignerVM.Html = string.Format(barcodeDesignerVM.Html, "",
                           barcode2D, "",
                           barcode3D, "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.DepartmentPreparedInbound"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionDate"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionNumber"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Attachments"), "",
                           "الاختصار", "");
                    }
                    result = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundBarcodeDesignerPartial", barcodeDesignerVM);
                    break;
                case BarcodeDesignType.Outbound:
                    if (string.IsNullOrEmpty(barcodeDesignerVM.Html))
                    {
                        barcodeDesignerVM.Html = "<div id=\"barCode\" style=\"position:relative;\" class=\"droppable ui-widget-header col-md-6 encryption_code size_c\"> </div>";
                    }
                    else
                    {
                        barcodeDesignerVM.Html = string.Format(barcodeDesignerVM.Html, "",
                           barcode2D, "",
                           barcode3D, "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.DirectedDepartment"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionDate"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.OutboundNumber"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Attachments"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Abbreviation"), "");
                    }
                    result = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundBarcodeDesignerPartial", barcodeDesignerVM);
                    break;
                case BarcodeDesignType.OutboundInternal:
                    if (string.IsNullOrEmpty(barcodeDesignerVM.Html))
                    {
                        barcodeDesignerVM.Html = "<div id=\"barCode\" style=\"position:relative;\" class=\"droppable ui-widget-header col-md-6 encryption_code size_c\"> </div>";
                    }
                    else
                    {
                        barcodeDesignerVM.Html = string.Format(barcodeDesignerVM.Html, "",
                           barcode2D, "",
                           barcode3D, "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.PreparedDepartment"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionDate"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionNumber"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Attachments"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Attachments"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Abbreviation"), "");
                    }
                    result = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InternalOutboundBarcodeDesignerPartial", barcodeDesignerVM);
                    break;
            }
            return result;
        }

        private List<TransactionCategoryVM> GetBarcodeDesignTypeLookups()
        {
            GetResult<IList<LookupVM>> lookupVMs = LookupsHelper.GetAdminLookupItems(LookupCategory.BarcodeDesignType, SessionInfo.CultureShortName);
            List<TransactionCategoryVM> transactionCategoryVMs = new List<TransactionCategoryVM>();
            if (lookupVMs != null)
            {
                foreach (LookupVM lookupVM in lookupVMs.Result)
                {
                    if (lookupVM.Id != BarcodeDesignType.Attachment.LookupIdentity(LookupCategory.BarcodeDesignType, SessionInfo.CultureShortName))
                    {
                        transactionCategoryVMs.Add(new TransactionCategoryVM()
                        {
                            Id = lookupVM.Id,
                            Text = lookupVM.Text,
                        });
                    }
                }
            }
            return transactionCategoryVMs;
        }

        #endregion Barcode

        #region Tree
        private void GetIndexView()
        {
            Dictionary<string, string> listOfActions = new Dictionary<string, string>()
                {
                   {DbRes.TResource("Admin.OrgUnitStructure.UnitInfo"), "OrgUnitInfo"},
                   {DbRes.TResource("Admin.OrgUnitStructure.UnitCounter"), "Counter"},
                   {DbRes.TResource("Admin.OrgUnitStructure.UnitUsers"), "OrgUnitUsers"},
                   {DbRes.TResource("Admin.OrgUnitStructure.UnitLinks"), "OrgUnitLinks"},
                   {DbRes.TResource("Admin.OrgUnitStructure.BarcodeDesigner"), "OrgUnitBarcodeDesigner"},
                };

            ViewData["ListOfActions"] = listOfActions;
        }




        public ActionResult GetRoot()
        {
            try
            {
                string url = $"api/Admin/GetOrgUnitStructureRoot?cultureName={SessionInfo.CultureShortName}&parentId{null}";
                var orgUnitStructureDesignDTO = HttpClientWrapper<GetResult<OrgUnitStructureDesignDTO>>.GetItemRequest(url).Result;

                List<OrgStructureInfoVM> orgStructureInfoVMs = OrgStructureInfoMapper.Map(orgUnitStructureDesignDTO.Result, SessionInfo.CultureShortName).OrgUnits;

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                      HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                ViewData["orgUnitDataInfo"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                ViewData["DepartmentsStructure"] = orgStructureInfoVMs;

                TreeViewModel tree = new TreeViewModel();
                OrgStructureInfoVM root;

                if (orgStructureInfoVMs.Where(o => !o.IsDeleted && o.ParentId == -1).FirstOrDefault() != null)
                {
                    root = orgStructureInfoVMs.Where(o => !o.IsDeleted && o.ParentId == -1).FirstOrDefault();
                }
                else
                {
                    return Json(new
                    {
                        Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OrgUnitTreeViewPartial", tree)
                    }, JsonRequestBehavior.AllowGet);
                }

                orgStructureInfoVMs = orgStructureInfoVMs.Where(o => !o.IsDeleted && o.ParentId != -1).ToList();

                List<TreeNode> nodes = new List<TreeNode>();

                if (orgStructureInfoVMs != null && orgStructureInfoVMs.Count != 0)
                {
                    nodes = orgStructureInfoVMs.Select(o => new TreeNode()
                    {
                        Id = o.Key,
                        ParentId = o.ParentId,
                        Name = o.Name,
                        HasChilds = o.HasChilds,
                        DepartmentNumber = o.Number.ToString(),
                        ExternalId = o.ExternalId
                    }).ToList();
                }

                tree.RootNode = new TreeNode
                {
                    Id = root.Key,
                    Name = root.Name,
                    Mode = tree.Mode,
                    HasChilds = root.HasChilds,
                    ParentId = root.ParentId,
                    DepartmentNumber = root.Number.ToString(),
                    ExternalId = root.ExternalId
                };

                tree.Nodes = nodes.Select(t => new TreeNode
                {
                    Id = t.Id,
                    IsSelected = t.IsSelected,
                    ParentId = t.ParentId,
                    Name = t.Name,
                    Mode = tree.Mode,
                    HasChilds = t.HasChilds,
                    DepartmentNumber = t.DepartmentNumber,
                    ExternalId = t.ExternalId
                }).ToDictionary(t => t.Id);

                tree.Nodes.Add(tree.RootNode.Id, tree.RootNode);

                foreach (var node in tree.Nodes.Values)
                {
                    if (tree.Nodes.TryGetValue(node.ParentId, out TreeNode parent) && node.Id != node.ParentId)
                    {
                        node.Parent = parent;
                        parent.Childs.Add(node);
                    }
                }
                return PartialView("_OrgUnitRootTreeViewPartial", tree);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult GetOrgStructureSearchById(int? id)
        {
            if (!id.HasValue)
            { 
                return Json(new { MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            string url = $"api/Admin/GetOrgUnitById?cultureName={SessionInfo.CultureShortName}&id={id}";
            GetResult<OrgStructureInfoDTO> orgStructureInfoDTO = HttpClientWrapper<GetResult<OrgStructureInfoDTO>>.GetItemRequest(url).Result;

            OrgStructureInfoVM orgStructureInfoVMs = OrgStructureInfoMapper.Map(orgStructureInfoDTO.Result, SessionInfo.CultureShortName);
            TreeNode treeNode = new TreeNode()
            {
                Id = orgStructureInfoVMs.Key,
                ParentId = 0,
                Name = orgStructureInfoVMs.Names[0].Text,
                HasChilds = false,
                DepartmentNumber = orgStructureInfoVMs.Number
            };
            if (orgStructureInfoVMs != null)
            {


                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OrgUnitChildsTreeViewPartial", treeNode), JsonRequestBehavior.AllowGet });
            }
            else
            {
                return Json(new { MessageText = DbRes.TResource("Admin.User.NotFound"), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetChildren(int id)
        {
            try
            {
                string url = $"api/Admin/GetOrgUnitStructureRoot?cultureName={SessionInfo.CultureShortName}&parentId={id}";
                var orgUnitStructureDesignDTO = HttpClientWrapper<GetResult<OrgUnitStructureDesignDTO>>.GetItemRequest(url).Result;

                List<OrgStructureInfoVM> orgStructureInfoVMs = OrgStructureInfoMapper.Map(orgUnitStructureDesignDTO.Result, SessionInfo.CultureShortName).OrgUnits;
                orgStructureInfoVMs = orgStructureInfoVMs.Where(o => !o.IsDeleted && o.ParentId != -1).ToList();
                List<TreeNode> nodes = new List<TreeNode>();

                if (orgStructureInfoVMs != null && orgStructureInfoVMs.Count != 0)
                {
                    nodes = orgStructureInfoVMs.Select(o => new TreeNode()
                    {
                        Id = o.Key,
                        ParentId = o.ParentId,
                        Name = o.Name,
                        HasChilds = o.HasChilds,
                        DepartmentNumber = o.Number.ToString(),
                        ExternalId = o.ExternalId
                    }).ToList();
                }

                ViewData["DepartmentsStructure"] = orgStructureInfoVMs;

                //return PartialView("_OrgUnitChildsTreeViewPartial", nodes);

                return PartialView("_OrgUnitSubTreeViewPartial", nodes);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteOrg(int id)
        {
            try
            {
                List<OrgStructureInfoVM> orgStructureInfoVMs = GetOrgStructuresLight();
                var resutl = orgStructureInfoVMs.FirstOrDefault(a => a.Key == id);

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest($"api/Admin/DeleteOrgUnit?orgUnitKey={resutl.Key}", null).Result;
                if (putResult.StatusCode != StatusCode.Ok)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

                }
            }

            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetNativeTree(OrgStructureInfoVM departmentsStructure, int id = 0)
        {
            try
            {
                TreeViewModel tree = new TreeViewModel();
                if (departmentsStructure == null && id == 0)
                {
                    return Json(new
                    {
                        Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OrgUnitTreeViewPartial", tree)
                    }, JsonRequestBehavior.AllowGet);
                }
                List<OrgStructureInfoVM> orgStructureInfoVMs = GetOrgStructuresLight();
                var resutl = orgStructureInfoVMs.FirstOrDefault(a => a.Key == id);
                if (resutl != null)//Need Delete Node
                {
                    PutResult putResult = HttpClientWrapper<PutResult>.PutRequest($"api/Admin/DeleteOrgUnit?orgUnitKey={resutl.Key}", null).Result;
                    if (putResult.StatusCode != StatusCode.Ok)
                    {
                        string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        orgStructureInfoVMs.RemoveAll(a => a.Key == resutl.Key);
                    }
                }
                else//Need Add Node
                {
                    if (departmentsStructure != null)
                    {
                        orgStructureInfoVMs.Add(departmentsStructure);
                    }
                }

                OrgStructureInfoVM root;

                if (orgStructureInfoVMs.Where(o => !o.IsDeleted && o.ParentId == -1).FirstOrDefault() != null)
                {
                    root = orgStructureInfoVMs.Where(o => !o.IsDeleted && o.ParentId == -1).FirstOrDefault();
                }
                else
                {
                    return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OrgUnitTreeViewPartial", tree) }, JsonRequestBehavior.AllowGet);
                }

                orgStructureInfoVMs = orgStructureInfoVMs.Where(o => !o.IsDeleted && o.ParentId != -1).ToList();

                List<TreeNode> nodes = new List<TreeNode>();

                if (orgStructureInfoVMs != null && orgStructureInfoVMs.Count != 0)
                {
                    nodes = orgStructureInfoVMs.Select(o => new TreeNode()
                    {
                        Id = o.Key,
                        ParentId = o.ParentId,
                        Name = o.Name
                    }).ToList();
                }

                if (id != 0)
                {
                    TreeNode node = nodes.Where(n => n.Id == id).FirstOrDefault();

                    if (node != null)
                    {
                        node.IsSelected = true;
                    }
                }

                tree.RootNode = new TreeNode { Id = root.Key, Name = root.Name, Mode = tree.Mode, ParentId = root.ParentId };

                TreeNode parent;

                tree.Nodes = nodes.Select(t => new TreeNode { Id = t.Id, IsSelected = t.IsSelected, ParentId = t.ParentId, Name = t.Name, Mode = tree.Mode })
                      .ToDictionary(t => t.Id);

                tree.Nodes.Add(tree.RootNode.Id, tree.RootNode);

                foreach (var node in tree.Nodes.Values)
                {
                    if (tree.Nodes.TryGetValue(node.ParentId, out parent) && node.Id != node.ParentId)
                    {
                        node.Parent = parent;
                        parent.Childs.Add(node);
                    }
                }

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OrgUnitTreeViewPartial", tree) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void BuildTree(TreeViewModel tree, List<TreeNode> nodes)
        {
            TreeNode parent;

            tree.Nodes = nodes.Select(t => new TreeNode { Id = t.Id, IsSelected = t.IsSelected, ParentId = t.ParentId, Name = t.Name, Mode = tree.Mode })
                  .ToDictionary(t => t.Id);

            tree.Nodes.Add(tree.RootNode.Id, tree.RootNode);

            foreach (var node in tree.Nodes.Values)
            {
                if (tree.Nodes.TryGetValue(node.ParentId, out parent) && node.Id != node.ParentId)
                {
                    node.Parent = parent;
                    parent.Childs.Add(node);
                }
            }
        }
        private void AddParentNode(List<OrgStructureInfoVM> orgStructureInfoVMs, ref List<OrgStructureInfoVM> orgStructureVMs, int id)
        {

            if (orgStructureInfoVMs.Where(o => o.Key == id).FirstOrDefault() != null && orgStructureVMs.Where(o => o.Key == id).FirstOrDefault() == null)
            {
                orgStructureVMs.Add(orgStructureInfoVMs.Where(o => o.Key == id).FirstOrDefault());

                AddParentNode(orgStructureInfoVMs, ref orgStructureVMs, orgStructureInfoVMs.Where(o => o.Key == id).FirstOrDefault().ParentId);
            }
        }
        private TreeViewModel BulidTree(List<OrgStructureInfoVM> orgStructureInfoVMs, int orgUnitKey)
        {
            OrgStructureInfoVM orgStructureInfoVM = orgStructureInfoVMs.Where(o => o.Key == orgUnitKey).FirstOrDefault();

            OrgStructureInfoVM root = orgStructureInfoVMs.Where(o => o.ParentId == -1).FirstOrDefault();

            List<OrgStructureInfoVM> orgStructureVMs = new List<OrgStructureInfoVM>();

            orgStructureInfoVMs = orgStructureInfoVMs.Where(o => !o.IsDeleted
                && !o.IsVirtualUnit && o.ParentId != 0 && o.ParentId != -1).ToList();

            orgStructureInfoVMs.ForEach(o =>
            {
                if ((o.Key == orgUnitKey || orgStructureInfoVM.LinkUnitsKeys.Contains(o.Key)) && !orgStructureVMs.Any(d => d.Key == o.Key))
                {
                    orgStructureVMs.Add(o);

                    AddParentNode(orgStructureInfoVMs, ref orgStructureVMs, o.ParentId);
                }
            });

            TreeViewModel tree = new TreeViewModel();
            List<TreeNode> nodes = new List<TreeNode>();

            tree.RootNode = new TreeNode { Id = 0, Mode = tree.Mode };

            orgStructureVMs.Where(o => o.ParentId == root.Key).ToList().ForEach(d =>
            {
                tree.RootNode.Childs.Add(AddChilds(orgStructureVMs, d, orgStructureInfoVM));
            });

            return tree;
        }
        private TreeNode AddChilds(List<OrgStructureInfoVM> orgUnitVMs, OrgStructureInfoVM orgUnitVM, OrgStructureInfoVM userOrgUnit)
        {
            TreeNode treeNode = new TreeNode()
            {
                Id = orgUnitVM.Key,
                DepartmentNumber = orgUnitVM.Number.ToString(),
                IsSelected = false,
                Selectable = false,
                Name = orgUnitVM.Name,
            };

            userOrgUnit.LinkUnitsKeys.ForEach(l =>
            {
                if (l == orgUnitVM.Key)
                {
                    treeNode.Selectable = true;
                }
            });

            if (userOrgUnit.Key == orgUnitVM.Key)
            {
                treeNode.Selectable = true;
            }

            orgUnitVMs.Where(o => o.ParentId == orgUnitVM.Key).ToList().ForEach(d =>
            {
                treeNode.Childs.Add(AddChilds(orgUnitVMs, d, userOrgUnit));
            });

            return treeNode;
        }
        public ActionResult NewOrgUnitInfo(OrgStructureInfoVM departmentsStructure)
        {
            try
            {
                ViewMode viewMode = ViewMode.Add;
                OrgUnitMode orgUnitMode;

                string htmlView;
                ViewData["Mode"] = viewMode;
                ViewData["OrgUnitKey"] = departmentsStructure.Key = 0;//Zero: New add
                ViewData["DialogType"] = (int)DialogMode.UnitInfo;

                string containerHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "DialogContainer", null);

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
                List<LocalizationVM> localizationVMList = cultureDTOs.Result.Select(c => new LocalizationVM()
                {

                    CultureId = c.Id,
                    CultureName = c.ShortName,
                    Text = departmentsStructure.Name
                }).ToList();



                List<OrgStructureInfoVM> orgStructureInfoVMs = GetOrgStructuresLight();
                OrgStructureInfoVM orgStructureParentInfoVM = orgStructureInfoVMs.FirstOrDefault(o => o.Key == departmentsStructure.ParentId);
                IList<AutoCompleteDataSource> autoCompleteDataSources = orgStructureInfoVMs.Select(o => new AutoCompleteDataSource()
                {
                    Label = o.Name,
                    Value = o.Key.ToString()
                }).ToList();
                ViewData["ListOfUnits"] = autoCompleteDataSources;

                List<AutoCompleteDataSource> OrgUnitUsers = new List<AutoCompleteDataSource>();
                if (orgStructureParentInfoVM != null)
                {
                    orgStructureParentInfoVM.userProfiles = GetUserByorgParentId(departmentsStructure.ParentId);
                }

                if (orgStructureParentInfoVM.userProfiles != null && orgStructureParentInfoVM.userProfiles.Count > 0)
                {
                    orgStructureParentInfoVM.userProfiles.ForEach(u =>
                    {
                        OrgUnitUsers.Add(new AutoCompleteDataSource() { Label = u.LocalName, Value = u.Id.ToString() });
                    });
                }
                else
                {
                    orgStructureParentInfoVM.userProfiles = GetAllUser();
                    if (orgStructureParentInfoVM.userProfiles != null && orgStructureParentInfoVM.userProfiles.Count > 0)
                    {
                        orgStructureParentInfoVM.userProfiles.ForEach(u =>
                        {
                            OrgUnitUsers.Add(new AutoCompleteDataSource() { Label = u.LocalName, Value = u.Id.ToString() });
                        });
                    }
                }
                ViewData["OrgUnitUsers"] = OrgUnitUsers;

                OrgStructureInfoAddVM orgStructureInfoAddVM = new OrgStructureInfoAddVM
                {
                    Key = departmentsStructure.Key,
                    Names = new List<LocalizationVM>()
                };
                orgStructureInfoAddVM.Names = localizationVMList;
                orgStructureInfoAddVM.Key = departmentsStructure.Key;
                orgStructureInfoAddVM.ParentId = departmentsStructure.ParentId;
                orgStructureInfoAddVM.ManagerId = departmentsStructure.ManagerId;
                orgStructureInfoAddVM.Number = departmentsStructure.Number;
                orgStructureInfoAddVM.TransactionsProcessingPeriod = departmentsStructure.TransactionsProcessingPeriod;
                orgStructureInfoAddVM.BarCode = departmentsStructure.BarCode;
                orgStructureInfoAddVM.IsVirtualUnit = departmentsStructure.IsVirtualUnit;

                orgUnitMode = OrgUnitMode.NotRoot;
                orgStructureInfoAddVM.IsRoot = false;
                if (departmentsStructure.ParentId == -1)
                {
                    orgUnitMode = OrgUnitMode.IsRoot;
                    orgStructureInfoAddVM.IsRoot = true;
                }
                ViewData["OrgUnitMode"] = (int)orgUnitMode;

                htmlView = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OrgUnitInfoAddPartial", orgStructureInfoAddVM);
                return Json(new { View = htmlView, ContainerHtml = containerHtml, Mode = (int)viewMode }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult AddNewOrgUnitInfo(int? Id)
        {
            GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
            ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

            OrgStructureInfoEditVM orgStructureInfoAddVM = new OrgStructureInfoEditVM();
            int parentId = (Id == null) ? 0 : Id.Value;
            orgStructureInfoAddVM.ParentId = parentId;
            orgStructureInfoAddVM.viewMode = "pointer-events: none;";
            List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups();

            ViewData["TransactionCategory"] = transactionCategoryVMs;
            return PartialView("_OrgUnitManagePartial", orgStructureInfoAddVM);
            //return View("_OrgUnitManagePartial");
        }

        #endregion

        private List<TransactionCategoryVM> GetTransactionSourceLookups()
        {
            GetResult<IList<LookupVM>> lookupVMs = LookupsHelper.GetAdminLookupItems(LookupCategory.TransactionCategories, SessionInfo.CultureShortName);
            List<TransactionCategoryVM> transactionSourceVMs = new List<TransactionCategoryVM>();

            if (lookupVMs != null)
            {
                foreach (LookupVM lookupVM in lookupVMs.Result)
                {
                    transactionSourceVMs.Add(new TransactionCategoryVM()
                    {
                        Id = (lookupVM.EnumReference != null) ? lookupVM.EnumReference.Value : -1,
                        Text = lookupVM.Text,
                    });
                }
            }
            return transactionSourceVMs;
        }
        private List<TransactionCategoryVM> MergeTransactionCategoryLookups(List<TransactionCategoryVM> transactionCategoryVMs)
        {
            List<TransactionCategoryVM> localizeTransactionCategoryVMs = GetTransactionSourceLookups();

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
        private void BuildBarCode(int key, List<OrgStructureInfoVM> orgStructureInfoVMs)
        {
            List<OrgStructureInfoVM> orgStructureInfoVMsCopy = orgStructureInfoVMs;

            orgStructureInfoVMs.Where(o => o.ParentId == key).ToList().ForEach(o =>
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                string barCode = o.Number.ToString();
                int parentId = o.ParentId;
                bool conter = false;

                if (o.Counter != null)
                {
                    conter = o.Counter.IsGeneral;
                }

                if (!conter)
                {
                    //o.BarCode = BarCode(barCode, parentId, orgStructureInfoVMsCopy);
                }

                BuildBarCode(o.Key, orgStructureInfoVMs);
            });
        }
        private void IsParentValid(List<OrgStructureInfoVM> orgStructureInfoVMs, int unitkey, int parent)
        {
            orgStructureInfoVMs.Where(o => o.ParentId == unitkey
                && !o.IsDeleted).ToList().ForEach(dep =>
                {
                    if (dep.Key == parent)
                    {
                        isParentValid = false;
                    }
                    else
                    {
                        IsParentValid(orgStructureInfoVMs, dep.Key, parent);
                    }
                });
        }
        public ActionResult GetYearTransactionsCount(int year, int orgUnit, bool isGeneralCounter)
        {
            GetResult<List<TransactionsCountDTO>> transactionsCount =
                      HttpClientWrapper<GetResult<List<TransactionsCountDTO>>>.GetItemRequest(string.Format("api/Admin/GetYearTransactionsCount?year={0}&orgUnit={1}&isGeneralCounter={2}", year, orgUnit, isGeneralCounter)).Result;

            return Json(new { data = TransactionsCountMapper.Map(transactionsCount.Result) }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetOrgUnitsUsedInTransaction(string orgUnitIds)
        {
            try
            {
                GetResult<List<int>> orgUnitsUsed =
                          HttpClientWrapper<GetResult<List<int>>>.GetItemRequest(string.Format("api/Admin/GetOrgUnitsUsedInTransaction?orgUnitIds={0}", orgUnitIds)).Result;

                return Json(new { OrgUnitsUsed = orgUnitsUsed.Result, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private OrgStructureInfoVM GetOrgStructureInfoByOrgUnitKey(int orgUnitKey, DialogMode dialogMode)
        {
            ViewData["OrgUnitKey"] = orgUnitKey;
            ViewData["DialogType"] = (int)dialogMode;
            string url = $"api/Admin/GetOrgUnitById?cultureName={SessionInfo.CultureShortName}&id={orgUnitKey}";
            GetResult<OrgStructureInfoDTO> orgStructureInfoDTO = HttpClientWrapper<GetResult<OrgStructureInfoDTO>>.GetItemRequest(url).Result;
            OrgStructureInfoVM orgStructureInfoVM = OrgStructureInfoMapper.Map(orgStructureInfoDTO.Result, SessionInfo.CultureShortName);
            return orgStructureInfoVM;
        }
        private OrgStructureInfoVM GetOrgUnitsGeneralCounter()
        {
            GetResult<OrgStructureInfoDTO> orgStructureInfoDTO = HttpClientWrapper<GetResult<OrgStructureInfoDTO>>.GetItemRequest("api/Admin/GetOrgUnitsGeneralCounter").Result;
            OrgStructureInfoVM orgStructureInfoVM = OrgStructureInfoMapper.Map(orgStructureInfoDTO.Result, SessionInfo.CultureShortName);
            return orgStructureInfoVM;
        }
        private List<UserProfileVM> GetUserByorgParentId(int id)
        {
            try
            {
                GetResult<List<UserProfileDTO>> userProfileDTOs =
                HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetUsersByOrgId?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, id)).Result;
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                List<UserProfileVM> userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);
                if (userProfileDTOs.Result != null)
                {
                    foreach (UserProfileVM userProfileVM in userProfileVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = userProfileVM.Id.ToString(),
                            Label = userProfileVM.LocalName
                        });
                    }
                }
                return userProfileVMs;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<UserProfileVM> GetAllUser()
        {
            try
            {
                GetResult<List<UserProfileDTO>> userProfileDTOs =
                HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetAllUsers?cultureName={0}", SessionInfo.CultureShortName)).Result;
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                List<UserProfileVM> userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);
                if (userProfileDTOs.Result != null)
                {
                    foreach (UserProfileVM userProfileVM in userProfileVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = userProfileVM.Id.ToString(),
                            Label = userProfileVM.LocalName
                        });
                    }
                }
                return userProfileVMs;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<OrgStructureInfoVM> GetOrgStructuresLight()
        {
            string url = $"api/Admin/GetOrgUnitsLight?cultureName={SessionInfo.CultureShortName}";
            var orgStructureInfoDTO = HttpClientWrapper<GetResult<List<OrgStructureInfoDTO>>>.GetItemRequest(url).Result;
            var result = OrgStructureInfoMapper.Map(orgStructureInfoDTO.Result, SessionInfo.CultureShortName);
            return result;
        }
        private List<OrgStructureInfoVM> GetOrgStructuresNew()
        {
            string url = $"api/Admin/GetOrgUnitsNew?cultureName={SessionInfo.CultureShortName}";
            var orgStructureInfoDTO = HttpClientWrapper<GetResult<List<OrgStructureInfoDTO>>>.GetItemRequest(url).Result;
            var result = OrgStructureInfoMapper.Map(orgStructureInfoDTO.Result, SessionInfo.CultureShortName);
            return result;
        }

        private List<OrgStructureInfoVM> GetAllUnitByLineage(string lineage)
        {
            string url = $"api/Admin/GetAllUnitByLineage?lineage={lineage}&cultureName={SessionInfo.CultureShortName}";
            var orgStructureInfoDTO = HttpClientWrapper<GetResult<List<OrgStructureInfoDTO>>>.GetItemRequest(url).Result;
            var result = OrgStructureInfoMapper.Map(orgStructureInfoDTO.Result, SessionInfo.CultureShortName);
            return result;
        }

        private List<OrgStructureInfoVM> GetOrgUnitsWithCounter()
        {
            string url = $"api/Admin/GetOrgUnitsWithCounter?cultureName={SessionInfo.CultureShortName}";
            var orgStructureInfoDTO = HttpClientWrapper<GetResult<List<OrgStructureInfoDTO>>>.GetItemRequest(url).Result;
            var result = OrgStructureInfoMapper.Map(orgStructureInfoDTO.Result, SessionInfo.CultureShortName);
            return result;
        }
        private List<OrgStructureInfoVM> GetOrgUnitsWithUser()
        {
            string url = $"api/Admin/GetOrgUnitsWithUser?cultureName={SessionInfo.CultureShortName}";
            var orgStructureInfoDTO = HttpClientWrapper<GetResult<List<OrgStructureInfoDTO>>>.GetItemRequest(url).Result;
            var result = OrgStructureInfoMapper.Map(orgStructureInfoDTO.Result, SessionInfo.CultureShortName);
            return result;
        }
        private List<OrgStructureInfoVM> GetOrgUnitsWithLinks()
        {
            string url = $"api/Admin/GetOrgUnitsWithLinks?cultureName={SessionInfo.CultureShortName}";
            var orgStructureInfoDTO = HttpClientWrapper<GetResult<List<OrgStructureInfoDTO>>>.GetItemRequest(url).Result;
            var result = OrgStructureInfoMapper.Map(orgStructureInfoDTO.Result, SessionInfo.CultureShortName);
            return result;
        }
        private TreeViewModel BuildGroupTree(List<GroupVM> groupVMs, string rootName)
        {
            TreeViewModel tree = new TreeViewModel();
            List<TreeNode> nodes = new List<TreeNode>();
            tree.RootNode = new TreeNode { Id = 0, Name = rootName };
            for (int i = 0; i < groupVMs.Count; i++)
            {
                TreeNode groupNode = new TreeNode()
                {
                    Id = groupVMs[i].Id,
                    ParentId = 0, // it is group, so, it is root with no parent 
                    Name = groupVMs[i].LocalName,
                    IsSelected = groupVMs[i].IsSelected
                };
                tree.RootNode.Childs.Add(groupNode);
            }
            nodes.Add(tree.RootNode);
            return tree;
        }

        [HttpGet]
        public ActionResult CheckOrgUnitNumber(string Number, int OrgUnitKey)
        {
            try
            {
                GetResult<bool> orgStructureInfoEditVM =
                   HttpClientWrapper<GetResult<bool>>.GetItemRequest(String.Format("api/Admin/CheckOrgUnitNumber?Number={0}&OrgUnitKey={1}", Number, OrgUnitKey)).Result;

                return Json(new { Exists = orgStructureInfoEditVM.Result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult CheckGeneralIOExist()
        {
            try
            {
                GetResult<string> GeneralIO =
                   HttpClientWrapper<GetResult<string>>.GetItemRequest(String.Format("api/Admin/getGeneralIoDepartment?cultureName={0}", SessionInfo.CultureShortName)).Result;
                 
                return Json(new { GeneralIOName = GeneralIO.Result }, JsonRequestBehavior.AllowGet);
                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}