using Engine.Attributes;
using Engine.Models;
using Engine.ViewModels;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace Engine.Old
{
    public class NotifyPropertyChangedProxy<T> : RealProxy where T : BaseNotifyPropertyChanged
    {
        private readonly BaseNotifyPropertyChanged _decorated; // GameSession

        public NotifyPropertyChangedProxy(T decorated) : base(typeof(T))
        {
            _decorated = decorated;
        }

        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage methodCall = msg as IMethodCallMessage;
            MethodInfo methodInfo = methodCall.MethodBase as MethodInfo;

            try
            {
                // Always call the method
                Console.WriteLine(methodInfo.DeclaringType.FullName + "#" + methodInfo.Name);
                object result = methodInfo.Invoke(_decorated, methodCall.InArgs);

                // Check if it is a set with the BaseNotifyPropertyChangedAttribute and if it is call the OnPropertyChanged as well
                if (methodInfo.Name.StartsWith("set") && !methodInfo.CustomAttributes.Any
                    (
                        a => a.AttributeType == typeof(BaseNotifyPropertyChangedAttribute)
                    ))
                {
                    BaseNotifyPropertyChangedAttribute attribute = Attribute.GetCustomAttribute
                            (
                                methodInfo,
                                typeof(BaseNotifyPropertyChangedAttribute)
                            ) as BaseNotifyPropertyChangedAttribute;

                    foreach (string property in attribute.PropertiesToNotify)
                    {
                        _decorated.OnPropertyChanged(property);
                    }

                    //attribute.AfterNotifyCall(_decorated as GameSession);
                }

                return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
            }
            catch (System.Exception e)
            {
                // Note: since this impl is on property setters and OnPropertyChanged there should never be an error.
                return new ReturnMessage(e, methodCall);
            }
        }
    }
}
