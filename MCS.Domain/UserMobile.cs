using System;
using System.ComponentModel.DataAnnotations.Schema;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class UserMobile : EntityBase
    {
        [ForeignKey("UserProfile")]
        public int UserId { get; set; }
        public string Token { get; set; }
        public string DeviceToken { get; set; }
        public string ActivationRequestCode { get; set; }
        public string ActivataionCode { get; set; }
        public string DeactivationRequestCode { get; set; }
        public string SignedCert { get; set; }
        public string CA { get; set; }
        public string CACRL { get; set; }
        public bool IsUpdated { get; set; }
        public int UpdateFlags { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public byte[] Logs { get; set; }
        public string Settings { get; set; }
        public string LoginName { get; set; }
        public int EntityId { get; set; }
        public bool AllowMobile { get; set; }
        public UserProfile UserProfile { get; set; }
        public int? DefaultEntityId { get; set; }
        public virtual OrgUnit DefaultEntity { get; set; }
        [NotMapped]
        public int? UserMobileClassId { get; set; }

    }
}