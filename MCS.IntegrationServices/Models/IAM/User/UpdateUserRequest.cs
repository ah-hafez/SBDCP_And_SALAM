using MCS.Common.CustomAttributes;
using MCS.DTO;
using MCS.IntegrationServices.Models.IAM.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class UpdateUserRequest : BaseRequest
    {
        [JsonProperty("id")]
        [Required]
        public int Id { get; set; }
        [CustomRequired("Admin.User.UserNameRequired")]
        [CustomStringLength("Admin.User.UserNameLength", 50, 0)]
        [JsonProperty("username")]
        public string Username { get; set; }

        [CustomRequired("Admin.User.TitleRequired")]
        [JsonProperty("titleId")]
        public int TitleId { get; set; }

        [CustomRequired("Admin.User.EmployeeCategoryRequired")]
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }

        [CustomRequired("Admin.User.ProcessingTimeRequired")]
        [CustomStringLength("Admin.User.ProcessingTimeLength", 3, 1)]
        [CustomRegularExpression("^[1-9][0-9]*$", "Admin.User.ProcessingTimeExpression")]
        [JsonProperty("transactionProcessingPeriod")]
        public int TransactionProcessingPeriod { get; set; }
        [JsonProperty("orgUnits")]
        public List<int> OrgUnits { get; set; }//OrgUnitDTO
        [CustomEmailAddress("Admin.User.EmailSyntax")]
        [CustomStringLength("Admin.User.EmailLength", 50, 0)]
        [JsonProperty("email")]
        public string Email { get; set; }

        [CustomStringLength("Admin.User.PhoneNumberLength", 12, 0)]
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty("names")]
        public List<LocalizationRequest> Names { get; set; }
        //[JsonProperty("roles")]
        //public List<int> Roles { get; set; }

        [CustomStringLength("Admin.User.UserNationalityIdLength", 10, 0)]
        [JsonProperty("userNationalId")]
        public string UserNationalId { get; set; }
        [JsonProperty("isManager")]
        public bool IsManager { get; set; }
        [JsonProperty("genderId")]
        public int GenderId { get; set; }
        [JsonProperty("mainOrgUnitId")]
        public int MainOrgUnitId { get; set; }
        [JsonProperty("userRoles")]
        public List<int> UserRoles { get; set; }
        [JsonProperty("internalNumber")]
        public string InternalNumber { get; set; }
        [JsonProperty("allowMobile")]
        public bool AllowMobile { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

    }





}