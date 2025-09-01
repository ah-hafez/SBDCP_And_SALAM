using System.Collections.Generic;

namespace MCS.DTO
{
    public class OrgUnitDTO
    {
        public int Id { get; set; }
        public int Key { get; set; }
        public string Name { get; set; }
        public string BarCode { get; set; }
        public string Number { get; set; }
        public int ParentId { get; set; }
        public bool IsVirtualUnit { get; set; }
        public bool IsSelected { get; set; }
        public bool IsActive { get; set; }
        public string Lineage { get; set; }
        public bool HasChilds { get; set; }
        public bool IsYesserRegistered { get; set; }
        public bool IsCurrentTreeRoot { get; set; }
        public List<int> LinkUnitsKeys { get; set; }
        public CounterDTO Counter { get; set; } = new CounterDTO();
        public List<UserProfileDTO> Users { get; set; } = new List<UserProfileDTO>();
        public List<BarcodeDesignerDTO> BarcodeDesigns { get; set; } = new List<BarcodeDesignerDTO>();
        public int? FollowupDepartment { get; set; }
    }
}
