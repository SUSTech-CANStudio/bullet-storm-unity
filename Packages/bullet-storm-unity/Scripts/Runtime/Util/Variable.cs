using System;

namespace CANStudio.BulletStorm.Util
{
    /// <summary>
    ///     Stores a single value of any type.
    /// </summary>
    [Serializable]
    public class Variable
    {
        private object objectValue;

        /// <summary>
        ///     Type of the stored value.
        /// </summary>
        public Type Type { get; private set; }

        public bool IsEmpty => Type is null;

        public bool HasValue<T>()
        {
            return typeof(T).IsAssignableFrom(Type);
        }

        /// <summary>
        ///     Try to get stored value.
        /// </summary>
        /// <param name="value">Stored value.</param>
        /// <typeparam name="T">Value type.</typeparam>
        /// <returns>True if success.</returns>
        public bool TryGetValue<T>(out T value)
        {
            if (HasValue<T>())
            {
                value = (T) objectValue;
                return true;
            }

            value = default;
            return false;
        }

        /// <summary>
        ///     Get stored value. Log error if value conflicts with given type.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <returns></returns>
        public T GetValue<T>()
        {
            if (TryGetValue(out T variable)) return variable;
            if (IsEmpty) BulletStormLogger.LogError("Variable is empty.");
            else BulletStormLogger.LogError("Can't convert " + Type + " to " + typeof(T));
            return default;
        }

        /// <summary>
        ///     Set value of the variable. Log error if stored value conflicts with new value.
        /// </summary>
        /// <param name="value">Any value.</param>
        /// <typeparam name="T">Value type.</typeparam>
        public void SetValue<T>(T value)
        {
            if (IsEmpty || Type.IsAssignableFrom(typeof(T)))
            {
                Type = typeof(T);
                objectValue = value;
            }
            else
            {
                BulletStormLogger.LogError("Can't convert " + typeof(T) + " to " + Type);
            }
        }

        /// <summary>
        ///     Reset the variable to empty.
        /// </summary>
        public void Reset()
        {
            Type = null;
            objectValue = null;
        }
    }
}