using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;

namespace Engine
{
    public class NotifyPropertyChangedSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (IsSetterOrGetter(method))
            {
                return interceptors;
            }
            return interceptors.Where(i => !(i is NotifyPropertyChangedInterceptor)).ToArray();
        }

        private bool IsSetterOrGetter(MethodInfo method)
        {
            return method.IsSpecialName
                && (
                    IsSetter(method)
                    || IsGetter(method)
                );
        }

        private bool IsGetter(MethodInfo method)
        {
            return method.Name.StartsWith("get_", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsSetter(MethodInfo method)
        {
            return method.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase);
        }
    }
}
