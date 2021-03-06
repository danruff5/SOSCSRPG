﻿using Engine.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class GameItem
    {
        public enum ItemCategory
        {
            Miscellaneous,
            Weapon,
            Consumable
        }

        public ItemCategory Category { get; }
        public int ItemTypeID { get; }
        public string Name { get; }
        public int Price { get; set; }
        public bool IsUnique { get; }
        public IAction Action { get; set; }

        public GameItem
        (
            ItemCategory category,
            int itemTypeID,
            string name,
            int price,
            bool isUnique = false,
            int minimumDamage = 0,
            int maximumDamage = 0
        )
        {
            Category = category;
            ItemTypeID = itemTypeID;
            Name = name;
            Price = price;
            IsUnique = isUnique;
        }

        public void PerformAction(LivingEntity actor, LivingEntity target)
        {
            Action?.Execute(actor, target);
        }

        public GameItem Clone()
        {
            GameItem i = new GameItem(Category, ItemTypeID, Name, Price, IsUnique);
            if (Action != null)
            {
                i.Action = Action;
            }

            return i;
        }
    }
}
