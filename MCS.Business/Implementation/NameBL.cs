using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.DataAccess;
using MCS.Domain;


namespace MCS.Business
{
    public class NameBL : BaseBL, INameBL
    {
        public int AddName(Name name)
        {
            try
            {
                INameRepository nameRepository = IoC.Resolve<NameRepository>();

                return nameRepository.AddName(name);
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

        public void UpdateName(Name name)
        {
            try
            {
                INameRepository nameRepository = IoC.Resolve<NameRepository>();

                nameRepository.UpdateName(name);
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

        public Name GetNameByCivilId(string civilID)
        {
            try
            {
                INameRepository nameRepository = IoC.Resolve<NameRepository>();

                return nameRepository.GetNames(n => n.CivilID == civilID).ToList().FirstOrDefault();
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

        public List<Name> GetCivilIds()
        {
            try
            {
                INameRepository nameRepository = IoC.Resolve<NameRepository>();

                return nameRepository.GetCivilIds();
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
