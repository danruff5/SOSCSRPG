using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine.Models;

namespace Engine.Factories
{
    public static class MonsterFactory
    {
        public static Monster GetMonster(int monsterID)
        {
            NotifyPropertyChangedProxy<Monster> p;
            switch (monsterID)
            {
                case 1:
                    Monster snake = new Monster("Snake", "Snake.png", 4, 4, 5, 1);
                    snake.CurrentWeapon = ItemFactory.CreateGameItem(1501);
                    AddLootItem(snake, 9001, 25);
                    AddLootItem(snake, 9002, 75);

                    p = new NotifyPropertyChangedProxy<Monster>(snake);

                    return snake; // p.GetTransparentProxy() as Monster;
                case 2:
                    Monster rat = new Monster("Rat", "Rat.png", 5, 5, 5, 1);
                    rat.CurrentWeapon = ItemFactory.CreateGameItem(1502);
                    AddLootItem(rat, 9003, 25);
                    AddLootItem(rat, 9004, 75);

                    p = new NotifyPropertyChangedProxy<Monster>(rat);

                    return rat; // p.GetTransparentProxy() as Monster;
                case 3:
                    Monster giantSpider = new Monster("Giant Spider", "GiantSpider.png", 10, 10, 10, 3);
                    giantSpider.CurrentWeapon = ItemFactory.CreateGameItem(1503);
                    AddLootItem(giantSpider, 9005, 25);
                    AddLootItem(giantSpider, 9006, 75);

                    p = new NotifyPropertyChangedProxy<Monster>(giantSpider);

                    return giantSpider;//p.GetTransparentProxy() as Monster;
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
    }
}
