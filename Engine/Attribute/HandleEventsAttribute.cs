using System;
using System.Runtime.CompilerServices;

namespace Engine.Attributes
{

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class HandleEventsAttribute : Attribute
    {
        public string MemberName { get; private set; }
        public string EventName { get; private set; }
        public string MethodName { get; private set; }

        public HandleEventsAttribute(string eventName, string methodName, [CallerMemberName] string memberName = null)
        {
            EventName = eventName;
            MethodName = methodName;
            MemberName = memberName;
        }
    }
}
