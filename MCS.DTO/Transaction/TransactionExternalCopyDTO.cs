using System;
using System.Collections.Generic;
using MCS.DTO.ExternalParties;

namespace MCS.DTO
{
    public class TransactionExternalCopyDTO
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
        public List<ExternalPartyAttachmentDTO> externalPartyAttachmentDTOs { get; set; }
        public int Status { get; set; }
        public int TransactionId { get; set; }
        public bool SendEmail { get; set; }
    }
}

