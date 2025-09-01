using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class SystemDefaultValuesBL : BaseBL, ISystemDefaultValuesBL
    {
        public IList<SystemDefaultValues> GetSystemDefaultValue()
        {
            try
            {
                ISystemDefaultValuesRepository systemDefaultValuesRepository = IoC.Resolve<SystemDefaultValuesRepository>();

                return systemDefaultValuesRepository.GetSystemDefaultValue();

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
