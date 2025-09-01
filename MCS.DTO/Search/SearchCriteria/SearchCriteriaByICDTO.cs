using System;

namespace MCS.DTO
{
    public class SearchCriteriaByICDTO : BaseSearchCriteriaDTO
    {
        public int year { get; set; }

        public string  transNumber { get; set; }
        public int orgId { get; set; }
        public int type { get; set; }

        public string culutre { get; set; }

        public int userId { get; set; }

    }
}
