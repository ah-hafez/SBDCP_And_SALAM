using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Persistence;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class CityRepository : BaseRepository<City>, ICityRepository
    {
        public CityRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
          : base(ambienTTransactionContextLocator)
        {

        }

        public IList<City> GetCities(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<City> cities = (from city in _oMCSDbContext.Cities
                                          select city);

                rowsCount = cities.Count();

                if (searchCriteria.Ascending)
                {
                    cities = cities.OrderBy(p => p.Id);
                }
                else
                {
                    cities = cities.OrderByDescending(p => p.Id);
                }

                if (searchCriteria.PageSize > -1)
                {
                    cities = cities.Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize).Take(searchCriteria.PageSize);
                }

                return cities.ToList().Select(c => new City
                {
                    Id = c.Id,
                    CityId = c.CityId,
                    LocalizationIdentifier = c.LocalizationIdentifier
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
    }
}
