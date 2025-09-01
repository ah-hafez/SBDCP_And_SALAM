namespace MCS.UI.Areas.User.Models.Shared
{
    public class DocumentVM
    {
        public int Id { get; set; }
        public string EncryptedId { get; set; }
        public string MimeType { get; set; }
        public byte[] Content { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public bool IsDeleted { get; set; }
        public int FromUserId { get; set; }
        public string FromUserName { get; set; }
        public int FromEntityId { get; set; }
        public string FromEntityName { get; set; }

        public bool Mode { get; set; }
        public string[] MainDocumentImages { get; set; }

        public string Number { get; set; }
        public bool HidePrint { get; set; }
        public int DocumentId { get; set; }
        public string Key { get; set; }

    }
}