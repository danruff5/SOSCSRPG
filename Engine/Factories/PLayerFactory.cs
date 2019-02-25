using Castle.DynamicProxy;
using Engine.Models;
using Engine.Proxy.HandleEvents;
using Engine.Proxy.NotifyPropertyChanged;
using System;

namespace Engine.Factories
{
    public class PlayerFactory : BaseFactory, IFactory
    {
        public T CreateType<T>(params object[] ctorArguments) where T : class
        {
            if (typeof(T) == typeof(Player))
            {
                return _generator.CreateClassProxy
                (
                    typeof(Player),
                    new Type[0],
                    new ProxyGenerationOptions(new NotifyPropertyChangedHook())
                    {
                        Selector = new NotifyPropertyChangedSelector()
                    },
                    new object[]
                    {
                        (string) ctorArguments[0],
                        (string) ctorArguments[1],
                        (int) ctorArguments[2],
                        (int) ctorArguments[3],
                        (int) ctorArguments[4],
                        (int) ctorArguments[5]
                    },
                    new NotifyPropertyChangedInterceptor(),
                    new HandleEventsInterceptor()
                ) as T;
            }

            throw new NotImplementedException();
        }
    }
}
