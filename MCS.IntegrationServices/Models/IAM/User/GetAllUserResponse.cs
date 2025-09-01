using MCS.Common.ApiControllerResults;
using MCS.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class GetAllUserResponse : GetAllBaseResponse
    {
        [JsonProperty("users")]
        public List<UserDetailsResponse> Users { get; set; }

    }

    public class UserDetailsResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("username")]

        public string Username { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("categoryId")]
        public int? CategoryId { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("mainOrgUnitName")]
        public string MainOrgUnitName { get; set; }
        [JsonProperty("mainOrgUnitId")]
        public int MainOrgUnitId { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty("internalNumber")]
        public string InternalNumber { get; set; }

        [JsonProperty("titleId")]
        public int TitleId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("transactionProcessingPeriod")]
        public int TransactionProcessingPeriod { get; set; }
        [JsonProperty("orgUnits")]
        public List<OrgunitResponse> OrgUnits { get; set; }//OrgUnitDTO

        [JsonProperty("names")]
        public List<LocalizationVM> Names { get; set; }

        [JsonProperty("userNationalId")]
        public string UserNationalId { get; set; }
        [JsonProperty("isManager")]
        public bool IsManager { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("genderId")]
        public int GenderId { get; set; }

        [JsonProperty("userRoles")]
        public List<UserRoleResponse> UserRoles { get; set; }

        [JsonProperty("allowMobile")]
        public bool AllowMobile { get; set; }

    }

    public class UserRoleResponse
    {
        [JsonProperty("roleName")]
        public string RoleName { get; set; }
        [JsonProperty("roleId")]
        public int RoleId { get; set; }
    }

    public class OrgunitResponse
    {
        [JsonProperty("orgUnitName")]
        public string OrgUnitName { get; set; }
        [JsonProperty("orgUnitId")]
        public int OrgUnitId { get; set; }
    }

}