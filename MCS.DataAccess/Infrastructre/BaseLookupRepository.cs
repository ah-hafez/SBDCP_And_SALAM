using System;
using System.Data.Entity;
using System.Linq;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class BaseLookupRepository<T> : BaseRepository<T> where T : LookupBase
    {
        private readonly IDbSet<T> _dbSet;

        public BaseLookupRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {
            _dbSet = _oMCSDbContext.Set<T>();
        }
        public override void Update(T entity)
        {
            try
            {
                T oldEntity = FindBy(a => a.Id == entity.Id);
                _oMCSDbContext.Entry(oldEntity).CurrentValues.SetValues(entity);

                foreach (Localization localization in entity.LocalizationIdentifier.Localizations)
                {
                    Localization currentlocalization = oldEntity.LocalizationIdentifier.Localizations
                     .Where(l => l.Id == localization.Id)
                     .FirstOrDefault();
                    if (currentlocalization != null)
                    {
                        _oMCSDbContext.Entry(currentlocalization).CurrentValues.SetValues(localization);
                    }
                }
                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public override void Delete(int id)
        {
            try
            {
                T entityToDelete = FindBy(a => a.Id == id);
                if (entityToDelete != null)
                {
                    if (entityToDelete.LocalizationIdentifier != null)
                    {

                        int localizationCount = entityToDelete.LocalizationIdentifier.Localizations.Count;
                        for (int i = 0; i < localizationCount; i++)
                        {
                            _oMCSDbContext.Entry(entityToDelete.LocalizationIdentifier.Localizations[0]).State = EntityState.Deleted;
                        }
                        _oMCSDbContext.Entry(entityToDelete.LocalizationIdentifier).State = EntityState.Deleted;
                    }
                    _oMCSDbContext.Set<T>().Remove(entityToDelete);
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
