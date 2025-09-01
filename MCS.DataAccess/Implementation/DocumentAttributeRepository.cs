using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class DocumentAttributeRepository : BaseRepository<DocumentAttribute>, IDocumentAttributeRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public DocumentAttributeRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
             : base(ambienTTransactionContextLocator)
        {

        }


        #endregion Constructors

        #region Methods

        public List<DocumentAttribute> GetDocumentAttributes()
        {
            try
            {


                return _oMCSDbContext.DocumentAttributes.ToList();

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }



        #endregion
    }
}
