using Castle.DynamicProxy;
using Engine.Factories;

namespace ConsoleApp1
{
    public class FooFactory : BaseFactory, IFactory
    {
        public T CreateType<T>(params object[] ctorArguments) where T : class
        {
            return _generator.CreateClassProxy
                (
                    typeof(Foo),
                    new ProxyGenerationOptions(Aspect.Instance as IProxyGenerationHook)
                    {
                        Selector = Aspect.Instance as IInterceptorSelector
                    },
                    new NotifyPropertyChangedAspect()
                ) as T;
        }
    }
}
