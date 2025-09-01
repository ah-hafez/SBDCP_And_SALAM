using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MCS.Framework.Localization;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Shared;
using MCS.UI.Areas.User.Mappers.Shared;
using static MCS.Common.Constants;
using MCS.UI.Areas.Admin.Models.Groups;
using MCS.Framework.Controls;
using Newtonsoft.Json;
using MCS.UI.Areas.Admin.Mappers;
using MCS.Framework.Controls.Mvc;
using MCS.UI.Areas.User;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.Admin.Controllers
{
    public class SystemSettingsController : AdminControllerBase
    {

        [HttpGet]
        public ActionResult GeneralSettings()
        {
            string message = string.Empty;
            string url = $"api/Setting/GetSettingByModelId?modelId={(int)SettingType.GeneralSettings}";

            Dictionary<string, SettingConfigVM> settingConfig = new Dictionary<string, SettingConfigVM>
            {
                {Constants.GeneralSettings.SupportEmail,new SettingConfigVM (){ Max = 200 , MaxLength = "200", ControlType = ControlType.Text, Regx = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
                    RegxMessage = DbRes.TValidation("Global.ExternalParty.InvalidEmail"), RequiredMessage = DbRes.TValidation("Admin.RequiredField")
                } },
                { Constants.GeneralSettings.NotifyEmployeeBeforeTaskExpiry, new SettingConfigVM (){ Min = 1, Max = 10 ,
                    RangeMessage = DbRes.TValidation("Global.Setting.ValueExceedTenDays"), RequiredMessage = DbRes.TValidation("Admin.RequiredField")} },
                {Constants.GeneralSettings.NotifyEmployeeBeforeFollowUpExpiry,new SettingConfigVM (){ Min = 1, Max = 10 ,
                    RangeMessage = DbRes.TValidation("Global.Setting.ValueExceedTenDays") ,RequiredMessage = DbRes.TValidation("Admin.RequiredField")} },
                { Constants.GeneralSettings.MaxOutboundNumberCanBooked, new SettingConfigVM (){ Min = 1, Max = 100 ,
                    RangeMessage = DbRes.TValidation("Global.Setting.ValueExceedOneHandredDays") ,RequiredMessage = DbRes.TValidation("Admin.RequiredField")} },
                {Constants.GeneralSettings.MaxInboundNumberCanBooked, new SettingConfigVM (){ Min = 1, Max = 100 ,
                    RangeMessage = DbRes.TValidation("Global.Setting.ValueExceedOneHandredDays") ,RequiredMessage = DbRes.TValidation("Admin.RequiredField")} },
                {Constants.GeneralSettings.MaxRequestSize , new SettingConfigVM (){ Min = 1, Max = 30 ,
                    RangeMessage = DbRes.TValidation("Global.Setting.MaximumRequestSize").Replace("#MinMB#","1").Replace("#MaxMB#", "30") ,RequiredMessage = DbRes.TValidation("Admin.RequiredField")} },
               
            };

            GetResult<List<SettingDTO>> settingDTOs = HttpClientWrapper<GetResult<List<SettingDTO>>>.GetItemRequest(url).Result;
            if (settingDTOs.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, SessionInfo.CultureShortName);
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            var settingVMList = SettingMapper.Map(settingDTOs.Result);
            foreach (var item in settingVMList)
            {
                if (settingConfig.TryGetValue(item.Key, out SettingConfigVM settingConfigVM))
                {
                    item.SettingConfigVM = settingConfigVM;
                }
            }
            GetResult<List<GroupDTO>> groups = HttpClientWrapper<GetResult<List<GroupDTO>>>
                  .GetItemRequest(string.Format("api/Admin/GetAllUserDefinedGroups?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

            List<GroupVM> GroupVMs = GroupMapper.Map(groups.Result).ToList();
            ViewData["Roles"] = GetGroupsAutoCompleteDataSource(GroupVMs);
            ViewData["AllActionsData"] = TransactionHelper.GetAllActions();

            return View("Settings", new SuperSettingVM { SubTitle = DbRes.TResource("Admin.GeneralSettings.Title"), SettingVMs = settingVMList });
        }

        private string GetGroupsAutoCompleteDataSource(List<GroupVM> groupVMs)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();


                if (groupVMs != null && groupVMs.Count() > 0)
                {
                    foreach (GroupVM groupVM in groupVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = groupVM.Id.ToString(),
                            Label = groupVM.LocalName
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

        [HttpGet]
        public ActionResult TraysSettings()
        {
            string message = string.Empty;
            string url = $"api/Setting/GetSettingByModelId?modelId={(int)SettingType.Tray}";

            Dictionary<string, SettingConfigVM> settingConfig = new Dictionary<string, SettingConfigVM>
            {
                { Constants.TraysSettings.InMyTransactionTray , new SettingConfigVM (){ Min = 1, Max = 50, RangeMessage = DbRes.TValidation("Tray.Setting.AllowedRange") ,RequiredMessage = DbRes.TValidation("Admin.RequiredField") } },
                { Constants.TraysSettings.InMyOrgUnitTray , new SettingConfigVM (){ Min = 1, Max = 50 , RangeMessage = DbRes.TValidation("Tray.Setting.AllowedRange") ,RequiredMessage = DbRes.TValidation("Admin.RequiredField")} },
                { Constants.TraysSettings.InExCopiesTray , new SettingConfigVM (){ Min = 1, Max = 50, RangeMessage = DbRes.TValidation("Tray.Setting.AllowedRange") ,RequiredMessage = DbRes.TValidation("Admin.RequiredField")} },
                { Constants.TraysSettings.InTasksTray  ,new SettingConfigVM (){ Min = 1, Max = 50, RangeMessage = DbRes.TValidation("Tray.Setting.AllowedRange") ,RequiredMessage = DbRes.TValidation("Admin.RequiredField")} },
                { Constants.TraysSettings.InCompleteTransactionsTray , new SettingConfigVM (){ Min = 1, Max = 50, RangeMessage = DbRes.TValidation("Tray.Setting.AllowedRange") ,RequiredMessage = DbRes.TValidation("Admin.RequiredField")} },
                { Constants.TraysSettings.InOutboundTray , new SettingConfigVM (){ Min = 1, Max = 50, RangeMessage = DbRes.TValidation("Tray.Setting.AllowedRange") ,RequiredMessage = DbRes.TValidation("Admin.RequiredField")} },
                { Constants.TraysSettings.InSentTray, new SettingConfigVM (){ Min = 1, Max = 50, RangeMessage = DbRes.TValidation("Tray.Setting.AllowedRange") ,RequiredMessage = DbRes.TValidation("Admin.RequiredField")} },
                { Constants.TraysSettings.InFollowUpTray , new SettingConfigVM (){ Min = 1, Max = 50, RangeMessage = DbRes.TValidation("Tray.Setting.AllowedRange") ,RequiredMessage = DbRes.TValidation("Admin.RequiredField")} },
                { Constants.TraysSettings.InCopiesTray , new SettingConfigVM (){ Min = 1, Max = 50, RangeMessage = DbRes.TValidation("Tray.Setting.AllowedRange") ,RequiredMessage = DbRes.TValidation("Admin.RequiredField")} },
                { Constants.TraysSettings.InManagerTray , new SettingConfigVM (){ Min = 1, Max = 50, RangeMessage = DbRes.TValidation("Tray.Setting.AllowedRange") ,RequiredMessage = DbRes.TValidation("Admin.RequiredField")} },
            };

            GetResult<List<SettingDTO>> settingDTOs = HttpClientWrapper<GetResult<List<SettingDTO>>>.GetItemRequest(url).Result;
            if (settingDTOs.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, SessionInfo.CultureShortName);
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            var settingVMList = SettingMapper.Map(settingDTOs.Result);
            foreach (var item in settingVMList)
            {
                if (settingConfig.TryGetValue(item.Key, out SettingConfigVM settingConfigVM))
                {
                    item.SettingConfigVM = settingConfigVM;
                }
            }
            return View("Settings", new SuperSettingVM { SubTitle = DbRes.TResource("Admin.TraySettings.Title"), SettingVMs = settingVMList });
        }
        [HttpGet]
        public ActionResult CounterSetting()
        {
            string message = string.Empty;
            string url = $"api/Setting/GetSettingByModelId?modelId={(int)SettingType.Counter}";

            Dictionary<string, SettingConfigVM> settingConfig = new Dictionary<string, SettingConfigVM>
            {
                {  Constants.CounterSetting.TheInitialValueOfOutboundNumber, new SettingConfigVM (){RequiredMessage = DbRes.TValidation("Admin.Setting.CounterInitialRequired.ar") } },
                {  Constants.CounterSetting.TheInitialValueOfOutboundDraftNumber, new SettingConfigVM (){RequiredMessage = DbRes.TValidation("Admin.Setting.CounterInitialRequired.ar")} },
                {  Constants.CounterSetting.TheInitialValueOfInboundNumber, new SettingConfigVM (){RequiredMessage = DbRes.TValidation("Admin.Setting.CounterInitialRequired.ar")} },
                {  Constants.CounterSetting.TheInitialValueOfInternalNumber, new SettingConfigVM (){RequiredMessage = DbRes.TValidation("Admin.Setting.CounterInitialRequired.ar")} },
            };

            GetResult<List<SettingDTO>> settingDTOs = HttpClientWrapper<GetResult<List<SettingDTO>>>.GetItemRequest(url).Result;
            if (settingDTOs.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, SessionInfo.CultureShortName);
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            var settingVMList = SettingMapper.Map(settingDTOs.Result);
            foreach (var item in settingVMList)
            {
                if (settingConfig.TryGetValue(item.Key, out SettingConfigVM settingConfigVM))
                {
                    item.SettingConfigVM = settingConfigVM;
                }
            }
            return View("Settings", new SuperSettingVM { SubTitle = DbRes.TResource("Admin.Setting.CounterSetting"), SettingVMs = settingVMList });
        }

        [HttpGet]
        public ActionResult DateAndNumbersSettings()
        {
            string message = string.Empty;
            string url = $"api/Setting/GetSettingByModelId?modelId={(int)SettingType.DateAndNumbers}";

            Dictionary<string, SettingConfigVM> settingConfig = new Dictionary<string, SettingConfigVM>
            {
                { Constants.DateAndNumbersSettings.DateType, new SettingConfigVM (){ LookupCategory = LookupCategory.DateType, ControlType = ControlType.Dropdown ,RequiredMessage = DbRes.TValidation("DateAndNumbers.Setting.DateTypeValidationMessage")} },
                { Constants.DateAndNumbersSettings.DateFormat, new SettingConfigVM (){ ControlType = ControlType.Text, IsRequired = false} },
                { Constants.DateAndNumbersSettings.NumberFormat, new SettingConfigVM (){ LookupCategory = LookupCategory.NumberFormat, ControlType = ControlType.Dropdown ,RequiredMessage = DbRes.TValidation("DateAndNumbers.Setting.NumberFormatValidationMessage")} },
            };

            GetResult<List<SettingDTO>> settingDTOs = HttpClientWrapper<GetResult<List<SettingDTO>>>.GetItemRequest(url).Result;
            if (settingDTOs.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, SessionInfo.CultureShortName);
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            var settingVMList = SettingMapper.Map(settingDTOs.Result);
            foreach (var item in settingVMList)
            {
                if (settingConfig.TryGetValue(item.Key, out SettingConfigVM settingConfigVM))
                {
                    item.SettingConfigVM = settingConfigVM;
                }
            }
            ViewData["DateType"] = GetLookupItemsForAutoComplete(LookupCategory.DateType);
            ViewData["NumberFormat"] = GetLookupItemsForAutoComplete(LookupCategory.NumberFormat);
            return View("Settings", new SuperSettingVM { SubTitle = DbRes.TResource("Admin.DateAndNumbersSettings.Title"), SettingVMs = settingVMList });
        }

        public static string GetLookupItemsForAutoComplete(LookupCategory lookupCategory)
        {
            try
            {
                GetResult<IList<LookupVM>> lookupVMs =LookupsHelper.GetLookupItems(lookupCategory, SessionInfo.CultureShortName);

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                if (lookupVMs.Result != null)
                {
                  
                    foreach (LookupVM lookupVM in lookupVMs.Result)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = lookupVM.Id.ToString(),
                            Label = lookupVM.Text
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
        public ActionResult SearchSettings()
        {
            string message = string.Empty;
            string url = $"api/Setting/GetSettingByModelId?modelId={(int)SettingType.Search}";

            Dictionary<string, SettingConfigVM> settingConfig = new Dictionary<string, SettingConfigVM>
            {
                //{ "MaximumNumberOfPagesThatWillAppearInSearchResult", new SettingConfigVM (){ Min = 1, Max = 9999, RangeMessage = DbRes.TValidation("Search.Setting.AllowedRangeForPages") ,RequiredMessage = DbRes.TValidation("Admin.RequiredField")} },
                { Constants.SearchSettings.MaximumNumber, new SettingConfigVM (){ Min = 1, Max = 50 , RangeMessage = DbRes.TValidation("Search.Setting.AllowedRangeForRecords") ,RequiredMessage = DbRes.TValidation("Admin.RequiredField")} },
            };

            GetResult<List<SettingDTO>> settingDTOs = HttpClientWrapper<GetResult<List<SettingDTO>>>.GetItemRequest(url).Result;
            if (settingDTOs.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, SessionInfo.CultureShortName);
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            var settingVMList = SettingMapper.Map(settingDTOs.Result);
            foreach (var item in settingVMList)
            {
                if (settingConfig.TryGetValue(item.Key, out SettingConfigVM settingConfigVM))
                {
                    item.SettingConfigVM = settingConfigVM;
                }
            }
            return View("Settings", new SuperSettingVM { SubTitle = DbRes.TResource("Admin.SearchSettings.Title"), SettingVMs = settingVMList });
        }
        [HttpGet]
        public ActionResult SMSSettings()
        {
            string message = string.Empty;
            string url = $"api/Setting/GetSettingByModelId?modelId={(int)SettingType.SMS}";

            Dictionary<string, SettingConfigVM> settingConfig = new Dictionary<string, SettingConfigVM>
            {
                {Constants.SMSSettings.SMSService, new SettingConfigVM (){ ControlType = ControlType.Checkbox,IsRequired = false } },
            };

            GetResult<List<SettingDTO>> settingDTOs = HttpClientWrapper<GetResult<List<SettingDTO>>>.GetItemRequest(url).Result;
            if (settingDTOs.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, SessionInfo.CultureShortName);
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            List<SettingVM> settingVMList = SettingMapper.Map(settingDTOs.Result.OrderBy(a => a.Id).ToList());
            foreach (var item in settingVMList)
            {
                if (settingConfig.TryGetValue(item.Key, out SettingConfigVM settingConfigVM))
                {
                    item.SettingConfigVM = settingConfigVM;
                }
            }
            return View("ServiceProviderSetting", new SuperSettingVM { SubTitle = DbRes.TResource("Admin.SMSSettings.Title"), SettingVMs = settingVMList });
        }
        [HttpGet]
        public ActionResult EmailSettings()
        {
            string message = string.Empty;
            string url = $"api/Setting/GetSettingByModelId?modelId={(int)SettingType.Email}";

            Dictionary<string, SettingConfigVM> settingConfig = new Dictionary<string, SettingConfigVM>
            {
                {Constants.EmailSettings.EmailService, new SettingConfigVM (){ ControlType = ControlType.Checkbox,IsRequired = false } },
            };

            GetResult<List<SettingDTO>> settingDTOs = HttpClientWrapper<GetResult<List<SettingDTO>>>.GetItemRequest(url).Result;
            if (settingDTOs.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, SessionInfo.CultureShortName);
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            List<SettingVM> settingVMList = SettingMapper.Map(settingDTOs.Result.OrderBy(a => a.Id).ToList());
            foreach (var item in settingVMList)
            {
                if (settingConfig.TryGetValue(item.Key, out SettingConfigVM settingConfigVM))
                {
                    item.SettingConfigVM = settingConfigVM;
                }
            }
            return View("ServiceProviderSetting", new SuperSettingVM { SubTitle = DbRes.TResource("Admin.EmailSettings.Title"), SettingVMs = settingVMList });
        }
        [HttpGet]
        public ActionResult SmartPhoneSettings()
        {
            string message = string.Empty;
            string url = $"api/Setting/GetSettingByModelId?modelId={(int)SettingType.SmartPhone}";

            Dictionary<string, SettingConfigVM> settingConfig = new Dictionary<string, SettingConfigVM>
            {
                {Constants.SmartPhoneSettings.SmartPhoneDomainURL, new SettingConfigVM (){Regx = "^((http|https):\\/\\/)?([a-zA-Z0-9]+(\\.[a-zA-Z0-9]+)+.*)$",
                    ControlType = ControlType.Text , RegxMessage = DbRes.TValidation("Settings.DomainURLValidationMessage"), RequiredMessage = DbRes.TValidation("Admin.RequiredField")} },
            };

            GetResult<List<SettingDTO>> settingDTOs = HttpClientWrapper<GetResult<List<SettingDTO>>>.GetItemRequest(url).Result;
            if (settingDTOs.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, SessionInfo.CultureShortName);
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            var settingVMList = SettingMapper.Map(settingDTOs.Result);
            foreach (var item in settingVMList)
            {
                if (settingConfig.TryGetValue(item.Key, out SettingConfigVM settingConfigVM))
                {
                    item.SettingConfigVM = settingConfigVM;
                }
            }
            return View("Settings", new SuperSettingVM { SubTitle = DbRes.TResource("Admin.SmartPhoneSettings.Title"), SettingVMs = settingVMList });
        }
        [HttpGet]
        public ActionResult AgencySettings()
        {
            string message = string.Empty;
            string url = $"api/Setting/GetSettingByModelId?modelId={(int)SettingType.Agency}";
            List<SettingVM> systemConfigList = SystemConfig();

            Dictionary<string, SettingConfigVM> settingConfig = new Dictionary<string, SettingConfigVM>
            {
                { Constants.AgencySettings.AgencyName, new SettingConfigVM (){IsRequired = false , ControlType = ControlType.Text } },
                { Constants.AgencySettings.AgencyNumber, new SettingConfigVM (){IsRequired = false , ControlType = ControlType.Text} },
                { Constants.AgencySettings.Logo, new SettingConfigVM (){IsRequired = false , ControlType = ControlType.ImageUpload} },
            };

            GetResult<List<SettingDTO>> settingDTOs = HttpClientWrapper<GetResult<List<SettingDTO>>>.GetItemRequest(url).Result;
            if (settingDTOs.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, SessionInfo.CultureShortName);
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            var settingVMList = SettingMapper.Map(settingDTOs.Result);
            foreach (var item in settingVMList)
            {
                if (settingConfig.TryGetValue(item.Key, out SettingConfigVM settingConfigVM))
                {
                    item.SettingConfigVM = settingConfigVM;
                }
                if (item.Key == SettingsConstants.Logo)
                {
                    item.SettingConfigVM.LogoHeight = systemConfigList.FirstOrDefault(sc => sc.Key == SettingsConstants.LogoHeight).Value;
                    item.SettingConfigVM.LogoWidth = systemConfigList.FirstOrDefault(sc => sc.Key == SettingsConstants.LogoWidth).Value;
                }
            }

            return View("Settings", new SuperSettingVM { SubTitle = DbRes.TResource("Admin.UnitSetting"), SettingVMs = settingVMList });
        }
        [HttpGet]
        public ActionResult VersionSettings()
        {
            string message = string.Empty;
            string url = $"api/Setting/GetSettingByModelId?modelId={(int)SettingType.Version}";

            Dictionary<string, SettingConfigVM> settingConfig = new Dictionary<string, SettingConfigVM>
            {
                { Constants.VersionSettings.VersionName , new SettingConfigVM () { ControlType = ControlType.Text  } },
                { Constants.VersionSettings.VersionNumber , new SettingConfigVM () { ControlType = ControlType.Text  } },
                { Constants.VersionSettings.VersionReleaseDate , new SettingConfigVM () { ControlType = ControlType.Text } },
                { Constants.VersionSettings.VersionComments , new SettingConfigVM () { ControlType = ControlType.Textarea } }
            };

            GetResult<List<SettingDTO>> settingDTOs = HttpClientWrapper<GetResult<List<SettingDTO>>>.GetItemRequest(url).Result;
            if (settingDTOs.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, SessionInfo.CultureShortName);
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            var settingVMList = SettingMapper.Map(settingDTOs.Result);
            foreach (var item in settingVMList)
            {
                if (settingConfig.TryGetValue(item.Key, out SettingConfigVM settingConfigVM))
                {
                    item.SettingConfigVM = settingConfigVM;
                }
            }
            return View("Settings", new SuperSettingVM
            {
                SubTitle = DbRes.TResource("Admin.VersionSetting.Title"),
                EnableSave = false,
                SettingVMs = settingVMList
            });
        }
        [HttpGet]
        public List<SettingVM> SystemConfig()
        {
            string message = string.Empty;
            #region systemConfig
            // to get system config (logoSize , logoHeight , logoWidth)
            string url = $"api/Setting/GetSettingByModelId?modelId={(int)SettingType.SystemConfiguration}";
            GetResult<List<SettingDTO>> SystemConfigDTO = HttpClientWrapper<GetResult<List<SettingDTO>>>.GetItemRequest(url).Result;
            if (SystemConfigDTO.StatusCode != StatusCode.Ok)
            {
                throw new Exception(ResourceHelper.GetResourceValue(ResourceSet.StatusCode, SessionInfo.CultureShortName));
            }
            List<SettingVM> SystemConfig = SettingMapper.Map(SystemConfigDTO.Result);
            #endregion
            return SystemConfig;
        }
        [HttpPost]
        public ActionResult UpdateServiceProviderSetting(SuperSettingVM superSettingVM)
        {
            string message = string.Empty;
            if (Convert.ToBoolean(superSettingVM.SettingVMs[0].Value))
            {
                //if (superSettingVM.SettingVMs[1].SettingConfigVM != null)
                //{
                //    superSettingVM.SettingVMs[1].Value = "false,true";//[0] => Wrapper / [1] => HTTP 
                //    if (superSettingVM.SettingVMs[1].SettingConfigVM.ConnectionProtocolType == ConnectionProtocolType.Wrapper)
                //    {
                //        superSettingVM.SettingVMs[1].Value = "true,false";
                //    }
                //}
            }
            else
            {
                var tempSetting = superSettingVM.SettingVMs[0];
                superSettingVM.SettingVMs = new List<SettingVM>();
                superSettingVM.SettingVMs.Add(tempSetting);
            }

            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Setting/UpdateSettings", SettingMapper.Map(superSettingVM.SettingVMs)).Result;
            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.UserCategory.UpdateSettingSucceeded") + " " + superSettingVM.SubTitle;
            return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateSetting(SuperSettingVM superSettingVM)
        {
            string message = string.Empty;
            SettingVM systemConfig = SystemConfig().FirstOrDefault(sc => sc.Key == SettingsConstants.LogoSize);
            foreach (var item in superSettingVM.SettingVMs)
            {
                if (item.BLOBValue != null)
                {
                    byte[] imgData = Convert.FromBase64String(item.BLOBValue);
                    float logoSize = (imgData.Length / 1024f);
                    float.TryParse(systemConfig.Value, out float allowedLogoSize);
                    if (allowedLogoSize != 0)
                    {
                        if (logoSize > allowedLogoSize)
                        {
                            message = DbRes.TValidation("Agency.Setting.LogoSizeValidation");
                            message = message.Replace("ToBeReplaced", (allowedLogoSize / 1024.0).ToString());
                            return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Setting/UpdateSettings", SettingMapper.Map(superSettingVM.SettingVMs)).Result;
            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.UserCategory.UpdateSettingSucceeded") + " " + superSettingVM.SubTitle;

            return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }

    }
}