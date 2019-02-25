using Engine.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Engine
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BaseNotifyPropertyChangedAttribute : Attribute
    {
        public BaseNotifyPropertyChangedAttribute
        (
            [CallerMemberName] string name = null,
            params string[] propertiesToNotify
        )
        {
            PropertiesToNotify.Add(name);

            if (propertiesToNotify != null)
            {
                PropertiesToNotify.AddRange(propertiesToNotify.ToList());
            }
        }

        public List<string> PropertiesToNotify { get; } = new List<string>();
    }
}
