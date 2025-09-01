using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileApi.Models
{
    public class UserInfo
    {
        public DataResult Result { get; set; }
        public string SessionToken { get; set; }
        public string DeviceToken { get; set; }
        public string ActivationRequestCode { get; set; }
        public string ActivationCode { get; set; }
        public string DeactivationRequestCode { get; set; }
        public string SignedCert { get; set; }
        public string CACert { get; set; }
        public string CACRL { get; set; }
        public string OID { get; set; }
        public string CMC { get; set; }
        public string ActivationWebServiceURL { get; set; }
    }
}