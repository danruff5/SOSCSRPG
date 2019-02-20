﻿using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Factories
{
    public static class TraderFactory
    {
        private static readonly List<Trader> _traders = new List<Trader>();

        static TraderFactory()
        {
            Trader susan = new Trader("Susan");
            susan.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            NotifyPropertyChangedProxy<Trader> pSusan = new NotifyPropertyChangedProxy<Trader>(susan);

            Trader farmerTed = new Trader("Farmer Ted");
            farmerTed.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            NotifyPropertyChangedProxy<Trader> pTed = new NotifyPropertyChangedProxy<Trader>(farmerTed);

            Trader peteTheHerbalist = new Trader("Pete The Herbalist");
            peteTheHerbalist.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            NotifyPropertyChangedProxy<Trader> pPete = new NotifyPropertyChangedProxy<Trader>(peteTheHerbalist);

            /*AddTraderToList(pSusan.GetTransparentProxy() as Trader);
            AddTraderToList(pTed.GetTransparentProxy() as Trader);
            AddTraderToList(pPete.GetTransparentProxy() as Trader);*/
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
    }
}
