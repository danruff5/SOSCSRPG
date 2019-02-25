using Engine.Attributes;

namespace Engine.Models
{
    public class GroupedInventoryItem : BaseNotifyPropertyChanged
    {
        public GroupedInventoryItem(GameItem item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }

        public virtual GameItem Item
        {
            get;
            [BaseNotifyPropertyChanged]
            set;
        }
        public virtual int Quantity
        {
            get;
            [BaseNotifyPropertyChanged]
            set;
        }
    }
}
