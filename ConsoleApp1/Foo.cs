using Engine.Attributes;
using Engine.Models;

namespace ConsoleApp1
{
    public class Foo : BaseNotifyPropertyChanged
    {
        public virtual string Name
        {
            get;
            [BaseNotifyPropertyChanged]
            set;
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
