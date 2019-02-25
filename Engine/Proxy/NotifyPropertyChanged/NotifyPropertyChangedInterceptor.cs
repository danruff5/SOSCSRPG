using Castle.DynamicProxy;
using Engine.Attributes;
using Engine.Models;

namespace Engine.Proxy.NotifyPropertyChanged
{
    class NotifyPropertyChangedInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
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
    }
}
