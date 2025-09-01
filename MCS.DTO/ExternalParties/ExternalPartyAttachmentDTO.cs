namespace MCS.DTO.ExternalParties
{
    public class ExternalPartyAttachmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DocumentDTO DocumentDTO { get; set; }
        public int PartyId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
