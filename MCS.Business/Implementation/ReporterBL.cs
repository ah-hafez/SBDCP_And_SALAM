using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class ReporterBL : BaseBL, IReporterBL
    {
        public int AddReporter(Reporter reporter)
        {
            try
            {
                IReporterRepository reporterRepository = IoC.Resolve<IReporterRepository>();
                return reporterRepository.AddReporter(reporter);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.RepoterExist);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public List<Reporter> GetReporters(string cultureName, int orgUnitId)
        {
            try
            {
                IReporterRepository reporterRepository = IoC.Resolve<ReporterRepository>();
                var result = reporterRepository.GetReporters(cultureName, orgUnitId);
                return result;
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

        public Reporter GetReporterById(int id, string cultureName)
        {
            try
            {
                IReporterRepository reporterRepository = IoC.Resolve<ReporterRepository>();
                var result = reporterRepository.GetReporterById(id, cultureName);
                return result;
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
