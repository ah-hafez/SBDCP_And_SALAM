using System;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class VIPTransactionAssignmentDTO
    {
        public int TrayId { get; set; }
        public int FromOrgUnitId { get; set; }

        public int? ToUserId { get; set; }

        [CustomRequired("User.Transaction.Assignment.ActionIdRequired")]
        public int ActionId { get; set; }

        [CustomRequired("User.Transaction.DeliveryMethodIdRequired")]
        public int DeliveryMethodId { get; set; }

        [CustomRequired("User.Transaction.Assignment.ToOrgUnitIdRequired")]
        public int ToOrgUnitId { get; set; }


        [CustomStringLength("User.Transaction.Assignment.RemarksLength", 500, 0)]
        public string Remarks { get; set; }

        public int? ReporterId { get; set; }

    }
}
