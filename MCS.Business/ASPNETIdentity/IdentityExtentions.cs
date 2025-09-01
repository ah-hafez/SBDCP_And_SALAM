using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using MCS.Common;

namespace MCS.Business.ASPNETIdentity
{
    public class CustomizePasswordValidation : PasswordValidator
    {
        public int LengthRequired { get; set; }

        public CustomizePasswordValidation(int length)
        {
            LengthRequired = length;
        }

        public override Task<IdentityResult> ValidateAsync(string item)
        {

            if (String.IsNullOrEmpty(item))
            {
                return Task.FromResult(IdentityResult.Failed(StatusCode.PasswordRequired.ToString()));
            };

            if (string.IsNullOrEmpty(item) || item.Length < RequiredLength)
            {
                return Task.FromResult(IdentityResult.Failed(StatusCode.InvalidPasswordRequiredLength.ToString()));
            }

            if (RequireNonLetterOrDigit && item.All(c => !IsLetterOrDigit(c)))
            {
                return Task.FromResult(IdentityResult.Failed(StatusCode.PasswordRequireNonLetterOrDigit.ToString()));
            }

            if (RequireDigit && item.All(c => !IsDigit(c)))
            {
                return Task.FromResult(IdentityResult.Failed(StatusCode.PasswordRequireDigit.ToString()));
            }

            if (RequireLowercase && item.All(c => !IsLower(c)))
            {

                return Task.FromResult(IdentityResult.Failed(StatusCode.PasswordRequireLowercase.ToString()));
            }

            if (RequireUppercase && item.All(c => !IsUpper(c)))
            {
                return Task.FromResult(IdentityResult.Failed(StatusCode.PasswordRequireUppercase.ToString()));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
