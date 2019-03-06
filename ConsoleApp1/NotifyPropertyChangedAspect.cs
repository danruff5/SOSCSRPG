using Castle.DynamicProxy;
using Engine.Attributes;
using Engine.Models;
using Engine.Proxy;
using System;
using System.Reflection;

namespace ConsoleApp1
{
    public class NotifyPropertyChangedAspect : BaseAspect
    {
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

        public override bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            // HACK, TODO: Fix to have bettor error. Attribute can be applied to any method but will not get triggered on it because not setter or getter.
            return ProxyCommon.IsSetterOrGetter(methodInfo)
                || ProxyCommon.HasAttribute<BaseNotifyPropertyChangedAttribute>(methodInfo);
        }
    }
}
