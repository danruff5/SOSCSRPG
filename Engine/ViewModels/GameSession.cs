using Engine.EventArgs;
using Engine.Factories;
using Engine.Models;
using System;
using System.Linq;

namespace Engine.ViewModels
{
    public class GameSession : BaseNotifyPropertyChanged
    {
        //private Monster _monster;
        //private Location _location;
        //private Trader _trader;
        //private Player _player;

        public World CurrentWorld { get; }

        public event EventHandler OnNewLocation;
        public event EventHandler OnNewMonster;
        public event EventHandler OnNewPlayer;

        public GameSession()
        {
            CurrentWorld = WorldFactory.CreateWorld();

            Player p = new Player("Daniel", "Fighter", 0, 10, 10, 1000000);
            NotifyPropertyChangedProxy<Player> pp = new NotifyPropertyChangedProxy<Player>(p);
            CurrentPlayer = p;//pp.GetTransparentProxy() as Player;

            if (!CurrentPlayer.Weapons.Any())
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001));
            }

            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(2001));
            CurrentPlayer.LearnRecipe(RecipeFactory.RecipeByID(1));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3001));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3002));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3003));

            CurrentLocation = CurrentWorld.LocationAt(0, 0);

            OnNewLocation += OnNewLocation_CurrentLocation;
            OnNewMonster += OnNewMonster_CurrentMonster;
            OnNewPlayer += OnNewPlayer_CurrentPlayer;
        }

        private void OnNewPlayer_CurrentPlayer(object sender, System.EventArgs e)
        {
            if (CurrentPlayer != null)
            {
                CurrentPlayer.OnActionPerformed -= OnActionPerformed_CurrentPlayer;
                CurrentPlayer.OnLevelUp -= OnLevelUp_CurrentPlayer;
                CurrentPlayer.OnKilled -= OnKilled_CurrentPlayer;
            }
            // TODO: need to do these before and after the real call...
            if (CurrentPlayer != null)
            {
                CurrentPlayer.OnActionPerformed += OnActionPerformed_CurrentPlayer;
                CurrentPlayer.OnLevelUp += OnLevelUp_CurrentPlayer;
                CurrentPlayer.OnKilled += OnKilled_CurrentPlayer;
            }
        }

        private void OnNewMonster_CurrentMonster(object sender, System.EventArgs e)
        {
            if (CurrentMonster != null)
            {
                CurrentMonster.OnActionPerformed -= OnActionPerformed_CurrentMonster;
                CurrentMonster.OnKilled -= OnKilled_CurrentMonster;
            }

            if (CurrentMonster != null)
            {
                CurrentMonster.OnActionPerformed += OnActionPerformed_CurrentMonster;
                CurrentMonster.OnKilled += OnKilled_CurrentMonster;

                RaiseMessage("");
                RaiseMessage($"You see a {CurrentMonster.Name} here!");
            }
        }

        private void OnNewLocation_CurrentLocation(object sender, System.EventArgs e)
        {
            CompleteQuestsAtLocation();
            GivePlayerQuestsAtLocation();
            GetMonsterAtLocation();

            CurrentTrader = CurrentLocation.TraderHere;
        }

        public event EventHandler<GameMessagesEventArgs> OnMessageRaised;
        private void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessagesEventArgs(message));
        }


        [BaseNotifyPropertyChanged
        (
            nameof(HasMonster)
        )]
        public Monster CurrentMonster { get; set; }

        [BaseNotifyPropertyChanged
        (
            nameof(HasLocationToNorth),
            nameof(HasLocationToEast),
            nameof(HasLocationToSouth),
            nameof(HasLocationToWest)
        )]
        public Location CurrentLocation {
            get;
            set;
        }

        [BaseNotifyPropertyChanged
        (
            nameof(HasTrader)
        )]
        public Trader CurrentTrader { get; set; }

        [BaseNotifyPropertyChanged]
        public Player CurrentPlayer { get; set; }

        public bool HasLocationToNorth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
        public bool HasLocationToEast => CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;
        public bool HasLocationToSouth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;
        public bool HasLocationToWest => CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;
        public bool HasMonster => CurrentMonster != null;
        public bool HasTrader => CurrentTrader != null;

        public void MoveNorth()
        {
            if (HasLocationToNorth)
            {
                // TODO: When setting the property from withen the class it does not call the property setter through the proxy...
                // because of 'this' context... This will always be this and not the proxy version
                this.CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
            }
        }
        public void MoveEast()
        {
            if (HasLocationToEast)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
            }
        }
        public void MoveSouth()
        {
            if (HasLocationToSouth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
            }
        }
        public void MoveWest()
        {
            if (HasLocationToWest)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
            }
        }

        private void CompleteQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                QuestStatus questToComplete =
                    CurrentPlayer.Quests.FirstOrDefault(q =>
                        q.PlayerQuest.ID == quest.ID
                        && !q.IsCompleted
                    );

                if (questToComplete != null)
                {
                    if (CurrentPlayer.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        // Remove the quest completion items from the player's inventory
                        CurrentPlayer.RemoveItemsFromInventory(quest.ItemsToComplete);

                        RaiseMessage("");
                        RaiseMessage($"You completed the '{quest.Name}' quest");

                        // Give the player the quest rewards
                        RaiseMessage($"You receive {quest.RewardExperiencePoints} experience points");
                        CurrentPlayer.AddExperience(quest.RewardExperiencePoints);

                        RaiseMessage($"You receive {quest.RewardGold} gold");
                        CurrentPlayer.ReceiveGold(quest.RewardGold);

                        foreach (ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);

                            RaiseMessage($"You receive a {rewardItem.Name}");
                            CurrentPlayer.AddItemToInventory(rewardItem);
                        }

                        // Mark the Quest as completed
                        questToComplete.IsCompleted = true;
                    }
                }
            }
        }
        private void GivePlayerQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == quest.ID))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));

                    RaiseMessage("");
                    RaiseMessage($"You receive the '{quest.Name}' quest");
                    RaiseMessage(quest.Description);

                    RaiseMessage("Return with:");
                    foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                    {
                        RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
                    }

                    RaiseMessage("And you will receive:");
                    RaiseMessage($"   {quest.RewardExperiencePoints} experience points");
                    RaiseMessage($"   {quest.RewardGold} gold");
                    foreach (ItemQuantity itemQuantity in quest.RewardItems)
                    {
                        RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
                    }
                }
            }
        }
        private void GetMonsterAtLocation()
        {
            CurrentMonster = CurrentLocation.GetMonster();
        }
        public void AttackCurrentMonster()
        {
            if (CurrentMonster != null)
            {
                return;
            }
            if (CurrentPlayer.CurrentWeapon == null)
            {
                RaiseMessage("You must select a weapon, to attack.");
                return;
            }

            CurrentPlayer.UseCurrentWeaponOn(CurrentMonster);

            if (CurrentMonster.IsDead)
            {
                // Get another monster to fight
                GetMonsterAtLocation();
            }
            else
            {
                CurrentMonster.UseCurrentWeaponOn(CurrentPlayer);
            }
        }
        public void UseCurrentConsumable()
        {
            if (CurrentPlayer.CurrentConsumable != null)
            {
                CurrentPlayer.UseCurrentConsumable();
            }
        }
        public void CraftItemUsing(Recipe recipe)
        {
            if (CurrentPlayer.HasAllTheseItems(recipe.Ingredients))
            {
                CurrentPlayer.RemoveItemsFromInventory(recipe.Ingredients);

                foreach (ItemQuantity itemQuantity in recipe.OutputItems)
                {
                    for (int i = 0; i < itemQuantity.Quantity; i++)
                    {
                        GameItem outputItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                        CurrentPlayer.AddItemToInventory(outputItem);
                        RaiseMessage($"You craft 1 {outputItem.Name}");
                    }
                }
            }
            else
            {
                RaiseMessage("You do not have the required ingredients:");
                foreach (ItemQuantity itemQuantity in recipe.Ingredients)
                {
                    RaiseMessage($"  {itemQuantity.Quantity} {ItemFactory.ItemName(itemQuantity.ItemID)}");
                }
            }
        }
        private void OnKilled_CurrentPlayer(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage($"You have been killed"); // TODO: Add message to event based on what killed.

            CurrentLocation = CurrentWorld.LocationAt(0, -1);
            CurrentPlayer.CompletelyHeal();
        }
        private void OnKilled_CurrentMonster(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage($"You defeated the {CurrentMonster.Name}!");

            RaiseMessage($"You receive {CurrentMonster.RewardExperiencePoints} experience points.");
            CurrentPlayer.AddExperience(CurrentMonster.RewardExperiencePoints);

            RaiseMessage($"You receive {CurrentMonster.Gold} gold.");
            CurrentPlayer.ReceiveGold(CurrentMonster.Gold);

            foreach (GameItem gameItem in CurrentMonster.Inventory)
            {
                RaiseMessage($"You receive one {gameItem.Name}.");
                CurrentPlayer.AddItemToInventory(gameItem);
            }
        }
        private void OnLevelUp_CurrentPlayer(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage($"You are now level {CurrentPlayer.Level}!");
        }
        private void OnActionPerformed_CurrentPlayer(object sender, string result)
        {
            RaiseMessage(result);
        }
        private void OnActionPerformed_CurrentMonster(object sender, string result)
        {
            RaiseMessage(result);
        }
    }
}
