using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;

namespace Engine.Proxy.HandleEvents
{
    public class HandleEventsSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (ProxyCommon.IsSetterOrGetter(method))
            {
                return interceptors;
            }
            return interceptors.Where(i => !(i is HandleEventsInterceptor)).ToArray();
        }
    }
}
