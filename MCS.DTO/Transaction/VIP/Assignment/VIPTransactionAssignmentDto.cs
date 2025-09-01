using MCS.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO.Transaction.Vip
{
    public class VIPTransactionAssignmentDto
    {
        public int Id { get; set; }
        public int? ToUserId { get; set; }
        public int ActionId { get; set; }
        public string ToUserName { get; set; }
        public string ToOrgUnitName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int ToOrgUnitId { get; set; }
        public string Remarks { get; set; }
        public bool IsAssigned { get; set; }
        public bool? IsCopy { get; set; } = false;
        public bool ChkConstant { get; set; }
        public int DeliveryMethodId { get; set; }
        public int FromUserId { get; set; }
        public int FromEntityId { get; set; }
        public string SpecialExplanation { get; set; }
        public string GeneralExplanation { get; set; }
        public int TransactionId { get; set; }
        public int TrayId { get; set; }

        public bool IsBcc { get; set; }
    }
}
