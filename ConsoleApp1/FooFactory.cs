using Castle.DynamicProxy;
using Engine.Factories;

namespace ConsoleApp1
{
    public class FooFactory : BaseFactory, IFactory
    {
        // CLEAN: Simplify the factories?
        public T CreateType<T>(params object[] ctorArguments) where T : class
        {
            Aspect<Foo> a = new Aspect<Foo>();
            a.baseAspects.Add(new NotifyPropertyChangedAspect());

            return _generator.CreateClassProxyWithTarget
                (
                    typeof(Foo),
                    new Foo(),
                    new ProxyGenerationOptions(a as IProxyGenerationHook)
                    {
                        Selector = a as IInterceptorSelector
                    },
                    a.baseAspects.ToArray()
                ) as T;
        }
    }
}
