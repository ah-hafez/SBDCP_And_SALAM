using System;

namespace MCS.IntegrationServices.Models
{
    public class ApiAuditLogVM
    {

        public int? UserId { get; set; }
        public string Machine { get; set; }
        public string RequestContentBody { get; set; }
        public string RequestContentType { get; set; }
        public string RequestIpAddress { get; set; }
        public string RequestMethod { get; set; }
        public string RequestHeaders { get; set; }
        public DateTime RequestTimestamp { get; set; }
        public string RequestUri { get; set; }
        public int ResponseStatusCode { get; set; }
        public DateTime ResponseTimestamp { get; set; }
        public string ResponseContentBody { get; set; }
        public string ResponseContentType { get; set; }
        public string ResponseHeaders { get; set; }
        public string Signature { get; set; }
    }
}