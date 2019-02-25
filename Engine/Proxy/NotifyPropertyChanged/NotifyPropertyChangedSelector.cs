using Castle.DynamicProxy;
using Engine.Proxy.HandleEvents;
using System;
using System.Linq;
using System.Reflection;

namespace Engine.Proxy.NotifyPropertyChanged
{
    public class NotifyPropertyChangedSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (ProxyCommon.IsSetterOrGetter(method))
            {
                return interceptors;
            }
            return interceptors.Where(i => !(i is NotifyPropertyChangedInterceptor || i is HandleEventsInterceptor)).ToArray();
        }
    }
}
