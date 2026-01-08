using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO.Search.SearchCriteria
{
    public class SearchCriteriaByICAndTransactionIdDTO : BaseSearchCriteriaDTO
    {

        public int transactionId { get; set; }

        public string culutre { get; set; }

        public int userId { get; set; }

    }
}
