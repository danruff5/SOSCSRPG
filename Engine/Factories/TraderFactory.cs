using Castle.DynamicProxy;
using Engine.Models;
using Engine.Proxy.HandleEvents;
using Engine.Proxy.NotifyPropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Factories
{
    public class TraderFactory : BaseFactory, IFactory
    {
        private static readonly List<Trader> _traders = new List<Trader>();

        static TraderFactory()
        {
            Trader susan = Create<TraderFactory, Trader>("Susan");
            susan.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            Trader farmerTed = Create<TraderFactory, Trader>("Farmer Ted");
            farmerTed.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            Trader peteTheHerbalist = Create<TraderFactory, Trader>("Pete The Herbalist");
            peteTheHerbalist.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            AddTraderToList(susan);
            AddTraderToList(farmerTed);
            AddTraderToList(peteTheHerbalist);
        }

        public static Trader GetTraderByName(string name)
        {
            return _traders.FirstOrDefault(t => t.Name == name);
        }

        public static void AddTraderToList(Trader trader)
        {
            if (_traders.Any(t => t.Name == trader.Name))
            {
                throw new ArgumentException($"There is already a trader named '{trader.Name}'");
            }

            _traders.Add(trader);
        }

        public T CreateType<T>(params object[] ctorArguments) where T : class
        {
            if (typeof(T) == typeof(Trader))
            {
                return _generator.CreateClassProxy
                (
                    typeof(Trader),
                    new Type[0],
                    new ProxyGenerationOptions(new NotifyPropertyChangedHook()) { Selector = new NotifyPropertyChangedSelector() },
                    new object[] { (string)ctorArguments[0] },
                    new NotifyPropertyChangedInterceptor(),
                    new HandleEventsInterceptor()
                ) as T;
            }

            throw new NotImplementedException();
        }
    }
}
