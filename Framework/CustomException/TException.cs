using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CustomException
{
    [Serializable]
    public sealed class TException<TExceptionArgs> : Exception, ISerializable
        where TExceptionArgs : ExceptionArgs
    {
        private const String c_args = "Args";
        private readonly TExceptionArgs m_args;

        public TException(string message = null, System.Exception innerException = null)
            : this(null, message, innerException)
        {
        }

        public TException(TExceptionArgs args, String message = null, System.Exception innerException = null)
            : base(message, innerException)
        {
            m_args = args;
        }

        //该构造器用于反序列化，由于类是密封的，所以构造器是私有的  
        //如果类不是密封的，这个构造器就应该是受保护的  
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        private TException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            m_args = (TExceptionArgs)info.GetValue(c_args, typeof(TExceptionArgs));
        }

        public TExceptionArgs Args
        {
            get { return m_args; }
        }

        //这个方法用于序列化，由于实现了ISerializable接口，所以它是公共的(该方法为ISerializable中定义的方法)  
        //在Exception类中已有实现，此类继承了Exception，并重写了该方法在Exception中的实现  

        public override string Message
        {
            get
            {
                String baseMsg = base.Message;
                return (m_args == null || string.IsNullOrWhiteSpace(m_args.Message))
                    ? baseMsg
                    : baseMsg + "(" + m_args.Message + ")";
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(c_args, m_args);
            base.GetObjectData(info, context);
        }

        public override bool Equals(object obj)
        {
            var other = obj as TException<TExceptionArgs>;
            if (obj == null)
            {
                return false;
            }
            return ReferenceEquals(m_args, other.m_args) && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
