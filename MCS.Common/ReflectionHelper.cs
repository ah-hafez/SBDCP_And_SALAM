using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace MCS.Common
{
    public class ReflectionHelper
    {
        public ReflectionHelper()
        {
        }

        /// <summary>
        /// Dynamically Creates an instance of the given type. 
        /// </summary>
        /// <param name="assemblyName">Assembly on which the type is hosted</param>
        /// <param name="typeName">type to create an instance from</param>
        /// <param name="aArgs">Type constructor's arguments.</param>
        /// <returns>An instance of the given type.</returns>
        public static object Create(string assemblyName, string typeName, object[] aArgs)
        {
            // (1) Create the arguments type array with arguments number
            Type[] aArgType = new Type[aArgs.Length];

            // (2) Form an array of the passed arguments' types.			
            for (int i = 0; i < aArgs.Length; i++)
            {
                aArgType[i] = aArgs[i].GetType();
            }

            // (3) Form a cache key for the given method
            string strCacheKey = GetCacheKey(typeName, aArgType, null);

            // (4) Get the c-tor object from the cache if already there;otherwise get it from 
            //	type object
            ConstructorInfo objCtor = (ConstructorInfo)GetFromCache(strCacheKey);
            if (objCtor == null)
            {

                // (5) Loads the type's assembly
                Assembly ObjAssembly = Assembly.Load(assemblyName);

                // (6) Get Type object of the given type name
                Type ObjType = ObjAssembly.GetType(typeName);
                if (ObjType == null)
                {
                    throw new Exception("Type could not be found");
                }

                // (7) Query the reference type object for a constructor method with the given 
                //	argument types
                objCtor = ObjType.GetConstructor(aArgType);

                // (8) If the c-tor has been found add it to the cache otherwise throw an exception
                if (objCtor == null)
                {
                    throw new Exception("Constructor could not be found");
                }

                AddToCache(strCacheKey, objCtor);
            }

            // (9) If the constrcutor found, call it. Otherwise throw an exception
            return objCtor.Invoke(aArgs);
        }

        #region Cache Implementation

        /// <summary>
        /// Determines if the Cahcing is enabled or not. MethodInfo objects are not guaranteed to be thread safe
        /// </summary>
        private static bool _cachEnabled = true;

        /// <summary>
        /// Determines if the Cahcing is enabled or not. MethodInfo objects are not guaranteed to be thread safe
        /// </summary>
        public static bool CachEnabled
        {
            get { return _cachEnabled; }
            set { _cachEnabled = value; }
        }

        private static Hashtable m_MethodsCache = new Hashtable();

        private void ClearCache()
        {
            m_MethodsCache.Clear();
        }

        /// <summary>
        /// Forms the cache key for the given method
        /// </summary>
        /// <param name="strMethodName">Method Name</param>
        /// <param name="aArgTypes">Array of the parameters' types of the given method</param>
        /// <param name="objReturnType">the method return value type</param>
        /// <returns></returns>
        protected static string GetCacheKey(string strMethodName, Type[] aArgTypes, Type objReturnType)
        {
            if (CachEnabled)
            {
                StringBuilder objParamsList = null;

                if (aArgTypes != null)
                {
                    objParamsList = new StringBuilder();
                    foreach (Type objType in aArgTypes)
                    {
                        objParamsList.AppendFormat(",{0}", objType.Name);
                    }
                }
                return string.Format("{0}.Param:{1}.Ret:{2}", strMethodName,
                    objParamsList != null ? objParamsList.ToString() : string.Empty,
                    objReturnType != null ? objReturnType.Name : string.Empty);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get the given method from the cache if found.
        /// </summary>
        /// <param name="strCacheKey">Method key to look for in the cache</param>
        /// <returns>The MemberInfo object if found in the cache otherwise null</returns>
        private static MemberInfo GetFromCache(string strCacheKey)
        {
            if (CachEnabled)
            {
                return (MemberInfo)m_MethodsCache[strCacheKey];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Add the given MethodInfo object to the cache
        /// </summary>
        /// <param name="strCacheKey">Key of the method info to be used in the cache</param>
        /// <param name="objMethodInfo">the object to cache</param>
        private static void AddToCache(string strCacheKey, MemberInfo objMethodInfo)
        {
            if (CachEnabled)
            {
                // Lock the cache storage for thread safety..
                lock (m_MethodsCache.SyncRoot)
                {
                    m_MethodsCache[strCacheKey] = objMethodInfo;
                }
            }
        }

        #endregion
    }
}
