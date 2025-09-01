using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using MCS.Framework;
using MCS.Framework.Exceptions;
using MCS.Business;
using MCS.Common;
using MCS.Domain;

namespace MCS.Integration.HUB.Helpers
{
    public static class HubHelper
    {
        public static string GetYesserMappedValue(YesserTypesMapping yesserTypesMapping, int cloudTypeId)
        {
            IYesserMappingBL yesserMappingBL = IoC.Resolve<YesserMappingBL>();
            YesserMapping yesserMapping = yesserMappingBL.GetYesserMappedValue(yesserTypesMapping, cloudTypeId);
            return yesserMapping.YesserTypeId;
        }
        public static string FormatHejriDateString(string hejriDate)
        {
            //"30/4/1440" => "1440/04/30"

            string[] hejriDateArray = hejriDate.Split('/').ToArray();

            for (int i = 1; i < hejriDateArray.Length; i++)
            {
                if (hejriDateArray[i].Length == 1)
                {
                    hejriDateArray[i] = "0" + hejriDateArray[i];
                }
            }

            hejriDate = string.Join("/", hejriDateArray);

            return hejriDate;
        }
    }
}
