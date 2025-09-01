using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Audit.EntityFramework;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    [AuditInclude]
    public class OrgUnit : EntityBase, ILocalizeEntity, IAuditable
    {
        #region Properties
        public int ManagerId { get; set; }
        public int? AssignmentPaperId { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public string Number { get; set; }
        public string BarCode { get; set; }
        public string LocalName { get; set; }
        public bool IsVirtualUnit { get; set; }
        public int TransactionsProcessingPeriod { get; set; }
        public bool IsDeleted { get; set; }
        public bool JoinToGeneralCounter { get; set; }
        public bool IsNew { get; set; }
        public bool HasChilds { get; set; }
        public string Lineage { get; set; }
        public int? ExternalId { get; set; }
        [NotMapped]
        public bool IsCurrentTreeRoot { get; set; }
        #endregion
        public virtual IList<UserProfile> Users { get; set; }
        public virtual Counter Counter { get; set; }
        public virtual AssignmentPaper AssignmentPaper { get; set; }
        public virtual OrgUnit Parent { get; set; }
        public virtual IList<OrgUnitLink> Links { get; set; }
        public virtual LocalizationIdentifier LocalizationIdentifier { get; set; }
        public virtual IList<BarcodeDesign> BarcodeDesigns { get; set; }
        public virtual IList<Reporter> Reporters { get; set; }
        public int? IoDepartment { get; set; }
        public int? FollowUpDepartment { get; set; }
        public bool IsExecutive { get; set; }
        public bool IsGeneralIoDepartment { get; set; }
        public bool ReceiveWithAcknowled { get; set; }
        public bool SendSpecialCopy { get; set; }
    }
}
