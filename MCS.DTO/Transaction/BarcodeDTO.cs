using MCS.Common;

namespace MCS.DTO
{
    public class BarcodeDTO
    {
        public string Value { get; set; }
        public byte[] Content { get; set; }
        public BarcodePrintType Type { get; set; }
        public string Templete { get; set; }
        public int ReferenceId { get; set; }
        public string EntityName { get; set; }
    }
}
