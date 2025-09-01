using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Common;

namespace MCS.DTO
{
    public class CredentialDTO
    {
        public string SignatureCurrentPasswordTxt { get; set; }
        public string SignatureNewPasswordTxt { get; set; }
        public PasswordType PasswordType { get; set; }
    }
}
