using System.Collections.Generic;

namespace MCS.UI.Areas.User.Models.OrgUnit
{
    public class OrgUnitVM
    {
        public int Id { get; set; }
        public int Key { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public int ParentId { get; set; }
        public bool IsVirtualUnit { get; set; }
        public bool IsSelected { get; set; }
        public bool HasChilds { get; set; }
        public string Lineage { get; set; }
        public List<int> LinkUnitsKeys { get; set; }
        public bool IsCurrentTreeRoot { get; set; }
    }
}