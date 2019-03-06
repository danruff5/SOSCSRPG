using Castle.DynamicProxy;
using Engine.Exception;
using System;
using System.Linq;
using System.Reflection;

namespace Engine.Proxy
{
    public static class ProxyCommon
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

        public static bool IsPropertyWithAttribute<T>(Type type, MethodInfo method) where T : Attribute
        {
            if (IsGetter(method))
            {
                // Check if corresponding setter has attribute.
                return HasAttribute<T>(type.GetProperty(GetPropertyName(method)).SetMethod);
            }
            else if (HasAttribute<T>(method))
            {
                if (IsSetter(method))
                {
                    return true;
                }
                else
                {
                    // The attribute cannot be applied to anything other than a setter method.
                    throw new InvalidAttributeDecelerationException(method, nameof(T));
                }
            }
            else
            {
                return false;
            }
        }

        public static string GetPropertyName(MethodInfo method)
        {
            if (IsSetterOrGetter(method))
            {
                return method.Name.Substring("?et_".Length);
            }
            throw new InvalidOperationException($"The method {method.DeclaringType}#{method.Name} is not a propertry.");
        }

        public static object GetMethodResult(IInvocation invocation, string name, params object[] arguments)
        {
            MethodInfo method = invocation.InvocationTarget.GetType().GetMethod(name);
            return method.Invoke(invocation.InvocationTarget, arguments);
        }

        public static void EditEvent(EventInfo @event, object obj, Delegate d, bool removeEvent = false)
        {
            if (removeEvent) // remove event
            {
                @event.GetRemoveMethod().Invoke(obj, new[] { d });
            }
            else // add event
            {
                @event.GetAddMethod().Invoke(obj, new[] { d });
            }
        }
    }
}
