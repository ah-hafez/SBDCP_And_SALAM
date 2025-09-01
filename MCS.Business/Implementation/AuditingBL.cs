using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.DataAccess;
using MCS.Framework;
using System.Collections.Generic;
using System;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Business
{
    public class AuditingBL : BaseBL, IAuditingBL
    {
        public void AddApiAuditLog(ApiAuditLog apiAuditLog)
        {
            try
            {
                AuditingRepository auditingRepository = IoC.Resolve<AuditingRepository>();
                auditingRepository.AddApiLog(apiAuditLog);
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

        public int GetLogBySignature(string signature)
        {
            try
            {
                AuditingRepository auditingRepository = IoC.Resolve<AuditingRepository>();
           return     auditingRepository.GetLogBySignature(signature);
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
