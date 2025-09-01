using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class DocumentAttributeBL : BaseBL, IDocumentAttributeBL
    {

        public List<DocumentAttribute> GetDocumentAttributes()
        {
            try
            {
                IDocumentAttributeRepository documentAttributeRepository = IoC.Resolve<DocumentAttributeRepository>();
                return documentAttributeRepository.GetDocumentAttributes();
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
