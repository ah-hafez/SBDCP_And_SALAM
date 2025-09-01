using Audit.EntityFramework;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.InteropServices;

namespace MCS.Domain
{
    [AuditInclude]
    public class UserGroup : EntityBase, IAuditable
    {

        public int GroupId { get; set; }
        public int UserId { get; set; }

        [NotMapped]
        public string AdminUserName { get; set; }
        public virtual Group Group { get; set; }
        public virtual UserProfile User { get; set; }
    }
}
