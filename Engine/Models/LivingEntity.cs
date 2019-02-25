using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Models
{
    public abstract class LivingEntity : BaseNotifyPropertyChanged
    {
        public ObservableCollection<GameItem> Inventory { get; }
        public ObservableCollection<GroupedInventoryItem> GroupedInventory { get; }
        public List<GameItem> Weapons => Inventory.Where(i => i.Category == GameItem.ItemCategory.Weapon).ToList();
        public List<GameItem> Consumables => Inventory.Where(i => i.Category == GameItem.ItemCategory.Consumable).ToList();

        public event EventHandler OnKilled;
        public event EventHandler<string> OnActionPerformed;

        protected LivingEntity
        (
            string name,
            int maximumHitPoints,
            int currentHitpoints,
            int gold,
            int level = 1
        )
        {
            Name = name;
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitpoints;
            Gold = gold;
            Level = level;

            Inventory = new ObservableCollection<GameItem>();
            GroupedInventory = new ObservableCollection<GroupedInventoryItem>();
        }

        public virtual string Name { get; [BaseNotifyPropertyChanged] set; }
        public virtual int CurrentHitPoints { get; [BaseNotifyPropertyChanged] set; }
        public virtual int MaximumHitPoints { get; [BaseNotifyPropertyChanged] set; }
        public virtual int Gold { get; [BaseNotifyPropertyChanged] set; }
        public virtual int Level { get; [BaseNotifyPropertyChanged] set; }

        // if (_currentWeapon != null)
        // {
        //     _currentWeapon.Action.OnActionPerformed -= RaiseActionPerformedEvent;
        // }
        // if (_currentWeapon != null)
        // {
        //     _currentWeapon.Action.OnActionPerformed += RaiseActionPerformedEvent;
        // }
        public virtual GameItem CurrentWeapon { get; [BaseNotifyPropertyChanged]set; }

        // if (_currentConsumable != null)
        // {
        //     _currentConsumable.Action.OnActionPerformed -= RaiseActionPerformedEvent;
        // }
        // if (_currentConsumable != null)
        // {
        //     _currentConsumable.Action.OnActionPerformed += RaiseActionPerformedEvent;
        // }
        public virtual GameItem CurrentConsumable { get; [BaseNotifyPropertyChanged]set; }

        public bool IsDead => CurrentHitPoints <= 0;
        public bool HasConsumable => Consumables.Any();

        public void AddItemToInventory(GameItem item)
        {
            Inventory.Add(item);

            if (item.IsUnique)
            {
                GroupedInventory.Add(new GroupedInventoryItem(item, 1));
            }
            else
            {
                if (!GroupedInventory.Any(gi => gi.Item.ItemTypeID == item.ItemTypeID))
                {
                    GroupedInventory.Add(new GroupedInventoryItem(item, 0));
                }

                GroupedInventory.First(gi => gi.Item.ItemTypeID == item.ItemTypeID).Quantity++;
            }

            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumable));
        }
        public void RemoveItemFromInventory(GameItem item) // TODO: Overload with GroupedInventoryItem
        {
            Inventory.Remove(item);

            GroupedInventoryItem groupedInventoryItemToRemove = item.IsUnique ?
                GroupedInventory.FirstOrDefault(gi => gi.Item == item) :
                GroupedInventory.FirstOrDefault(gi => gi.Item.ItemTypeID == item.ItemTypeID);

            if (groupedInventoryItemToRemove.Quantity == 1)
            {
                GroupedInventory.Remove(groupedInventoryItemToRemove);
            }
            else
            {
                groupedInventoryItemToRemove.Quantity--;
            }

            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumable));
        }
        public void RemoveItemsFromInventory(List<ItemQuantity> itemQuantities)
        {
            foreach (ItemQuantity itemQuantity in itemQuantities)
            {
                for (int i = 0; i < itemQuantity.Quantity; i++)
                {
                    RemoveItemFromInventory(Inventory.First(item => item.ItemTypeID == itemQuantity.ItemID));
                }
            }
        }
        public bool HasAllTheseItems(List<ItemQuantity> items)
        {
            foreach (ItemQuantity item in items)
            {
                if (Inventory.Count(i => i.ItemTypeID == item.ItemID) < item.Quantity)
                {
                    return false;
                }
            }

            return true;
        }

        public void TakeDamage(int hitPointsOfDamage)
        {
            CurrentHitPoints -= hitPointsOfDamage;

            if (IsDead)
            {
                CurrentHitPoints = 0;
                RaiseOnKilledEvent();
            }
        }
        public void UseCurrentWeaponOn(LivingEntity target)
        {
            CurrentWeapon.PerformAction(this, target);
        }
        public void UseCurrentConsumable()
        {
            CurrentConsumable.PerformAction(this, this);
            RemoveItemFromInventory(CurrentConsumable);
        }
        public void Heal(int hitPointsToHeal)
        {
            CurrentHitPoints += hitPointsToHeal;

            if (CurrentHitPoints > MaximumHitPoints)
            {
                CurrentHitPoints = MaximumHitPoints;
            }
        }
        public void CompletelyHeal()
        {
            CurrentHitPoints = MaximumHitPoints;
        }
        public void ReceiveGold(int amountOfGold)
        {
            Gold += amountOfGold;
        }
        public void SpendGold(int amountOfGold)
        {
            if (amountOfGold > Gold)
            {
                throw new ArgumentOutOfRangeException($"{Name} only has {Gold} gold, and cannot spend {amountOfGold} gold");
            }

            Gold -= amountOfGold;
        }

        private void RaiseOnKilledEvent()
        {
            OnKilled?.Invoke(this, new System.EventArgs());
        }
        private void RaiseActionPerformedEvent(object sender, string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
    }
}
