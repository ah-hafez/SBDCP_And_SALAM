using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Domain
{
    public class MergeDepartment
    {
        public int Id { get; set; }
        public int MergedEntityId { get; set; }
        public int BaseEntityId { get; set; }
        public int ManagerId { get; set; }
        public List<Localization> NewEntityNames { get; set; }
    }
}
