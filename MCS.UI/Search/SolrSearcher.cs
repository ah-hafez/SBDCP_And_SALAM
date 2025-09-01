using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.DSL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MCS.Framework.Controls.Mvc;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.UI;

namespace MCS.UI
{
    public class SolrSearcher : ISearcher
    {
        #region Attributes

        private static ISolrOperations<TransactionInfo> solrWorker = null;

        #endregion Attributes

        #region Constructors

        static SolrSearcher()
        {
            new SolrServiceFactory.Instance<TransactionInfo>().Start();

            solrWorker = ServiceLocator.Current.GetInstance<ISolrOperations<TransactionInfo>>();
        }

        #endregion Constructors

        #region Methods

        public IList<ISearchResult> Search(SearchCriteria searchCriteria, out int rowsCount)
        {
            searchCriteria.PageSize = GridHelper.PageSize;
            searchCriteria.PageIndex = (searchCriteria.PageIndex == 0) ? 1 : searchCriteria.PageIndex;

            int start = (searchCriteria.PageIndex - 1) * searchCriteria.PageSize;

            SolrQueryResults<TransactionInfo> solrQueryResults = solrWorker.Query(SolrQuery.All, new QueryOptions
            {
                FilterQueries = BuildFilterQueries(searchCriteria),
                Rows = searchCriteria.PageSize,
                Start = start,
                OrderBy = GetSelectedSort(searchCriteria),
                SpellCheck = new SpellCheckingParameters(),

                Facet = new FacetParameters
                {
                    Queries = SelectedFacetFields(searchCriteria)
                }
            });

            rowsCount = solrQueryResults.NumFound;

            IList<ISearchResult> searchResults = new List<ISearchResult>();

            foreach (TransactionInfo result in solrQueryResults)
            {
                ISearchResult searchResult = new SolrSearchResult();

                ((SolrSearchResult)searchResult).DocId = result.DocId;
                ((SolrSearchResult)searchResult).TypeId = result.TransactionTypeId;
                ((SolrSearchResult)searchResult).Number = result.Number;
                ((SolrSearchResult)searchResult).Barcode = result.Barcode;
                ((SolrSearchResult)searchResult).Subject = result.Subject;
                ((SolrSearchResult)searchResult).Date = result.Date;
                ((SolrSearchResult)searchResult).DateH = result.DateH;
                ((SolrSearchResult)searchResult).PartyNameAr = result.PartyNameAr;
                ((SolrSearchResult)searchResult).PartyNameEn = result.PartyNameEn;
                ((SolrSearchResult)searchResult).SignedByNameAr = result.SignedByNameAr;
                ((SolrSearchResult)searchResult).SignedByNameEn = result.SignedByNameEn;
                ((SolrSearchResult)searchResult).ConfidentialityNameAr = result.ConfidentialityNameAr;
                ((SolrSearchResult)searchResult).ConfidentialityNameEn = result.ConfidentialityNameEn;
                ((SolrSearchResult)searchResult).PriorityNameAr = result.PriorityNameAr;
                ((SolrSearchResult)searchResult).PriorityNameEn = result.PriorityNameEn;
                ((SolrSearchResult)searchResult).StatusNameAr = result.StatusNameAr;
                ((SolrSearchResult)searchResult).StatusNameEn = result.StatusNameEn;
                ((SolrSearchResult)searchResult).PermissionCode = result.PermissionCode;
                ((SolrSearchResult)searchResult).PriorityId = result.PriorityId;
                ((SolrSearchResult)searchResult).PartyId = result.PartyId;
                ((SolrSearchResult)searchResult).OrgUnitId = result.OrgUnitId;
                ((SolrSearchResult)searchResult).OrgUnitNameAr = result.OrgUnitNameAr;
                ((SolrSearchResult)searchResult).OrgUnitNameEn = result.OrgUnitNameEn;
                ((SolrSearchResult)searchResult).SignedByUserId = result.SignedByUserId;
                ((SolrSearchResult)searchResult).DirectedToUserId = result.DirectedToUserId;
                ((SolrSearchResult)searchResult).StatusId = result.StatusId;
                ((SolrSearchResult)searchResult).LetterTypeId = result.LetterTypeId;
                ((SolrSearchResult)searchResult).TransactionTypeNameAr = result.TransactionTypeNameAr;
                ((SolrSearchResult)searchResult).TransactionTypeNameEn = result.TransactionTypeNameEn;
                ((SolrSearchResult)searchResult).WithArchiving = result.WithArchiving;
                ((SolrSearchResult)searchResult).TransactionCategoryId = result.TransactionCategoryId;
                ((SolrSearchResult)searchResult).ColorCode = !String.IsNullOrEmpty(result.Color) ? Int32.Parse(result.Color) : 0;
                searchResults.Add(searchResult);
            }

            return searchResults;
        }

