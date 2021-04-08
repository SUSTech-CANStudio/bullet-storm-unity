using System;
using CANStudio.BulletStorm.Util;
using UnityEditor;
using UnityEngine;

namespace CANStudio.BulletStorm.Editor
{
    /// <summary>
    ///     Static class for read and write editor preferences.
    /// </summary>
    public static class Preferences
    {
        /// <summary>
        ///     Access preferences.
        /// </summary>
        public static SerializedPreference _ => SerializedPreference.Load();

        /// <summary>
        ///     Write all changes in preference to disk.
        /// </summary>
        public static void ApplyChanges()
        {
            _.Save();
        }
    }

    [Serializable]
    public class SerializedPreference
    {
        private const string PreferenceKey = "BulletStorm_Preference";

        private static readonly Lazy<SerializedPreference> Cached = new Lazy<SerializedPreference>(() =>
        {
            var json = EditorPrefs.GetString(PreferenceKey);
            SerializedPreference preference = null;
            try
            {
                preference = JsonUtility.FromJson<SerializedPreference>(json);
            }
            catch (Exception e)
            {
                BulletStormLogger.LogWarning($"{e} occured when loading preferences:\n" + e.StackTrace);
            }

            return preference ?? new SerializedPreference();
        });

        public Mesh shapePreviewMesh;
        public Material shapePreviewMaterial;

        private SerializedPreference()
        {
        }

        internal static SerializedPreference Load()
        {
            return Cached.Value;
        }

        internal void Save()
        {
            var json = JsonUtility.ToJson(this);
            EditorPrefs.SetString(PreferenceKey, json);
        }
    }
}