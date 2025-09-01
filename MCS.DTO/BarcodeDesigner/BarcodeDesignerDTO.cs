namespace MCS.DTO
{
    public class BarcodeDesignerDTO
    {
        public int Id { get; set; }        
        public int TypeId { get; set; }
        public bool IsGeneral { get; set; }  
        public string Html { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string HtmlAttachment { get; set; }
    }
}
