using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class Tray : EntityBase
    {
        public int Sort { get; set; }
        public virtual Lookup Name { get; set; }
        public string LocalName { get; set; }
    }
}
