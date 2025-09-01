using System.Collections.Generic;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.Lookups;
using AdminLookupVM = MCS.UI.Areas.Admin.Models.Lookups;
using AdminLookupMapper = MCS.UI.Areas.Admin.Mappers.LookupMapper;
using MCS.UI.Areas.User.Mappers.Transaction;
using System.Linq;
using System;
using MCS.UI.Areas.Admin.Models.OrgUnit;
using Newtonsoft.Json;
using System.Net.Http;
using System.Web.Mvc;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Areas.Admin.Models;

namespace MCS.UI
{
    public class OrgHelper
    {
        public static OrgUnitDTO GetOrgUnit(int orgUnitId, string cultureName)
        {
            IList<OrgUnitDTO> OrgUnits = CacheHelper.Get(CachedObjectsKey.OrgUnits, cultureName) as IList<OrgUnitDTO>;

            if (OrgUnits == null || !OrgUnits.Any(o => o.Id == orgUnitId))
            {
                GetResult<OrgUnitDTO> orgUnitDTO =
                  HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, orgUnitId)).Result;

                if (OrgUnits == null)
                {
                    OrgUnits = new List<OrgUnitDTO>();
                }

                OrgUnits.Add(orgUnitDTO.Result);

                CacheHelper.Remove(CachedObjectsKey.OrgUnits, cultureName);
                CacheHelper.Insert(CachedObjectsKey.OrgUnits, OrgUnits, cultureName);
            }

            return OrgUnits.First(o => o.Id == orgUnitId);
        }

        public static List<OrgUnitDTO> GetOrgUnits(int? parentId, int? userId, string cultureName)
        {
            string CacheKey = CachedObjectsKey.OrgUnits;

            CacheKey = CacheKey + "_ParentId_" + (parentId.HasValue ? parentId.Value.ToString() : "-1");

            if (userId.HasValue)
            {
                CacheKey = CacheKey + "_UserId_" + (userId.HasValue ? userId.Value.ToString() : "-1");
            }

            List<OrgUnitDTO> OrgUnits = CacheHelper.Get(CacheKey, cultureName) as List<OrgUnitDTO>;

            if (OrgUnits == null)
            {
                var orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>
                  .GetItemRequest(string.Format("api/Common/GetOrgUnits?cultureName={0}&parentId={1}&UserId={2}", SessionInfo.CultureShortName, parentId, userId)).Result;

                OrgUnits = orgUnitDTOs.Result;

                CacheHelper.Insert(CacheKey, OrgUnits, cultureName);
            }

            return OrgUnits;
        }


        public static ExternalPartyDTO GetExternalParty(int externalPartyId)
        {
            ExternalPartyDTO externalPartyDTO = new ExternalPartyDTO();
            GetResult<ExternalPartyDTO> partyEditDTO =
              HttpClientWrapper<GetResult<ExternalPartyDTO>>.GetItemRequest(String.Format("api/Common/GetExternalParty?id={0}", externalPartyId)).Result;
            externalPartyDTO = partyEditDTO.Result;
            return externalPartyDTO;
        }

        public static void UpdateOrgUnitService()
        {
            UpdateSectorDataSAP();
            UpdateSubSectorDataSAP();
            UpdateDivisionAP();
            UpdateDepartmentSAP();
            UpdateSectionSAP();
        }



        //1
        public static void UpdateSectorDataSAP()
        {
            string message = "";
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, SystemConfigurations.SapUrl.ToString() + "v2/cust_Sector?&$select=externalCode,mdfSystemStatus,externalName_ar_SA,externalName_en_US/externalCode&$format=json");
            request.Headers.Add("Authorization", "Basic " + SystemConfigurations.SapToken.ToString());
            var response = client.SendAsync(request).Result;
            var resultJson = response.Content.ReadAsStringAsync().Result;
            var resultVm = JsonConvert.DeserializeObject<SectorSapVM>(resultJson);
            var resultDto = OrgUnitMapper.Map(resultVm);

            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/UpdateOrgunitSAP", resultDto).Result;

            if (postResult.StatusCode != StatusCode.Ok)
            {
                throw new Exception();
            }
        }
        //2
        public static void UpdateSubSectorDataSAP()
        {
            string message = "";
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, SystemConfigurations.SapUrl.ToString() + "v2/cust_Sub_Sector?$format=JSON&$expand=cust_Sector&$select=externalCode,cust_Name_ar_SA,cust_Name_en_US,mdfSystemStatus,cust_Sector/externalCode&$format=json");
            request.Headers.Add("Authorization", "Basic " + SystemConfigurations.SapToken.ToString());
            var response = client.SendAsync(request).Result;
            var resultJson = response.Content.ReadAsStringAsync().Result;
            var resultVm = JsonConvert.DeserializeObject<SubSectorVM>(resultJson);
            var resultDto = OrgUnitMapper.Map(resultVm);

            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/UpdateOrgunitSAP", resultDto).Result;

            if (postResult.StatusCode != StatusCode.Ok)
            {
                throw new Exception();
            }
        }

        //3
        public static void UpdateDivisionAP()
        {
            string message = "";
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, SystemConfigurations.SapUrl.ToString() + "v2/FODivision?$format=JSON&$expand=cust_Sector&$select=externalCode,name_ar_SA,name_en_US,status,cust_Sector/externalCode&$format=json");
            request.Headers.Add("Authorization", "Basic " + SystemConfigurations.SapToken.ToString());
            var response = client.SendAsync(request).Result;
            var resultJson = response.Content.ReadAsStringAsync().Result;
            var resultVm = JsonConvert.DeserializeObject<DivisionSapVM>(resultJson);
            var resultDto = OrgUnitMapper.Map(resultVm);
            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/UpdateOrgunitSAP", resultDto).Result;

            if (postResult.StatusCode != StatusCode.Ok)
            {
                throw new Exception();
            }
        }
        //3
        public static void UpdateDepartmentSAP()
        {
            string message = "";
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, SystemConfigurations.SapUrl.ToString() + "v2/FODepartment?$select=externalCode,name_ar_SA,name_en_US,status,cust_toDivision/externalCode&$format=json&$format=JSON&$expand=cust_toDivision&$select=externalCode");
            request.Headers.Add("Authorization", "Basic " + SystemConfigurations.SapToken.ToString());
            var response = client.SendAsync(request).Result;
            var resultJson = response.Content.ReadAsStringAsync().Result;
            var resultVm = JsonConvert.DeserializeObject<DepartmentSapVM>(resultJson);
            var resultDto = OrgUnitMapper.Map(resultVm);

            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/UpdateOrgunitSAP", resultDto).Result;

            if (postResult.StatusCode != StatusCode.Ok)
            {
                throw new Exception();
            }
        }
        //4
        public static void UpdateSectionSAP()
        {
            string message = "";
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, SystemConfigurations.SapUrl.ToString() + "v2/cust_Section?$format=JSON&$expand=cust_toDepartment");
            request.Headers.Add("Authorization", "Basic " + SystemConfigurations.SapToken.ToString());
            var response = client.SendAsync(request).Result;
            var resultJson = response.Content.ReadAsStringAsync().Result;
            var resultVm = JsonConvert.DeserializeObject<SectionSapVM>(resultJson);
            var resultDto = OrgUnitMapper.Map(resultVm);

            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/UpdateOrgunitSAP", resultDto).Result;

            if (postResult.StatusCode != StatusCode.Ok)
            {
                throw new Exception();
            }
        }




    }
}
