using MobileApi.Common;
using MobileApi.Domain;
using MobileApi.Models;
using MobileApi.UtilityClasses;
using MobileAPIs.Wrappers;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Spire.Doc;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Resources;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml;
using MCS.Framework.Encryption;
using MCS.Framework.MultiTenants;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.Common.Utility;
using System.Web.Http.Results;
using System.Globalization;
using iTextSharp.text.pdf;
using System.Configuration;
using ZXing;
using MobileAPIs.UtilityClasses;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Core;

namespace MobileApi.Controllers
{
    [BasicAuthentication]
    public class DataController : ApiController
    {
        private static string LogFilePath = @"c://logs//logMobile.ada";
        private string sToken = string.Empty;
        public string Token
        {
            get
            {
                return sToken != string.Empty ? sToken : Request.Headers.Authorization.ToString();
            }
            set
            {
                sToken = value;
            }
        }

        private int nEntityId = 0;
        public int EntityId
        {
            get
            {
                return (Request.Headers.Contains("EntityId") && Request.Headers.GetValues("EntityId").First() != string.Empty ? int.Parse(Request.Headers.GetValues("EntityId").First()) : nEntityId);
            }
            set
            {
                nEntityId = value;
            }
        }
        /// <summary>
        /// This method will return status 200 if the service is reachable and the link is correct
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult CheckURL()
        {
            return Ok();
        }

