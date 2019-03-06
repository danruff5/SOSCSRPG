using System.Reflection;
using System.Runtime.Serialization;

namespace Engine.Exception
{
    public class InvalidAttributeDecelerationException : System.Exception
    {
        private readonly MemberInfo _info;

        public InvalidAttributeDecelerationException
        (
            MemberInfo memberInfo,
            string AttributeTypeName
        ) : base($"{AttributeTypeName} cannot be applied to " +
                 $"{memberInfo.DeclaringType}#{memberInfo.Name}.")
        {
            _info = memberInfo;
        }

        public InvalidAttributeDecelerationException() { }
        public InvalidAttributeDecelerationException(string message) : base(message) { }
        public InvalidAttributeDecelerationException(string message, System.Exception innerException) : base(message, innerException) { }
        protected InvalidAttributeDecelerationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
