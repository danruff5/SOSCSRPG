﻿using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Models
{
    public class Player : LivingEntity
    {
        private string _characterClass;
        private int _experiencePoints;

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

        public string CharacterClass
        {
            get { return _characterClass; }
            set
            {
                _characterClass = value;
                OnPropertyChanged();
            }
        }
        public int ExperiencePoints
        {
            get { return _experiencePoints; }
            private set
            {
                _experiencePoints = value;
                OnPropertyChanged();

                SetLevelAndMaximumHitPoints();
            }
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
