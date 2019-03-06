using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Reflection;
using Engine.Exceptions;
using System.Linq;

namespace ConsoleApp1
{
    public class Aspect : Singleton<Aspect>, IInterceptorSelector, IProxyGenerationHook
    {
        private readonly List<BaseAspect> Aspects = new List<BaseAspect>();
        // TODO: Make singleton based get

        public void MethodsInspected() { }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
            if (ShouldInterceptMethod(type, memberInfo as MethodInfo))
            {
                throw new NotProxyableException(memberInfo, GetType().Name);
            }
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            return interceptors.Where(i => ((BaseAspect)i).ShouldInterceptMethod(type, method)).ToArray();
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            // TODO: Get BaseAspect from the applied IInterceptor
            return methodInfo.IsSpecialName && 
                (
                    !methodInfo.Name.StartsWith("add_", StringComparison.OrdinalIgnoreCase) 
                    && !methodInfo.Name.StartsWith("remove_", StringComparison.OrdinalIgnoreCase)
                ); // HACK:
        }
    }

    public abstract class BaseAspect : IInterceptor
    {
        protected BaseAspect()
        {
            Aspect.Instance.Aspects.Add(this);
        }

        public abstract bool ShouldInterceptMethod(Type type, MethodInfo methodInfo);
        public abstract void Intercept(IInvocation invocation);
    }
}
