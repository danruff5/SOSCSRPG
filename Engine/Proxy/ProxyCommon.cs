using System;
using System.Linq;
using System.Reflection;

namespace Engine.Proxy
{
    // TODO: Add in default impls of apply on proerties and getters or setters.
    // TODO: Combine *Hook's, *Interceptor's, *Selector's into pointcut's, aspect.
    internal static class ProxyCommon
    {
        public static T AsAttribute<T>(MethodInfo methodInfo) where T : class
        {
            if (typeof(T).IsArray)
            {
                if (methodInfo.CustomAttributes.Any(a => a.AttributeType == typeof(T).GetElementType()))
                {
                    return Attribute.GetCustomAttributes(methodInfo, typeof(T).GetElementType()) as T;
                }
            }
            else
            {
                if (methodInfo.CustomAttributes.Any(a => a.AttributeType == typeof(T)))
                {
                    return Attribute.GetCustomAttribute(methodInfo, typeof(T)) as T;
                }
            }


            return null;
        }

        public static bool HasAttribute<T>(MethodInfo method) where T : class
        {
            return method.CustomAttributes.Any(a => a.AttributeType == typeof(T));
        }

        public static bool IsSetter(MethodInfo method)
        {
            return method != null
                && method.IsSpecialName
                && method.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsGetter(MethodInfo method)
        {
            return method != null
                && method.IsSpecialName
                && method.Name.StartsWith("get_", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsSetterOrGetter(MethodInfo method)
        {
            return IsSetter(method)
                || method.Name.StartsWith("get_", StringComparison.OrdinalIgnoreCase);
        }
    }
}
