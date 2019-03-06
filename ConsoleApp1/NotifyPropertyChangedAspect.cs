using Castle.DynamicProxy;
using Engine.Attributes;
using Engine.Models;
using Engine.Proxy;
using System;
using System.Reflection;

namespace ConsoleApp1
{
    // Aspect
    public class NotifyPropertyChangedAspect : BaseAspect
    {
        // Advice
        public override void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            BaseNotifyPropertyChangedAttribute attribute = ProxyCommon.AsAttribute<BaseNotifyPropertyChangedAttribute>(invocation.Method);
            if (attribute != null)
            {
                BaseNotifyPropertyChanged @base = invocation.InvocationTarget as BaseNotifyPropertyChanged;

                foreach (string property in attribute.PropertiesToNotify)
                {
                    @base.OnPropertyChanged(property);
                }
            }
        }

        // Pointcut with error checking.
        public override bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return ProxyCommon.IsPropertyWithAttribute<BaseNotifyPropertyChangedAttribute>(type, methodInfo);
        }
    }
}
