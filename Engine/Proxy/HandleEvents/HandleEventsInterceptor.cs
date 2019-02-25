using Castle.DynamicProxy;
using Engine.Attributes;
using Engine.Models;
using System;
using System.Reflection;

namespace Engine.Proxy.HandleEvents
{
    public class HandleEventsInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            // Get all the set attributes.
            HandleEventsAttribute[] attributes = ProxyCommon.AsAttribute<HandleEventsAttribute[]>(invocation.Method);

            // The attribute can only be applied to Properties that are objects and have a getter.
            if (attributes == null || attributes.Length == 0)
            {
                invocation.Proceed();
                return;
            }

            if (GetMethod(invocation, $"get_{attributes[0].MemberName}") != null)
            {
                // Need to remove the listeners before changing the object.
                foreach (HandleEventsAttribute attribute in attributes)
                {
                    EditEvent
                    (
                        attribute.MemberName,
                        attribute.EventName,
                        attribute.MethodName,
                        invocation.InvocationTarget,
                        /*removeEvent*/ true
                    );
                }
            }

            invocation.Proceed();

            if (GetMethod(invocation, $"get_{attributes[0].MemberName}") != null)
            {
                // Need to add the listeners on new object
                foreach (HandleEventsAttribute attribute in attributes)
                {
                    EditEvent
                    (
                        attribute.MemberName,
                        attribute.EventName,
                        attribute.MethodName,
                        invocation.InvocationTarget,
                        /*removeEvent*/ false
                    );
                }
            }
        }

        public void EditEvent(string memberName, string eventName, string methodName, object obj, bool removeEvent = false)
        {
            EventInfo _event;

            if (typeof(GameItem) == obj.GetType().GetProperty(memberName)?.PropertyType) // typeof(GameItem)
            {
                // LivingEntity.get_CurrentWeapon.get_Action.OnActionPerformed
                _event = obj.GetType().GetProperty(memberName).PropertyType.GetProperty("Action").PropertyType.GetEvent(eventName);
            }
            else // typeof(LivingEntity)
            {
                // GameSession.get_CurrentMonster.ActionPerformed
                _event = obj.GetType().GetProperty(memberName).PropertyType.GetEvent(eventName);
            }

            MethodInfo _method = obj.GetType().GetMethod(methodName);
            Delegate d = Delegate.CreateDelegate
            (
                _event.EventHandlerType,
                obj,
                _method
            );

            if (removeEvent) // remove event
            {
                if (typeof(GameItem) == obj.GetType().GetProperty(memberName)?.PropertyType)
                {
                    // CurrentWeapon.Action.OnActionPerformed -= RaiseActionPerformedEvent
                    object _currentWeapon = obj.GetType().GetProperty(memberName).GetValue(obj);
                    object _action = _currentWeapon.GetType().GetProperty(eventName).GetValue(_currentWeapon);
                    EventInfo _onActionPerformed = _action.GetType().GetEvent("OnActionPerformed");
                    _onActionPerformed.GetRemoveMethod().Invoke(_action, new[] { d });
                }
                else
                {
                    // CurrentMonster.ActionPerformed -= OnActionPerformed_CurrentMonster
                    object _currentMonster = obj.GetType().GetProperty(memberName).GetValue(obj);
                    EventInfo _actionPerformed = _currentMonster.GetType().GetEvent(eventName);
                    _actionPerformed.GetRemoveMethod().Invoke(_currentMonster, new[] { d });
                }
            }
            else // add event
            {
                if (typeof(GameItem) == obj.GetType().GetProperty(memberName)?.PropertyType)
                {
                    // CurrentWeapon.Action.OnActionPerformed += RaiseActionPerformedEvent
                    object _currentWeapon = obj.GetType().GetProperty(memberName).GetValue(obj);
                    object _action = _currentWeapon.GetType().GetProperty("Action").GetValue(_currentWeapon);
                    EventInfo _onActionPerformed = _action.GetType().GetEvent(eventName);
                    _onActionPerformed.GetAddMethod().Invoke(_action, new[] { d });
                }
                else
                {
                    // CurrentMonster.ActionPerformed += OnActionPerformed_CurrentMonster
                    object _currentMonster = obj.GetType().GetProperty(memberName).GetValue(obj);
                    EventInfo _actionPerformed = _currentMonster.GetType().GetEvent(eventName);
                    _actionPerformed.GetAddMethod().Invoke(_currentMonster, new[] { d });
                }
            }
        }
        public object GetMethod(IInvocation invocation, string name, params object[] arguments)
        {
            MethodInfo method = invocation.InvocationTarget.GetType().GetMethod(name);
            return method.Invoke(invocation.InvocationTarget, arguments);
        }
    }
}
