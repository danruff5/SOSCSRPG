﻿using Castle.DynamicProxy;
using Engine.Proxy.HandleEvents;
using Engine.Proxy.NotifyPropertyChanged;
using Engine.ViewModels;
using System;

namespace Engine.Factories
{
    public class GameSessionFactory : BaseFactory, IFactory
    {
        public GameSessionFactory() { }

        public T CreateType<T>(params object[] ctorArguments) where T : class
        {
            if (typeof(T) == typeof(GameSession))
            {
                return _generator.CreateClassProxy
                (
                    typeof(GameSession),
                    new ProxyGenerationOptions(new NotifyPropertyChangedHook())
                    {
                        Selector = new NotifyPropertyChangedSelector()
                    },
                    new NotifyPropertyChangedInterceptor(),
                    new HandleEventsInterceptor()
                ) as T;
            }

            throw new NotImplementedException();
        }
    }
}
