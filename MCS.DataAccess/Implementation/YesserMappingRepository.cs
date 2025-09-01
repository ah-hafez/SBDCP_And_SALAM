using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class YesserMappingRepository : BaseRepository<YesserMapping>, IYesserMappingRepository
    {
        public YesserMappingRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        public YesserMapping GetYesserMappedValue(YesserTypesMapping yesserTypesMapping, int cloudTypeId)
        {
            var yesserMappingResult = _oMCSDbContext.YesserMappings.Where(yesserMapping => yesserMapping.TypeId == (int)yesserTypesMapping && yesserMapping.CloudTypeId == cloudTypeId).FirstOrDefault();
            return yesserMappingResult;
        }
        public YesserMapping GetCloudMappedValue(YesserTypesMapping yesserTypesMapping, string yesserTypeId)
        {
            var cloudMappingResult = _oMCSDbContext.YesserMappings.Where(yesserMapping => yesserMapping.TypeId == (int)yesserTypesMapping && yesserMapping.YesserTypeId == yesserTypeId).FirstOrDefault();
            return cloudMappingResult;
        }
        public List<YesserNewEntites> GetNewYesserEntities()
        {
            return _oMCSDbContext.YesserNewEntites.ToList();
        }
        public List<YesserMapping> GetYesserMappedEntities(string cultureName)
        {
            return (from y in _oMCSDbContext.YesserMappings.ToList()
                    where y.TypeId == (int)YesserTypesMapping.DestinationId
                    select new
                    {
                        TypeId = y.TypeId,
                        Id = y.Id,
                        CloudTypeId = y.CloudTypeId,
                        CloudType = y.CloudType,
                        YesserTypeId = y.YesserTypeId
                    }).ToList().Select(t => new YesserMapping
                    {
                        TypeId = t.TypeId,
                        Id = t.Id,
                        CloudTypeId = t.CloudTypeId,
                        CloudType = t.CloudType != null ? new ExternalParty
                        {
                            Id = t.CloudTypeId,
                            LocalName = t.CloudType.Name.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text
                        } : new ExternalParty(),
                        YesserTypeId = t.YesserTypeId,
                    }).AsQueryable().ToList();

        }
        public void SaveYesserMapping(int id, int cloudTypeId)
        {
            YesserNewEntites yesserNewEntity = _oMCSDbContext.YesserNewEntites.FirstOrDefault(y => y.Id == id);

            YesserMapping yesserMapping = new YesserMapping()
            {
                TypeId = 5,
                CloudTypeId = cloudTypeId,
                YesserTypeId = yesserNewEntity.YesserID
            };

            ExternalParty externalParty = _oMCSDbContext.ExternalParties.FirstOrDefault(e => e.Id == cloudTypeId && e.IsActive.Value);

            externalParty.YasserRegistered = true;

            _oMCSDbContext.YesserMappings.Add(yesserMapping);

            _oMCSDbContext.YesserNewEntites.Remove(yesserNewEntity);

            _oMCSDbContext.SaveChanges();
        }

        public void UpdatePKById(int yesserMappingId, byte[] exponent, byte[] modulus)
        {
            var yesserMapping = _oMCSDbContext.YesserMappings.Where(yM => yM.Id == yesserMappingId).FirstOrDefault();

            yesserMapping.Exponent = exponent;
            yesserMapping.Modulus = modulus;

            _oMCSDbContext.Entry(yesserMapping).State = System.Data.Entity.EntityState.Modified;
            _oMCSDbContext.SaveChanges();
        }

        public void UpdatePKByCloudId(int cloudTyeId, byte[] exponent, byte[] modulus)
        {
            var yesserMapping = _oMCSDbContext.YesserMappings.Where(
                yM => yM.CloudTypeId == cloudTyeId &&
            yM.TypeId == (int)YesserTypesMapping.DestinationId).FirstOrDefault();

            yesserMapping.Exponent = exponent;
            yesserMapping.Modulus = modulus;

            _oMCSDbContext.Entry(yesserMapping).State = System.Data.Entity.EntityState.Modified;
            _oMCSDbContext.SaveChanges();
        }

        public void AddNewEntity(string yesserID, string nameAR, string nameEN)
        {
            bool isEntityWaitingProcess = _oMCSDbContext.YesserNewEntites.Where(t => t.YesserID == yesserID).Any();

            if (!isEntityWaitingProcess)
            {
                YesserNewEntites entity = new YesserNewEntites();
                entity.YesserID = yesserID;
                entity.NameAr = nameAR;
                entity.NameEn = nameEN;

                _oMCSDbContext.YesserNewEntites.Add(entity);
                _oMCSDbContext.SaveChanges();
            }
        }
    }
}
