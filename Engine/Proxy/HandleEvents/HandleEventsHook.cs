using Castle.DynamicProxy;
using Engine.Attributes;
using Engine.Exceptions;
using System;
using System.Reflection;

namespace Engine.Proxy.HandleEvents
{
    public class HandleEventsHook : IProxyGenerationHook
    {
        public void MethodsInspected() { }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            if (ProxyCommon.IsSetter(memberInfo as MethodInfo))
            {
                throw new NotProxyableException(memberInfo, GetType().Name);
            }
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return ProxyCommon.IsSetterOrGetter(methodInfo)
                || ProxyCommon.HasAttribute<HandleEventsAttribute>(methodInfo);
        }
    }
}
