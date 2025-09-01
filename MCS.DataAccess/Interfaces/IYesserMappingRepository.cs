using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Common;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IYesserMappingRepository : IRepository<YesserMapping>
    {
        YesserMapping GetYesserMappedValue(YesserTypesMapping yesserTypesMapping, int cloudTypeId);
        YesserMapping GetCloudMappedValue(YesserTypesMapping yesserTypesMapping, string yesserTypeId);
        void UpdatePKById(int yesserMappingId, byte[] exponent, byte[] modulus);
        void UpdatePKByCloudId(int cloudTyeId, byte[] exponent, byte[] modulus);
        void AddNewEntity(string yesserID, string nameAR, string nameEN);
        List<YesserNewEntites> GetNewYesserEntities();
        List<YesserMapping> GetYesserMappedEntities(string cultureName);
        void SaveYesserMapping(int id, int cloudEntityId);
    }
}
