using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Reflection;
using Engine.Exceptions;
using System.Linq;

namespace ConsoleApp1
{
    public class Aspect<T> : IInterceptorSelector, IProxyGenerationHook
    {
        public readonly List<BaseAspect> baseAspects = new List<BaseAspect>();

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
            return baseAspects.Any(a => a.ShouldInterceptMethod(type, methodInfo));
        }
    }

    public abstract class BaseAspect : IInterceptor
    {
        public abstract bool ShouldInterceptMethod(Type type, MethodInfo methodInfo);
        public abstract void Intercept(IInvocation invocation);
    }
}
