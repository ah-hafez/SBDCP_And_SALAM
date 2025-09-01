using System;
using MCS.Framework;
using MCS.Framework.Exceptions;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using System.Collections.Generic;

namespace MCS.Business
{
    public class YesserMappingBL : IYesserMappingBL
    {
        public YesserMapping GetYesserMappedValue(YesserTypesMapping yesserTypesMapping, int cloudTypeId)
        {
            IYesserMappingRepository yesserMappingRepository = IoC.Resolve<YesserMappingRepository>();
            var yesserMapping = yesserMappingRepository.GetYesserMappedValue(yesserTypesMapping, cloudTypeId);
            return yesserMapping;
        }
        public YesserMapping GetCloudMappedValue(YesserTypesMapping yesserTypesMapping, string yesserTypeId, bool throwException = true)
        {
            IYesserMappingRepository yesserMappingRepository = IoC.Resolve<YesserMappingRepository>();
            var yesserMapping = yesserMappingRepository.GetCloudMappedValue(yesserTypesMapping, yesserTypeId);

            if (throwException && yesserMapping == null)
            {
                throw new System.Exception("Invalid Mapping : " + yesserTypesMapping.ToString());
            }

            return yesserMapping;
        }
        public List<YesserNewEntites> GetNewYesserEntities()
        {
            IYesserMappingRepository yesserMappingRepository = IoC.Resolve<YesserMappingRepository>();

            return yesserMappingRepository.GetNewYesserEntities();
        }
        public List<YesserMapping> GetYesserMappedEntities(string cultureName)
        {
            IYesserMappingRepository yesserMappingRepository = IoC.Resolve<YesserMappingRepository>();

            return yesserMappingRepository.GetYesserMappedEntities(cultureName);
        }
        public void SaveYesserMapping(int id, int cloudEntityId)
        {
            IYesserMappingRepository yesserMappingRepository = IoC.Resolve<YesserMappingRepository>();

            yesserMappingRepository.SaveYesserMapping(id, cloudEntityId);
        }
        public void UpdatePKById(int yesserMappingId, byte[] exponent, byte[] modulus)
        {
            IYesserMappingRepository yesserMappingRepository = IoC.Resolve<YesserMappingRepository>();
            yesserMappingRepository.UpdatePKById(yesserMappingId, exponent, modulus);
        }
        public void UpdatePKByCloudId(int cloudTyeId, byte[] exponent, byte[] modulus)
        {
            IYesserMappingRepository yesserMappingRepository = IoC.Resolve<YesserMappingRepository>();
            yesserMappingRepository.UpdatePKByCloudId(cloudTyeId, exponent, modulus);
        }

        public void AddNewEntity(string yesserID, string nameAR, string nameEN)
        {
            IYesserMappingRepository yesserMappingRepository = IoC.Resolve<YesserMappingRepository>();
            yesserMappingRepository.AddNewEntity(yesserID, nameAR, nameEN);
        }
    }
}
