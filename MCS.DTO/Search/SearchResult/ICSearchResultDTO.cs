namespace MCS.DTO
{
    public class ICSearchResultDTO : BaseSearchResultDTO
    {
        public int MainDocId { get; set; }

        public int IsMain { get; set; }

        public string GUID { get; set; }

        public int IsInIc { get; set; }

        public string IcName { get; set; }
        public int? OrderFileNumber { get; set; }
        public string Description { get; set; }
        public string ModifiedUser { get; set; }
        public string FullClassificationName { get; set; }

    }
}
