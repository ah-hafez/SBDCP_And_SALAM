using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI;

namespace MCS.UI
{
    public class DatabaseSearcher : ISearcher
    {
        #region Methods

        public IList<ISearchResult> Search(SearchCriteria searchCriteria, out int rowsCount)
        {
            GetResult<List<TransactionDTO>> transactionDTOs =
                   HttpClientWrapper<GetResult<List<TransactionDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactions?cultureName={0}", SessionInfo.CultureShortName)).Result;

            rowsCount = 0;

            IList<ISearchResult> searchResults = new List<ISearchResult>();

            foreach (TransactionDTO transactionDTO in transactionDTOs.Result)
            {
                ISearchResult searchResult = new DatabaseSearchResult();

                searchResults.Add(searchResult);
            }

            return searchResults;
        }

        #endregion Methods
    }
}