using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class QuestStatus : BaseNotifyPropertyChanged
    {
        public Quest PlayerQuest { get; }

        public QuestStatus(Quest quest)
        {
            PlayerQuest = quest;
            IsCompleted = false;
        }

        public virtual bool IsCompleted
        {
            get;
            [BaseNotifyPropertyChanged] set;
        }
    }
}
