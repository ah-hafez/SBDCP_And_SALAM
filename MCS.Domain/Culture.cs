using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class Culture : EntityBase
    {
        public string ShortName { get; set; }
        public int? NameId { get; set; }
        public virtual Lookup Name { get; set; }
    }
}
