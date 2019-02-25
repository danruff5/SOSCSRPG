using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;

namespace Engine
{
    public class NotifyPropertyChangedHook : IProxyGenerationHook
    {
        public void MethodsInspected() { }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            if (IsSetter(memberInfo as MethodInfo))
            {
                throw new NotProxyableException(memberInfo, GetType().Name);
            }
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return IsSetterOrGetter(methodInfo) || HasAttribute(methodInfo);
        }

        private bool HasAttribute(MethodInfo method)
        {
            return method.CustomAttributes.Any(a => a.AttributeType == typeof(BaseNotifyPropertyChangedAttribute));
        }

        private bool IsSetter(MethodInfo method)
        {
            return method != null
                && method.IsSpecialName
                && method.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsSetterOrGetter(MethodInfo method)
        {
            return IsSetter(method)
                || method.Name.StartsWith("get_", StringComparison.OrdinalIgnoreCase);
        }
    }
}
