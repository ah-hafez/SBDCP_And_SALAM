using MCS.IntegrationServices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace MCS.IntegrationServices
{
    [Serializable()]
    public class UserVM
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public string SessionId { get; set; }
        public List<UserOrgUnitVM> UserOrgUnits { get; set; }
        public string Name { get; set; }
        public List<LocalizationVM> LoclizationName { get; set; }
        public string UserName { get; set; }
        public string UserCategoryName { get; set; }
        // public List<LocalizationVM> LoclizationUserCategory { get; set; }
        public List<string> Claims { get; set; }
        public string BaseOrgUnitName { get; set; }
        // public List<TrayDetailsVM> TrayDetails { get; set; }
        [JsonIgnore]
        public byte[] Signature { get; set; }
        [JsonIgnore]
        public byte[] Marking { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string EncryptionKey { get; set; }

    }
}