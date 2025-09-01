
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class BarcodeDesign : EntityBase
    {
       public string Html { get; set; }
       public bool IsGeneral { get; set; }
       public int TypeId { get; set; }
       public virtual Lookup Type { get; set; }
       public int Width { get; set; }
       public int Height { get; set; }
       public string AttachmentHtml { get; set; }
    }
}
