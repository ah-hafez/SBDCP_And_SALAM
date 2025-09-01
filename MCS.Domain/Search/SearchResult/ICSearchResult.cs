namespace MCS.Domain
{
    public class ICSearchResult : BaseSearchResult
    {

        public int MainDocId { get; set; }

        public int IsMain { get; set; }

        public int Permission { get; set; }

        public string GUID { get; set; }

        public int IsInIc { get; set; }

        public string IcName { get; set; }

        public int? OrderFileNumber { get; set; }
        public string Description { get; set; }



    }
}
