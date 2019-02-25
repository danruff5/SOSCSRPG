using Castle.DynamicProxy;
using Engine.Models;
using Engine.Proxy.HandleEvents;
using Engine.Proxy.NotifyPropertyChanged;
using System;

namespace Engine.Factories
{
    public class LocationFactory : BaseFactory, IFactory
    {
        public T CreateType<T>(params object[] ctorArguments) where T : class
        {
            if (typeof(T) == typeof(Location))
            {
                return _generator.CreateClassProxy
                (
                    typeof(Location),
                    new Type[0],
                    new ProxyGenerationOptions(new NotifyPropertyChangedHook()) { Selector = new NotifyPropertyChangedSelector() },
                    new object[]
                    {
                        (int) ctorArguments[0],
                        (int) ctorArguments[1],
                        (string) ctorArguments[2],
                        (string) ctorArguments[3],
                        (string) ctorArguments[4]
                    },
                    new NotifyPropertyChangedInterceptor(),
                    new HandleEventsInterceptor()
                ) as T;
            }

            throw new NotImplementedException();
        }
    }
}
