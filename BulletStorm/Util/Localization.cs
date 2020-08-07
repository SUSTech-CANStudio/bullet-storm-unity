using System;
using System.Collections.Generic;
using UnityEditor;

namespace BulletStorm.Util
{
    public class Localization
    {
        public static Localization Instance => Lazy.Value;
        
        private static readonly Lazy<Localization> Lazy = new Lazy<Localization>(() => new Localization());
        
        private Dictionary<string, string> tooltips;

        private Localization()
        {
#if UNITY_EDITOR
            BulletStormLogger.Log(EditorPrefs.GetString("language"));
#endif
        }

        public static string Tooltip(string tooltip) => Translate(tooltip, Instance.tooltips);
        
        private static string Translate(string origin, IReadOnlyDictionary<string, string> dictionary)
        {
            return dictionary is null || !dictionary.TryGetValue(origin, out var translate) || translate is null ||
                   translate.Length == 0
                ? origin
                : translate;
        }
    }
}