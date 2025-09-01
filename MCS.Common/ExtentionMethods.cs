using MCS.Framework;
using System;
using System.Reflection;

namespace MCS.Common
{
    public static class ExtentionMethods
    {
        public static object GetPropertyValueByPath(this object objectInstance, string path)
        {
            var propertyNames = path.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var propertyValue = objectInstance.GetType().GetProperty(propertyNames[0]).GetValue(objectInstance, null);

            if (propertyNames.Length == 1 || propertyValue == null)
            {
                return propertyValue;
            }
            else
            {
                return propertyValue.GetPropertyValueByPath(path.Replace(propertyNames[0] + ".", ""));
            }
        }

        public static int LookupInternalID(this int lookupID, LookupCategory lookupCategory, string cultureName)
        {
            return IoC.Resolve<ILookupHelper>().GetLookupInternalID(lookupID, lookupCategory, cultureName);
        }
             
        public static int LookupIdentity(this Enum internalMember, LookupCategory lookupCategory, string cultureName)
        {
            Type memberEnumType = internalMember.GetType();
            int memberEnumValue = (int)Enum.Parse(memberEnumType, internalMember.ToString(), true);
            if(cultureName==string.Empty)
            {
                cultureName = "ar";
            }
            return IoC.Resolve<ILookupHelper>().GetLookupIdentity(memberEnumValue, lookupCategory, cultureName);
        }

    }
}