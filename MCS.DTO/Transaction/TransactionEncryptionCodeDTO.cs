using MCS.Common;
using System;

namespace MCS.DTO
{
    public class TransactionEncryptionCodeDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public UserProfileDTO User { get; set; }
        public int OrgUnitId { get; set; }
        public OrgUnitDTO OrgUnit { get; set; }
        public DateTime CodeExpireDate { get; set; }
        public EncryptionChannel EncryptionChannel { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModefiedOn { get; set; }
        public int? ModefiedBy { get; set; }
    }
}
