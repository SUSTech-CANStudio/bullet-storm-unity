using System.Reflection;

namespace CANStudio.BulletStorm.Util
{
    public static class System
    {
        public static bool SendMessage(object @object, string functionName)
        {
            if (@object is null) return false;
            var type = @object.GetType();
            MethodInfo method = null;
            while (method is null)
            {
                method = type.GetMethod(functionName,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                type = type.BaseType;
                if (type is null) return false;
            }

            method.Invoke(@object, null);
            return true;
        }
    }
}