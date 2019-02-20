using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class QuestStatus :BaseNotifyPropertyChanged
    {
        private bool _isCompleated;

        public Quest PlayerQuest { get; }

        public QuestStatus(Quest quest)
        {
            PlayerQuest = quest;
            IsCompleted = false;
        }

        public bool IsCompleted
        {
            get { return _isCompleated; }
            set
            {
                _isCompleated = value;

                OnPropertyChanged();
            }
        }
    }
}
