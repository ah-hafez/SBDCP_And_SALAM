namespace MobileApi.Domain
{
    public class ArchivingType 
    {
        public int TransSourceId { get; set; } = (int)enArchivingType.TransSourceID;
        public int IncludedItemId { get; set; } = (int)enArchivingType.IncludedItem;
        public int Explanations { get; set; } = (int)enArchivingType.Explaination;
    }
}
