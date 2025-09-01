using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common.TransactionContext;
using MCS.Domain;
using System.Data.Entity;
using System.Linq.Expressions;

namespace MCS.DataAccess
{

    public class SystemDefaultValuesRepository : BaseRepository<SystemDefaultValues>, ISystemDefaultValuesRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public SystemDefaultValuesRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public IList<SystemDefaultValues> GetSystemDefaultValue()
        {
            try
            {
                return _oMCSDbContext.SystemDefaultValues.AsEnumerable().Select(t => new SystemDefaultValues
                {
                    Id = t.Id,
                    CategoryId = t.CategoryId,
                    TypeId = t.TypeId,
                    DefaultValueId = t.DefaultValueId
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion Methods
    }

}


