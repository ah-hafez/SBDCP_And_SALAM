using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class CultureRepository : BaseRepository<Culture>, ICultureRepository
    {
        #region Attributes

        

        #endregion Attributes

        #region Constructors

        public CultureRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {
            
        }

        #endregion Constructors

        #region Methods

        public List<Culture> GetCultures()
        {
            try
            {
                return _oMCSDbContext.Cultures.ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Culture GetCultureById(int cultureId)
        {
            try
            {
                return this.FindBy(c => c.Id == cultureId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion Methods
    }
}
