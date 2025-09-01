using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class Setting : EntityBase
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public byte[] BLOBValue { get; set; }
        public int? Type { get; set; }
        public string Description { get; set; }
        public int ModelId { get; set; }
        public string ResourceId { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
