using System.Collections.Generic;

namespace MCS.Domain
{
    public class ChangeEntityName
    {
        public int EntityFromId { get; set; }
        public int EntityToId { get; set; }
        public virtual List<Localization> EntityFromLocalizations { get; set; }
        public virtual List<Localization> EntityToLocalizations { get; set; }
    }
}
