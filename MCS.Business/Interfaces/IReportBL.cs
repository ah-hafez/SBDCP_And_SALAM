using System.Collections.Generic;
using MCS.Domain;

namespace MCS.Business
{
    public interface IReportBL
    {
        IList<TransactionReportResult> TransactionReportSearch(SearchCriteriaTransactionReport searchCriteriaTransactionReport, out int TotalCount);

    }
}