        /// <summary>
        /// This method is to authenticate the logged in user to use
        /// the service APIs
        /// </summary>
        /// <param name="userName">The user name for the logged in user</param>
        /// <param name="password">The password for the logged in user</param>
        /// <param name="deviceToken">The token for the user device used</param>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The action result and the list of supported languages by the service</returns>
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Login(string userName, string password, string deviceToken, string languageAbbreviation)
        {
            try
            {
                DataResult result = new DataResult();
                int userId = -1; string userFullName = string.Empty;
                string userDefaultEntityName = string.Empty;
                int entityId = 0;
                var isUserValid = CheckUserLogin(userName, password, deviceToken, result, out userId, out userFullName, out userDefaultEntityName, out entityId, languageAbbreviation);

                if (!isUserValid)
                {
                    return Content(HttpStatusCode.Forbidden, result);
                }

                DateTime lastLoginDate = Utilities.FormatDateTimeNow();

                AuthenticationModule authentication = new AuthenticationModule();

                string token = authentication.GenerateTokenForUser(userName, userId);
               //string settings = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Settings.xml"));


                UserMobile userMobileDTO = new UserMobile()
                {
                    UserId = userId,
                    Token = token,
                    LastLoginDate = lastLoginDate
                };

                PutResult putResult = HttpClientWrapper<PutResult>
                                                      .PutRequest("api/MobileApi/UpdateUserMobile", languageAbbreviation, userMobileDTO, Token)
                                                      .Result;

                //provide the list of supported languages, initially it is hardcoded and to be stored in a database table later on 
                IList<Langauge> supportedLanguages = new List<Langauge>
                {

                    //add arabic language
                    new Langauge() { Code = 1, Name = "ar" },

                    //add english language
                    new Langauge() { Code = 2, Name = "en" }
                };

                return Ok(new { token, supportedLanguages, userId, userFullName, userDefaultEntityName, entityId });
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to logout the logged in user and end the user session  
        /// </summary>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The action result</returns>
        [HttpPost]
        public IHttpActionResult Logout(string languageAbbreviation)
        {
            try
            {
                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult, true))
                {
                    return iHttpActionResult;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to check if there is an update happened on the user info, settings or org. structure
        /// </summary>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>If information updated or not</returns>
        [HttpGet]
        public IHttpActionResult CheckForUpdate(string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                UpdateInfo updateInfo = new UpdateInfo();

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;

                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result.IsUpdated.ToString() != null)
                {
                    updateInfo.IsUpdated = Convert.ToBoolean(getResultUserMobile.Result.IsUpdated);
                }

                if (getResultUserMobile.Result.UpdateFlags.ToString() != null)
                {
                    enUpdateFlags updateFlags = (enUpdateFlags)Enum.Parse(typeof(enUpdateFlags), getResultUserMobile.Result.UpdateFlags.ToString());

                    updateInfo.SettingsUpdated = (updateFlags & enUpdateFlags.SettingsUpdated) == enUpdateFlags.SettingsUpdated;
                    updateInfo.RevocationNeeded = (updateFlags & enUpdateFlags.RevocationNeeded) == enUpdateFlags.RevocationNeeded;
                    updateInfo.OrgChartUpdated = (updateFlags & enUpdateFlags.OrgChartUpdated) == enUpdateFlags.OrgChartUpdated;
                    updateInfo.ResourcesUpdated = (updateFlags & enUpdateFlags.ResourcesUpdated) == enUpdateFlags.ResourcesUpdated;
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(updateInfo);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to get the logged in user settings
        /// </summary>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The settings for the logged in user</returns>
        [HttpGet]
        public IHttpActionResult GetSettings(string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;

                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                 .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                 .Result;

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(getResultUserMobile.Result.Settings);

                List<Setting> settings = new List<Setting>();
                XmlNodeList xnList = doc.SelectNodes("/SETTINGS/SETTING");
                foreach (XmlNode xn in xnList)
                {
                    settings.Add(new Setting()
                    {
                        Key = xn["KEY"].InnerText,
                        Value = xn["VALUE"].InnerText
                    });
                }


                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(settings);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to get the org. structure/hierarchy
        /// </summary>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>the org. structure/hierarchy</returns>
        [HttpGet]
        public IHttpActionResult GetOrgHierarchy(int? parentId, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                var orgUnitDTOs = HttpClientWrapper<GetResult<List<UserMobileOrgUnitDTO>>>
                                                  .GetItemRequest($"api/MobileApi/UserMobileGetOrgHierarchy?parentId={parentId}", languageAbbreviation, Token)
                                                  .Result;
                List<Entity> entites = new List<Entity>();
                if (orgUnitDTOs.Result != null)
                {
                    foreach (UserMobileOrgUnitDTO UserMobileOrgUnitDTO in orgUnitDTOs.Result)
                    {
                        entites.Add(MapEntity(UserMobileOrgUnitDTO));

                    }
                }
                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                OrgHierarchy orgHierarchy = new OrgHierarchy();
                orgHierarchy.Entities = entites;
                return Ok(orgHierarchy);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to get the org. structure/hierarchy
        /// </summary>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>the org. structure/hierarchy</returns>
        [HttpGet]
        public IHttpActionResult GetOrgHierarchyAutoComplete(string searchQuery, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                var orgUnitDTOs = HttpClientWrapper<GetResult<List<UserMobileOrgUnitDTO>>>
                                                  .GetItemRequest($"api/MobileApi/UserMobileGetOrgHierarchyAC?searchQuery={searchQuery}", languageAbbreviation, Token)
                                                  .Result;

                List<Entity> entites = new List<Entity>();
                if (orgUnitDTOs.Result != null)
                {
                    foreach (UserMobileOrgUnitDTO UserMobileOrgUnitDTO in orgUnitDTOs.Result)
                    {
                        entites.Add(MapEntity(UserMobileOrgUnitDTO));

                    }
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                OrgHierarchy orgHierarchy = new OrgHierarchy();
                orgHierarchy.Entities = entites;
                return Ok(orgHierarchy);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        [HttpGet]
        public IHttpActionResult GetExternalHierarchy(int? parentId, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                var externalParties = HttpClientWrapper<GetResult<List<UserMobileExternalPartyDTO>>>
                                                  .GetItemRequest($"api/MobileApi/UserMobileGetExternalOrgHierarchy?parentId={parentId}", languageAbbreviation, Token)
                                                  .Result;

                List<Entity> entites = new List<Entity>();
                if (externalParties.Result != null)
                {
                    foreach (UserMobileExternalPartyDTO userMobileExternalPartyDTO in externalParties.Result)
                    {
                        entites.Add(MapExternalParty(userMobileExternalPartyDTO));

                    }
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(entites ?? new List<Entity>());
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        [HttpGet]
        public IHttpActionResult GetExternalHierarchyAutoComplete(string searchQuery, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                var externalParties = HttpClientWrapper<GetResult<List<UserMobileExternalPartyDTO>>>
                                                  .GetItemRequest($"api/MobileApi/UserMobileGetExternalOrgHierarchyAC?searchQuery={searchQuery}", languageAbbreviation, Token)
                                                  .Result;
                List<Entity> entites = new List<Entity>();
                if (externalParties.Result != null)
                {
                    foreach (UserMobileExternalPartyDTO userMobileExternalPartyDTO in externalParties.Result)
                    {
                        entites.Add(MapExternalParty(userMobileExternalPartyDTO));

                    }
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }
                OrgHierarchy orgHierarchy = new OrgHierarchy();
                orgHierarchy.externalEntities = entites;
                return Ok(orgHierarchy);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to get the messages resource file
        /// </summary>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The messages resource file</returns>
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetResourceFile(string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();


                var list = new List<MobileResource>();
                var path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_GlobalResources/Messages.resx");

                using (ResXResourceReader resxReader = new ResXResourceReader(path))
                {
                    foreach (DictionaryEntry entry in resxReader)
                    {
                        if (entry.Key.ToString().ToLower().EndsWith("_" + languageAbbreviation.ToLower()))
                        {
                            list.Add(new MobileResource()
                            {
                                ResourceCode = entry.Key.ToString().Substring(0, entry.Key.ToString().Length - 3),
                                ResourceValue = entry.Value.ToString()
                            });
                        }
                    }
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to get the logged in user information
        /// </summary>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The logged in user info</returns>
        [HttpGet]
        public IHttpActionResult GetUserInfo(string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;

                UserInfo userInfo = new UserInfo();



                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result.ActivationRequestCode != null)
                {
                    userInfo.ActivationRequestCode = getResultUserMobile.Result.ActivationRequestCode.ToString();
                }

                if (getResultUserMobile.Result.ActivataionCode != null)
                {
                    userInfo.ActivationCode = getResultUserMobile.Result.ActivataionCode.ToString();
                }

                if (getResultUserMobile.Result.DeactivationRequestCode != null)
                {
                    userInfo.DeactivationRequestCode = getResultUserMobile.Result.DeactivationRequestCode.ToString();
                }

                if (getResultUserMobile.Result.SignedCert != null)
                {
                    userInfo.SignedCert = getResultUserMobile.Result.SignedCert.ToString();
                }

                if (getResultUserMobile.Result.CA != null)
                {
                    userInfo.CACert = getResultUserMobile.Result.CA.ToString();
                }

                if (getResultUserMobile.Result.CACRL != null)
                {
                    userInfo.CACRL = getResultUserMobile.Result.CACRL.ToString();
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to get the logged in user trays, permissions, confidentialities and transaction sources
        /// </summary>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>UserAuthorization object contains user trays, permissions, confidentialities and transaction sources</returns>
        [HttpGet]
        public IHttpActionResult GetAuthenticatedItems(string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;



                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                int entityId = getResultUserMobile.Result.EntityId;

                string culture = languageAbbreviation;

                GetResult<UserAuthorization> getResulUserAuthorization = HttpClientWrapper<GetResult<UserAuthorization>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserAuthorization?userId={0}&userName={1}&entityId={2}", userId, userName, entityId), languageAbbreviation, Token)
                                                  .Result;

                UserAuthorization userAuthorization = new UserAuthorization();
                //UserAuthorization userAuthorization = new UserAuthorization()
                //{
                //    TransactionConfidentialities = new WebApiService().GetConfidentialities(userId, culture, TransCategoryEnum.All),
                //    TransactionSources = new WebApiService().GetTransactionSources(userId, culture, TransCategoryEnum.All),
                //    TransactionPriorities = new WebApiService().GetPriorities(userId, culture, TransCategoryEnum.All),
                //    TransactionTypes = new WebApiService().GetTransactionType(userId, culture, TransCategoryEnum.All),
                //    AttachmentTypes = new WebApiService().GetAttachmentType(userId, culture, TransCategoryEnum.All),
                //    IncludedItemTypes = new WebApiService().GetIncludedItemType(userId, culture, TransCategoryEnum.All),
                //    Trays = new WebApiService().GetUserTrays(userId, entityId, culture),
                //    Permissions = new WebApiService().GetUserMobilePermissions(userId, userName),
                //    TransCategories = new WebApiService().GetTransCategories(),
                //    Processes = new WebApiService().GetProcesses(userId, culture, TransCategoryEnum.All),
                //    TransactionPartyDirection = new WebApiService().GetTransPartyDirection(),
                //    RowStatus = new WebApiService().GetRowStatus(),
                //    AttachmentMethods = new WebApiService().GetAttachmentMethods(),
                //    ArchivingTypes = new WebApiService().GetArchivingTypes(),
                //    AttachConfidentialities = new WebApiService().GetAttachConfidentialities(userId, culture, TransCategoryEnum.All)
                //};

                List<int> normalUserExecludedTrayIds = new List<int> { 8, 11, 99, 100 };
                List<int> vipUserIncludedTrayIds = new List<int> { 1, 2, 10 };
                List<Tray> trayList = new List<Tray>();

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                if (userName.Contains("معالي"))
                {
                    trayList = getResulUserAuthorization.Result.Trays.Where(d => vipUserIncludedTrayIds.Contains(d.TrayId)).ToList();
                }
                else
                {
                    trayList = getResulUserAuthorization.Result.Trays.Where(d => !normalUserExecludedTrayIds.Contains(d.TrayId)).ToList();
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(getResulUserAuthorization.Result ?? new UserAuthorization());
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to get the logged in user trays in order to refresh the mobile app trays and counters
        /// </summary>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>UserAuthorization object contains user trays</returns>
        [HttpGet]
        public IHttpActionResult GetTrays(string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;



                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                int entityId = getResultUserMobile.Result.EntityId;

                string culture = languageAbbreviation;

                GetResult<List<Tray>> trays = HttpClientWrapper<GetResult<List<Tray>>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserTrays?userId={0}&entityId={1}", userId, entityId), languageAbbreviation, Token)
                                                  .Result;

                IList<string> TrayIds = new List<string>();
                List<Tray> trayList = new List<Tray>();
                if (getResultUserMobile.Result.UserMobileClass == UserMobileClass.VipUser)
                {
                    TrayIds = ConfigurationManager.AppSettings["VipUserTrayIds"].Split(',');
                }
                else if (getResultUserMobile.Result.UserMobileClass == UserMobileClass.ReporterUser)
                {
                    TrayIds = ConfigurationManager.AppSettings["ReporterUserTrayIds"].Split(',');
                }
                else
                {
                    TrayIds = ConfigurationManager.AppSettings["NormalUserTrayIds"].Split(',');
                }
                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }


                return Ok(trayList);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This methid is to get the transactions related to the passed tray id
        /// </summary>
        /// <param name="trayId">The tray id to get the transactions for</param>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The list of transaction for the passed tray id</returns>
        [HttpGet]
        public IHttpActionResult GetTrayTransactions([FromUri] int trayId, bool isAscending, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;




                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                GetResult<List<Domain.Transaction>> getResultTrayTransaction = HttpClientWrapper<GetResult<List<Domain.Transaction>>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetTrayTransactions?userId={0}&entityId={1}&trayId={2}&isAscending={3}", userId, entityId, trayId, isAscending), languageAbbreviation, Token)
                                                  .Result;

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(getResultTrayTransaction.Result);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This methid is to get the transactions related to the passed tray id
        /// </summary>
        /// <param name="trayId">The tray id to get the transactions for</param>
        /// <param name="criteria">The criteria which the tray will be filtered based on</param>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The list of transaction for the passed tray id</returns>
        [HttpPost]
        public IHttpActionResult FilterTrayTransactions([FromUri] int trayId, FilterCriteria criteria, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;




                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                //DataSet dsCorespondences = new WebApiService().GetUserMobileTrayTransactions(userId, entityId, (languageAbbreviation), trayId);

                //DataTable dtCorespondences = dsCorespondences.Tables[0];


                IList<Models.Transaction> transactions = new List<Models.Transaction>();
                Models.Transaction transaction = null;


                //for (int i = 0; i < dtCorespondences.Rows.Count; i++)
                //{
                //    transaction = new Models.Transaction();

                //    transaction.TransID = Convert.ToInt32(dtCorespondences.Rows[i]["TRANS_ID"].ToString());
                //    transaction.TransNo = dtCorespondences.Rows[i]["TRANS_NO"].ToString();
                //    transaction.TransCategory = Convert.ToInt32(dtCorespondences.Rows[i]["TRANS_CATEGORY"].ToString());
                //    transaction.TransTitle = HttpUtility.HtmlEncode(dtCorespondences.Rows[i]["SUBJECT"].ToString());

                //    if (dtCorespondences.Columns.Contains("LOCALE_VALUE") && !string.IsNullOrEmpty(dtCorespondences.Rows[i]["LOCALE_VALUE"].ToString()))
                //    {
                //        transaction.TransFrom = dtCorespondences.Rows[i]["LOCALE_VALUE"].ToString();
                //        transaction.ReadOnly = false;
                //    }
                //    else if (dtCorespondences.Columns.Contains("CONCERNED_ENTITY_NAME") && !string.IsNullOrEmpty(dtCorespondences.Rows[i]["CONCERNED_ENTITY_NAME"].ToString()))
                //    {
                //        transaction.TransFrom = dtCorespondences.Rows[i]["CONCERNED_ENTITY_NAME"].ToString();
                //        transaction.ReadOnly = false;
                //    }
                //    else if (dtCorespondences.Columns.Contains("PREV_ELC_ASSIGNTO_ENTITY_ID") && !string.IsNullOrEmpty(dtCorespondences.Rows[i]["PREV_ELC_ASSIGNTO_ENTITY_ID"].ToString()))
                //    {
                //        transaction.TransFrom = dtCorespondences.Rows[i]["PREV_ELC_ASSIGNTO_ENTITY_ID"].ToString();
                //        transaction.ReadOnly = false;
                //    }
                //    else
                //    {
                //        transaction.TransID = Convert.ToInt32(dtCorespondences.Rows[i]["TRANS_PARTY_ID"].ToString());
                //        transaction.TransFrom = dtCorespondences.Rows[i]["SOURCE_ENTITY_NAME"].ToString();
                //        transaction.ReadOnly = true;
                //    }

                //    if (!string.IsNullOrEmpty(dtCorespondences.Rows[i]["TRANS_DATE_HJ"].ToString()))
                //    {
                //        transaction.TransDate = Utilities.GetDateFormatString(dtCorespondences.Rows[i]["TRANS_DATE_HJ"].ToString(), true, true);
                //    }

                //    transaction.Has_Supporting_Attachments = false;//to do:get from DB.
                //    transaction.FileSize = "";//to do:get from DB.

                //    transactions.Add(transaction);
                //}
                //GetResult<List<Domain.Transaction>> getResultTrayTransaction = HttpClientWrapper<GetResult<List<Domain.Transaction>>>
                //                               .GetItemRequest(string.Format("api/MobileApi/FilterTrayTransactions?userId={0}&entityId={1}&trayId={2}&filterCriteria={3}", userId, entityId, trayId , criteria), languageAbbreviation, Token)
                //                               .Result;

                PostResult postResult = HttpClientWrapper<PostResult>
                                                  .PostRequest(string.Format("api/MobileApi/FilterTrayTransactions?userId={0}&entityId={1}&trayId={2}", userId, entityId, trayId), criteria, languageAbbreviation, Token)
                                                  .Result;

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }


                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(postResult.Result);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to get the transaction data for the passed transaction id
        /// </summary>
        /// <param name="transId">The transaction id to get the data for</param>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The transaction data for the transaction passed id</returns>
        [HttpGet]
        public IHttpActionResult GetTransaction([FromUri] int transId, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();


                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;

                TransData transData = new TransData();
                int entityId = -1;

                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                string ip = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostAddress);
                string systemAddress = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostName);


                GetResult<TransData> getResultTransData = HttpClientWrapper<GetResult<TransData>>
                                                                 .GetItemRequest(string.Format("api/MobileApi/GetTransaction?userId={0}&transId={1}", userId, transId), languageAbbreviation, Token)
                                                                 .Result;
                //      getResultTransData.Result.Names = null;
                var hijriSymbol = MessageResources.GetResourceText(ResourceText.HijriSymbol, languageAbbreviation);
                getResultTransData.Result.TransDateHJ = getResultTransData.Result.TransDateHJ;
                getResultTransData.Result.FormattedTransDate = getResultTransData.Result.TransDateHJ;
                
                foreach (var archiveRecord in getResultTransData.Result.archiveRecords.Where(x => x.MimeContent == System.Net.Mime.MediaTypeNames.Application.Octet))
                {
                    archiveRecord.MimeContent = System.Net.Mime.MediaTypeNames.Application.Pdf;
                } 
                if (!string.IsNullOrEmpty(getResultTransData.Result.PriorityDateHJ))
                {
                    getResultTransData.Result.PriorityDateHJ = getResultTransData.Result.PriorityDateHJ;
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(getResultTransData.Result);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to get the document for the passed document id
        /// </summary>
        /// <param name="docId">The document id to get</param>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The document for the passed id</returns>
        [HttpGet]
        public IHttpActionResult GetDocument([FromUri] string docId, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;



                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                string ip = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostAddress);
                string systemAddress = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostName);

                GetResult<DocumentDTO> getResultDocument = HttpClientWrapper<GetResult<DocumentDTO>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetDocumentById?documentId={0}", docId), languageAbbreviation, Token)
                                                  .Result;

                DocumentDTO convertedDocument = ConvertToPDF(getResultDocument.Result, userName);

                DataResult result = new DataResult();

                if (convertedDocument == null || convertedDocument.Content == null)
                {
                    result.Code = MessageCode.DataNotReturned;
                    result.Description = MessageResources.GetResourceText(ResourceText.CorrespondenceUpdatedUnsuccessfully, languageAbbreviation);

                    return Content(HttpStatusCode.InternalServerError, result);
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                if (convertedDocument != null)
                {
                    var resultData = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(convertedDocument.Content)
                    };

                    resultData.Content.Headers.ContentDisposition =
                        new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                        {
                            FileName = convertedDocument.Name
                        };

                    resultData.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    var response = ResponseMessage(resultData);

                    return response;
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to assign the transaction for the logged in user
        /// </summary>
        /// <param name="transId">The transaction id to assign</param>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The transaction assigned</returns>
        [HttpGet]
        public IHttpActionResult TransAssignTrack([FromUri] int transId, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;



                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                string ip = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostAddress);
                string systemAddress = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostName);

                GetResult<TransAssignTrack> transAssignTrack = HttpClientWrapper<GetResult<TransAssignTrack>>
                                                  .GetItemRequest($"api/MobileApi/AssignmentTrack?transId={transId}&userId={userId}&entityId={entityId}", languageAbbreviation, Token)
                                                  .Result;
                DataResult result = new DataResult();

                if (transAssignTrack.Result == null)
                {
                    result.Code = MessageCode.CorrespondenceUpdatedUnsuccessfully;
                    result.Description = MessageResources.GetResourceText(ResourceText.CorrespondenceUpdatedUnsuccessfully, languageAbbreviation);

                    return Content(HttpStatusCode.BadRequest, result);
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(transAssignTrack.Result);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to create transaction using the passed transaction data
        /// </summary>
        /// <param name="transData">The transaction data</param>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The created transaction data</returns>
        [HttpPost]
        public IHttpActionResult CreateTransaction([FromBody] TransData transData, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;

                TransResult transResult = new TransResult();

                int entityId = -1;



                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                string ip = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostAddress);
                string systemAddress = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostName);

                transData.UserId = userId;
                transData.EntityId = entityId;

                //transData.TransDateHJ = Utilities.GetDateString(Utilities.GetDateHijriString(DateConverter.ConvertGregToHijri(DateTime.Now), true), DateTime.Now);

                if (transData.PriorityDate != null)
                {
                    transData.PriorityDateHJ = DateTimeUtility.ConvertToUmAlQuraCalendar(transData.PriorityDate.Value);
                }


                PostObjectResult<TransactionDetailsDTO> postObjectResult = HttpClientWrapper<PostObjectResult<TransactionDetailsDTO>>
                                               .PostRequest($"api/MobileApi/CreateTransaction?userId={userId}", transData, languageAbbreviation, Token)
                                               .Result;


                GetResult<TransData> getResultTransData = HttpClientWrapper<GetResult<TransData>>
                                                                 .GetItemRequest(string.Format("api/MobileApi/GetTransaction?transId={0}&userId={1}", postObjectResult.Result.Id, userId), languageAbbreviation, Token)
                                                                 .Result;

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(getResultTransData.Result);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to update a transaction data
        /// </summary>
        /// <param name="transData">The updated transaction data</param>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The updated transaction data</returns>
        [HttpPost]
        public IHttpActionResult UpdateTransaction([FromBody] TransData transData, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;



                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                string ip = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostAddress);
                string systemAddress = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostName);

                if (transData.PriorityDate != null)
                {
                    transData.PriorityDateHJ = DateTimeUtility.ConvertToUmAlQuraCalendar(transData.PriorityDate.Value);
                }

                PostResult postResult = HttpClientWrapper<PostResult>
                                               .PostRequest($"api/MobileApi/UpdateTransaction?userId={userId}&EntityId={entityId}&cultureName={languageAbbreviation}", transData, languageAbbreviation, Token)
                                               .Result;

                GetResult<TransData> getResultTransData = HttpClientWrapper<GetResult<TransData>>
                                                                 .GetItemRequest(string.Format("api/MobileApi/GetTransaction?userId={0}&transId={1}", userId, transData.TransId), languageAbbreviation, Token)
                                                                 .Result;

                getResultTransData.Result.TransDateHJ += " " + MessageResources.GetResourceText(ResourceText.HijriSymbol, languageAbbreviation);
                getResultTransData.Result.FormattedTransDate = getResultTransData.Result.TransDateHJ;
                if (!string.IsNullOrEmpty(getResultTransData.Result.PriorityDateHJ))
                {
                    getResultTransData.Result.PriorityDateHJ += " " + MessageResources.GetResourceText(ResourceText.HijriSymbol, languageAbbreviation);
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(getResultTransData.Result);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to Assign a Copy 
        /// </summary>
        /// <param name="TransCopies">The Assign Copy transaction</param>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The Assign Copy transaction</returns>
        [HttpPost]
        public IHttpActionResult AssignmentCopies([FromBody] List<TransPartiy> TransCopies, int transId,  string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();


                if(TransCopies.Count == 0)
                {

                 return   Content(HttpStatusCode.BadRequest, "TransCopies is Empty");
                }
                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;



                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                string ip = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostAddress);
                string systemAddress = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostName);
                 

                PostResult postResult = HttpClientWrapper<PostResult>
                                               .PostRequest($"api/MobileApi/AddAssignmentCopies?transactionId={transId}&userId={userId}&EntityId={entityId}", TransCopies, languageAbbreviation, Token)
                                               .Result;
                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        [HttpPost]
        public IHttpActionResult SetDefaultEntity(int defaultEntityId, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;



                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                string ip = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostAddress);
                string systemAddress = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostName);



                PostResult postResult = HttpClientWrapper<PostResult>
                                               .PostRequest($"api/MobileApi/SetDefaultEntity?userId={userId}&entityId={defaultEntityId}", null, Token)
                                               .Result;

                if (postResult.StatusCode != MCS.Common.StatusCode.Ok)
                {
                    DataResult result = new DataResult
                    {
                        Code = MessageCode.CorrespondenceIdNotValid,
                        Description = MessageResources.GetResourceText(ResourceText.FaildToSetDefaultEntity, languageAbbreviation)
                    };

                    return Content(HttpStatusCode.BadRequest, result);
                }


                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to search for transactions by passing a search criteria
        /// </summary>
        /// <param name="searchCriteria">search criteria to search transactions</param>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>List of transactions matching the passed search criteria</returns>
        [HttpPost]
        public IHttpActionResult MobileSearch([FromBody] SearchCriteria searchCriteria, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;



                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                if (searchCriteria.EntityId == -1)
                {
                    searchCriteria.EntityId = entityId;
                }

                PostObjectResult<List<SearchTransactionDTO>> getResultMobileSearch = HttpClientWrapper<PostObjectResult<List<SearchTransactionDTO>>>
                                                  .PostRequest("api/MobileApi/MobileSearch", searchCriteria, languageAbbreviation, Token)
                                                  .Result;


                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(getResultMobileSearch.Result);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to update the transaction status
        /// </summary>
        /// <param name="transStatus">The transaction status to update including the transation id and status</param>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The action result</returns>
        [HttpPost]
        public IHttpActionResult UpdateTransStatus([FromBody] TransStatus transStatus, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;


                transStatus.StatusId = transStatus.StatusId == 9 ? 2 : transStatus.StatusId;
                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                string ip = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostAddress);
                string systemAddress = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostName);

                PutResult putResult = HttpClientWrapper<PutResult>
                                                  .PutRequest(string.Format("api/MobileApi/UserMobileUpdateTransactionStatus?transId={0}&statusId={1}&userId={2}&orgUnitId={3}&reason={4}", transStatus.TransId, transStatus.StatusId, userId, entityId, transStatus.Reason), languageAbbreviation, Token)
                                                  .Result;

                if (putResult.StatusCode != MCS.Common.StatusCode.Ok)
                {
                    DataResult result = new DataResult
                    {
                        Code = MessageCode.CorrespondenceUpdatedUnsuccessfully,
                        Description = MessageResources.GetResourceText(ResourceText.CorrespondenceUpdatedUnsuccessfully, languageAbbreviation)
                    };

                    return Content(HttpStatusCode.BadRequest, result);
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to assign the transaction having the passed id to the logged in user
        /// </summary>
        /// <param name="transId">The transaction id to assign to the logged in user</param>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The action result</returns>
        [HttpGet]
        public IHttpActionResult SpecializeTransaction([FromUri] int transId, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;



                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                string ip = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostAddress);
                string systemAddress = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostName);

                PostResult postResult = HttpClientWrapper<PostResult>
                                                  .PostRequest($"api/MobileApi/SpecializeTransaction?TransId={transId}&userId={userId}&entityId={entityId}", languageAbbreviation, Token)
                                                  .Result;

                DataResult result = new DataResult();

                if (postResult.StatusCode != MCS.Common.StatusCode.Ok)
                {
                    result.Code = MessageCode.CorrespondenceUpdatedUnsuccessfully;
                    result.Description = MessageResources.GetResourceText(ResourceText.CorrespondenceUpdatedUnsuccessfully, languageAbbreviation);

                    return Content(HttpStatusCode.BadRequest, result);
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to assign back the transaction to the logged in user
        /// </summary>
        /// <param name="transAssignBack">Object contains the transaction is and the notes</param>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The action result</returns>
        [HttpPost]
        public IHttpActionResult AssignItBack(int TransId,string Notes, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();


                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;



                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                string ip = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostAddress);
                string systemAddress = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostName);

                PostResult postResult = HttpClientWrapper<PostResult>
                                                  .PostRequest($"api/MobileApi/AssignItBack?TransId={TransId}&Notes={Notes}&userId={userId}&entityId={entityId}", languageAbbreviation, Token)
                                                  .Result;

                DataResult result = new DataResult();

                if (postResult.StatusCode != MCS.Common.StatusCode.Ok)
                {
                    result.Code = MessageCode.CorrespondenceUpdatedUnsuccessfully;
                    result.Description = MessageResources.GetResourceText(ResourceText.CorrespondenceUpdatedUnsuccessfully, languageAbbreviation);

                    return Content(HttpStatusCode.BadRequest, result);
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This Method is return the user's entity accomplishments
        /// </summary>
        /// <param name="reportPeriodType">0 for Year, 1 for Month, and 2 for Week</param>
        /// <param name="numberOfPeriodRepetition">Number of period's repetetion to return</param>
        /// <param name="languageAbbreviation">Language Abbreviation "ar" or "en"</param>
        /// <returns>List of periods and each period accomplishments</returns>
        [HttpGet]
        public IHttpActionResult GetEntityAccompleshmentsReport(ReportPeriodType reportPeriodType, int numberOfPeriodRepetition, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;



                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                GetResult<List<EntityAccomplishmentReportInfoResult>> entityAccomplishmentReportInfoGetResult = HttpClientWrapper<GetResult<List<EntityAccomplishmentReportInfoResult>>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetEntitiesAccompleshmentsReport?entityId={0}&periodCount={1}&selectedPeriod={2}", entityId, numberOfPeriodRepetition, (int)reportPeriodType), languageAbbreviation, Token)
                                                  .Result;

                List<EntityAccomplishmentReportInfo> result = new List<EntityAccomplishmentReportInfo>();
                if (entityAccomplishmentReportInfoGetResult.Result != null && entityAccomplishmentReportInfoGetResult.Result.Count > 0)
                {


                    result = entityAccomplishmentReportInfoGetResult.Result.Select(e => new EntityAccomplishmentReportInfo
                    {
                        PeriodFrom = e.FROM_DATE,
                        PeriodTo = e.TO_DATE,
                        Counts = new List<ReportItem>(new ReportItem[] {
                                     new ReportItem(){Text= MessageResources.GetResourceText(ResourceText.MyTransactionsDashboardLabel, languageAbbreviation), Value = e.TRANSACTIONS ,TrayId = (int)TrayType.MyTransactions},
                                     new ReportItem(){Text= MessageResources.GetResourceText(ResourceText.DelayedTransactionsDashboardLabel, languageAbbreviation), Value = e.DELAYED,TrayId = (int)TrayType.Late},
                                     new ReportItem(){Text= MessageResources.GetResourceText(ResourceText.WithAppointmentTransactionsDashboardLabel, languageAbbreviation), Value = e.WITH_APPOITMENT , TrayId=(int)TrayType.HasDate},
                                    // new ReportItem(){Text= MessageResources.GetResourceText(ResourceText.CopiesDashboardLabel, languageAbbreviation), Value = e.TRANS_PARTIES } 
                        })
                    }).ToList();
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This Method is return the user's accomplishments
        /// </summary>
        /// <param name="languageAbbreviation">Language Abbreviation "ar" or "en"</param>
        /// <returns>User's accomplishments</returns>
        [HttpGet]
        public IHttpActionResult GetUserAccompleshmentsReport(string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;



                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                GetResult<UserAccomplishmentReportInfo> userAccomplishmentReportInfoGetResult = HttpClientWrapper<GetResult<UserAccomplishmentReportInfo>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserAccompleshmentsReport?userId={0}&entityId={1}", userId, entityId), languageAbbreviation, Token)
                                                  .Result;
                List<ReportItem> Counts = new List<ReportItem>(new ReportItem[] {
                                     new ReportItem(){Text= MessageResources.GetResourceText(ResourceText.MyTransactionsDashboardLabel, languageAbbreviation), Value = userAccomplishmentReportInfoGetResult.Result.TransactionCount,TrayId = (int)TrayType.MyTransactions},
                                                                new ReportItem(){Text= MessageResources.GetResourceText(ResourceText.DelayedTransactionsDashboardLabel, languageAbbreviation), Value = userAccomplishmentReportInfoGetResult.Result.DelayedCount,TrayId = (int)TrayType.Late},
                                                                new ReportItem(){Text= MessageResources.GetResourceText(ResourceText.WithAppointmentTransactionsDashboardLabel, languageAbbreviation), Value = userAccomplishmentReportInfoGetResult.Result.WithAppointmentCount,TrayId = (int)TrayType.HasDate}});

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(Counts);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        [HttpGet]
        public IHttpActionResult GetUserSignature(string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;


                GetResult<SignatureData> getResultSignatureData = HttpClientWrapper<GetResult<SignatureData>>
                                            .GetItemRequest($"api/MobileApi/GetUserSignature?userId={userId}", languageAbbreviation, Token)
                                            .Result;

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(getResultSignatureData.Result ?? new SignatureData());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public IHttpActionResult AddUserSignature([FromBody] SignatureData signatureData, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;

                if (signatureData.Password != null && signatureData.Password != string.Empty)
                {
                    signatureData.Password = StringCipher.Encrypt(signatureData.Password);
                }
                else
                {
                    signatureData.Password = string.Empty;
                }

                PostResult postResult = HttpClientWrapper<PostResult>
                                               .PostRequest($"api/MobileApi/AddUserSignature?userId={userId}", signatureData, languageAbbreviation, Token)
                                               .Result;


                bool isAdded = postResult.StatusCode == MCS.Common.StatusCode.Ok;

                DataResult result = new DataResult();

                if (!isAdded)
                {
                    result.Code = MessageCode.CorrespondenceUpdatedUnsuccessfully;
                    result.Description = MessageResources.GetResourceText(ResourceText.CorrespondenceUpdatedUnsuccessfully, languageAbbreviation);

                    return Content(HttpStatusCode.BadRequest, result);
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        [HttpPost]
        public IHttpActionResult AuthenticateUserSignature(string password, string languageAbbreviation)
        {

            IHttpActionResult iHttpActionResult = Ok();

            if (!PreRequest(languageAbbreviation, out iHttpActionResult))
            {
                return iHttpActionResult;
            }

            DataResult result = new DataResult();

            int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;

            GetResult<SignatureData> getResultSignatureData = HttpClientWrapper<GetResult<SignatureData>>
                                            .GetItemRequest($"api/MobileApi/GetUserSignature?userId={userId}", languageAbbreviation, Token)
                                            .Result;
            bool isAuthenticated = false;
            if (getResultSignatureData.Result != null && !string.IsNullOrEmpty(getResultSignatureData.Result.Password))
            {
                password = StringCipher.Encrypt(password);
                if (password == getResultSignatureData.Result.Password)
                {
                    isAuthenticated = true;
                }
                else
                {
                    result.Code = MessageCode.IncorrectSignaturePassword;
                    result.Description = MessageResources.GetResourceText(ResourceText.IncorrectSignaturePassword, languageAbbreviation);

                    isAuthenticated = false;
                }
            }
            else
            {
                result.Code = MessageCode.NoSignaturePasswordExist;
                result.Description = MessageResources.GetResourceText(ResourceText.NoSignaturePasswordExist, languageAbbreviation);

                isAuthenticated = false;
            }

            if (!PostRequest(languageAbbreviation, out iHttpActionResult))
            {
                return iHttpActionResult;
            }

            if (!isAuthenticated)
            {
                return Content(HttpStatusCode.Forbidden, result);
            }
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult IsUserAuthenticated(string userName, string password, string languageAbbreviation)
        {

            IHttpActionResult iHttpActionResult = Ok();

            if (!PreRequest(languageAbbreviation, out iHttpActionResult))
            {
                return iHttpActionResult;
            }

            bool isAuthenticated = true;
            DataResult result = new DataResult();

            if (string.IsNullOrEmpty(userName))
            {
                result.Code = MessageCode.InvalidUserNameOrPassword;
                result.Description = MessageResources.GetResourceText(ResourceText.InvalidUserName, languageAbbreviation);
                isAuthenticated = false;
            }

            if (!Utilities.IsAuthenticated(userName, password))
            {
                result.Code = MessageCode.UnauthenticatedUser;
                result.Description = MessageResources.GetResourceText(ResourceText.UnauthenticatedUser, languageAbbreviation);
                isAuthenticated = false;
            }

            if (!PostRequest(languageAbbreviation, out iHttpActionResult))
            {
                return iHttpActionResult;
            }

            if (!isAuthenticated)
            {
                return Content(HttpStatusCode.Forbidden, result);
            }

            return Ok();
        }

        /// <summary>
        /// This method is to return the terms and conditions of the Application
        /// </summary>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The actions result</returns>
        [HttpGet]
        public IHttpActionResult GetTermsAndConditions(string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string sTermsAndConditions = MessageResources.GetResourceText(ResourceText.TermsAndConditions, languageAbbreviation);

                string html = "<html><body><b>" + sTermsAndConditions + "</body></html>";

                return new TextResult(html, Request);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to return the About eMorasalat
        /// </summary>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The actions result</returns>
        [HttpGet]
        public IHttpActionResult GetAbouteMorasalat(string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string sAboutMorasalat = MessageResources.GetResourceText(ResourceText.AboutMorasalat, languageAbbreviation);

                string html = "<html><body><b>" + sAboutMorasalat + "</body></html>";

                return new TextResult(html, Request);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to return the Contact Us
        /// </summary>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The actions result</returns>
        [HttpGet]
        public IHttpActionResult GetContactUs(string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string sContactUs = MessageResources.GetResourceText(ResourceText.ContactUs, languageAbbreviation);
                string html = "<html><body><b>" + sContactUs + "</body></html>";

                return new TextResult(html, Request);

            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        ///// <summary>
        ///// This method is to get the list of the transactions assigned for the logged in user
        ///// </summary>
        ///// <param name="languageName">The language name for the interface language used</param>
        ///// <returns>List of transactions</returns>
        //[HttpGet]
        //public IHttpActionResult GetTransactions(LanguageName languageName)
        //{
        //    try
        //    {                
        //        IHttpActionResult iHttpActionResult = Ok();

        //        if (!PreRequest(languageName, out iHttpActionResult))
        //        {
        //            return iHttpActionResult;
        //        }

        //        string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;

        //        DateTime lastLoginDate = DateTime.MinValue;
        //        IList<Models.Transaction> transactions = new List<Models.Transaction>();

        //        DataSet dsTransactions = GatewayService.GetCorrespodencesByUserName(userName.Trim());

        //        for (int j = 0; j < dsTransactions.Tables.Count; j++)
        //        {
        //            DataTable dtCorespondences = dsTransactions.Tables[j];
        //            Models.Transaction transaction = null;

        //            for (int i = 0; i < dtCorespondences.Rows.Count; i++)
        //            {
        //                transaction = new Models.Transaction();

        //                transaction.TransID = dtCorespondences.Rows[i]["TRANS_ID"].ToString();
        //                transaction.TransNo = dtCorespondences.Rows[i]["TRANS_NO"].ToString();
        //                transaction.TransCategory = dtCorespondences.Rows[i]["TRANS_CATEGORY"].ToString();
        //                transaction.TransTitle = HttpUtility.HtmlEncode(dtCorespondences.Rows[i]["SUBJECT"].ToString());

        //                if (!string.IsNullOrEmpty(dtCorespondences.Rows[i]["TRANS_DATE_HJ"].ToString()))
        //                {
        //                    transaction.TransDate = Utilities.GetDateFormatString(dtCorespondences.Rows[i]["TRANS_DATE_HJ"].ToString(), true, true);
        //                }

        //                if (j == 0)
        //                {
        //                    transaction.TransFrom = dtCorespondences.Rows[i]["LOCALE_VALUE"].ToString();
        //                    transaction.ReadOnly = false;
        //                }
        //                else if (j == 1)
        //                {
        //                    transaction.TransFrom = dtCorespondences.Rows[i]["CONCERNED_ENTITY_NAME"].ToString();
        //                    transaction.ReadOnly = false;
        //                }
        //                else
        //                {
        //                    transaction.TransID = "!" + dtCorespondences.Rows[i]["TRANS_PARTY_ID"].ToString();
        //                    transaction.TransFrom = dtCorespondences.Rows[i]["SOURCE_ENTITY_NAME"].ToString();
        //                    transaction.ReadOnly = true;
        //                }

        //                transaction.Has_Supporting_Attachments = false;//to do:get from DB.
        //                transaction.FileSize = "";//to do:get from DB.

        //                transactions.Add(transaction);
        //            }
        //        }

        //        if (!PostRequest(languageName, out iHttpActionResult))
        //        {
        //            return iHttpActionResult;
        //        }

        //        return Ok(transactions);
        //    }
        //    catch (Exception ex)
        //    {
        //        SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

        //        return null;
        //    }
        //}

        ///// <summary>
        ///// This method is to get the transaction as XML string
        ///// </summary>
        ///// <param name="transactionId">The transaction id to get</param>
        ///// <param name="languageName">The language name for the interface language used</param>
        ///// <returns>XML string represents the transaction</returns>
        //[HttpGet]
        //public IHttpActionResult GetTransactionXML([FromUri]string transactionId, LanguageName languageName)
        //{
        //    try
        //    {
        //        IHttpActionResult iHttpActionResult = Ok();

        //        if (!PreRequest(languageName, out iHttpActionResult))
        //        {
        //            return iHttpActionResult;
        //        }               

        //        string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
        //        int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;

        //        DateTime lastLoginDate = DateTime.MinValue;
        //        DataResult result = new DataResult();

        //        if (string.IsNullOrEmpty(transactionId))
        //        {
        //            result.Code = MessageCode.CorrespondenceIdNotValid;
        //            result.Description = MessageResources.GetResourceText(ResourceText.CorrespondenceIdNotValid, languageName);

        //            return Content(HttpStatusCode.BadRequest, result);
        //        }

        //        StringBuilder sbXml = new StringBuilder();
        //        DataSet ds = GatewayService.GetCorrespondenceById(transactionId.Trim('!'), userId, (languageName == LanguageName.Arabic ? "ar" : "en"), transactionId.StartsWith("!"));
        //        bool isError = BuildTransactionXML(userId, transactionId, ds, sbXml, languageName);

        //        if (isError)
        //        {
        //            result.Code = MessageCode.CorrespondenceGetUnsuccessful;
        //            result.Description = MessageResources.GetResourceText(ResourceText.CorrespondenceGetUnsuccessful, languageName);

        //            return Content(HttpStatusCode.BadRequest, result);
        //        }

        //        if (!PostRequest(languageName, out iHttpActionResult))
        //        {
        //            return iHttpActionResult;
        //        }

        //        return Ok(sbXml.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

        //        return null;
        //    }
        //}        

        /// <summary>
        /// This method is to sign the request using the certificate configured
        /// </summary>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The signed request</returns>
        [HttpGet]
        public IHttpActionResult SignRequest(string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;

                SignRequest signRequest = new Models.SignRequest();

                string url = System.Configuration.ConfigurationManager.AppSettings["CsrURL"].ToString(); //"http://192.168.1.67:8080/v1/csr";
                var uri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";

                DataContractJsonSerializer ser = new DataContractJsonSerializer(signRequest.GetType());
                MemoryStream ms = new MemoryStream();

                ser.WriteObject(ms, signRequest);

                String json = Encoding.UTF8.GetString(ms.ToArray());

                StreamWriter writer = new StreamWriter(request.GetRequestStream());

                writer.Write(json);
                writer.Close();

                WebResponse webrespon = request.GetResponse();
                StreamReader streamResult = new StreamReader(webrespon.GetResponseStream());

                string strResult = streamResult.ReadToEnd();

                JavaScriptSerializer js = new JavaScriptSerializer();
                Dictionary<string, string> obj = js.Deserialize<Dictionary<string, string>>(strResult);

                string cacrl = obj["CACRL"];
                string ca = obj["CACert"];
                string publicKey = obj["SignedCert"];

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(signRequest);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        /// <summary>
        /// This method is to get the document stream by the document id
        /// </summary>
        /// <param name="docId">The document id</param>
        /// <param name="isPrimary">is primary document</param>
        /// <param name="languageName">The language name for the interface language used</param>
        /// <returns>The document stream</returns>
        [HttpGet]
        public IHttpActionResult GetDocumentById([FromUri] string docId, bool isPrimary, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;

                DataResult result = new DataResult();

                if (string.IsNullOrEmpty(docId))
                {
                    result.Code = MessageCode.CorrespondenceIdNotValid;
                    result.Description = MessageResources.GetResourceText(ResourceText.CorrespondenceIdNotValid, languageAbbreviation);

                    return Content(HttpStatusCode.BadRequest, result);
                }

                //ArchivesInfo objArchives = new ArchivesInfo();
                string[] IDs = new string[] { docId };

                //DataSet dsFileXml = objArchives.GetDocsByAttachRecordIDs(IDs);

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                //if (dsFileXml != null && dsFileXml.Tables[0].Rows.Count > 0)
                //{
                //    string mimeType = string.Empty;

                //    if (dsFileXml.Tables[0].Rows[0][ArchivedItem.DOCNAME] != null)
                //    {
                //        mimeType = Path.GetExtension(dsFileXml.Tables[0].Rows[0][ArchivedItem.DOCNAME] as string).Replace(".", string.Empty);
                //    }

                //    if (dsFileXml.Tables[0].Rows[0][ArchivedItem.DMS_DOCDATA] != null &&
                //        dsFileXml.Tables[0].Rows[0][ArchivedItem.DMS_DOCDATA].ToString() != string.Empty)
                //    {
                //        byte[] byteArray = (byte[])dsFileXml.Tables[0].Rows[0][ArchivedItem.DMS_DOCDATA];
                //        Stream stream = new MemoryStream(byteArray);

                //        if (isPrimary)
                //        {
                //            Stream pdfStream = null;

                //            switch (mimeType.ToLower())
                //            {
                //                case "tiff":
                //                case "tif":
                //                    pdfStream = ConvertTiff2PDF(stream, userName);
                //                    break;
                //                case "doc":
                //                    bool useWordAPIs = System.Configuration.ConfigurationManager.AppSettings["UseWordAPIs"].ToString() == "true" ? true : false;
                //                    if (useWordAPIs)
                //                    {
                //                        pdfStream = ConvertWord2PDF(byteArray, userName);
                //                    }
                //                    else
                //                    {
                //                        pdfStream = ConvertWord2PDF(stream, userName);
                //                    }
                //                    break;
                //                case "html":
                //                    pdfStream = ConvertHtml2PDF(stream, userName);
                //                    break;
                //            }

                //            if (pdfStream != null)
                //            {
                //                return Ok(pdfStream);
                //            }
                //        }

                //        return Ok(stream);
                //    }
                //}

                result.Code = MessageCode.DocumentDataInvalid;
                result.Description = MessageResources.GetResourceText(ResourceText.DocumentDataInvalid, languageAbbreviation);

                return Content(HttpStatusCode.BadRequest, result);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        [HttpGet]
        public IHttpActionResult GetUserPrivileges(string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;


                GetResult<List<string>> getUserPermisionsData = HttpClientWrapper<GetResult<List<string>>>
                                            .GetItemRequest($"api/MobileApi/GetUserPrivileges?userId={userId}", languageAbbreviation, Token)
                                            .Result;

                List<string> permitionList = getUserPermisionsData.Result;

                if (bool.Parse(System.Configuration.ConfigurationManager.AppSettings["GrantFullPermissionOnAppByDefault"].ToString()))
                {
                    var x = new PermissionName();

                    permitionList.Add(x.CreateInternalOutbound);
                    permitionList.Add(x.CreateOutboundDraft);
                    permitionList.Add(x.EditInbound);
                    permitionList.Add(x.EditInternalOutbound);
                    permitionList.Add(x.EditOutboundDraft);
                    permitionList.Add(x.EntityAccompleshmentsReport);
                    permitionList.Add(x.Manager);
                    permitionList.Add(x.UserAccompleshmentsReport);
                    permitionList.Add(x.Sign);
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(getUserPermisionsData.Result.Distinct() ?? new List<string>());
            }
            catch (Exception ex)
            {
                string inner = string.Empty;
                string stackTrace = string.Empty;
                Exception innerException = ex.InnerException;
                stackTrace = ex.StackTrace;

                while (innerException != null)
                {
                    inner += "\n" + ex.InnerException.Message;
                    stackTrace += "\n" + innerException.StackTrace;
                    innerException = innerException.InnerException;
                }
                File.AppendAllText(LogFilePath, DateTime.Now.ToString() + "\n" + stackTrace + "\n" + ex.Message + "\n" + inner + "\n");
                return null;
            }
        }

        private bool CheckUserLogin(string userName, string password, string userDeviceToken, DataResult result, out int userId, out string userFullName, out string userDefaultEntityName, out int entityId, string languageName)
        {
            userId = -1;
            userFullName = string.Empty;
            userDefaultEntityName = string.Empty;
            entityId = 0;
            if (string.IsNullOrEmpty(userName))
            {
                result.Code = MessageCode.InvalidUserNameOrPassword;
                result.Description = MessageResources.GetResourceText(ResourceText.InvalidUserName, languageName);

                return false;
            }

            int nTenantId = -1;
            if (!Utilities.IsAuthenticated(userName, password))
            {
                result.Code = MessageCode.UnauthenticatedUser;
                result.Description = MessageResources.GetResourceText(ResourceText.UnauthenticatedUser, languageName);

                return false;
            }
            else
            {
                #region Multi Tenant Enabled
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    GetResult<TenantInfo> tenantResult = HttpClientWrapper<GetResult<TenantInfo>>.GetItemRequest(string.Format("api/MultiTenant/GetTenantInfo?username={0}", userName), languageName, Token).Result;
                    if (tenantResult.StatusCode != MCS.Common.StatusCode.Ok)
                    {
                        result.Code = MessageCode.UnauthenticatedUser;
                        result.Description = MessageResources.GetResourceText(ResourceText.UnauthenticatedUser, languageName);

                        return false;
                    }
                    else
                    {
                        if (tenantResult.Result == null)
                        {
                            result.Code = MessageCode.UnauthenticatedUser;
                            result.Description = MessageResources.GetResourceText(ResourceText.UnauthenticatedUser, languageName);

                            return false;
                        }
                        else
                        {
                            nTenantId = tenantResult.Result.Id;
                        }
                    }
                }
                #endregion

                LoginInfoDTO loginInfoDTO = new LoginInfoDTO()
                {
                    UserName = userName,
                    Password = AESEncrytDecry.EncryptStringAES(password),
                    IsWindowsLogin = Convert.ToBoolean(ConfigurationManager.AppSettings["IsWindowsLogin"].ToString())
                };

                PostObjectResult<UserDTO> postResultUserDTO =
                   HttpClientWrapper<PostObjectResult<UserDTO>>.PostRequest("api/Login/Login?cultureName=" + languageName, loginInfoDTO, languageName, string.Empty, nTenantId).Result;

                Token = postResultUserDTO.Result.AccessToken;
            }

            GetResult<UserData> getResultUserData = HttpClientWrapper<GetResult<UserData>>
                                                    .GetItemRequest(string.Format("api/MobileApi/GetUserInfo?userName={0}", userName), languageName, Token, nTenantId)
                                                    .Result;

            if (getResultUserData.Result == null)
            {
                result.Code = MessageCode.InvalidUserNameOrPassword;
                result.Description = MessageResources.GetResourceText(ResourceText.InvalidUserName, languageName);

                return false;
            }

            userName = getResultUserData.Result.LoginName.ToString();
            userFullName = getResultUserData.Result.FullName.ToString();
            userDefaultEntityName = getResultUserData.Result.DefaultEntityName.ToString();
            userId = string.IsNullOrEmpty(getResultUserData.Result.PersonId.ToString()) ? -1 : getResultUserData.Result.PersonId;

            if (!getResultUserData.Result.AllowMobile)
            {
                result.Code = MessageCode.UnauthenticatedUserOnMobile;
                result.Description = MessageResources.GetResourceText(ResourceText.UnauthenticatedUserOniPad, languageName);
                return false;
            }

            //DataTable dtUserIPad = GatewayService.GetUserIPad(null, userName, null, null, null, null, null, null);
            GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageName, Token, nTenantId)
                                                  .Result;

            if (getResultUserMobile.Result != null)
            {
                string deviceToken = getResultUserMobile.Result.DeviceToken;

                if (!string.IsNullOrEmpty(deviceToken) && deviceToken != userDeviceToken)
                {
                    result.Code = MessageCode.UserConnectedToAnotherMobile;
                    result.Description = MessageResources.GetResourceText(ResourceText.UserConnectedToAnotheriPad, languageName);

                    return false;
                }
                entityId = getResultUserMobile.Result.EntityId;
            }

            return true;
        }

        //private bool BuildTransactionXML(int userId, string transactionId, DataSet ds, StringBuilder sbXml, string languageAbbreviation)
        //{
        //    TransCategoryEnum transCategory = TransCategoryEnum.Inbound;
        //    bool isReadOnly = false;
        //    int entityId = 0;

        //    if (ds != null && ds.Tables.Count > 0)
        //    {
        //        sbXml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        //        sbXml.Append("<ELEMENTS>");

        //        DataTable dtMain = ds.Tables[0];

        //        if (dtMain.Rows.Count > 0)
        //        {
        //            if (dtMain.Rows[0]["ENTITY_ID"] != null)
        //            {
        //                entityId = Convert.ToInt32(dtMain.Rows[0]["ENTITY_ID"]);
        //            }

        //            transCategory = (TransCategoryEnum)Convert.ToInt32(dtMain.Rows[0]["TRANS_CATEGORY"].ToString());

        //            string rootType = string.Empty;
        //            isReadOnly = Convert.ToBoolean(dtMain.Rows[0]["READ_ONLY"]);

        //            if (isReadOnly)
        //            {
        //                if (transCategory == TransCategoryEnum.Inbound)
        //                {
        //                    rootType = LabelResources.GetResourceText(ResourceText.InboundCopies, languageAbbreviation);
        //                }
        //                else
        //                {
        //                    rootType = LabelResources.GetResourceText(ResourceText.OutboundCopies, languageAbbreviation);
        //                }
        //            }
        //            else
        //            {
        //                if (transCategory == TransCategoryEnum.Inbound)
        //                {
        //                    rootType = LabelResources.GetResourceText(ResourceText.Inbound, languageAbbreviation);
        //                }
        //                else
        //                {
        //                    rootType = LabelResources.GetResourceText(ResourceText.Outbound, languageAbbreviation);
        //                }
        //            }

        //            int transOwnershipId = 0;

        //            if (dtMain.Rows[0]["TRANS_OWNERSHIP_ID"] != null && !string.IsNullOrEmpty(dtMain.Rows[0]["TRANS_OWNERSHIP_ID"].ToString()))
        //            {
        //                transOwnershipId = Convert.ToInt32(dtMain.Rows[0]["TRANS_OWNERSHIP_ID"]);
        //            }

        //            string showPrioConf = System.Configuration.ConfigurationManager.AppSettings["Show_Priorities_And_Confidentialities"].ToString();
        //            string isAssignmentPaper = System.Configuration.ConfigurationManager.AppSettings["Use_New_Ehala_Form_Functionality"].ToString();
        //            string assignPaperMode = System.Configuration.ConfigurationManager.AppSettings["Assign_Paper_Mode"].ToString();

        //            sbXml.Append("<DOCUMENT ID=\"" + transactionId + "\" TRANS_NO=\"" + dtMain.Rows[0]["TRANS_NO"].ToString() + "\" TRANS_ID=\"" + transactionId + "\" TRANS_CATEGORY=\"" + dtMain.Rows[0]["TRANS_CATEGORY"].ToString() + "\" TRANS_OWNERSHIP_ID=\"" + transOwnershipId + "\" CID=\"\" LEVEL_PATH=\"ROOT/" + rootType + "\" Show_Priorities_And_Confidentialities=\"" + showPrioConf + "\" Use_New_Ehala_Form_Functionality=\"" + isAssignmentPaper + "\" Assign_Paper_Mode=\"" + assignPaperMode + "\">");
        //            sbXml.Append("<ENTITY ID=\"").Append(entityId.ToString()).Append("\"/>");
        //            sbXml.Append("<PRIMARY_METADATA>");
        //            sbXml.Append("<INFO TYPE=\"TEXT\">");
        //            sbXml.Append("<PRIMARY_LABEL>" + LabelResources.GetResourceText(ResourceText.Confidentiality, languageAbbreviation) + "</PRIMARY_LABEL>");
        //            sbXml.Append("<PRIMARY_VALUE>" + dtMain.Rows[0]["CNFN_DESCRIPTION_AR"].ToString() + "</PRIMARY_VALUE>");
        //            sbXml.Append("</INFO>");
        //            sbXml.Append("<INFO TYPE=\"TEXT\">");
        //            sbXml.Append("<PRIMARY_LABEL>" + LabelResources.GetResourceText(ResourceText.Priority, languageAbbreviation) + "</PRIMARY_LABEL>");
        //            sbXml.Append("<PRIMARY_VALUE>" + dtMain.Rows[0]["PRIORITY_DESC_AR"].ToString() + "</PRIMARY_VALUE>");
        //            sbXml.Append("</INFO>");

        //            if (dtMain.Rows[0]["REMIND_DATE_HJ"] != null && !string.IsNullOrEmpty(dtMain.Rows[0]["REMIND_DATE_HJ"].ToString()) &&
        //                (dtMain.Rows[0]["PRIORITY_NO"].ToString() == MCSSettings.GetValue(MCSSettingsConstants.MCSSettingsConstantItems.ConPriority) ||
        //                dtMain.Rows[0]["PRIORITY_NO"].ToString() == MCSSettings.GetValue(MCSSettingsConstants.OutboundConPriority)))
        //            {
        //                sbXml.Append("<INFO TYPE=\"TEXT\">");
        //                sbXml.Append("<PRIMARY_LABEL>" + LabelResources.GetResourceText(ResourceText.PriorityDate, languageAbbreviation) + "</PRIMARY_LABEL>");
        //                sbXml.Append("<PRIMARY_VALUE>" + Utilities.GetDateFormatString(dtMain.Rows[0]["REMIND_DATE_HJ"].ToString(), false, true) + "</PRIMARY_VALUE>");
        //                sbXml.Append("</INFO>");
        //            }

        //            sbXml.Append("<INFO TYPE=\"TEXTAREA\">");
        //            sbXml.Append("<PRIMARY_LABEL>" + LabelResources.GetResourceText(ResourceText.Subject, languageAbbreviation) + "</PRIMARY_LABEL>");
        //            sbXml.Append("<PRIMARY_VALUE>" + HttpUtility.HtmlEncode(dtMain.Rows[0]["SUBJECT"].ToString()) + "</PRIMARY_VALUE>");
        //            sbXml.Append("</INFO>");
        //            sbXml.Append("</PRIMARY_METADATA>");

        //            if (dtMain.Rows[0]["ATTACH_TRANS_ID"] != null && !string.IsNullOrEmpty(dtMain.Rows[0]["ATTACH_TRANS_ID"].ToString()))
        //            {
        //                string docId = dtMain.Rows[0]["ATTACH_TRANS_ID"].ToString();
        //                Stream fileStream = GetOriginalFileStream(docId);

        //                if (fileStream != null)
        //                {
        //                    sbXml.Append("<PRIMARY_PARTS TITLE=\"" + dtMain.Rows[0]["ATTACH_DESC_AR"].ToString() + "\" EXT=\"PDF\">");
        //                    sbXml.Append("<PRIMARY_PART>");
        //                    sbXml.Append("<PRIMARY_ID>" + docId + "</PRIMARY_ID>");
        //                    sbXml.Append("<PRIMARY_DATA TYPE=\"REF\"></PRIMARY_DATA>");
        //                    sbXml.Append("<PRIMARY_SIZE>" + dtMain.Rows[0]["DATASIZE"].ToString() + "</PRIMARY_SIZE>");
        //                    sbXml.Append("<HASH>" + HashingFile(fileStream) + "</HASH>");
        //                    sbXml.Append("</PRIMARY_PART>");
        //                    sbXml.Append("</PRIMARY_PARTS>");
        //                }
        //            }

        //            //set default priority
        //            sbXml.Append("<DefaultPriority>");
        //            sbXml.Append("<ID>").Append(dtMain.Rows[0]["PRIORITY_NO"].ToString()).Append("</ID>");
        //            sbXml.Append("<VALUE>").Append(dtMain.Rows[0]["PRIORITY_DESC_AR"].ToString()).Append("</VALUE>");

        //            if (dtMain.Rows[0]["REMIND_DATE_HJ"] != null && !string.IsNullOrEmpty(dtMain.Rows[0]["REMIND_DATE_HJ"].ToString()) &&
        //                (dtMain.Rows[0]["PRIORITY_NO"].ToString() == MCSSettings.GetValue(MCSSettingsConstants.MCSSettingsConstantItems.ConPriority) ||
        //                dtMain.Rows[0]["PRIORITY_NO"].ToString() == MCSSettings.GetValue(MCSSettingsConstants.OutboundConPriority)))
        //            {
        //                string remindDate = dtMain.Rows[0]["REMIND_DATE_HJ"].ToString();
        //                sbXml.Append("<PRIORITY_DATE>").Append(remindDate.Substring(0, 8)).Append("</PRIORITY_DATE>");
        //                sbXml.Append("<PRIORITY_TIME>").Append(remindDate.Substring(8)).Append("</PRIORITY_TIME>");
        //            }

        //            sbXml.Append("</DefaultPriority>");

        //            //set default confid
        //            sbXml.Append("<DefaultConfidentiality>");
        //            sbXml.Append("<ID>").Append(dtMain.Rows[0]["CONFID_ID"].ToString()).Append("</ID>");
        //            sbXml.Append("<VALUE>").Append(dtMain.Rows[0]["CNFN_DESCRIPTION_AR"].ToString()).Append("</VALUE>");
        //            sbXml.Append("</DefaultConfidentiality>");
        //        }

        //        if (transCategory == TransCategoryEnum.Inbound && !isReadOnly)
        //        {
        //            sbXml.Append("<SUPPORTING_DOCUMENT_PARTS>");
        //            DataTable dtSupporting = ds.Tables[1];

        //            if (dtSupporting.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < dtSupporting.Rows.Count; i++)
        //                {
        //                    string strType = dtSupporting.Rows[i]["ATTACH_DESC_AR"].ToString();

        //                    if (dtSupporting.Rows[i]["DEL_ITEM_TYPE_DESC_AR"] != null &&
        //                        dtSupporting.Rows[i]["DEL_ITEM_TYPE_DESC_AR"] != DBNull.Value)
        //                    {
        //                        strType += " - " + dtSupporting.Rows[i]["DEL_ITEM_TYPE_DESC_AR"].ToString();
        //                    }

        //                    sbXml.Append("<SUPPORTING_DOCUMENT_PART>");
        //                    sbXml.Append("<SUPPORTING_DOCUMENT_METADATA>");
        //                    sbXml.Append("<SUPPORTING_DOCUMENT_INFO TYPE=\"TEXT\">");
        //                    sbXml.Append("<SUPPORTING_DOCUMENT_LABEL>" + LabelResources.GetResourceText(ResourceText.Type, languageAbbreviation) + "</SUPPORTING_DOCUMENT_LABEL>");
        //                    sbXml.Append("<SUPPORTING_DOCUMENT_VALUE>" + strType + "</SUPPORTING_DOCUMENT_VALUE>");
        //                    sbXml.Append("</SUPPORTING_DOCUMENT_INFO>");
        //                    sbXml.Append("<SUPPORTING_DOCUMENT_INFO TYPE=\"TEXT\">");
        //                    sbXml.Append("<SUPPORTING_DOCUMENT_LABEL>" + LabelResources.GetResourceText(ResourceText.Confidentiality, languageAbbreviation) + "</SUPPORTING_DOCUMENT_LABEL>");
        //                    sbXml.Append("<SUPPORTING_DOCUMENT_VALUE>" + dtSupporting.Rows[i]["ATTCNFN_DESCRIPTION_AR"].ToString() + "</SUPPORTING_DOCUMENT_VALUE>");
        //                    sbXml.Append("</SUPPORTING_DOCUMENT_INFO>");
        //                    sbXml.Append("</SUPPORTING_DOCUMENT_METADATA>");
        //                    sbXml.Append("<SUPPORTING_DOCUMENT_ID>" + dtSupporting.Rows[i]["ATTACH_TRANS_ID"].ToString() + "</SUPPORTING_DOCUMENT_ID>");
        //                    sbXml.Append("<SUPPORTING_DOCUMENT_PAGE_NUMBER>" + -1 + "</SUPPORTING_DOCUMENT_PAGE_NUMBER>");

        //                    string docId = dtSupporting.Rows[i]["ATTACH_TRANS_ID"].ToString();
        //                    Stream fileStream = GetOriginalFileStream(docId);

        //                    if (fileStream != null)
        //                    {
        //                        sbXml.Append("<SUPPORTING_DOCUMENT_TITLE>" + strType + "</SUPPORTING_DOCUMENT_TITLE>");
        //                        sbXml.Append("<SUPPORTING_DOCUMENT_EXT>" + dtSupporting.Rows[i]["DOCNAME"].ToString().Split('.')[1] + "</SUPPORTING_DOCUMENT_EXT>");
        //                        sbXml.Append("<SUPPORTING_DOCUMENT_DATA TYPE=\"REF\"></SUPPORTING_DOCUMENT_DATA>");
        //                        sbXml.Append("<SUPPORTING_SIZE>" + dtSupporting.Rows[i]["DATASIZE"].ToString() + "</SUPPORTING_SIZE>");
        //                        sbXml.Append("<HASH>" + HashingFile(fileStream) + "</HASH>");
        //                        sbXml.Append("</SUPPORTING_DOCUMENT_PART>");
        //                    }
        //                }
        //            }

        //            sbXml.Append("</SUPPORTING_DOCUMENT_PARTS>");
        //        }

        //        sbXml.Append("<ASSIGNEES>");

        //        if (ds.Tables.Count >= 3)
        //        {
        //            DataTable dtAssignees = ds.Tables[2];

        //            if (dtAssignees.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < dtAssignees.Rows.Count; i++)
        //                {
        //                    sbXml.Append("<ASSIGNEE>");
        //                    sbXml.Append("<ASSIGNEE_ID>" + dtAssignees.Rows[i]["ASSIGNEE_ID"].ToString() + "</ASSIGNEE_ID>");
        //                    sbXml.Append("<ASSIGNEE_NAME_AR>" + dtAssignees.Rows[i]["ASSIGNEE_NAME"].ToString() + "</ASSIGNEE_NAME_AR>");
        //                    sbXml.Append("<ASSIGNEE_NAME_EN>" + dtAssignees.Rows[i]["ASSIGNEE_NAME"].ToString() + "</ASSIGNEE_NAME_EN>");
        //                    sbXml.Append("</ASSIGNEE>");
        //                }
        //            }
        //        }

        //        sbXml.Append("</ASSIGNEES>");

        //        sbXml.Append("<ACTIONS>");

        //        if (ds.Tables.Count >= 4)
        //        {
        //            DataTable dtActions = ds.Tables[3];

        //            if (dtActions.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < dtActions.Rows.Count; i++)
        //                {
        //                    string actionID = dtActions.Rows[i]["ACTION_ID"].ToString();

        //                    sbXml.Append("<ACTION Show_In_Copy=\"1\">");
        //                    sbXml.Append("<ACTION_ID>" + actionID + "</ACTION_ID>");
        //                    sbXml.Append("<ACTION_NAME_AR>" + HttpUtility.HtmlEncode(dtActions.Rows[i]["ACTION_NAME"].ToString()) + "</ACTION_NAME_AR>");
        //                    sbXml.Append("<ACTION_NAME_EN>" + HttpUtility.HtmlEncode(dtActions.Rows[i]["ACTION_NAME"].ToString()) + "</ACTION_NAME_EN>");

        //                    if (actionID == System.Configuration.ConfigurationManager.AppSettings["DefaultActionID"].ToString())
        //                    {
        //                        sbXml.Append("<ISDEFAULT>true</ISDEFAULT>");
        //                    }

        //                    sbXml.Append("</ACTION>");
        //                }
        //            }
        //        }

        //        sbXml.Append("</ACTIONS>");

        //        sbXml.Append("<COMMENTS>");

        //        if (ds.Tables.Count >= 5)
        //        {
        //            DataTable dtNotes = ds.Tables[4];

        //            if (dtNotes.Rows.Count > 0)
        //            {
        //                string[] IDs = new string[dtNotes.Rows.Count];

        //                for (int i = 0; i < dtNotes.Rows.Count; i++)
        //                {
        //                    IDs[i] = dtNotes.Rows[i]["ATTACH_TRANS_ID"].ToString();
        //                }

        //                ArchivesInfo objArchives = new ArchivesInfo();
        //                DataSet dsFileXml = objArchives.GetDocsByAttachRecordIDs(IDs);

        //                for (int i = 0; i < dtNotes.Rows.Count; i++)
        //                {
        //                    sbXml.Append("<NOTE>");
        //                    sbXml.Append("<NOTE_ID>" + dtNotes.Rows[i]["ATTACH_TRANS_ID"].ToString() + "</NOTE_ID>");
        //                    sbXml.Append("<AUTHOR>" + dtNotes.Rows[i]["USERNAME"].ToString() + "</AUTHOR>");
        //                    sbXml.Append("<DATE>" + Utilities.GetDateFormatString(dtNotes.Rows[i]["ATT_DATE_HJ"].ToString(), true, true) + "</DATE>");
        //                    sbXml.Append("<ACTION>" + "" + "</ACTION>");

        //                    if (dsFileXml.Tables[0].Rows[i][ArchivedItem.DMS_DOCDATA] != null &&
        //                        dsFileXml.Tables[0].Rows[i][ArchivedItem.DMS_DOCDATA].ToString() != string.Empty)
        //                    {
        //                        byte[] byteArray = (byte[])dsFileXml.Tables[0].Rows[i][ArchivedItem.DMS_DOCDATA];
        //                        string strDocData = Utilities.GetString(byteArray);

        //                        sbXml.Append("<CONTENT>" + strDocData + "</CONTENT>");
        //                    }

        //                    sbXml.Append("</NOTE>");
        //                }
        //            }
        //        }

        //        sbXml.Append("</COMMENTS>");
        //        sbXml.Append("<TRACKS>");

        //        if (ds.Tables.Count >= 6)
        //        {
        //            DataTable dtTracks = ds.Tables[5];

        //            if (dtTracks.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < dtTracks.Rows.Count; i++)
        //                {
        //                    sbXml.Append("<TRACK>");
        //                    sbXml.Append("<FROM_USER>" + dtTracks.Rows[i]["FROM_USER"].ToString() + "</FROM_USER>");

        //                    if (dtTracks.Rows[i]["TO_USER"] != null && !string.IsNullOrEmpty(dtTracks.Rows[i]["TO_USER"].ToString()))
        //                    {
        //                        sbXml.Append("<TO_USER>" + dtTracks.Rows[i]["TO_USER"].ToString() + "</TO_USER>");
        //                    }
        //                    else
        //                    {
        //                        sbXml.Append("<TO_USER>" + LabelResources.GetResourceText(ResourceText.DeptReception, languageAbbreviation) + " " + dtTracks.Rows[i]["DEPT"].ToString() + "</TO_USER>");
        //                    }

        //                    sbXml.Append("<DATE>" + dtTracks.Rows[i]["DATE_HJ"].ToString() + "</DATE>");
        //                    sbXml.Append("<ACTION>" + dtTracks.Rows[i]["NOTE"].ToString() + "</ACTION>");
        //                    sbXml.Append("<REMARKS>" + dtTracks.Rows[i]["REMARKS_AR"].ToString() + "</REMARKS>");
        //                    sbXml.Append("</TRACK>");
        //                }
        //            }
        //        }

        //        sbXml.Append("</TRACKS>");
        //        sbXml.Append("<Priorities>");

        //        DataView dvPriorities = GatewayService.GetLookUp(userId, CachedItemsKeys.Priority, transCategory, languageAbbreviation);

        //        if (dvPriorities.Count > 0)
        //        {
        //            for (int i = 0; i < dvPriorities.Count; i++)
        //            {
        //                sbXml.Append("<Priority>");
        //                sbXml.Append("<TAB_ID>").Append(dvPriorities[i]["TAB_ID"]).Append("</TAB_ID>");
        //                sbXml.Append("<TRANS_CATEGORY>").Append(dvPriorities[i]["TRANS_CATEGORY"]).Append("</TRANS_CATEGORY>");
        //                sbXml.Append("<PRIORITY_DESC_AR>").Append(dvPriorities[i]["PRIORITY_DESC_AR"]).Append("</PRIORITY_DESC_AR>");
        //                sbXml.Append("<ACTIVE>").Append(dvPriorities[i]["ACTIVE"]).Append("</ACTIVE>");

        //                if (dvPriorities[i]["TAB_ID"].ToString() == MCSSettings.GetValue(MCSSettingsConstants.MCSSettingsConstantItems.ConPriority)
        //                    ||
        //                    dvPriorities[i]["TAB_ID"].ToString() == MCSSettings.GetValue(MCSSettingsConstants.OutboundConPriority))
        //                {
        //                    sbXml.Append("<WITH_APPOINTMENT>").Append("True").Append("</WITH_APPOINTMENT>");
        //                }

        //                sbXml.Append("</Priority>");
        //            }
        //        }

        //        sbXml.Append("</Priorities>");
        //        sbXml.Append("<Confindentials>");

        //        DataView dvConfindentials = GatewayService.GetLookUp(userId, CachedItemsKeys.Confindential, transCategory, languageAbbreviation);

        //        if (dvConfindentials.Count > 0)
        //        {
        //            for (int i = 0; i < dvConfindentials.Count; i++)
        //            {
        //                sbXml.Append("<Confindential>");
        //                sbXml.Append("<TAB_ID>").Append(dvConfindentials[i]["TAB_ID"]).Append("</TAB_ID>");
        //                sbXml.Append("<TRANS_CATEGORY>").Append(dvConfindentials[i]["TRANS_CATEGORY"]).Append("</TRANS_CATEGORY>");
        //                sbXml.Append("<DESCRIPTION_AR>").Append(dvConfindentials[i]["DESCRIPTION_AR"]).Append("</DESCRIPTION_AR>");
        //                sbXml.Append("<ACL_RESOURCE_NAME>").Append(dvConfindentials[i]["ACL_RESOURCE_NAME"]).Append("</ACL_RESOURCE_NAME>");
        //                sbXml.Append("<ACTIVE>").Append(dvConfindentials[i]["ACTIVE"]).Append("</ACTIVE>");
        //                sbXml.Append("</Confindential>");
        //            }
        //        }

        //        sbXml.Append("</Confindentials>");
        //        sbXml.Append("<PredefinedAssignees>");

        //        DataTable predefinedAssignees = GatewayService.GetAssignmentPaperAssignees(entityId, userId, languageAbbreviation).Tables[0];

        //        if (predefinedAssignees.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < predefinedAssignees.Rows.Count; i++)
        //            {
        //                sbXml.Append("<PredefinedAssignee>");
        //                sbXml.Append("<AssigneeName>").Append(predefinedAssignees.Rows[i]["AssigneeName"]).Append("</AssigneeName>");
        //                sbXml.Append("<IsPerson>").Append(predefinedAssignees.Rows[i]["IsPerson"]).Append("</IsPerson>");

        //                if (predefinedAssignees.Rows[i]["IsPerson"].ToString() == "1")
        //                {
        //                    sbXml.Append("<AssigneeId>").Append(predefinedAssignees.Rows[i]["AssigneeId"]).Append("</AssigneeId>");
        //                }
        //                else
        //                {
        //                    sbXml.Append("<AssigneeId>").Append(-1).Append("</AssigneeId>");
        //                }

        //                sbXml.Append("<ENTITYID>").Append(predefinedAssignees.Rows[i]["ENTITYID"]).Append("</ENTITYID>");

        //                if (predefinedAssignees.Rows[i]["IS_DYNAMIC"].ToString().ToLower() == "true")
        //                {
        //                    sbXml.Append("<IS_DYNAMIC>").Append(1).Append("</IS_DYNAMIC>");
        //                }
        //                else
        //                {
        //                    sbXml.Append("<IS_DYNAMIC>").Append(0).Append("</IS_DYNAMIC>");
        //                }

        //                sbXml.Append("</PredefinedAssignee>");
        //            }
        //        }

        //        sbXml.Append("</PredefinedAssignees>");
        //        sbXml.Append("</DOCUMENT>");
        //        sbXml.Append("</ELEMENTS>");
        //    }
        //    else
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        private Stream GetOriginalFileStream(string docId)
        {
            //ArchivesInfo objArchives = new ArchivesInfo();
            string[] IDs = new string[] { docId };
            //DataSet dsFileXml = objArchives.GetDocsByAttachRecordIDs(IDs);

            //if (dsFileXml != null && dsFileXml.Tables[0].Rows.Count > 0)
            //{
            //    if (dsFileXml.Tables[0].Rows[0][ArchivedItem.DMS_DOCDATA] != null &&
            //        dsFileXml.Tables[0].Rows[0][ArchivedItem.DMS_DOCDATA].ToString() != string.Empty)
            //    {
            //        byte[] byteArray = (byte[])dsFileXml.Tables[0].Rows[0][ArchivedItem.DMS_DOCDATA];

            //        return new MemoryStream(byteArray);
            //    }
            //}

            return null;
        }

        private string HashingFile(Stream fileStream)
        {
            using (HashAlgorithm hashAlg = new SHA1Managed())
            {
                byte[] hash = hashAlg.ComputeHash(fileStream);

                return BitConverter.ToString(hash);
            }
        }

        #region TO DO Later
        private Entity FillEntityData(OrgUnitDTO orgUntiDTO, Entity entity, List<UserProfileDTO> usersDTO)
        {
            if (orgUntiDTO.Number != "")
            {
                entity.UserDefinedId = orgUntiDTO.Number.ToString();
            }

            if (orgUntiDTO.ParentId != -1)
            {
                entity.ParentId = orgUntiDTO.ParentId;
            }

            entity.Active = true;

            if (orgUntiDTO.Name != null && orgUntiDTO.Name != string.Empty)
            {
                entity.Name = orgUntiDTO.Name;
            }

            List<Models.Person> persons = new List<Models.Person>();

            foreach (UserProfileDTO userProfileDTO in usersDTO)
            {
                Models.Person person = new Models.Person();

                person.EntityId = entity.Id;

                if (userProfileDTO.LocalName != null && userProfileDTO.LocalName != string.Empty)
                {
                    person.Name = userProfileDTO.LocalName;
                }
                if (userProfileDTO.Id == -1)
                {
                    person.Id = userProfileDTO.Id;
                }

                persons.Add(person);
            }

            entity.Persons = persons;

            return entity;
        }

        //private void FillEntityChilds(Entity entity, List<OrgUnitDTO> orgUntiDTO)
        //{
        //    Entity childEntity = null;
        //    entity.Childs = new List<Entity>();

        //    var orgUnitChildDTOs = orgUntiDTO.Where(ouc => ouc.ParentId == entity.Id).Select(o => new OrgUnitDTO
        //    {
        //        Id = o.Id,
        //        Number = o.Number,
        //        Name = o.Name,
        //        ParentId = o.ParentId,
        //        Users = o.Users,
        //        HasChilds = o.HasChilds
        //    }).ToList();

        //    foreach (OrgUnitDTO orgUnitChildDTO in orgUnitChildDTOs)
        //    {
        //        if (orgUnitChildDTO.Id != -1)
        //        {
        //            childEntity = new Entity();
        //            childEntity.Id = orgUnitChildDTO.Id;

        //            childEntity.Active = true;

        //            childEntity = FillEntityData(orgUnitChildDTO, childEntity, orgUnitChildDTO.Users);
        //            entity.Childs.Add(childEntity);
        //        }
        //    }

        //    foreach (Entity e in entity.Childs)
        //    {
        //        FillEntityChilds(e, orgUnitChildDTOs);
        //    }
        //}

        //private ExternalEntity FillExternalEntityData(DataRowView dr, ExternalEntity externalEntity, DataTable dtPersons)
        //{
        //    if (dr["PARENT_ID"] != null && dr["PARENT_ID"] != DBNull.Value)
        //    {
        //        externalEntity.ParentId = Convert.ToInt32(dr["PARENT_ID"]);
        //    }

        //    if (dr["ACTIVE"] != null && dr["ACTIVE"] != DBNull.Value)
        //    {
        //        externalEntity.Active = Convert.ToBoolean(dr["ACTIVE"]);
        //    }

        //    if (dr["NAME"] != null && dr["NAME"] != DBNull.Value)
        //    {
        //        externalEntity.Name = dr["NAME"].ToString();
        //    }

        //    if (dr["POBOX"] != null && dr["POBOX"] != DBNull.Value)
        //    {
        //        externalEntity.POBOX = dr["POBOX"].ToString();
        //    }

        //    if (dr["ZIPCODE"] != null && dr["ZIPCODE"] != DBNull.Value)
        //    {
        //        externalEntity.ZIPCODE = dr["ZIPCODE"].ToString();
        //    }

        //    if (dr["ADDRESS"] != null && dr["ADDRESS"] != DBNull.Value)
        //    {
        //        externalEntity.ADDRESS = dr["ADDRESS"].ToString();
        //    }

        //    if (dr["USERDEFINED_ID"] != null && dr["USERDEFINED_ID"] != DBNull.Value)
        //    {
        //        externalEntity.UserDefinedId = dr["USERDEFINED_ID"].ToString();
        //    }

        //    List<Models.PersonInfo> persons = new List<Models.PersonInfo>();
        //    DataView dvPersons = new DataView(dtPersons);

        //    dvPersons.RowFilter = "ENTITY_ID = " + externalEntity.Id + " AND ACTIVE = 1";

        //    for (int i = 0; i < dvPersons.Count; i++)
        //    {
        //        Models.PersonInfo person = new Models.PersonInfo();

        //        person.EntityId = externalEntity.Id;

        //        if (dvPersons[i]["NAME"] != null && dvPersons[i]["NAME"] != DBNull.Value)
        //        {
        //            person.Name = dvPersons[i]["NAME"].ToString();
        //        }
        //        if (dvPersons[i]["PERSON_ID"] != null && dvPersons[i]["PERSON_ID"] != DBNull.Value)
        //        {
        //            person.Id = Convert.ToInt32(dvPersons[i]["PERSON_ID"]);
        //        }

        //        persons.Add(person);
        //    }

        //    externalEntity.Persons = persons;

        //    return externalEntity;
        //}

        //private void FillExternalEntityChilds(ExternalEntity entity, DataView dvEntities, DataTable dtPersons)
        //{
        //    ExternalEntity childEntity = null;
        //    entity.Childs = new List<ExternalEntity>();

        //    dvEntities.RowFilter = "PARENT_ID = " + entity.Id + " AND ACTIVE = 1";

        //    for (int i = 0; i < dvEntities.Count; i++)
        //    {
        //        if (dvEntities[i]["ENTITY_ID"] != null && dvEntities[i]["ENTITY_ID"] != DBNull.Value)
        //        {
        //            childEntity = new ExternalEntity();
        //            childEntity.Id = Convert.ToInt32(dvEntities[i]["ENTITY_ID"]);

        //            if (dvEntities[i]["ACTIVE"] != null && dvEntities[i]["ACTIVE"] != DBNull.Value)
        //            {
        //                childEntity.Active = Convert.ToBoolean(dvEntities[i]["ACTIVE"]);
        //            }

        //            childEntity = FillExternalEntityData(dvEntities[i], childEntity, dtPersons);
        //            entity.Childs.Add(childEntity);
        //        }
        //    }

        //    foreach (ExternalEntity e in entity.Childs)
        //    {
        //        FillExternalEntityChilds(e, dvEntities, dtPersons);
        //    }
        //}
        #endregion

        private Stream ConvertTiff2PDF(Stream content, string userName)
        {
            string tempFolderPath = AppDomain.CurrentDomain.BaseDirectory.Replace("/", "\\") + "TempFiles" + "\\";

            try
            {
                TiffImageSplitter tiff = new TiffImageSplitter();
                PdfSharp.Pdf.PdfDocument doc = new PdfSharp.Pdf.PdfDocument();
                int pageCount = tiff.GetPageCount(content);

                for (int i = 0; i < pageCount; i++)
                {
                    PdfSharp.Pdf.PdfPage page = new PdfSharp.Pdf.PdfPage();
                    Image tiffImg = tiff.GetTiffImage(content, i);
                    XImage img = XImage.FromGdiPlusImage(tiffImg);

                    page.Width = img.PointWidth;
                    page.Height = img.PointHeight;

                    doc.Pages.Add(page);

                    XGraphics xgr = XGraphics.FromPdfPage(doc.Pages[i]);

                    xgr.DrawImage(img, 0, 0);
                }

                Stream pdfStream = new MemoryStream();

                doc.Save(pdfStream, false);

                pdfStream.Position = 0;

                return pdfStream;
            }
            catch (Exception ex)
            {
                FileStream exStream = new FileStream(tempFolderPath + userName + "_log.txt", FileMode.Append, FileAccess.Write);

                using (StreamWriter sw = new StreamWriter(exStream))
                {
                    sw.WriteLine(ex.ToString());

                    Exception innerEx = ex.InnerException;

                    while (innerEx != null)
                    {
                        sw.WriteLine(innerEx.Message);
                        innerEx = innerEx.InnerException;
                    }

                    sw.Close();
                    exStream.Close();
                }

                return null;
            }
        }

        private Stream ConvertWord2PDF(byte[] wordBytes, string userName)
        {
            string tempFolderPath = AppDomain.CurrentDomain.BaseDirectory.Replace("/", "\\") + "TempFiles" + "\\";

            try
            {
                Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();

                object oMissing = System.Reflection.Missing.Value;

                File.WriteAllBytes(tempFolderPath + userName + ".docx", wordBytes);

                word.Visible = false;
                word.ScreenUpdating = false;

                // Cast as Object for word Open method
                Object filename = (Object)tempFolderPath + userName + ".docx";

                // Use the dummy value as a placeholder for optional arguments
                Microsoft.Office.Interop.Word.Document doc = word.Documents.Open(ref filename, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                doc.Activate();

                object outputFileName = filename.ToString().Replace(".docx", ".pdf");
                object fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;

                // Save document into PDF Format
                doc.SaveAs(ref outputFileName,
                    ref fileFormat, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                // Close the Word document, but leave the Word application open.
                // doc has to be cast to type _Document so that it will find the
                // correct Close method.                
                object saveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;

                doc.Close(ref saveChanges, ref oMissing, ref oMissing);

                doc = null;

                // word has to be cast to type _Application so that it will find
                // the correct Quit method.
                word.Quit(ref oMissing, ref oMissing, ref oMissing);
                word = null;

                return new FileStream(outputFileName.ToString(), FileMode.Open);
            }
            catch (Exception ex)
            {
                FileStream exStream = new FileStream(tempFolderPath + userName + "_log.txt", FileMode.Append, FileAccess.Write);

                using (StreamWriter sw = new StreamWriter(exStream))
                {
                    sw.WriteLine(ex.ToString());

                    Exception innerEx = ex.InnerException;

                    while (innerEx != null)
                    {
                        sw.WriteLine(innerEx.Message);
                        innerEx = innerEx.InnerException;

                    }
                    sw.Close();
                    exStream.Close();
                }

                return null;
            }
        }

        private Stream ConvertWord2PDF(Stream content, string userName)
        {
            string tempFolderPath = AppDomain.CurrentDomain.BaseDirectory.Replace("/", "\\") + "TempFiles" + "\\";

            try
            {
                Spire.Doc.Document document = new Spire.Doc.Document();

                document.LoadFromStream(content, Spire.Doc.FileFormat.Auto);
                document.HtmlExportOptions.ImageEmbedded = true;
                document.HtmlExportOptions.CssStyleSheetType = CssStyleSheetType.External;

                //Save doc stream as html
                document.SaveToFile(tempFolderPath + userName + ".html", Spire.Doc.FileFormat.Html);

                FileStream fileStreamCSS = new FileStream(tempFolderPath + userName + "_styles.css", FileMode.Append, FileAccess.Write);

                using (StreamWriter sw = new StreamWriter(fileStreamCSS))
                {
                    sw.WriteLine(System.Configuration.ConfigurationManager.AppSettings["WordSettings"]);
                    sw.Close();
                    fileStreamCSS.Close();
                }

                Spire.Pdf.HtmlConverter.Qt.HtmlConverter.Convert(tempFolderPath + userName + ".html", tempFolderPath + userName + ".pdf", true,
                    1000 * 1000, new SizeF(PdfPageSize.A4), new PdfMargins(0, 0));

                FileStream fileStreamPDF = new FileStream(tempFolderPath + userName + ".pdf", FileMode.Open, FileAccess.Read);

                fileStreamPDF.Position = 0;

                return fileStreamPDF;
            }
            catch (Exception ex)
            {
                FileStream exStream = new FileStream(tempFolderPath + userName + "_log.txt", FileMode.Append, FileAccess.Write);

                using (StreamWriter sw = new StreamWriter(exStream))
                {
                    sw.WriteLine(ex.ToString());
                    Exception innerEx = ex.InnerException;

                    while (innerEx != null)
                    {
                        sw.WriteLine(innerEx.Message);
                        innerEx = innerEx.InnerException;
                    }

                    sw.Close();
                    exStream.Close();
                }

                return null;
            }
        }

        private Stream ConvertHtml2PDF(Stream content, string userName)
        {
            string tempFolderPath = AppDomain.CurrentDomain.BaseDirectory.Replace("/", "\\") + "TempFiles" + "\\";

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    content.CopyTo(memoryStream);
                    File.WriteAllText(tempFolderPath + userName + ".html", Encoding.UTF8.GetString(memoryStream.ToArray()), System.Text.Encoding.Unicode);
                }

                Spire.Pdf.HtmlConverter.Qt.HtmlConverter.Convert(tempFolderPath + userName + ".html", tempFolderPath + userName + ".pdf", true,
                    1000 * 1000, new SizeF(PdfPageSize.A4), new PdfMargins(0, 0));

                FileStream fileStreamPDF = new FileStream(tempFolderPath + userName + ".pdf", FileMode.Open, FileAccess.Read);

                fileStreamPDF.Position = 0;

                return fileStreamPDF;
            }
            catch (Exception ex)
            {
                FileStream exStream = new FileStream(tempFolderPath + userName + "_log.txt", FileMode.Append, FileAccess.Write);

                using (StreamWriter sw = new StreamWriter(exStream))
                {
                    sw.WriteLine(ex.ToString());

                    Exception innerEx = ex.InnerException;

                    while (innerEx != null)
                    {
                        sw.WriteLine(innerEx.Message);
                        innerEx = innerEx.InnerException;
                    }

                    sw.Close();
                    exStream.Close();
                }

                return null;
            }
        }

        private DocumentDTO ConvertToPDF(DocumentDTO document, string userName)
        {
            if (document != null && document.Content != null)
            {
                string mimeType = document.MimeType;

                byte[] byteArray = document.Content;
                Stream stream = new MemoryStream(byteArray);
                Stream pdfStream = null;
                switch (mimeType.ToLower())
                {
                    case System.Net.Mime.MediaTypeNames.Image.Tiff:
                        pdfStream = ConvertTiff2PDF(stream, userName);
                        break;
                    case System.Net.Mime.MediaTypeNames.Text.Html:
                        pdfStream = ConvertHtml2PDF(stream, userName);
                        break;
                    case System.Net.Mime.MediaTypeNames.Application.Octet:
                        pdfStream = ConvertWord2PDF(byteArray, userName);//CreatePDF(byteArray, userName);
                        break;
                }

                if (pdfStream != null)
                {
                    byte[] buffer = new byte[16 * 1024];

                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;

                        while ((read = pdfStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }

                        document.Content = ms.ToArray();
                        
                        document.Name = document.Name != null? document.Name.Split('.')[0] + ".pdf" : "morasalate.pdf" ;
                    }
                }

                return document;
            }

            return null;
        }
        private Stream CreatePDF(byte[] wordBytes, string userName)
        {
            string tempFolderPath = AppDomain.CurrentDomain.BaseDirectory.Replace("/", "\\") + "TempFiles" + "\\";
            var guid =  Guid.NewGuid();
            var path = tempFolderPath + userName +"_" +guid;
            File.WriteAllBytes(path + ".docx", wordBytes);


            Application app = new Application();
            app.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            app.Visible = false;
            var oldPath = path;
            var objPresSet = app.Documents;
            var objPres = objPresSet.Open(path + ".docx", MsoTriState.msoTrue, MsoTriState.msoTrue, MsoTriState.msoCTrue);
            var pdfPath = Path.ChangeExtension(path, ".pdf");

            try
            {
                objPres.ExportAsFixedFormat(
                    pdfPath,
                    WdExportFormat.wdExportFormatPDF,
                    false,
                    WdExportOptimizeFor.wdExportOptimizeForPrint,
                    WdExportRange.wdExportAllDocument
                );
            }
            catch
            {
                pdfPath = null;
            }
            finally
            { 
                objPres.Close();
                ((_Application)app).Quit();
                File.Delete(oldPath);
            }

            byte[] bPDF = System.IO.File.ReadAllBytes(pdfPath);
            System.IO.File.Delete(pdfPath);
            Stream stream = new MemoryStream(bPDF);
            return stream;
        }


        private bool PreRequest(string languageAbbreviation, out IHttpActionResult iHttpActionResult)
        {
            AuthenticationIdentity authenticationIdentity = Thread.CurrentPrincipal.Identity as AuthenticationIdentity;

            if (authenticationIdentity == null || string.IsNullOrEmpty(authenticationIdentity.UserName))
            {
                iHttpActionResult = BadRequest();

                return false;
            }

            DataResult result = new DataResult();
            DateTime lastLoginDate = DateTime.MinValue;

            LoginInfoDTO loginInfoDTO = new LoginInfoDTO()
            {
                UserName = authenticationIdentity.UserName,
                Password = string.Empty
            };

            PostObjectResult<string> postResultUserDTO =
               HttpClientWrapper<PostObjectResult<string>>.PostRequest("api/Login/LoginByMobile?cultureName=" + languageAbbreviation, loginInfoDTO, languageAbbreviation, string.Empty, -1).Result;

            Token = postResultUserDTO.Result;


            GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", authenticationIdentity.UserId, authenticationIdentity.UserName), languageAbbreviation, Token)
                                                  .Result;

            iHttpActionResult = Ok();

            if (getResultUserMobile.Result != null && getResultUserMobile.RowsCount == 1)
            {
                lastLoginDate = Convert.ToDateTime(getResultUserMobile.Result.LastLoginDate);
            }
            else
            {
                result.Code = MessageCode.UnauthenticatedUserOnMobile;
                result.Description = MessageResources.GetResourceText(ResourceText.UnauthenticatedUserOniPad, languageAbbreviation);

                iHttpActionResult = Content(HttpStatusCode.Forbidden, result);

                return false;
            }

            if (!Convert.ToBoolean(getResultUserMobile.Result.AllowMobile))
            {
                result.Code = MessageCode.UnauthenticatedUserOnMobile;
                result.Description = MessageResources.GetResourceText(ResourceText.UnauthenticatedUserOniPad, languageAbbreviation);

                iHttpActionResult = Content(HttpStatusCode.Forbidden, result);

                return false;
            }
            //if (!Convert.ToBoolean(getResultUserMobile.Result.AllowMobile))
            //{
            //    result.Code = MessageCode.UnauthenticatedUserOnMobile;
            //    result.Description = MessageResources.GetResourceText(ResourceText.UnauthenticatedUserOniPad, languageAbbreviation);

            //    iHttpActionResult = Content(HttpStatusCode.Forbidden, result);

            //    return false;
            //}

            //if (getResultUserMobile.Result.Token == null || getResultUserMobile.Result.Token.ToString() == string.Empty || Request.Headers.Authorization.ToString() != getResultUserMobile.Result.Token.ToString())
            //{
            //    result.Code = MessageCode.YouAreNotLoggedIn;
            //    result.Description = MessageResources.GetResourceText(ResourceText.InvalidUserName, languageAbbreviation);

            //    iHttpActionResult = Content(HttpStatusCode.Unauthorized, result);

            //    return false;
            //}

            GetResult<UserData> getResultUserData = HttpClientWrapper<GetResult<UserData>>
                                                    .GetItemRequest(string.Format("api/MobileApi/GetUserInfo?userName={0}", authenticationIdentity.UserName), languageAbbreviation, Token)
                                                    .Result;

            if (getResultUserData.Result == null && getResultUserData.RowsCount == 0)
            {
                result.Code = MessageCode.InvalidUserNameOrPassword;
                result.Description = MessageResources.GetResourceText(ResourceText.InvalidUserName, languageAbbreviation);

                iHttpActionResult = Content(HttpStatusCode.Unauthorized, result);

                return false;
            }

            //bool isTokenTimeout = Utilities.IsTokenTimedout(lastLoginDate);

            //if (!isTokenTimeout)
            //{
            //    result.Code = MessageCode.SessionTokenTimedOut;
            //    result.Description = MessageResources.GetResourceText(ResourceText.SessionTokenTimedOut, languageAbbreviation);

            //    iHttpActionResult = Content(HttpStatusCode.Unauthorized, result);

            //    return false;
            //}

            return true;
        }

        private bool PostRequest(string languageAbbreviation, out IHttpActionResult iHttpActionResult, bool isLogout = false)
        {
            iHttpActionResult = Ok();

            //No need to check the authenticationIdentity nullability, it is already checked in the PrePost
            AuthenticationIdentity authenticationIdentity = Thread.CurrentPrincipal.Identity as AuthenticationIdentity;

            DateTime lastLoginDate = Utilities.FormatDateTimeNow();

            string token = isLogout ? null : Token;

            UserMobile userMobileDTO = new UserMobile()
            {
                UserId = authenticationIdentity.UserId,
                Token = token,
                LastLoginDate = lastLoginDate
            };

            PutResult putResult = HttpClientWrapper<PutResult>
                                                  .PutRequest("api/MobileApi/UpdateUserMobile", languageAbbreviation, userMobileDTO, Token)
                                                  .Result;


            if (putResult.StatusCode != MCS.Common.StatusCode.Ok)
            {
                DataResult result = new DataResult
                {
                    Code = MessageCode.LogoutUnsuccessfully,
                    Description = MessageResources.GetResourceText(ResourceText.LogoutUnsuccessfully, languageAbbreviation)
                };

                iHttpActionResult = Content(HttpStatusCode.BadRequest, result);

                return false;
            }

            return true;
        }

        [HttpGet]
        public IHttpActionResult GetAssignmentPaper(string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;
                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }
                if (EntityId > 0)
                {
                    entityId = EntityId;
                }

                GetResult<List<TransAssignPaper>> getResultTransAssignPaper = HttpClientWrapper<GetResult<List<TransAssignPaper>>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetAssignmentPaper?userId={0}", userId), languageAbbreviation, Token)
                                                  .Result;

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok(getResultTransAssignPaper.Result);
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }

        public Entity MapEntity(UserMobileOrgUnitDTO userMobileOrgUnitDTO)
        {
            Entity entity = new Entity();
            entity.Id = userMobileOrgUnitDTO.Id;
            //  entity.IsCabinet 
            entity.ParentId = userMobileOrgUnitDTO.ParentId;
            entity.Active = userMobileOrgUnitDTO.Active;
            entity.UserDefinedId = userMobileOrgUnitDTO.UserDefinedId;
            entity.Name = userMobileOrgUnitDTO.Name;
            entity.IsVirtual = userMobileOrgUnitDTO.IsVirtual;
            entity.Persons = MapPerson(userMobileOrgUnitDTO.Persons);
            entity.HasChilds = userMobileOrgUnitDTO.HasChilds;

            return entity;
        }
        public Entity MapExternalParty(UserMobileExternalPartyDTO userMobileExternalPartyDTO)
        {
            Entity entity = new Entity();
            entity.Id = userMobileExternalPartyDTO.Id;
            //  entity.IsCabinet 
            entity.ParentId = userMobileExternalPartyDTO.ParentId;
            entity.Active = userMobileExternalPartyDTO.Active.HasValue ? userMobileExternalPartyDTO.Active.Value : false;
            entity.UserDefinedId = userMobileExternalPartyDTO.UserDefinedId;
            entity.Name = userMobileExternalPartyDTO.Name;
            entity.IsVirtual = userMobileExternalPartyDTO.IsVirtual;
            entity.Persons = MapPerson(userMobileExternalPartyDTO.Persons);


            return entity;
        }

        public List<Person> MapPerson(List<UserMobileOrgUnitUsersDTO> userMobileOrgUnitUsersDTOs)
        {
            List<Person> personInfos = new List<Person>();
            Person person;
            foreach (UserMobileOrgUnitUsersDTO userMobileOrgUnitUsersDTO in userMobileOrgUnitUsersDTOs)
            {
                person = new Person();
                person.Id = userMobileOrgUnitUsersDTO.Id;
                person.Name = userMobileOrgUnitUsersDTO.Name;
                person.EntityId = userMobileOrgUnitUsersDTO.EntityId;
                personInfos.Add(person);
            }

            return personInfos;
        }
        [HttpGet]
        public IHttpActionResult SetCopyAsViewed([FromUri] int transId, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;




                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                PutResult putResult = HttpClientWrapper<PutResult>
                                                      .PutRequest($"api/MobileApi/SetCopyAsViewed?transId={transId}&toUserId={userId}&toOrgUnit={entityId}", languageAbbreviation, Token)
                                                      .Result;

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                // SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }
        public IHttpActionResult DigitalSigning(int paperId, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;



                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                string ip = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostAddress);
                string systemAddress = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostName);

                GetResult<dynamic> putResult = HttpClientWrapper<GetResult<dynamic>>
                                                  .GetItemRequest(string.Format("api/Transaction/DigitalSigning?paperId={0}", paperId), languageAbbreviation, Token)
                                                  .Result;

                if (putResult.StatusCode != MCS.Common.StatusCode.Ok || putResult.Result.errorHappend == true)
                {
                    DataResult result = new DataResult
                    {
                        Code = MessageCode.CorrespondenceUpdatedUnsuccessfully,
                        Description = MessageResources.GetResourceText(ResourceText.CorrespondenceUpdatedUnsuccessfully, languageAbbreviation)
                    };

                    return Content(HttpStatusCode.BadRequest, result);
                }

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }
        [HttpPost]
        public IHttpActionResult UpdateTransactionDocument(DocumentDTO documentDTO, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;

                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                DataResult result = new DataResult();

                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                documentDTO.FromEntityId = entityId;
                documentDTO.FromUserId = userId;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/MobileApi/AddTransactionDocument"), documentDTO, languageAbbreviation, Token).Result;

                return Ok(postResult.StatusCode.ToString());
            }
            catch (Exception ex)
            {
                string inner = string.Empty;
                Exception innerException = ex.InnerException;

                while (innerException != null)
                {
                    inner += "\n" + ex.InnerException.Message;
                    innerException = innerException.InnerException;
                }
                // File.AppendAllText(LogFilePath, DateTime.Now.ToString() + "\n" + ex.StackTrace + "\n" + ex.Message + "\n" + inner + "\n");

                return null;
            }
        }
        [HttpPost]
        public IHttpActionResult RejectTransaction(int transId, string languageAbbreviation)
        {
            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;



                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }




                PutResult postResult = HttpClientWrapper<PutResult>
                                               .PutRequest($"api/MobileApi/RejectTransaction?transactionId={transId}&orgUnitId={entityId}&remarks=''&userId={userId}&cultureName={languageAbbreviation}"
                                               , null, Token).Result;




                return Ok();
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
            //try
            //{
            //    string message = string.Empty;
            //    int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
            //    GetResult<TransData> getResultTransData = HttpClientWrapper<GetResult<TransData>>
            //                                                     .GetItemRequest(string.Format("api/MobileApi/GetTransaction?userId={0}&transId={1}", userId, transData.TransId), languageAbbreviation, Token)
            //                                                     .Result;

            //    if (outboundInternalEditDTO.StatusCode == StatusCode.TransactionNotFound)
            //    {
            //        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, outboundInternalEditDTO.StatusCode.ToString());
            //        TempData["InfoMessage"] = new NotificationInformationVM { Message = message, MessageType = MessageType.Warning };
            //        return RedirectToAction("DashboardHome", "Shared");
            //    }
            //    var outboundInternalEditVM = OutboundInternalMapper.Map(outboundInternalEditDTO.Result);

            //    PutResult putResult =
            //        HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/RejectTransaction?transactionId={0}&orgUnitId={1}&trayActionTypeId={2}&remarks={3}&userId={4}",
            //        transactionId, SessionInfo.OrgUnitId, (int)trayActionType, remarks, SessionInfo.CurrentUser.Id), null).Result;

            //    if (putResult.StatusCode != StatusCode.Ok)
            //    {
            //        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

            //        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            //    }

            //    return Json("");


            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }
        [HttpPost]
        public IHttpActionResult SendTransaction(int transId, string languageAbbreviation)
        {
            try
            {

                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;

                GetResult<TransData> getResultTransData = HttpClientWrapper<GetResult<TransData>>
                                                              .GetItemRequest(string.Format("api/MobileApi/GetTransaction?userId={0}&transId={1}", userId, transId), languageAbbreviation, Token)
                                                              .Result;


                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                string ip = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostAddress);
                string systemAddress = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostName);

                TransactionAssignmentDTO transactionAssignmentDTO = new TransactionAssignmentDTO();
                GetResult<SettingDTO> SettingValue;
                GetResult<SettingDTO> DirectedToUser;
                switch ((int)getResultTransData.Result.StatusLevel)
                {
                    case 1:
                        SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/MobileApi/GetSettingValue?Key={0}", Constants.GeneralSettings.SecondOrgUnitFoldDown), languageAbbreviation, Token).Result;
                        DirectedToUser = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/MobileApi/GetSettingValue?Key={0}", Constants.GeneralSettings.SecondUserFoldDown), languageAbbreviation, Token).Result;
                        if (SettingValue.Result.Value != null)
                        {
                            getResultTransData.Result.ConcernedEntityId = int.Parse(SettingValue.Result.Value);
                            getResultTransData.Result.InitialAssignToPersonId = 0;
                            getResultTransData.Result.StatusLevel = 2;

                            transactionAssignmentDTO.ActionId = 3;
                            transactionAssignmentDTO.DeliveryMethodId = 236;
                            transactionAssignmentDTO.FromOrgUnitId = entityId;
                            transactionAssignmentDTO.FromUserId = userId;
                            transactionAssignmentDTO.ToOrgUnitId = getResultTransData.Result.ConcernedEntityId;
                            transactionAssignmentDTO.ToUserId = int.Parse(DirectedToUser.Result.Value);

                        }
                        break;
                    case 2:
                        SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/MobileApi/GetSettingValue?Key={0}", Constants.GeneralSettings.ThirdOrgUnitFoldDown), languageAbbreviation, Token).Result;
                        DirectedToUser = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/MobileApi/GetSettingValue?Key={0}", Constants.GeneralSettings.ThirdUserFoldDown), languageAbbreviation, Token).Result;
                        if (SettingValue.Result.Value != null)
                        {
                            getResultTransData.Result.ConcernedEntityId = int.Parse(SettingValue.Result.Value);
                            getResultTransData.Result.InitialAssignToPersonId = 0;
                            getResultTransData.Result.StatusLevel = 3;

                            transactionAssignmentDTO.ActionId = 3;
                            transactionAssignmentDTO.DeliveryMethodId = 236;
                            transactionAssignmentDTO.FromOrgUnitId = entityId;
                            transactionAssignmentDTO.FromUserId = userId;
                            transactionAssignmentDTO.ToOrgUnitId = getResultTransData.Result.ConcernedEntityId;
                            transactionAssignmentDTO.ToUserId = int.Parse(DirectedToUser.Result.Value);

                        }
                        break;
                    case 3:
                        SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/MobileApi/GetSettingValue?Key={0}", Constants.GeneralSettings.FortOrgUnitFoldDown), languageAbbreviation, Token).Result;
                        DirectedToUser = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/MobileApi/GetSettingValue?Key={0}", Constants.GeneralSettings.FortUserFoldDown), languageAbbreviation, Token).Result;
                        if (SettingValue.Result.Value != null)
                        {
                            getResultTransData.Result.ConcernedEntityId = int.Parse(SettingValue.Result.Value);
                            getResultTransData.Result.InitialAssignToPersonId = 0;
                            getResultTransData.Result.StatusLevel = 4;

                            transactionAssignmentDTO.ActionId = 3;
                            transactionAssignmentDTO.DeliveryMethodId = 236;
                            transactionAssignmentDTO.FromOrgUnitId = entityId;
                            transactionAssignmentDTO.FromUserId = userId;
                            transactionAssignmentDTO.ToOrgUnitId = getResultTransData.Result.ConcernedEntityId;
                            transactionAssignmentDTO.ToUserId = null;

                        }

                        break;
                    default:
                        SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/MobileApi/GetSettingValue?Key={0}", Constants.GeneralSettings.FirstOrgUnitFoldDown), languageAbbreviation, Token).Result;

                        if (SettingValue.Result.Value != null)
                        {
                            getResultTransData.Result.ConcernedEntityId = int.Parse(SettingValue.Result.Value);
                            getResultTransData.Result.InitialAssignToPersonId = 0;
                            getResultTransData.Result.StatusLevel = 2;

                            transactionAssignmentDTO.ActionId = 3;
                            transactionAssignmentDTO.DeliveryMethodId = 236;
                            transactionAssignmentDTO.FromOrgUnitId = entityId;
                            transactionAssignmentDTO.FromUserId = userId;
                            transactionAssignmentDTO.ToOrgUnitId = getResultTransData.Result.ConcernedEntityId;
                            transactionAssignmentDTO.ToUserId = null;


                        }
                        break;
                }


                PostResult postResult = HttpClientWrapper<PostResult>
                                               .PostRequest($"api/MobileApi/UpdateTransaction?userId={userId}&EntityId={entityId}&cultureName={languageAbbreviation}", getResultTransData.Result, languageAbbreviation, Token)
                                               .Result;
                if (getResultTransData.Result.StatusLevel != null)
                {


                    List<TransactionAssignmentDTO> transactionAssignmentDTOs = new List<TransactionAssignmentDTO>();

                    transactionAssignmentDTOs.Add(transactionAssignmentDTO);
                    // PostResult postAssign = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/PostAssignTransaction?transactionId={0}", getResultTransData.Result.TransId), transactionAssignmentDTOs).Result;
                    PostResult postAssign = HttpClientWrapper<PostResult>
                                                                     .PostRequest(string.Format("api/MobileApi/PostAssignTransaction?transactionId={0}", getResultTransData.Result.TransId), transactionAssignmentDTOs, languageAbbreviation, Token)
                                                                     .Result;
                }


                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                //SSSException.HandleException(ex, ExceptionContextConstants.GetCurrentContextKey());

                return null;
            }
        }


        [HttpPost]
        public IHttpActionResult RejectManagement([FromBody] TransStatus transStatus, string languageAbbreviation)
        {

            try
            {
                IHttpActionResult iHttpActionResult = Ok();

                if (!PreRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                string userName = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserName;
                int userId = ((AuthenticationIdentity)Thread.CurrentPrincipal.Identity).UserId;
                int entityId = -1;



                GetResult<UserMobile> getResultUserMobile = HttpClientWrapper<GetResult<UserMobile>>
                                                  .GetItemRequest(string.Format("api/MobileApi/GetUserMobile?userId={0}&userName={1}", userId, userName), languageAbbreviation, Token)
                                                  .Result;

                if (getResultUserMobile.Result != null & getResultUserMobile.RowsCount == 1)
                {
                    entityId = getResultUserMobile.Result.EntityId;
                }

                string ip = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostAddress);
                string systemAddress = Utilities.GetIP4Address(HttpContext.Current.Request.UserHostName);




                if (!PostRequest(languageAbbreviation, out iHttpActionResult))
                {
                    return iHttpActionResult;
                }

                return Ok();
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        protected byte[] GetBarcodeImage(TransactionBarcodesDTO transactionBarcodesDTOs, string languageAbbreviation)
        {
            //    GetResult<TransactionBarcodesDTO> transactionBarcodesDTOs =
            //HttpClientWrapper<GetResult<TransactionBarcodesDTO>>.GetItemRequest(string.Format("api/Transaction/GetTransactionBarcodes?cultureName={0}&transactionId={1}&orgUnitId={2}",)).Result;
            byte[] barcodeImg = null;

            BarcodeDTO barcode = transactionBarcodesDTOs.BarcodeDTOs.Where(b => b.Type == BarcodePrintType.Transaction).FirstOrDefault();

            if (barcode != null)
            {

                FillBarcodeDesign(transactionBarcodesDTOs.TransactionBarcodeHtmlDesign, barcode, transactionBarcodesDTOs, transactionBarcodesDTOs.TransactionDesignWidth, transactionBarcodesDTOs.TransactionDesignHeight);
                barcodeImg = barcode.Content;
            }
            return barcodeImg;
        }
        protected static byte[] GetBarcodeImageTopdf(TransactionBarcodesDTO transactionBarcodesDTOs, string languageAbbreviation)
        {
            //    GetResult<TransactionBarcodesDTO> transactionBarcodesDTOs =
            //HttpClientWrapper<GetResult<TransactionBarcodesDTO>>.GetItemRequest(string.Format("api/Transaction/GetTransactionBarcodes?cultureName={0}&transactionId={1}&orgUnitId={2}",)).Result;
            byte[] barcodeImg = null;

            BarcodeDTO barcode = transactionBarcodesDTOs.BarcodeDTOs.Where(b => b.Type == BarcodePrintType.Transaction).FirstOrDefault();

            if (barcode != null)
            {

                FillBarcodeDesign(transactionBarcodesDTOs.TransactionBarcodeHtmlDesign, barcode, transactionBarcodesDTOs, transactionBarcodesDTOs.TransactionDesignWidth, transactionBarcodesDTOs.TransactionDesignHeight);
                barcodeImg = barcode.Content;
            }
            return barcodeImg;
        }
        public static void FillBarcodeDesign(string HtmlDesign, BarcodeDTO barcodeVM, TransactionBarcodesDTO transactionBarcodesVM, int width, int heigth)
        {
            try
            {
                string s = DateCalendar(transactionBarcodesVM.TransactionDate, "ar", true);
                string barcode2D = MCS.MobileAPIs.Common.Barcode.DrawBarcode(barcodeVM.Value, BarcodeFormat.CODE_128, 45, 160);
                string barcode3D = MCS.MobileAPIs.Common.Barcode.DrawBarcode(barcodeVM.Value, BarcodeFormat.QR_CODE, 45, 45);
                string Logo = "~/Content/User/lib/images/BClogo.png";
                //if (SessionInfo.CultureShortName == Constants.Languages.English)
                //{
                //    HtmlDesign = HtmlDesign.Replace("direction", Constants.LeftDirection);
                //}
                //HtmlDesign = HtmlDesign.Replace("{1}", string.Empty);
                HtmlDesign = HtmlDesign.Replace("{1}", barcode2D);
                //HtmlDesign = HtmlDesign.Replace("{3}", string.Empty);
                HtmlDesign = HtmlDesign.Replace("{2}", barcode3D);
                HtmlDesign = HtmlDesign.Replace("{3}", Logo);


                HtmlDesign = HtmlDesign.Replace("{4}", "رقم المعاملة" + ":");
                HtmlDesign = HtmlDesign.Replace("{5}", ArabicDigitConverter.ConvertToArabic(transactionBarcodesVM.TransactionNumber.ToString()) + " / " + ArabicDigitConverter.ConvertToArabic("36") + " / " + ArabicDigitConverter.ConvertToArabic(s.Substring(6, 5)));



                HtmlDesign = HtmlDesign.Replace("{8}", "التاريخ" + " :  ");
                HtmlDesign = HtmlDesign.Replace("{9}", ArabicDigitConverter.ConvertToArabic(transactionBarcodesVM.TransactionDateH));

                //HtmlDesign = HtmlDesign.Replace("{16}", string.Empty);
                //HtmlDesign = HtmlDesign.Replace("{17}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TanentName") + " :  ");
                //HtmlDesign = HtmlDesign.Replace("{18}", SessionInfo.CurrentUser.TenantName);
                HtmlDesign = HtmlDesign.Replace("{6}", "المرفقات" + " :  ");

                string attachmentValue = "لا يوجد";

                if (transactionBarcodesVM.AttachmentBarcodes != null)
                {
                    foreach (AttachmentBarcodeDTO attachmentBarcodeVM in transactionBarcodesVM.AttachmentBarcodes)
                    {
                        attachmentValue = string.Empty;

                        attachmentValue = attachmentValue + string.Format("{0} {1} {2}", attachmentBarcodeVM.Count.ToString(), attachmentBarcodeVM.Name, ",");
                    }

                    attachmentValue = attachmentValue.TrimEnd(',');
                    HtmlDesign = HtmlDesign.Replace("{10}", attachmentValue);

                }
                else
                {
                    HtmlDesign = HtmlDesign.Replace("{10}", attachmentValue);
                }

                barcodeVM.Content = ConvertHtmlToImageBytes(HtmlDesign, width, heigth);
                barcodeVM.Templete = HtmlDesign;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string DateCalendar(DateTime DateConv, string DateLangCulture, bool WithDate = false)
        {
            DateConv = DateConv.AddDays(-1);
            DateTimeFormatInfo DTFormat;
            DateLangCulture = DateLangCulture.ToLower();
            DateLangCulture = "ar-sa";
            /// Set the date time format to the given culture
            DTFormat = new CultureInfo(DateLangCulture, false).DateTimeFormat;
            DTFormat.Calendar = new HijriCalendar();

            /// We format the date structure to whatever we want
            DTFormat.ShortDatePattern = "dd/MM/yyyy";
            if (WithDate == true)
            {
                return DateConv.Date.ToString("dd/MM/yyyy" + " " + DateConv.ToShortTimeString(), DTFormat);
            }
            else
            {
                return DateConv.Date.ToString("dd/MM/yyyy", DTFormat);

            }

        }
        public static byte[] ConvertHtmlToImageBytes(string htmlString, int width, int height)
        {
            try
            {
                //string header = "<head><meta charset='utf-8'></head>";


                var htmlToImageConv = new NReco.ImageGenerator.HtmlToImageConverter();

                htmlToImageConv.Width = width;

                htmlToImageConv.Height = height;

                return htmlToImageConv.GenerateImage(htmlString, NReco.ImageGenerator.ImageFormat.Png);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static byte[] AddDateAndDectionNumber(byte[] data, int Transid, TransactionBarcodesDTO transactionBarcodesVM)
        {

            int barcodeImageX = Convert.ToInt32(ConfigurationManager.AppSettings["BARCODE_IMAGE_X"].ToString());
            int barcodeImageY = Convert.ToInt32(ConfigurationManager.AppSettings["BARCODE_IMAGE_Y"].ToString());
            int barcodeImageScale = Convert.ToInt32(ConfigurationManager.AppSettings["BARCODE_IMAGE_SCALE"].ToString());

            //create pdfreader object to read sorce pdf
            PdfReader pdfReader = new PdfReader(data);
            //create stream of filestream or memorystream etc. to create output file
            using (MemoryStream msOutput = new MemoryStream())
            {
                //create pdfstamper object which is used to add addtional content to source pdf file
                PdfStamper pdfStamper = new PdfStamper(pdfReader, msOutput);
                iTextSharp.text.Rectangle rect = pdfReader.GetPageSizeWithRotation(1);
                BaseFont bf = BaseFont.CreateFont(@"C:\Windows\Fonts\ARIAL.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                iTextSharp.text.Font f = new iTextSharp.text.Font(bf, 18);
                PdfLayer layer = new PdfLayer("WatermarkLayer", pdfStamper.Writer);

                //iterate through all pages in source pdf
                long centerWidth = Convert.ToInt64(0.50 * rect.Width);
                long centerHeight = Convert.ToInt64(0.50 * rect.Height);
                //Rectangle class in iText represent geomatric representation... in this case, rectanle object would contain page geomatry

                //pdfcontentbyte object contains graphics and text content of page returned by pdfstamper
                PdfContentByte cb = pdfStamper.GetOverContent(1);
                cb.SetFontAndSize(BaseFont.CreateFont(
                                BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 6);
                if (Transid != null)
                {
                    iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(GetBarcodeImageTopdf(transactionBarcodesVM, "ar"));
                    //image1.scaleToFit(200, 200);
                    image1.ScalePercent(barcodeImageScale);
                    image1.SetAbsolutePosition(centerWidth - barcodeImageX, centerHeight + barcodeImageY);
                    cb.AddImage(image1);
                }
                //cb.BeginText();
                //string text = "Some random blablablabla...";
                //// put the alignment and coordinates here

                //// cb.ShowTextAligned(1, text, 520, 640, 0);
                //cb.EndText();



                pdfStamper.Close();

                return msOutput.ToArray();

            }
        }





    }
}