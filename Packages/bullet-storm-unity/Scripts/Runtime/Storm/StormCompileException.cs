using System;

namespace CANStudio.BulletStorm.Storm
{
    public class StormCompileException : Exception
    {
        private readonly int eventIndex;
        private readonly Type eventType;
        private readonly string message;

        public StormCompileException(int eventIndex, Type eventType, string message)
        {
            this.eventIndex = eventIndex;
            this.eventType = eventType;
            this.message = message;
        }

        public override string Message => eventType.Name + " at index " + eventIndex + ": " + message;
    }
}