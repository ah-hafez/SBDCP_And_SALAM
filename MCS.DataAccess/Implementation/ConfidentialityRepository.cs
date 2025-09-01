using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework.Persistence;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class ConfidentialityRepository : BaseRepository<ConfidentialityLevel>, IConfidentialityRepository
    {
        public ConfidentialityRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
          : base(ambienTTransactionContextLocator)
        {

        }

        public IList<ConfidentialityLevel> GetConfidentialities(SearchCriteria searchCriteria, int groupId, out int rowsCount)
        {
            try
            {
         
                IQueryable<Group> group = _oMCSDbContext.Groups
                                                        .Where(g => g.Id == groupId)
                                                        .Select(g => g);

                rowsCount = group.SingleOrDefault().Permissions.Count();
                List<Permission> permissions;

                if (searchCriteria.Ascending)
                {
                    permissions = group.SingleOrDefault()
                                                      .Permissions
                                                      .OrderBy(p => p.Id)
                                                      .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                                      .Take(searchCriteria.PageSize)
                                                      .ToList();
                }
                else
                {
                    permissions = group.SingleOrDefault()
                                                   .Permissions
                                                   .OrderByDescending(p => p.Id)
                                                   .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                                   .Take(searchCriteria.PageSize)
                                                   .ToList();
                }

                List<ConfidentialityLevel> confidentialities = permissions.Select(p =>
                                                                            {
                                                                                ConfidentialityLevel confidentiality = new ConfidentialityLevel
                                                                                {
                                                                                    Localization = p.Name.Localizations.Select(l => new Localization
                                                                                    {
                                                                                        Id = l.Id,
                                                                                        Text = l.Text,
                                                                                        Culture = l.Culture
                                                                                    }).ToList()
                                                                                };
                                                                                return confidentiality;
                                                                            }).ToList();
                return confidentialities;


            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

    }
}
