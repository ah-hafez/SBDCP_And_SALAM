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
using MCS.UI.Areas.Admin.Models.Lookups;
using MCS.UI.Areas.User.Mappers.ExternalParties;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.ExternalParties;
using MCS.UI.Areas.Admin.Models;
using MCS.Common.Utility;
using CustomGrid = MCS.GridMvc.Ajax.GridExtensions;
namespace MCS.UI.Areas.Admin.Controllers
{
    public class JsTreeModel
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public bool children { get; set; }
    }

    public class ExternalPartiesController : AdminControllerBase
    {
        public ActionResult Index()
        {
            try
            {
                GetTreeData();
                //GetRoot();
                return View();

                //GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                //ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                //List<ExternalPartyListTypeVM> PartyTypes = GetExternalPartyListTypeLookups();

                //List<ManagerDTO> managers = new List<ManagerDTO>();

                //List<ManagerVM> managerVMs = ManagerMapper.Map(managers);
                //if (managerVMs == null)
                //{
                //    managerVMs = new List<ManagerVM>();
                //}

                //IAjaxGrid grid = (AjaxGrid<ManagerVM>)new AjaxGridFactory().CreateAjaxGrid(managerVMs.AsQueryable(), 1, false, managers.Count);

                //ViewData["GridData"] = grid;

                //ExternalPartyViewModel externalPartyViewModel = new ExternalPartyViewModel();

                //externalPartyViewModel.AddExternalParty.Types = PartyTypes;
                //externalPartyViewModel.EditExternalParty.Types = PartyTypes;

                //return View(externalPartyViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult AddExternalPartyInfo(int? parentId)
        {
            GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
           
            GetResult<string> maxNumber =
                   HttpClientWrapper<GetResult<string>>.GetItemRequest(String.Format("api/Common/GetLastNumber?parentId={0}", parentId)).Result;
             
            ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

            List<ExternalPartyListTypeVM> PartyTypes = GetExternalPartyListTypeLookups();

            List<ManagerDTO> managers = new List<ManagerDTO>();

            List<ManagerVM> managerVMs = ManagerMapper.Map(managers);
            if (managerVMs == null)
            {
                managerVMs = new List<ManagerVM>();
            }

            IAjaxGrid grid = (AjaxGrid<ManagerVM>)new AjaxGridFactory().CreateAjaxGrid(managerVMs.AsQueryable(), 1, false, managers.Count);

            ViewData["GridData"] = grid;
            ViewData["AddMode"] = true;
            ExternalPartyViewModel externalPartyViewModel = new ExternalPartyViewModel();

            externalPartyViewModel.AddExternalParty.Types = PartyTypes;
            externalPartyViewModel.AddExternalParty.ParentId = parentId;
            externalPartyViewModel.AddExternalParty.PartyNumber = maxNumber.Result;
            return PartialView("_ExternalPartiesAddPartial", externalPartyViewModel);//, externalPartyViewModel.AddExternalParty);
        }



        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddExternalParty(ExternalPartyViewModel externalPartyViewModel)
        {
            try
            {
                string message = string.Empty;
                externalPartyViewModel.AddExternalParty.Name[0].Text = externalPartyViewModel.AddExternalParty.NameAr;
                if (!externalPartyViewModel.AddExternalParty.NameEn.IsNullOrEmpty())
                {
                    externalPartyViewModel.AddExternalParty.Name[1].Text = externalPartyViewModel.AddExternalParty.NameEn;
                }
                else
                {
                    externalPartyViewModel.AddExternalParty.Name[1].Text = "NA";
                }
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Common/PostParty", ExternalPartyMapper.Map(externalPartyViewModel.AddExternalParty)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.ExternalParty.AddSucceeded");
                return Json(new { MessageText = message, MessageType = MessageType.Information, Id = postResult.Id }, JsonRequestBehavior.AllowGet);


                //JsTreeModel jsTreeModel = new JsTreeModel()
                //{
                //    text = externalPartyAddVM.Name.Where(n => n.CultureName == SessionInfo.CultureShortName).FirstOrDefault().Text,
                //    id = postResult.Id.ToString(),
                //    parent = externalPartyAddVM.ParentId.HasValue ? externalPartyAddVM.ParentId.Value.ToString() : "root"
                //};

                //return Json(new { MessageText = message, MessageType = MessageType.Information, partyID = postResult.Id, JsTree = jsTreeModel }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditExternalParty(ExternalPartyEditVM externalPartyEditVM)
        {
            try
            {
                externalPartyEditVM.Name[0].Text = externalPartyEditVM.NameAr;
                if (!externalPartyEditVM.NameEn.IsNullOrEmpty())
                {
                    externalPartyEditVM.Name[1].Text = externalPartyEditVM.NameEn;
                }
                else
                {
                    externalPartyEditVM.Name[1].Text = "NA";
                }

                string message = string.Empty;

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Common/PutParty", ExternalPartyMapper.Map(externalPartyEditVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error, }, JsonRequestBehavior.AllowGet);
                }
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.ExternalParty.UpdateSucceeded");

                return Json(new
                {
                    MessageText = message,
                    MessageType = MessageType.Information,
                    partyID = ExternalPartyMapper.Map(externalPartyEditVM).Id,
                    Id = ExternalPartyMapper.Map(externalPartyEditVM).Id,
                    PartyName = ExternalPartyMapper.Map(externalPartyEditVM).Name.Where(e => e.CultureName == SessionInfo.CultureShortName).FirstOrDefault().Text
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<ExternalPartyDTO> exteralPartyList = new List<ExternalPartyDTO>();

        [HttpPost]
        public ActionResult DeleteExternalParty(string ids)
        {
            try
            {
                string message = string.Empty;

                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/DeletePartites?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.ExternalParty.DeleteSucceeded");

                return Json(new { MessageText = message, MessageType = MessageType.Information, ExteralPartyUsedList = deleteResult.Result, ExteralPartyList = exteralPartyList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetPartyById(int id)
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                List<ExternalPartyListTypeVM> PartyTypes = GetExternalPartyListTypeLookups();

                GetResult<ExternalPartyEditDTO> partyEditDTO =
                   HttpClientWrapper<GetResult<ExternalPartyEditDTO>>.GetItemRequest(String.Format("api/Common/GetExternalParty?id={0}", id)).Result;

                ExternalPartyEditVM externalPartyListTypeVM = ExternalPartyMapper.Map(partyEditDTO.Result);

                externalPartyListTypeVM.NameAr = externalPartyListTypeVM.Name[0].Text;
                externalPartyListTypeVM.NameEn = externalPartyListTypeVM.Name[1].Text;

                externalPartyListTypeVM.Types = MergeExternalPartyListTypeLookups(externalPartyListTypeVM.Types);

                GetResult<List<ManagerDTO>> managerDTOs =
                     HttpClientWrapper<GetResult<List<ManagerDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalPartyManagers?PartyId={0}&PageIndex=1&PageSize={1}&cultureName={2}", id, GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
                if (managerDTOs.Result != null)
                {
                    for (int i = 0; i < managerDTOs.Result.Count; i++)
                    {
                        managerDTOs.Result[0].PartyId = id;
                    }
                }
                IAjaxGrid grid = (AjaxGrid<ManagerVM>)new AjaxGridFactory().CreateAjaxGrid(ManagerMapper.Map(managerDTOs.Result).AsQueryable(), 1, false, managerDTOs.RowsCount.Value);

                externalPartyListTypeVM.ManagersManagementViewModel = new ManagersManagementViewModel();
                externalPartyListTypeVM.ManagersManagementViewModel.AddManager.PartyId = id;
                // model.AddManager.PartyId = id;

                ViewData["GridData"] = grid;

                ViewData["AddMode"] = false;

                return PartialView("_ExternalPartiesEditPartial", externalPartyListTypeVM);

                //return Json(new
                //{
                //    EditHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ExternalPartiesEditPartial", externalPartyListTypeVM),
                //    ManagersHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "ManagersManagement", model),
                //}, JsonRequestBehavior.AllowGet
                //);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetExternalPartyChilds(int? id)
        {
            try
            {
                List<ExternalPartyDTO> parties = new List<ExternalPartyDTO>();
                List<JsTreeModel> items = new List<JsTreeModel>();
                string result = string.Empty;

                result = GetSearch(id);

                parties = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(String.Format("api/Common/GetExternalPartiesByParentId?parentId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result.Result;

                exteralPartyList = parties;

                //if (parties != null && parties.Count != 0)
                //{
                //    items = parties.Select(p => new JsTreeModel()
                //    {
                //        id = p.Id.ToString(),
                //        parent = p.ParentId.HasValue ? p.ParentId.Value.ToString() : "root",
                //        text = p.LocalName,
                //        children = p.HasChilds
                //    }).ToList();
                //}

                // return new JsonResult { Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                //return PartialView("_OrgUnitSubTreeViewPartial", nodes);


                List<TreeNode> nodes = new List<TreeNode>();
                if (parties != null && parties.Count != 0)
                {
                    nodes = parties.Select(o => new TreeNode()
                    {
                        Id = o.Id,
                        ParentId = o.ParentId.Value,
                        Name = o.LocalName,
                        HasChilds = o.HasChilds,
                        DepartmentNumber = o.Number.ToString()
                    }).ToList();
                }


                return PartialView("_ExternalPartiesSubTreeViewPartial", nodes);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetExternalPartyRoot()
        {
            try
            {
                JsTreeModel item = new JsTreeModel()
                {
                    id = "root",
                    parent = "#",
                    text = DbRes.TResource("Admin.ExternalParty.ExternalParty"),
                    children = true
                };

                return new JsonResult { Data = item, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult GetExternalSearchById(int id)
        {
             

            GetResult<ExternalPartyEditDTO> parties = HttpClientWrapper<GetResult<ExternalPartyEditDTO>>.GetItemRequest(String.Format("api/Common/GetExternalParty?id={0}", id)).Result;

            ExternalPartyEditVM externalPartyVMs = ExternalPartyMapper.Map(parties.Result);
            TreeNode treeNode = new TreeNode()
            {
                Id = externalPartyVMs.Id,
                ParentId = 0,
                Name = externalPartyVMs.Name[0].Text,
                HasChilds = false,
                DepartmentNumber = externalPartyVMs.PartyNumber.ToString()
            };
            if (externalPartyVMs != null)
            {
               

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ExternalPartiesChildsTreeViewPartial", treeNode), JsonRequestBehavior.AllowGet });
            }
            else
            {
                return Json(new { MessageText = DbRes.TResource("Admin.User.NotFound"), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetExternalParties(string str)
        {
            try
            {
                List<ExternalPartyDTO> parties = new List<ExternalPartyDTO>();
                List<JsTreeModel> items = new List<JsTreeModel>();
                string result = string.Empty;

                result = GetSearch(str);

                parties = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(String.Format("api/Common/GetExternalPartiesBySearchCriteria?{0}", result)).Result.Result;

                exteralPartyList = parties;
                ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(parties));
                JsTreeModel item = new JsTreeModel()
                {
                    id = "root",
                    parent = "#",
                    text = DbRes.TResource("Admin.ExternalParty.ExternalParty"),
                    children = true
                };

                items.Add(item);

                if (parties != null && parties.Count != 0)
                {
                    items.AddRange(parties.Select(p => new JsTreeModel()
                    {
                        id = p.Id.ToString(),
                        parent = p.ParentId.HasValue ? p.ParentId.Value.ToString() : "root",
                        text = p.LocalName,
                        children = p.HasChilds
                    }).ToList()
                );
                }

                return new JsonResult { Data = items, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddManager(ManagerAddVM managerAddVM)
        {
            try
            {
                string message = string.Empty;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Common/PostExternalPartyManager", ManagerMapper.Map(managerAddVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<ManagerDTO>> managerDTOs =
                     HttpClientWrapper<GetResult<List<ManagerDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalPartyManagers?PartyId={0}&PageIndex=1&PageSize={1}&cultureName={2}", ManagerMapper.Map(managerAddVM).PartyId, GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (managerDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, managerDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<ManagerVM>)new AjaxGridFactory().CreateAjaxGrid(ManagerMapper.Map(managerDTOs.Result).AsQueryable(), 1, false, managerDTOs.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.ExternalParty.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ManagersManagementGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditManager(ManagerEditVM managerEditVM)
        {
            try
            {
                string message = string.Empty;

                var putResult = HttpClientWrapper<PostResult>.PostRequest("api/Common/PutExternalPartyManager", ManagerMapper.Map(managerEditVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<ManagerDTO>> managerDTOs =
                 HttpClientWrapper<GetResult<List<ManagerDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalPartyManagers?PartyId={0}&PageIndex=1&PageSize={1}&cultureName={2}", ManagerMapper.Map(managerEditVM).PartyId, GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (managerDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, managerDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<ManagerVM>)new AjaxGridFactory().CreateAjaxGrid(ManagerMapper.Map(managerDTOs.Result).AsQueryable(), 1, false, managerDTOs.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.ExternalParty.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ManagersManagementGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteManager(string ids, string partyId)
        {
            try
            {
                string message = string.Empty;

                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/DeleteExternalPartyManagers?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<ManagerDTO>> managerDTOs =
                    HttpClientWrapper<GetResult<List<ManagerDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalPartyManagers?partyId={0}&PageIndex=1&PageSize={1}&cultureName={2}", partyId, GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (managerDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, managerDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<ManagerVM>)new AjaxGridFactory().CreateAjaxGrid(ManagerMapper.Map(managerDTOs.Result).AsQueryable(), 1, false, managerDTOs.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.ExternalParty.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ManagersManagementGridPartial", grid), MessageText = message, MessageType = MessageType.Information, ManagersUsedList = deleteResult.Result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetManager(string id)
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

                int externalPartyManagerId = 0;

                if (!string.IsNullOrEmpty(id))
                {
                    externalPartyManagerId = Convert.ToInt32(id);
                }

                GetResult<ManagerEditDTO> managerEditDTO =
                   HttpClientWrapper<GetResult<ManagerEditDTO>>.GetItemRequest(String.Format("api/Common/GetExternalPartyManagerById?externalPartyManagerId={0}", externalPartyManagerId)).Result;

                if (managerEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, managerEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.ExternalParty.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ManagersManagementEditPartial", ManagerMapper.Map(managerEditDTO.Result)), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateManagersGrid(int? page, string param)
        {
            try
            {
                string parameters = GridHelper.GetGridParameters();

                GetResult<List<ManagerDTO>> managerDTOs =
                   HttpClientWrapper<GetResult<List<ManagerDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalPartyManagers?PartyId={0}&{1}&cultureName={2}", param, parameters, SessionInfo.CultureShortName)).Result;

                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(ManagerMapper.Map(managerDTOs.Result).AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, managerDTOs.RowsCount.Value);

                return Json(new { Html = grid.ToJson("_ManagersManagementGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GenerateExternalPartyNumber(int? id)
        {
            try
            {
                List<ExternalPartyDTO> parties = new List<ExternalPartyDTO>();
                string result = string.Empty;
                string generatedNumber = string.Empty;
                string parentNumber = string.Empty;

                result = GetSearch(id);

                parties = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(String.Format("api/Common/GetExternalPartiesByParentId?parentId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result.Result;

                GetResult<ExternalPartyEditDTO> parentPartyDTO = null;
                if (id.HasValue)
                    parentPartyDTO = HttpClientWrapper<GetResult<ExternalPartyEditDTO>>.GetItemRequest(String.Format("api/Common/GetExternalParty?id={0}", id)).Result;

                int digitNumber = (parties != null && parties.Count > 0) ? parties.Count.ToString().Length : 0;
                parentNumber = (parentPartyDTO != null && parentPartyDTO.Result != null) ? parentPartyDTO.Result.PartyNumber : string.Empty;
                string partyNumber = (parties != null && parties.Count > 0) ? (parties.Count + 1).ToString() : "1";

                generatedNumber = string.Format("{0}0{1}", parentNumber, partyNumber);

                return new JsonResult { Data = generatedNumber, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult CheckPartyNumber(string Number, int partyId = -1)
        {
            try
            {
                GetResult<bool> partyEditDTO =
                   HttpClientWrapper<GetResult<bool>>.GetItemRequest(String.Format("api/Common/CheckPartyNumber?Number={0}&partyId={1}", Number, partyId)).Result;

                return Json(new { Exists = partyEditDTO.Result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<ExternalPartyListTypeVM> GetExternalPartyListTypeLookups()
        {
            GetResult<IList<LookupVM>> lookups = LookupsHelper.GetAdminLookupItems(LookupCategory.PartyType, SessionInfo.CultureShortName);
            List<ExternalPartyListTypeVM> partyTypeListVMs = new List<ExternalPartyListTypeVM>();

            foreach (LookupVM lookupVM in lookups.Result)
            {
                partyTypeListVMs.Add(new ExternalPartyListTypeVM()
                {
                    Id = (lookupVM.EnumReference != null) ? lookupVM.EnumReference.Value : -1,
                    Text = lookupVM.Text
                });
            }

            return partyTypeListVMs;
        }

        private List<ExternalPartyListTypeVM> MergeExternalPartyListTypeLookups(List<ExternalPartyListTypeVM> partyTypeListVMs)
        {
            List<ExternalPartyListTypeVM> localizePartyTypeListVMs = GetExternalPartyListTypeLookups();

            foreach (ExternalPartyListTypeVM PartyTypeListVM in partyTypeListVMs)
            {
                if (localizePartyTypeListVMs.Where(l => l.Id == PartyTypeListVM.Id &&
                    PartyTypeListVM.IsSelected == true).SingleOrDefault() != null)
                {
                    localizePartyTypeListVMs.Where(l => l.Id == PartyTypeListVM.Id &&
                        PartyTypeListVM.IsSelected == true).SingleOrDefault().IsSelected = true;
                }
            }

            return localizePartyTypeListVMs;
        }

        private ActionResult GetTreeData(int? id = null)
        {
            string result = string.Empty;
            List<TreeNode> nodes = new List<TreeNode>();
            List<ExternalPartyDTO> parties = new List<ExternalPartyDTO>();
            //result = GetSearch(id);
            string url = $"api/Common/GetExternalParties?parentId={id}&cultureName={SessionInfo.CultureShortName}&getVirtual=true";
            parties = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(String.Format(url)).Result.Result;

            exteralPartyList = parties;
            ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(parties));
            if (parties != null && parties.Count != 0)
            {
                nodes = parties.Select(p => new TreeNode()
                {
                    Id = p.Id,
                    ParentId = p.ParentId.HasValue ? p.ParentId.Value : 0,
                    Name = p.LocalName,
                    HasChilds = p.HasChilds
                }).ToList();
            }
            if (id != null && id != 0)
            {
                TreeNode node = nodes.Where(n => n.Id == id).FirstOrDefault();

                if (node != null)
                {
                    node.IsSelected = true;
                }
            }
            var tree = new TreeViewModel();

            tree.RootNode = new TreeNode { Id = 0, Name = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.ExternalParty.ExternalParty"), Mode = tree.Mode };

            BuildTree(tree, nodes);
            return PartialView("_ExternalPartiesRootTreeViewPartial", tree);
            //return tree;
        }

        private void BuildTree(TreeViewModel tree, List<TreeNode> nodes)
        {
            TreeNode parent;

            tree.Nodes = nodes.Select(t => new TreeNode { Id = t.Id, IsSelected = t.IsSelected, ParentId = t.ParentId, Name = t.Name, Mode = tree.Mode, HasChilds = t.HasChilds })
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

        private string GetSearch(int? id)
        {
            StringBuilder result = new StringBuilder();
            string filter = string.Empty;

            result.Append("CultureName=").Append(System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);

            string columnName = "ParentId";

            result.Append("&Filters[").Append(0).Append("].ColumnName=")
                  .Append(columnName).Append("&Filters[").Append(0)
                  .Append("].Type=").Append(FilterType.Equals).Append("&Filters[")
                  .Append(0).Append("].Value=").Append(id);

            return result.ToString();
        }

        private string GetSearch(string str)
        {
            StringBuilder result = new StringBuilder();
            string filter = string.Empty;

            result.Append("CultureName=").Append(System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);

            string columnName = "Name";

            result.Append("&Filters[").Append(0).Append("].ColumnName=")
                  .Append(columnName).Append("&Filters[").Append(0)
                  .Append("].Type=").Append(FilterType.Equals).Append("&Filters[")
                  .Append(0).Append("].Value=").Append(str);

            return result.ToString();
        }
    }
}