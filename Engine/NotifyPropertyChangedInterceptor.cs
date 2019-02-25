using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;

namespace Engine
{
    class NotifyPropertyChangedInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            BaseNotifyPropertyChangedAttribute attribute = AsAttribute<BaseNotifyPropertyChangedAttribute>(invocation.Method);
            if (attribute != null) {
                BaseNotifyPropertyChanged @base = invocation.InvocationTarget as BaseNotifyPropertyChanged;

                foreach (string property in attribute.PropertiesToNotify)
                {
                    @base.OnPropertyChanged(property);
                }
            }
        }

        private T AsAttribute<T>(MethodInfo methodInfo) where T : class
        {
            if (methodInfo.CustomAttributes.Any(a => a.AttributeType == typeof(T)))
            {
                return Attribute.GetCustomAttribute(methodInfo, typeof(T)) as T;
            }
            return null;
        }
    }
}
