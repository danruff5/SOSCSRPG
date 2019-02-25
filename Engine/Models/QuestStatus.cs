using Engine.Attributes;

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
            [BaseNotifyPropertyChanged]
            set;
        }
    }
}
