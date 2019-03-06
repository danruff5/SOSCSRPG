using Castle.DynamicProxy;
using Engine.Attributes;
using Engine.Models;
using Engine.Proxy;
using System;
using System.Reflection;

namespace ConsoleApp1
{
    public class HandleEventsAspect : BaseAspect
    {
        public override void Intercept(IInvocation invocation)
        {
            // Get all the set attributes.
            HandleEventsAttribute[] attributes = ProxyCommon.AsAttribute<HandleEventsAttribute[]>(invocation.Method);

            // The attribute can only be applied to Properties that are objects and have a getter.
            if (attributes == null || attributes.Length == 0)
            {
                invocation.Proceed();
                return;
            }

            if (ProxyCommon.GetMethodResult(invocation, $"get_{attributes[0].MemberName}") != null)
            {
                // Need to remove the listeners before changing the object.
                EditEvents(attributes, invocation, /*removeEvent*/ true);
            }

            invocation.Proceed();

            if (ProxyCommon.GetMethodResult(invocation, $"get_{attributes[0].MemberName}") != null)
            {
                // Need to add the listeners on new object
                EditEvents(attributes, invocation, /*removeEvent*/ false);
            }
        }

        public override bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return ProxyCommon.IsPropertyWithAttribute<HandleEventsAttribute>(type, methodInfo);
        }

        private void EditEvents(HandleEventsAttribute[] attributes, IInvocation invocation, bool removeEvent = false)
        {
            foreach (HandleEventsAttribute attribute in attributes)
            {
                EventInfo _event;
                object _obj;

                // CLEAN: This is ugly codeage bacause of eventing structure

                if (typeof(GameItem) == invocation.InvocationTarget.GetType().GetProperty(attribute.MemberName)?.PropertyType)
                {
                    // LivingEntity.get_CurrentWeapon.get_Action.OnActionPerformed
                    // CurrentWeapon.Action.OnActionPerformed -= RaiseActionPerformedEvent
                    object _currentWeapon = invocation.InvocationTarget.GetType().GetProperty(attribute.MemberName).GetValue(invocation.InvocationTarget);
                    _obj = _currentWeapon.GetType().GetProperty("Action").GetValue(_currentWeapon);
                    _event = invocation.InvocationTarget.GetType().GetProperty(attribute.MemberName).PropertyType.GetProperty("Action").PropertyType.GetEvent(attribute.EventName); ;
                }
                else
                {
                    // GameSession.get_CurrentMonster.ActionPerformed
                    // CurrentMonster.ActionPerformed -= OnActionPerformed_CurrentMonster
                    _obj = invocation.InvocationTarget.GetType().GetProperty(attribute.MemberName).GetValue(invocation.InvocationTarget);
                    _event = invocation.InvocationTarget.GetType().GetProperty(attribute.MemberName).PropertyType.GetEvent(attribute.EventName);
                }


                MethodInfo _method = invocation.InvocationTarget.GetType().GetMethod(attribute.MethodName);
                Delegate d = Delegate.CreateDelegate
                (
                    _event.EventHandlerType,
                    invocation.InvocationTarget,
                    _method
                );

                ProxyCommon.EditEvent(_event, _obj, d, removeEvent);
            }
        }
    }
}
