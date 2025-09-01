using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.YESSER.Proxy
{
    public static class HubConstantValues
    {
        public static class ErrorCodes
        {
            public const string FaultExceptionCode = "C000004002";
            public const string CryptographicExceptionCode = "C000004011";
            public const string ExceptionCode = "C000004002";
            public const string SSLNegotiationExceptionCode = "C000004006";
            public const string AuthenticationExceptionCode = "C000004005";
        }
        public static class FaultReasons
        {
            public const string FaultExceptionReason = "Internal Error";
            public const string CryptographicExceptionReason = "Internal Error, cannot decrypt the message";
            public const string ExceptionReason = "Internal Error";
            public const string SSLNegotiationExceptionReason = "Authentication failure. SSL negotiation faild";
            public const string AuthenticationExceptionReason = "Authentication failure The username and password received are not correct";
            public const string TenantExceptionReason = "Internal Error. Tenant doesn't exist";
        }
        public static class CloudResponses
        {
            public const string FailureResponse = "Failed";
            public const string NotFoundResponse = "Not Found";
            public const string SuccessResponse = "Success";
            public const string PendingResponse = "Pending";
            public const string ConfirmedResponse = "Accepted";
            public const string RejectResponse = "Rejected";
        }
    }
}