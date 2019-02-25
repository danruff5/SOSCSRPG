using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Models
{
    public class Player : LivingEntity
    {
        public ObservableCollection<QuestStatus> Quests { get; }
        public ObservableCollection<Recipe> Recipes { get; }

        public Player
        (
            string name, 
            string characterClass, 
            int experiencePoints,
            int maximumHitPoints, 
            int currentHitPoints, 
            int gold
        ) : base(name, maximumHitPoints, currentHitPoints, gold)
        {
            CharacterClass = characterClass;
            ExperiencePoints = experiencePoints;

            Quests = new ObservableCollection<QuestStatus>();
            Recipes = new ObservableCollection<Recipe>();
        }

        public event EventHandler OnLevelUp;

        public virtual string CharacterClass
        {
            get;
            [BaseNotifyPropertyChanged] set;
        }
        public virtual int ExperiencePoints
        {
            get;
            [BaseNotifyPropertyChanged] set;
        }
        
        public void LearnRecipe(Recipe recipe)
        {
            if(!Recipes.Any(r => r.ID == recipe.ID))
            {
                Recipes.Add(recipe);
            }
        }

        public void AddExperience(int experiencePoints)
        {
            ExperiencePoints += experiencePoints;
        }
        private void SetLevelAndMaximumHitPoints()
        {
            int originalLevel = Level;
 
            Level = (ExperiencePoints / 100) + 1;
 
            if (Level != originalLevel)
            {
                MaximumHitPoints = Level * 10;
 
                OnLevelUp?.Invoke(this, System.EventArgs.Empty);
            }
        }
    }
}