        private IList<ISolrQuery> BuildFilterQueries(SearchCriteria searchCriteria)
        {
            IList<ISolrQuery> solrQueries = new List<ISolrQuery>();

            foreach (Filter filter in searchCriteria.Filters)
            {
                switch (filter.Type)
                {
                    case FilterType.Contains:
                        //solrQueries.Add(new SolrQuery(filter.ColumnName + @":*" + filter.Value + "*"));
                        solrQueries.Add(new SolrQuery(filter.ColumnName + ":" + filter.Value));
                        break;
                    case FilterType.Equals:
                        {
                            if (filter.ColumnName == SearchFields.TransactionCategoryId &&
                                filter.Value == (TransactionCategory.Inbound).ToString())
                            {
                                solrQueries.Add((Query.Field(filter.ColumnName).Is(filter.Value) ||
                                    Query.Field(filter.ColumnName).Is((TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName)))));
                            }
                            else if (filter.ColumnName == SearchFields.OrgUnitId)
                            {
                                solrQueries.Add((Query.Field(filter.ColumnName).Is(filter.Value) ||
                                    Query.Field(SearchFields.Assignments).In<string>(filter.Value)) ||
                                    Query.Field(SearchFields.Assignments).Is<string>(null));
                            }
                            else if (filter.ColumnName == SearchFields.SubjectClassifications)
                            {
                                string[] subjectClassificationIds = filter.Value.Split(new string[] { "," },
                                  StringSplitOptions.None);
                                solrQueries.Add(
                              Query.Field(SearchFields.SubjectClassifications).In<string>(subjectClassificationIds));
                            }
                            else
                            {
                                solrQueries.Add(Query.Field(filter.ColumnName).Is(filter.Value));
                            }
                        }
                        break;
                }
            }

            SolrQueryByRange<DateTime?> dateRange =
                   new SolrQueryByRange<DateTime?>(SearchFields.Date, searchCriteria.FromDateTime, searchCriteria.ToDateTime);

            solrQueries.Add(dateRange);

            solrQueries.Add(Query.Field(SearchFields.PermissionCode).In<string>(SessionInfo.CurrentUser.Claims.ToArray()));

            return solrQueries;
        }

        private SortOrder[] GetSelectedSort(SearchCriteria searchCriteria)
        {
            string orderBy = searchCriteria.OrderBy;

            if (orderBy.EndsWith("Name"))
            {
                orderBy += CultureInfo.CurrentCulture.TextInfo.ToTitleCase(SessionInfo.CultureShortName);
            }
            else
            {
                orderBy = "DocId";
            }

            orderBy += (searchCriteria.Ascending) ? " ASC" : " DESC";

            return new[] { SortOrder.Parse(orderBy) }.Where(o => o != null).ToArray();
        }

        private ICollection<ISolrFacetQuery> SelectedFacetFields(SearchCriteria searchCriteria)
        {
            IEnumerable<string> fields = searchCriteria.Filters.Select(f => f.ColumnName);
            ICollection<ISolrFacetQuery> facetFields = new Collection<ISolrFacetQuery>();

            foreach (string field in fields)
            {
                facetFields.Add(new SolrFacetFieldQuery(field));
            }

            return facetFields;
        }

        #endregion Methods
    }
}
