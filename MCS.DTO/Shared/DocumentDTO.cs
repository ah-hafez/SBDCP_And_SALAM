namespace MCS.DTO
{
    public class DocumentDTO
    {
        public int Id { get; set; }
        public string MimeType { get; set; }
        public byte[] Content { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public bool IsDeleted { get; set; }
        public string ECMID { get; set; }
        public int? FromUserId { get; set; }
        public int? FromEntityId { get; set; }
        public string FromUserName { get; set; }
        public string FromEntityName { get; set; }
        public string OldWordConent { get; set; }
        public int DocumentId { get; set; }


    }
}
