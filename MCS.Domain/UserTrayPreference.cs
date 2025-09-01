using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class UserTrayPreference : EntityBase
    {
        public int TrayId { get; set; }
        public virtual Tray Tray { get; set; }
    }
}
