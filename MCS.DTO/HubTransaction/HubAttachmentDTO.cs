namespace MCS.DTO.HubTransaction
{
    public class HubAttachmentDTO
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public AttachmentTypeDTO Type { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public DocumentInfoDTO DocumentInfo { get; set; }
        public string ExternalAttachementId { get; set; }
    }
}
