using System;
using System.Runtime.Serialization;

namespace UConsole
{

    [Serializable]
    public class NoSuchCommandException : Exception
    {

        public string command { get; private set; }

        public NoSuchCommandException() : base() { }

        public NoSuchCommandException(string message, string command)
            : base(message)
        {
            this.command = command;
        }

        protected NoSuchCommandException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
                this.command = info.GetString("command");
        }

        // Perform serialization
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (info != null)
                info.AddValue("command", command);
        }
    }
}
