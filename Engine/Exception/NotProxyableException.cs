using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Engine.Exceptions
{
    [Serializable]
    internal class NotProxyableException : Exception
    {
        private MemberInfo _info;

        public NotProxyableException
        (
            MemberInfo memberInfo,
            string GenerationHookName = null
        ) : base($"{memberInfo.MemberType} " +
                 $"{memberInfo.DeclaringType}#{memberInfo.Name} is not proxyable using " +
                 $"{GenerationHookName ?? "a generation hook"}.")
        {
            _info = memberInfo;
        }

        public NotProxyableException() { }
        public NotProxyableException(string message) : base(message) { }
        public NotProxyableException(string message, Exception innerException) : base(message, innerException) { }
        protected NotProxyableException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}