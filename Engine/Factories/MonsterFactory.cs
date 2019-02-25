using Castle.DynamicProxy;
using Engine.Models;
using Engine.Proxy.HandleEvents;
using Engine.Proxy.NotifyPropertyChanged;
using System;

namespace Engine.Factories
{
    public class MonsterFactory : BaseFactory, IFactory
    {
        public static Monster GetMonster(int monsterID)
        {
            switch (monsterID)
            {
                case 1:
                    Monster snake = Create<MonsterFactory, Monster>("Snake", "Snake.png", 4, 4, 5, 1);
                    snake.CurrentWeapon = ItemFactory.CreateGameItem(1501);
                    AddLootItem(snake, 9001, 25);
                    AddLootItem(snake, 9002, 75);

                    return snake;
                case 2:
                    Monster rat = Create<MonsterFactory, Monster>("Rat", "Rat.png", 5, 5, 5, 1);
                    rat.CurrentWeapon = ItemFactory.CreateGameItem(1502);
                    AddLootItem(rat, 9003, 25);
                    AddLootItem(rat, 9004, 75);

                    return rat;
                case 3:
                    Monster giantSpider = Create<MonsterFactory, Monster>("Giant Spider", "GiantSpider.png", 10, 10, 10, 3);
                    giantSpider.CurrentWeapon = ItemFactory.CreateGameItem(1503);
                    AddLootItem(giantSpider, 9005, 25);
                    AddLootItem(giantSpider, 9006, 75);

                    return giantSpider;
                default:
                    throw new ArgumentException(string.Format("MonsterType '{0}' does not exist", monsterID));
            }
        }

        private static void AddLootItem(Monster monster, int itemID, int percentage)
        {
            if (RandomNumberGenerator.NumberBetween(1, 100) <= percentage)
            {
                monster.AddItemToInventory(ItemFactory.CreateGameItem(itemID));
            }
        }

        public T CreateType<T>(params object[] ctorArguments) where T : class
        {
            if (typeof(T) == typeof(Monster))
            {
                return _generator.CreateClassProxy
                (
                    typeof(Monster),
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
