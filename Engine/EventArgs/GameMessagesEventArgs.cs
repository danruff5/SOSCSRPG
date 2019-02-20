using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EventArgs
{
    public class GameMessagesEventArgs : System.EventArgs
    {
        public string Message { get; set; }

        public GameMessagesEventArgs(string message)
        {
            Message = message;
        }
    }
}
