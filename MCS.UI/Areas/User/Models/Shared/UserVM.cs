using System.Collections.Generic;
using MCS.UI.Areas.User.Models.File;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.Shared
{
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
        public List<LocalizationVM> LoclizationUserCategory { get; set; }
        public List<string> Claims { get; set; }
        public string BaseOrgUnitName { get; set; }
        public List<TrayDetailsVM> TrayDetails { get; set; }
        public byte[] Signature { get; set; }
        public byte[] SignatureBehalf { get; set; }
        public byte[] SignatureCommand { get; set; }
        public byte[] Marking { get; set; }
        public byte[] MessageSignature { get; set; }
        public byte[] SealSignatureDoc { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string TenantName { get; set; }
        public byte[] TenantLogo { get; set; }

        public int? UserImageId { get; set; }

        public int CultureId { get; set; }
        public int ThemeId { get; set; }
        public string ThemePath { get; set; }
        public bool SMSNotifications { get; set; }
        public bool IsFollowUpUser { get; set; }
        public bool HasSignaturePasswordText { get; set; }
        public bool IsVIPUser { get; set; }
        public int DefaultDisplay { get; set; }
        public bool IsManager { get; set; }
        public string InternalNumber { get; set; }
        public string RequestId { get; set; }
        public bool DefaultAssignmentPaper { get; set; }



    }
}