using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class NameRepository : BaseRepository<Name>, INameRepository
    {

        #region Attributes



        #endregion Attributes

        #region Constructors

        public NameRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddName(Name name)
        {
            try
            {
                _oMCSDbContext.Names.Add(name);

                _oMCSDbContext.SaveChanges();

                return name.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateName(Name name)
        {
            try
            {
                Name oldName = GetNameById(name.Id);

                if (oldName != null)
                {
                    _oMCSDbContext.Entry(oldName).CurrentValues.SetValues(name);

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Name> GetNames(Expression<Func<Name, bool>> @where)
        {
            try
            {
                return _oMCSDbContext.Names.Where(@where).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Name GetNameById(int nameId)
        {
            try
            {
                return this.FindBy(n => n.Id == nameId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<Name> GetCivilIds()
        {
            try
            {
                return _oMCSDbContext.Names.AsEnumerable().Select(t => new Name {
                    Id = t.Id,
                    CivilID = t.CivilID,
                    Address = t.Address,
                    Email = t.Email,
                    FirstName = t.FirstName,
                    MobileNumber = t.MobileNumber,
                    NationalityId = t.NationalityId,
                    Phone = t.Phone
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion
    }
}
