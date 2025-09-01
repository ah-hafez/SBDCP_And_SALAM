using System.ComponentModel;
using MCS.Framework.Localization;

namespace MCS.Common.CustomAttributes
{
    public class CustomDisplayNameAttribute : DisplayNameAttribute
    {
        private string _resourceKey;

        public CustomDisplayNameAttribute(string resourceKey)
        {
            _resourceKey = resourceKey;
        }
        public override string DisplayName
        {
            get
            {
                return DbRes.TResource(_resourceKey);
            }
        }
    }
}
