using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YESSER.NCS.MCS.DTO;
using YESSER.NCS.MCS.UI.Areas.User.Models.UserPreferences;

namespace YESSER.NCS.MCS.UI.Areas.User.Mappers.UserPreferences
{
    public static class CredentialMapper
    {
        public static CredentialDTO Map(CredentialVM credentialVM)
        {
            if (credentialVM == null)
            {
                return null;
            }
            var result = new CredentialDTO();
            result.PasswordType = credentialVM.PasswordType;
            result.SignatureCurrentPasswordTxt = credentialVM.SignatureCurrentPasswordTxt;
            result.SignatureNewPasswordTxt = credentialVM.SignatureNewPasswordTxt;
            return result;
        }
    }
}