using System;
using System.ComponentModel;
using System.Reflection;
using MCS.Framework.Localization;

namespace MCS.UI.Helpers
{
    public static class LocalizeHelper
    {
        public static string GetEnumResource(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return DbRes.TResource(((DescriptionAttribute)attrs[0]).Description);
            }
            return en.ToString();
        }
    }
}