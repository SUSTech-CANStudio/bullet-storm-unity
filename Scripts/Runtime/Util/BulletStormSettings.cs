using UnityEngine;

namespace CANStudio.BulletStorm.Util
{
    /// <summary>
    /// Runtime settings.
    /// </summary>
    public class BulletStormSettings : ScriptableObject
    {
        
        
        public const string ResourcePath = "BulletStorm/settings.asset";
        public static BulletStormSettings Instance => _instance ? _instance : _instance = Load();
        
        private static BulletStormSettings _instance;

        private static BulletStormSettings Load() => Resources.Load<BulletStormSettings>(ResourcePath);
    }
}