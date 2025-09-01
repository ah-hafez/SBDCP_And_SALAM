using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public interface IYesserMappingBL
    {
        YesserMapping GetYesserMappedValue(YesserTypesMapping yesserTypesMapping, int cloudId);
        YesserMapping GetCloudMappedValue(YesserTypesMapping yesserTypesMapping, string yesserTypeId, bool throwException = true);
        List<YesserNewEntites> GetNewYesserEntities();
        void UpdatePKById(int yesserMappingId, byte[] exponent, byte[] modulus);
        void UpdatePKByCloudId(int cloudTyeId, byte[] exponent, byte[] modulus);
        void AddNewEntity(string yesserID, string nameAR, string nameEN);
        void SaveYesserMapping(int id, int cloudEntityId);
        List<YesserMapping> GetYesserMappedEntities(string cultureName);
    }
}
