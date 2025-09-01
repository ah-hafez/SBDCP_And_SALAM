using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class SignedDeliveryReportBL : BaseBL, ISignedDeliveryReportBL
    {
        public IList<SignedDeliveryReport> GetSignedDeliveryReport(string date, int? orgunitId)
        {
            try
            {
                ISignedDeliveryReportRepository signedDeliveryReportRepository = IoC.Resolve<SignedDeliveryReportRepository>();
                return signedDeliveryReportRepository.GetSignedDeliveryReport(date , orgunitId);
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
