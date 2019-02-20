using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Monster : LivingEntity
    {
        public string ImageName { get; }

        public int RewardExperiencePoints { get; }

        public Monster
        (
            string name,
            string imageName,
            int maximumHitPoints,
            int currentHitpoints,
            int rewardExperiencePoints,
            int gold
        ) : base(name, maximumHitPoints, currentHitpoints, gold)
        {
            ImageName = $"/Engine;component/Images/Monsters/{imageName}";
            RewardExperiencePoints = rewardExperiencePoints;
        }
    }
}
