using System;

namespace MCS.DTO
{
    public class TransactionCopyDTO
    {
        public int Id { get; set; }
        public int Key { get; set; }
        public int? UserId { get; set; }

        public string UserName { get; set; }
        public int OrgUnitId { get; set; }

        public string OrgUnitName { get; set; }
        public int? FromUserId { get; set; }
        public string FromUserName { get; set; }
        public int FromOrgUnitId { get; set; }
        public string FromOrgUnitName { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public object[] ActionTypeId { get; set; }
        public int? IsSent { get; set; }

        public bool SendEmail { get; set; }
        
        public DateTime? SentDate { get; set; }
        public int Status { get; set; }
        public string SpecialExplanation { get; set; }
        public string GeneralExplanation { get; set; }
        public bool SpecialCopy { get; set; }
        public bool IsOpr { get; set; }
        public bool IsBcc { get; set; }

        public int? OprEntityId { get; set; }
        public string OprEntityName { get; set; }
        public string ViewedOnDateH { get; set; }
        public string ViewedBy { get; set; }

    }
}
