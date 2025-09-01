using System.Linq;

namespace MCS.Framework.ObjectExtensions
{
    public static class Extensions
    {
        public static T ShallowCopy<T>(this T source) where T : class, new()
        {
            // Get properties from EF that are read/write and not marked with the NotMappedAttribute
            var sourceProperties = typeof(T)
                                    .GetProperties()
                                    .Where(p => p.CanRead && p.CanWrite);
            var newObj = new T();

            foreach (var property in sourceProperties)
            {
                // Copy value
                property.SetValue(newObj, property.GetValue(source, null), null);
            }

            return newObj;
        }
    }
}
