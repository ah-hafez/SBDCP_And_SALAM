using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ICityRepository
    {
        IList<City> GetCities(SearchCriteria searchCriteria, out int rowsCount);
    }
}
