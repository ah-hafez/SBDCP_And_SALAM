using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class CityBL : BaseBL, ICityBL
    {
        public IList<City> GetCities(SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                ICityRepository citiesRepository = IoC.Resolve<CityRepository>();
                return citiesRepository.GetCities(searchCriteria, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
    }
}
