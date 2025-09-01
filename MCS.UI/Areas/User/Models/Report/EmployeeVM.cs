using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Report
{
    public class EmployeeVM
    {
        [CustomDisplayName("User.InboundCertificate.SignedBy")]
        public int EmployeeId { get; set; }
    }
}