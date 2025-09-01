using Microsoft.Office.Interop.Outlook;
using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using Exception = System.Exception;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MorasalatOutlookAddIn
{
    public static class Helper
    {
        private static UserDTO _userProfile;
        public static string GetEmailAddress
        {
            get
            {
                return new Application().Session.CurrentUser.AddressEntry.GetExchangeUser().PrimarySmtpAddress; //"m.farhan@sssit.net";
            }
        }
        public static string GetCultureName
        {
            get
            {
                int languageId = new Outlook.Application().LanguageSettings.get_LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI);
                //Microsoft languages ids
                int[] languageIds = new int[] { 0x0401, 0x1000, 0x3801, 0x4001, 0x2001, 0x2C01 };
                string languageTag = "en";
                if (languageIds.Contains(languageId))
                {
                    languageTag = "ar";
                }

                return "ar"; //languageTag;
            }
        }

        public static int? UserOrgUnitId
        {
            get
            {
                if (_userProfile == null)
                    return null;

                UserOrgUnitDTO userOrgUnitVM = _userProfile.UserOrgUnits.Where(u => u.IsSelected == true).FirstOrDefault();

                if (userOrgUnitVM != null)
                {
                    return userOrgUnitVM.Id;
                }

                return null;
            }
        }

        public static List<UserOrgUnitDTO> UserOrgUnits
        {
            get
            {
                if (_userProfile == null)
                    return null;

                 return _userProfile.UserOrgUnits.Where(u => u.IsSelected == true).ToList();

            }
        }

        public static string SessionId
        {
            get
            {
                if (_userProfile == null)
                    return null;

                return _userProfile.SessionId;
            }
        }

        public static int? UserId
        {
            get
            {
                if (_userProfile == null)
                    return null;

                return _userProfile.Id;
            }
        }

        public static string AccessToken
        {
            get
            {
                if (_userProfile == null)
                    return null;

                return _userProfile.AccessToken;
            }
        }


        public static UserDTO UserObj
        {
            set
            {
                if (_userProfile == null)
                {
                    _userProfile = value;
                }
            }
        }

        public enum OutlookFields
        {
            moraslatTransNo,
            moraslatTransId,
            moraslatAssignment
        }
        public enum ConfirgurationKeys
        {
            WebApiUrl,
            EmailDeliveryMethodId,
            ValidUploadExtenstions
        }


        public static void AddFieldToOutlook(Helper.OutlookFields fieldName, string fieldValue,Outlook.MailItem mailItem)
        {
            try
            {
                var f = mailItem.UserProperties.Find(fieldName.ToString());
                if (f == null)
                {
                    f = mailItem.UserProperties.Add(fieldName.ToString(), Outlook.OlUserPropertyType.olText);
                }
                if(fieldValue.Length>0)
                    f.Value = fieldValue;

                mailItem.Save();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }

        public static string GetOutlookFieldValue(Outlook.MailItem mailItem, Helper.OutlookFields fieldName)
        {
            var f = mailItem.UserProperties.Find(fieldName.ToString());
            if (f != null)
            {
                return f.Value;
            }
            return string.Empty;
        }



    }
}
