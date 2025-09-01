using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class UserCategoryTray : EntityBase, IAuditable
    {
        public int UserCategoryId { get; set; }
        public virtual UserCategory UserCategory { get; set; }
        public virtual Tray Tary { get; set; }
    }
}
