namespace MCS.DTO
{
    public class BaseSearchCriteriaDTO
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool Ascending { get; set; }
        public string CultureName { get; set; }
        public string OrderBy { get; set; }
        public int? OrgUnitId { get; set; }

        public int? UserId { get; set; }
        public int? TransactionCategoryId { get; set; }
        public bool Global { get; set; } = false;

    }
}
